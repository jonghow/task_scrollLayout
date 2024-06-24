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

    public Dictionary<LoadedResourceCategory, Dictionary<string, UnityEngine.Object>> m_DicCategoryByKey = new Dictionary<LoadedResourceCategory, Dictionary<string, Object>>();

    private GameObject _loadedScrollview;
    private bool _isInitialized = false;
    private System.Action _onLoadedComplete;

    public bool CheckInitialize() => this._isInitialized;

    async void Initialize()
    {
        if (_isInitialized) return;
        m_DicCategoryByKey.Clear();

        // Use UniTask By Addressable AsyncLoading
        AddResouceCache(LoadedResourceCategory.Prefab, $"InfinityScrollView", await GetPrefabAsyncLoading($"InfinityScrollView"));

        StringBuilder sb = new StringBuilder();
        string toStringFromsb;

        //Consume
        for (int i = 0; i < 3; ++i)
        {
            sb.Clear();
            sb.Append($"Consume{i}");
            toStringFromsb = sb.ToString();

            AddResouceCache(LoadedResourceCategory.Sprite, $"{toStringFromsb}", await GetSpriteAsyncLoading($"{toStringFromsb}"));
        }

        //Equipment
        for (int i = 0; i < 5; ++i)
        {
            sb.Clear();
            sb.Append($"Equipment{i}");
            toStringFromsb = sb.ToString();

            AddResouceCache(LoadedResourceCategory.Sprite, $"{toStringFromsb}", await GetSpriteAsyncLoading($"{toStringFromsb}"));
        }

        //Goods
        for (int i = 0; i < 3; ++i)
        {
            sb.Clear();
            sb.Append($"Goods{i}");
            toStringFromsb = sb.ToString();

            AddResouceCache(LoadedResourceCategory.Sprite, $"{toStringFromsb}", await GetSpriteAsyncLoading($"{toStringFromsb}"));
        }

        _isInitialized = true;
        _onLoadedComplete?.Invoke();
    }

    private void AddResouceCache(LoadedResourceCategory category, string addressable , UnityEngine.Object loadedObject)
    {
        if (m_DicCategoryByKey.ContainsKey(category) == false)
            m_DicCategoryByKey.Add(category, new Dictionary<string, Object>());

        m_DicCategoryByKey[category].Add(addressable, loadedObject);
    }

    public UnityEngine.Object GetResourceCache(LoadedResourceCategory category, string addressable)
    {
        Dictionary<string, UnityEngine.Object> dicFinder = null;
        UnityEngine.Object ret = null;

        if (m_DicCategoryByKey.TryGetValue(category, out dicFinder))
            dicFinder.TryGetValue(addressable, out ret);

        return ret;
    }
    private async UniTask<GameObject> GetPrefabAsyncLoading(string addressableName)
    {
        UniTask<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(addressableName).Task.AsUniTask();
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
