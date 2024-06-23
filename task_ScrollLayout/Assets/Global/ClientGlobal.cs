using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ClientGlobal
{
    public enum LoadedResourceCategory
    {
        None,
        Prefab,
        Sprite,
        Max,
    }
}
