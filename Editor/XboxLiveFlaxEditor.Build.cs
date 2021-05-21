using Flax.Build;
using Flax.Build.NativeCpp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class XboxLiveFlaxEditor : GameEditorModule
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // C#-only scripting
        BuildNativeCode = false;
    }

    /// <inheritdoc />
    public override void Setup(BuildOptions options)
    {
        base.Setup(options);

        options.ScriptingAPI.FileReferences.Add(Path.Combine(FolderPath, "Microsoft.Xbox.Services.DevTools.dll"));
    }
}
