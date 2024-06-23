using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ClientGlobal;
using Cysharp.Threading.Tasks;
using UnityEditor.AddressableAssets.HostingServices;
using System.Text;
using System.Threading;
using Unity.VisualScripting;

public class ResourceManager 
{
    public static ResourceManager Instance;
    public static ResourceManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ResourceManager();
            Instance.Initialize();
        }

        return Instance;
    }

    public Dictionary<LoadedResourceCategory, string> m_DicCategoryByKey = new Dictionary<LoadedResourceCategory, string>();
    public Dictionary<string, UnityEngine.Object> m_DicKeyByOriginal = new Dictionary<string, UnityEngine.Object>();

    private GameObject _loadedScrollview;
    private bool _isInitialized = false;

    async void Initialize()
    {
        if (_isInitialized) return;

        m_DicCategoryByKey.Clear();
        m_DicKeyByOriginal.Clear();

        // Use UniTask By Addressable AsyncLoading
        AddResouceCache(LoadedResourceCategory.Prefab, $"InfinityScrollView", await GetPrefabAsyncLoading($"InfinityScrollView"));

        StringBuilder sb = new StringBuilder();
        string toStringFromsb;

        for (int i = 0; i < 2; ++i)
        {
            sb.Clear();
            sb.Append($"Casual{i}");
            toStringFromsb = sb.ToString();

            AddResouceCache(LoadedResourceCategory.Sprite, $"{toStringFromsb}", await GetSpriteAsyncLoading($"{toStringFromsb}"));
        }

        _isInitialized = true;
    }

    private void AddResouceCache(LoadedResourceCategory category, string addressable , UnityEngine.Object loadedObject)
    {
        if (m_DicCategoryByKey.ContainsKey(category) == false)
            m_DicCategoryByKey.Add(category, addressable);

        if(m_DicKeyByOriginal.ContainsKey(addressable) == false)
            m_DicKeyByOriginal.Add(addressable, loadedObject);
    }

    public UnityEngine.Object GetResourceCache(LoadedResourceCategory category, string addressable)
    {
        string retString = null;
        UnityEngine.Object ret = null;

        if (m_DicCategoryByKey.TryGetValue(category, out retString) == false)
            return null;

        if (m_DicKeyByOriginal.TryGetValue(retString , out ret) == false)
            return null;

        return ret;
    }
    private async UniTask<GameObject> GetPrefabAsyncLoading(string addressableName)
    {
        UniTask<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("InfinityScrollView").Task.AsUniTask();
        GameObject ret = await asyncOperationHandle;
        return ret;
    }
    private async UniTask<Sprite> GetSpriteAsyncLoading(string addressableName)
    {
        UniTask<Sprite> asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>($"{addressableName}").Task.AsUniTask();
        Sprite ret = await asyncOperationHandle;
        return ret;
    }
}
