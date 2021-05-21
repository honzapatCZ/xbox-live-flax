using Flax.Build;
using Flax.Build.NativeCpp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class XboxLiveFlax : GameModule
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // C#-only scripting
        BuildNativeCode = true;
        BuildCSharp = true;  
    }

    /// <inheritdoc />
    public override void Setup(BuildOptions options)
    {
        options.SourcePaths.Clear();

        options.PublicDependencies.Add("OnlinePlatform");

        options.SourceFiles.AddRange(Directory.GetFiles(FolderPath, "*.*", SearchOption.TopDirectoryOnly));
        //options.SourcePaths.AddRange(Directory.GetDirectories(FolderPath).Where(y=>!y.Contains("XboxLiveFlaxEditor")));

        options.CompileEnv.IncludePaths.Add(Path.Combine(FolderPath, "include"));
        options.LinkEnv.InputLibraries.Add(Path.Combine(FolderPath, "lib", "x64", "v141", "Release", "Microsoft.Xbox.Services.141.UWP.Ship.Cpp.lib"));

        if (options.ScriptingAPI.Defines.Contains("FLAX_EDITOR"))
        {
            options.PublicDependencies.Add("XboxLiveFlaxEditor");
        }

        base.Setup(options);       
    }
}
