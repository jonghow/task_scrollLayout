using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    // Start is called before the first frame update

    [ReadOnly(true)][SerializeField] GameObject loadedScrollview; 
    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>("InfinityScrollView").Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            var gameobject = GameObject.Find("Canvas");
            loadedScrollview = GameObject.Instantiate(obj.Result, gameobject.transform);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
