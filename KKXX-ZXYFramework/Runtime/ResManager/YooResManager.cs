using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using System.IO;
using System.Threading.Tasks;


public class YooResManager
{

    /// <summary>
    /// 资源路径根目录
    /// 唯一目录
    /// </summary>
    public const string YooAssetResPath = "AYooAssetRes";


    #region 普通单例

    private static YooResManager instance = null;

    public static YooResManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new YooResManager();
            }
            return instance;
        }
    }

    #endregion

    public bool ISInitial = false;

    public ResourcePackage package = null;

    private YooResManager()
    {

        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        package = YooAssets.CreatePackage("DefaultPackage");

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        InitializeYooAsset();

    }

#if UNITY_EDITOR

    private void InitializeYooAsset()
    {
        EditorSimulateModeParameters initParameters = new EditorSimulateModeParameters();
        string simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
        initParameters.SimulateManifestFilePath = simulateManifestFilePath;
        package.InitializeAsync(initParameters).Completed += (e) =>
        {
              ISInitial = true;
              Debug.Log("<Color=#E60000>YooAsset初始化完成</Color>");
        };
    }

#else

    private void InitializeYooAsset()
    {
        var initParameters = new OfflinePlayModeParameters();
        initParameters.DecryptionServices = new FileOffsetDecryption();
        package.InitializeAsync(initParameters).Completed += (e) =>
        {
            ISInitial = true;
            Debug.Log("<Color=#E60000>YooAsset初始化完成</Color>");
        };
    }
   
#endif

    private void FixPath(ref string path)
    {
        if (!path.Contains(YooAssetResPath + "/"))
        {
            path = YooAssetResPath + "/" + path;
        }
        if (!path.Contains("Assets/"))
        {
            path = "Assets/" + path;
        }
    }

    public async Task<T> LoadAssetAsync<T>(string path) where T: UnityEngine.Object
    {
        while (!ISInitial)
        {
            await Task.Yield(); 
        }
        FixPath(ref path);
        AssetHandle async = YooResManager.Instance.package.LoadAssetAsync<T>(path);
        while (!async.IsDone)
        {
            await Task.Yield();
        }
        return async.GetAssetObject<T>();
    }

    public async void LoadAssetAsync<T>(string path,System.Action<T> action) where T : UnityEngine.Object
    {
        FixPath(ref path);
        T t = await LoadAssetAsync<T>(path);
        action?.Invoke(t);
    }

    public async Task<List<T>> LoadAllAssetAsync<T>(string path) where T : UnityEngine.Object
    {
        while (!ISInitial)
        {
            await Task.Yield();
        }
        FixPath(ref path);
        AllAssetsHandle async = YooResManager.Instance.package.LoadAllAssetsAsync<T>(path);
        while (!async.IsDone)
        {
            await Task.Yield();
        }
        List<T> t = new List<T>();
        Object[] objects = async.AllAssetObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            t.Add(objects[i] as T);
        }
        return t;
    }

    public async void LoadAllAssetAsync<T>(string path, System.Action<List<T>> action) where T : UnityEngine.Object
    {
        FixPath(ref path);
        List<T> t = await LoadAllAssetAsync<T>(path);
        action?.Invoke(t);
    }

    public T LoadAssetSync<T>(string path) where T : UnityEngine.Object
    {
        if (!ISInitial)
        {
            Debug.LogError("The resource has not been initialized ！！！！");
            return null;
        }
        FixPath(ref path);
        AssetHandle async = YooResManager.Instance.package.LoadAssetSync<T>(path);
        return async.GetAssetObject<T>();
    }

    public async Task LoadSceneAsync(string path)
    {
        while (!ISInitial)
        {
            await Task.Yield();
        }
        FixPath(ref path);
        YooAsset.SceneHandle async = YooResManager.Instance.package.LoadSceneAsync(path, UnityEngine.SceneManagement.LoadSceneMode.Single);
        while (!async.IsDone)
        {
            await Task.Yield();
        }
    }


}

/// <summary>
/// 资源文件偏移加载解密类
/// </summary>
public class FileOffsetDecryption : IDecryptionServices
{

    AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
    {
        managedStream = null;
        return AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
    }

    AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
    {
        managedStream = null;
        return AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
    }

    public static ulong GetFileOffset()
    {
        return 32;
    }
}