using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEditor;

public static partial class XmlUtility 
{
    public static async UniTask<List<ItemData>> LoadXML(string _fileName)
    {
        TextAsset txtAsset = (TextAsset)await LoadXMLAsync($"{_fileName}");

        XmlDocument xmlDoc = new XmlDocument();
        Debug.Log(txtAsset.text);
        xmlDoc.LoadXml(txtAsset.text);

        XmlNodeList all_nodes = xmlDoc.SelectNodes("ItemDataSheet");
        List<ItemData> itemDataList = new List<ItemData>();

        foreach (XmlNode parentNode in all_nodes)
        {
            foreach(XmlNode childNode in parentNode.ChildNodes)
            {
                var itemData = new ItemData();
                itemData.Parse((XmlElement)childNode);
                itemDataList.Add(itemData);
            }
        }

        return itemDataList;
    }

    public static async UniTask<TextAsset> LoadXMLAsync(string addressableName)
    {
        UniTask<TextAsset> asyncOperationHandle = Addressables.LoadAssetAsync<TextAsset>(addressableName).Task.AsUniTask();
        TextAsset ret = await asyncOperationHandle;
        return ret;
    }
}
