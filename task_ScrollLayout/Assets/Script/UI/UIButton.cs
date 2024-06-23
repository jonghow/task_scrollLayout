using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEditor;

public class UIButton : MonoBehaviour
{
    string uiBackGround = "UIBackGround";
    GameObject backGroundObject;

    public async void OnClickOpenUI()
    {
        UIWindowManager.GetInstance().OpenUIWindow($"InfinityScrollView");
    }

    async UniTask<GameObject> asyncLoadingBackGroundObject()
    {
        // Addressable Ref Count 체크를 위한 Addressable 생성

        UniTask<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(uiBackGround).Task.AsUniTask();
        return await asyncOperationHandle;
    }

    public void OnClickCloseUI()
    {
        UIWindowManager.GetInstance().CloseUIWindow($"InfinityScrollView");

        var infiniteScrollView = GameObject.Find("InfinityScrollView(Clone)");
        GameObject.Destroy(infiniteScrollView);
    }
}
