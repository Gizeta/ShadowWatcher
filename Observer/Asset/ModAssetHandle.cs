using Cute;
using ShadowWatcher.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ShadowWatcher.Asset
{
    public class ModAssetHandle : AssetHandle
    {
        private Action phase
        {
            get
            {
                return typeof(AssetHandle).GetField<Action>("phase", this);
            }
            set
            {
                typeof(AssetHandle).SetField("phase", this, value);
            }
        }

        private AssetRequestContext requestContext
        {
            get
            {
                return typeof(AssetHandle).GetField<AssetRequestContext>("requestContext", this);
            }
            set
            {
                typeof(AssetHandle).SetField("requestContext", this, value);
            }
        }

        private void invokeMethod(string name)
        {
            typeof(AssetHandle).InvokeMethod(name, this);
        }

        public ModAssetHandle(string _name, string expectedDataHash, bool _unloadCommon, bool _unloadTemporary) : base(_name, expectedDataHash, "common", false, false)
        {
            phase = new Action(_PhaseIdle);
            unloadCommon = _unloadCommon;
            unloadTemporary = _unloadTemporary;
        }

        private IEnumerator __PlatformDependentLoad()
        {
            var request = AssetBundle.LoadFromFileAsync(Path.Combine(AssetModder.modPath, filename));
            yield return request;

            var bundle = request.assetBundle;
            Toolbox.AssetManager.SetAssetBundle(filename, bundle);

            var pathList = bundle.GetAllAssetNames();
            var objList = bundle.LoadAllAssets();
            var nameChk = false;
            for (var i = 0; i < Toolbox.AssetManager.NoUnloadAssetName.Count; i++)
            {
                if (filename.StartsWith(Toolbox.AssetManager.NoUnloadAssetName[i]))
                {
                    nameChk = true;
                }
            }
            if (!unloadCommon && !nameChk)
            {
                bundle.Unload(false);
                bundle = null;
            }

            var registList = new List<AssetObject>();
            for (var i = 0; i < objList.Length; i++)
            {
                for (var j = 0; j < pathList.Length; j++)
                {
                    var localfilename = Path.GetFileNameWithoutExtension(pathList[j]);
                    if (objList[i].name.ToLower().Equals(localfilename.ToLower()))
                    {
                        var pathWithoutExtension = Path.ChangeExtension(pathList[j], ".any");
                        registList.Add(new AssetObject(pathWithoutExtension, objList[i]));
                    }
                }
            }
            Toolbox.AssetManager.SetObjectList(filename, registList);
            
            invokeMethod("_LoadPostProcess");
            _Fin();
        }

        private void _Fin()
        {
            Action<AssetHandle> callback = null;
            if (requestContext != null)
            {
                var semaphore = requestContext.semaphore;
                if (semaphore != null)
                {
                    semaphore.Post();
                }
                callback = requestContext.callback;
            }

            phase = new Action(_PhaseIdle);
            requestContext = null;
            callback?.Invoke(this);
        }

        private void __LoadCancel() { }

        private void _PhaseIdle()
        {
            phase = new Action(_PhaseLoading);
            Toolbox.AssetManager.AddJob(__PlatformDependentLoad(), new Action(__LoadCancel));
        }

        private void _PhaseLoading() { }
    }
}
