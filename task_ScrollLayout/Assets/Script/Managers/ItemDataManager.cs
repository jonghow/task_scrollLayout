using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class ItemData
{
    public void Parse(XmlElement xNode)
    {
        int.TryParse(xNode.GetAttribute("ID"), out this.ID);
        int.TryParse(xNode.GetAttribute("Gold"), out this.Gold);
        Item = xNode.GetAttribute("Item");
        UseGoods = xNode.GetAttribute("UseGoods");
    }

    public int ID;
    public int Gold;
    public string Item;
    public string UseGoods;
}

public class ItemDataManager 
{
    public static ItemDataManager Instance;

    public List<ItemData> ItemDataList = new List<ItemData>();
    private bool _isInitialized = false;

    public static ItemDataManager GetInstance()
    {
        if(Instance == null)
        {
            Instance = new ItemDataManager();
            Instance.Initialize();
        }

        return Instance;
    }

    public async void Initialize()
    {
        if(_isInitialized) return;

        ItemDataList.Clear();

        // xml Parse
        ItemDataList = await XmlUtility.LoadXML($"GameData");

        _isInitialized = true;
    }

    public ItemData GetItemData(int index) => ItemDataList.Count > index ? ItemDataList[index] : null;
}
