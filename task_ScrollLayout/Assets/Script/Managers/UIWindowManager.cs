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
using UnityEngine.Timeline;
using UnityEditor.VersionControl;
using System.Xml.Linq;

public class UIWindowManager
{
    public static UIWindowManager Instance;
    public static UIWindowManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new UIWindowManager();
            Instance.Initialize();
        }

        return Instance;
    }

    private Dictionary<string, GameObject> _dicWindows = new Dictionary<string, GameObject>();
    private GameObject _backBlackWindow;
    private GameObject _canvas;
    private string uiBackGround = "UIBackGround";

    private bool _isInitialized = false;

    async void Initialize()
    {
        if (_isInitialized) return;

        _dicWindows.Clear();

        _backBlackWindow = await asyncLoadingBackGroundObject();
        ClientUtility.SetActive(_backBlackWindow, false);
        _canvas = GameObject.Find("Canvas");
        _backBlackWindow.transform.SetParent(_canvas.transform);
        _backBlackWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        _isInitialized = true;
    }

    async UniTask<GameObject> asyncLoadingBackGroundObject()
    {
        // Addressable Ref Count 체크를 위한 Addressable 생성
        UniTask<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(uiBackGround).Task.AsUniTask();
        return await asyncOperationHandle;
    }

    public void OpenUIWindow(string name)
    {
        if (_dicWindows.ContainsKey(name) == true) return;

        UnityEngine.Object unityObject = ResourceManager.GetInstance().GetResourceCache(ClientGlobal.LoadedResourceCategory.Prefab, $"{name}");

        if (unityObject != null)
        {
            var instantiateObject = GameObject.Instantiate(unityObject, _canvas.transform);
            _dicWindows.Add(name,(GameObject)instantiateObject);
        }

        CheckOnBlackBack();
    }

    public void CloseUIWindow(string name)
    {
        GameObject targetObject = null;
        if (_dicWindows.TryGetValue(name, out targetObject) == false) return;

        _dicWindows.Remove(name);
        GameObject.Destroy(targetObject);

        CheckOnBlackBack();
    }

    private void CheckOnBlackBack()
    {
        ClientUtility.SetActive(this._backBlackWindow, _dicWindows.Count > 0);
    }
}
