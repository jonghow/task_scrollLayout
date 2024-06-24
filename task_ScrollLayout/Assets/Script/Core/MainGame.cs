using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

public class MainGame : MonoBehaviour
{
    async void Start()
    {
        ResourceManager.GetInstance();
        ItemDataManager.GetInstance();
        UIWindowManager.GetInstance();
    }
}
