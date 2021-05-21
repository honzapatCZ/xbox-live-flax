// Copyright (C) 2021 Nejcraft Do Not Redistribute

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FlaxEditor;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using Microsoft.Xbox.Services.DevTools;
using Microsoft.Xbox.Services.DevTools.Authentication;

namespace XboxLiveFlax
{
    /// <summary>
    /// XboxLiveHelperWindow.
    /// </summary>
    public class XboxLiveHelperWindow : CustomEditorWindow
    {
        public override void Initialize(LayoutElementsContainer layout)
        {
			DevAccount devAccount = ToolAuthentication.LoadLastSignedInUser();
			layout.Header("Account");
			if (devAccount != null)
            {
				layout.Label("Username: " + devAccount.Name);
				layout.Label("Id: " + devAccount.Id);
				layout.Label("Type: " + devAccount.AccountType);
			}
				
			else
				layout.Label("Not logged in");

			layout.Header("Sandbox");

			var sb = layout.TextBox();
			var setSb = layout.Button("Set Sandbox");
			setSb.Button.Clicked += () => SandboxHelper.SetSandbox(sb.Text);
			var retail = layout.Button("Switch to retail");
			setSb.Button.Clicked += () => SandboxHelper.SetSandbox("RETAIL");
		}
	}

	// Token: 0x02000003 RID: 3
	internal class SandboxHelper
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002409 File Offset: 0x00000609
		internal static string GetSandbox()
		{
			return SandboxHelper.RegWOW6432.GetRegKey(SandboxHelper.RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\XboxLive", "Sandbox");
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002420 File Offset: 0x00000620
		internal static bool IsValidSandbox(string sandbox)
		{
			if (string.IsNullOrEmpty(sandbox))
			{
				return false;
			}
			MatchCollection matchCollection = Regex.Matches(sandbox, "[a-zA-Z0-9.]+");
			return matchCollection.Count > 0 && matchCollection[0].Value.Equals(sandbox);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002460 File Offset: 0x00000660
		internal static void SetSandbox(string sandboxId)
		{
			Debug.Log("Setting the Sandbox");
			SandboxHelper.RegWOW6432.SetRegKey(SandboxHelper.RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\XboxLive", "Sandbox", sandboxId);
			new System.Diagnostics.Process
			{
				StartInfo = new System.Diagnostics.ProcessStartInfo
				{
					WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
					FileName = "cmd.exe",
					Arguments = "/C " + "net stop XblAuthManager && net start XblAuthManager"
				}
			}.Start();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000253C File Offset: 0x0000073C
		internal static void OpenAppsInSandboxMode(bool shouldRelaunch)
		{
			string sandbox = SandboxHelper.GetSandbox();
			string text = (!sandbox.Equals("RETAIL", StringComparison.CurrentCultureIgnoreCase)).ToString().ToLower();
			SandboxHelper.OpenApp("Xbox App", string.Concat(new string[]
			{
				"start \"\" \"msxbox://sandbox?enabled=",
				text,
				"&id=",
				sandbox,
				"\""
			}), false, "Xbox", shouldRelaunch);
			SandboxHelper.OpenApp("Xbox Companion App", "start xbox:", true, "XboxApp", shouldRelaunch);
			SandboxHelper.OpenApp("Windows Store App", string.Concat(new string[]
			{
				"start \"\" \"ms-windows-store:sandbox?enabled=",
				text,
				"&id=",
				sandbox,
				"\""
			}), true, "WinStore.App", shouldRelaunch);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000025FA File Offset: 0x000007FA
		private static void OpenApp(string friendlyName, string command)
		{
			SandboxHelper.OpenApp(friendlyName, command, false, "", false);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000260C File Offset: 0x0000080C
		private static void OpenApp(string friendlyName, string command, bool killProcess, string processName, bool shouldRelaunch)
		{
            System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName(processName);
			if (processesByName.Length == 0 && shouldRelaunch)
			{
				return;
			}
			if (killProcess && processesByName.Length != 0)
			{
				Console.WriteLine("Closing {0}", friendlyName);
				System.Diagnostics.Process[] array = processesByName;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Kill();
				}
				Console.WriteLine("Closed {0}", friendlyName);
			}
			Console.WriteLine("Starting {0}", friendlyName);
			new System.Diagnostics.Process
			{
				StartInfo = new System.Diagnostics.ProcessStartInfo
				{
					WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
					FileName = "cmd.exe",
					Arguments = "/C " + command
				}
			}.Start();
			Console.WriteLine("Started {0}", friendlyName);
		}

		// Token: 0x04000001 RID: 1
		private const string SANDBOX_REGISTRY_KEY = "SOFTWARE\\Microsoft\\XboxLive";

		// Token: 0x02000004 RID: 4
		private enum RegSAM
		{
			// Token: 0x04000003 RID: 3
			QueryValue = 1,
			// Token: 0x04000004 RID: 4
			SetValue,
			// Token: 0x04000005 RID: 5
			CreateSubKey = 4,
			// Token: 0x04000006 RID: 6
			EnumerateSubKeys = 8,
			// Token: 0x04000007 RID: 7
			Notify = 16,
			// Token: 0x04000008 RID: 8
			CreateLink = 32,
			// Token: 0x04000009 RID: 9
			WOW64_32Key = 512,
			// Token: 0x0400000A RID: 10
			WOW64_64Key = 256,
			// Token: 0x0400000B RID: 11
			WOW64_Res = 768,
			// Token: 0x0400000C RID: 12
			Read = 131097,
			// Token: 0x0400000D RID: 13
			Write = 131078,
			// Token: 0x0400000E RID: 14
			Execute = 131097,
			// Token: 0x0400000F RID: 15
			AllAccess = 983103
		}

		// Token: 0x02000005 RID: 5
		private static class RegHive
		{
			// Token: 0x04000010 RID: 16
			public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(2147483650u);

			// Token: 0x04000011 RID: 17
			public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(2147483649u);
		}

		// Token: 0x02000006 RID: 6
		private static class RegWOW6432
		{
			// Token: 0x0600000D RID: 13
			[DllImport("Advapi32.dll")]
			private static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions, int samDesired, out int phkResult);

			// Token: 0x0600000E RID: 14
			[DllImport("Advapi32.dll")]
			private static extern uint RegCloseKey(int hKey);

			// Token: 0x0600000F RID: 15
			[DllImport("Advapi32.dll")]
			public static extern int RegQueryValueEx(int hKey, string lpValueName, int lpReserved, ref uint lpType, StringBuilder lpData, ref uint lpcbData);

			// Token: 0x06000010 RID: 16
			[DllImport("Advapi32.dll")]
			public static extern int RegSetValueExA(int hKey, string lpValueName, int lpReserved, int dwType, byte[] lpData, int cbData);

			// Token: 0x06000011 RID: 17 RVA: 0x000026D4 File Offset: 0x000008D4
			internal static string GetRegKey(UIntPtr inHive, string inKeyName, string inPropertyName)
			{
				int num = 0;
				string result;
				try
				{
					uint num2 = SandboxHelper.RegWOW6432.RegOpenKeyEx(inHive, inKeyName, 0u, 257, out num);
					if (num2 != 0u)
					{
						result = null;
					}
					else
					{
						uint num3 = 0u;
						uint num4 = 1024u;
						StringBuilder stringBuilder = new StringBuilder(1024);
						SandboxHelper.RegWOW6432.RegQueryValueEx(num, inPropertyName, 0, ref num3, stringBuilder, ref num4);
						result = stringBuilder.ToString();
					}
				}
				finally
				{
					if (num != 0)
					{
						SandboxHelper.RegWOW6432.RegCloseKey(num);
					}
				}
				return result;
			}

			// Token: 0x06000012 RID: 18 RVA: 0x00002748 File Offset: 0x00000948
			internal static uint SetRegKey(UIntPtr inHive, string inKeyName, string inPropertyName, string inPropertyValue)
			{
				int num = 0;
				uint result = 0u;
				try
				{
					uint num2 = SandboxHelper.RegWOW6432.RegOpenKeyEx(inHive, inKeyName, 0u, 258, out num);
					if (num2 != 0u)
					{
						return num2;
					}
					byte[] bytes = Encoding.Default.GetBytes(inPropertyValue);
					byte[] array = new byte[bytes.Length + 1];
					Array.Copy(bytes, 0, array, 0, bytes.Length);
					result = (uint)SandboxHelper.RegWOW6432.RegSetValueExA(num, inPropertyName, 0, 1, array, array.Length);
				}
				finally
				{
					if (num != 0)
					{
						SandboxHelper.RegWOW6432.RegCloseKey(num);
					}
				}
				return result;
			}
		}
	}
}
