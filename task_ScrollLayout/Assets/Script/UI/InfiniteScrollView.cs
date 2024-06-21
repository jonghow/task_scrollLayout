using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using UnityEditor;

[RequireComponent(typeof(ScrollRect))]
public class InfiniteScrollView : MonoBehaviour
{
    [SerializeField] InfiniteItemBase _Item;

    private Action _onUpdateCallback;
    private const int _createBufferCount = 3; // Notice : 스크롤 뷰에 사용되는 버퍼 아이템 값 
    private int _createItemCount; // Notice : 생성되는 갯수
    private float offset;
    private int _dataCount; 

    [Tooltip("Base Item Height")] [ReadOnly(false)] private float _itemHeight;

    [SerializeField] private ScrollRect _scrollRect;
    [ReadOnly(true)][SerializeField] private List<GameObject> itemList = new List<GameObject>();

    private void Awake() 
    {
        Init(); 
    }

    private void Init()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _dataCount = MailManager.GetInstance().MailList.Count;

        CreateItems();
        InitContentsLayoutSetting();
    }

    private void CreateItems()
    {
        var rectTransform = _scrollRect.GetComponent<RectTransform>();
        itemList.Clear();

        _itemHeight = _Item == null ? 0 : _Item.GetItemHeight(); // Notice : Item Height

        int ItemCount;
        ItemCount = _Item == null ? 0 : (int)(rectTransform.rect.height / _itemHeight) + _createBufferCount;

        for (int i = 0; i < ItemCount; ++i)
        {
            GameObject Item = Instantiate(_Item.gameObject, _scrollRect.content);

            var ItemComponent = Item.GetComponent<InfiniteItemBase>();
            itemList.Add(Item);

            Item.transform.localPosition = new Vector3(0f, - i * _itemHeight);
            SetData(ItemComponent, i);
        }

        offset = itemList.Count * _itemHeight;
    }

    public void InitContentsLayoutSetting()
    {
        if (_scrollRect != null)
        {
            Vector2 newV = new Vector2(_scrollRect.content.sizeDelta.x, (_dataCount * _itemHeight));
            _scrollRect.content.sizeDelta = newV;
        }
    }

    public bool UpdateItemPostion(InfiniteItemBase item, float contentsY, float scrollHeight)
    {
        if (item.transform.localPosition.y + contentsY > (_itemHeight * 2))
        {
            item.transform.localPosition -= new Vector3(0, itemList.Count * _itemHeight);
            UpdateItemPostion(item, contentsY, scrollHeight);
            return true;
        }
        else if (item.transform.localPosition.y + contentsY < -scrollHeight - (_itemHeight))
        {
            item.transform.localPosition += new Vector3(0, itemList.Count * _itemHeight);
            UpdateItemPostion(item, contentsY, scrollHeight);
            return true;
        }

        return false;
    }

    private void SetData(InfiniteItemBase item, int idx)
    {
        if (idx < 0 || idx >= _dataCount)
        {
            item.gameObject.SetActive(false);
            return;
        }

        item.gameObject.SetActive(true);
        item.SetData(MailManager.GetInstance().GetMail(idx));
    }

    void Update()
    {
        float contentsY = _scrollRect.content.anchoredPosition.y;
        float scrollHeight = _scrollRect.GetComponent<RectTransform>().rect.height;

        foreach (GameObject obj in itemList)
        {
            var item = obj.GetComponent<InfiniteItemBase>();

            bool isChanged = UpdateItemPostion(item, contentsY, scrollHeight);
            if (isChanged)
            {
                int idx = (int)(-item.transform.localPosition.y / _itemHeight);
                SetData(item, idx);
            }
        }
    }
}
