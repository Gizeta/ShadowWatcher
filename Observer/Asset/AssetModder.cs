using Cute;
using ShadowWatcher.Helper;
using ShadowWatcher.Socket;
using System.Collections.Generic;
using System.IO;

namespace ShadowWatcher.Asset
{
    public class AssetModder
    {
        internal static string modPath;

        public AssetModder(string basePath)
        {
            modPath = Path.Combine(basePath, "mod");
        }

        public void SetUp()
        {
            Observer.OnTick += checkHasLoaded;
        }

        private void checkHasLoaded()
        {
            var assets = Toolbox.AssetManager.GetField<Dictionary<string, AssetHandle>>("handleDictionary");
            if (assets.ContainsKey("card_1000110100.unity3d"))
            {
                init(assets);
            }
        }

        private void init(Dictionary<string, AssetHandle> assets)
        {
            if (Directory.Exists(modPath))
            {
                var folder = new DirectoryInfo(modPath);
                foreach (var file in folder.GetFiles())
                {
                    var filename = file.Name;
                    if (filename.StartsWith("card_") && assets.ContainsKey(filename))
                    {
                        var oldHandle = assets[filename];
                        Toolbox.AssetManager.UnloadAssetBundle(filename);
                        assets[filename] = new ModAssetHandle(filename, oldHandle.GetProperty<string>("manifestDataHash"), oldHandle.unloadCommon, oldHandle.unloadTemporary);
                        Sender.Send("Mod", filename);
                    }
                }
            }

            Observer.OnTick -= checkHasLoaded;
        }
    }
}
