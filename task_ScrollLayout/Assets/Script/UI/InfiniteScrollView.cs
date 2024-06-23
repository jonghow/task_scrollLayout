using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using UnityEditor;
using Unity.VisualScripting;
using static UnityEditor.Progress;

[RequireComponent(typeof(ScrollRect))]

public class InfiniteScrollView : MonoBehaviour
{
    [RequireComponentInChildren(typeof(InfiniteScrollItemGroup))]
    [SerializeField] InfiniteItemBase _Item;

    private Action _onUpdateCallback;
    private const int _createBufferCount = 3; // Notice : 스크롤 뷰에 사용되는 버퍼 아이템 값 
    private int _createItemCount; // Notice : 생성되는 갯수
    private float offset;
    private int _dataCount;
    private int _itemRowCount;

    [Tooltip("Base Item Height")] [ReadOnly(false)] private float _itemHeight;
    [Tooltip("Base Item Width")][ReadOnly(false)] private float _itemWidth;

    [SerializeField] private ScrollRect _scrollRect;
    private RectTransform _scrollRectTransform;
    [ReadOnly(true)][SerializeField] private List<GameObject> itemList = new List<GameObject>(); // Group으로 관리

    [SerializeField] private GameObject _groupObject;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRectTransform = GetComponent<RectTransform>();
        _dataCount = ItemDataManager.GetInstance().MailList.Count;

        CreateItems();
        InitContentsLayoutSetting();
    }

    private void CreateItems()
    {
        var rectTransform = _scrollRect.GetComponent<RectTransform>();
        itemList.Clear();

        _itemHeight = _Item == null ? 0 : _Item.GetItemHeight(); // Notice : Item Height
        _itemWidth = _Item == null ? 0 : _Item.GetItemWidth(); // Notice : Item Width

        int ItemCount = _Item == null ? 0 : (int)(rectTransform.rect.height / _itemHeight) + _createBufferCount;
        _itemRowCount = _Item == null ? 0 : (int)(rectTransform.rect.width / _itemWidth);

        for (int i = 0; i < ItemCount; ++i)
        {
            GameObject groupObject = GameObject.Instantiate(_groupObject);
            groupObject.transform.SetParent(_scrollRect.content.transform);
            groupObject.transform.localPosition = new Vector3(0f, -i * _itemHeight);
            ClientUtility.SetActive(ref groupObject, true);

            var groupObjectRectSize = groupObject.transform.GetComponent<RectTransform>().sizeDelta;
            groupObjectRectSize = new Vector2(_scrollRectTransform.sizeDelta.x, _itemHeight);
            groupObject.transform.GetComponent<RectTransform>().sizeDelta = groupObjectRectSize; // Notice : GroupSize Conversion

            itemList.Add(groupObject);

            for (int j = 0; j < _itemRowCount; ++j)
            {
                GameObject Item = Instantiate(_Item.gameObject, _scrollRect.content);
                Item.transform.SetParent(groupObject.transform);

                var ItemComponent = Item.GetComponent<InfiniteItemBase>();

                Item.transform.localPosition = new Vector3(j * _itemWidth, 0);
                SetData(ItemComponent, i * _itemRowCount + j); // 
            }
        }

        offset = itemList.Count * _itemHeight;
    }

    public void InitContentsLayoutSetting()
    {
        if (_scrollRect != null)
        {
            Vector2 newV = new Vector2(_scrollRect.content.sizeDelta.x, (MathF.Ceiling((float)_dataCount / (float)_itemRowCount)) * _itemHeight);
            _scrollRect.content.sizeDelta = newV;
        }
    }

    public bool UpdateItemPostion(GameObject groupObject, float contentsY, float scrollHeight)
    {
        if (groupObject.transform.localPosition.y + contentsY > (_itemHeight * 2))
        {
            int idx = (int)(groupObject.transform.localPosition.x / _itemWidth);

            groupObject.transform.localPosition -= new Vector3(idx * _itemWidth, itemList.Count * _itemHeight);
            UpdateItemPostion(groupObject, contentsY, scrollHeight);
            return true;
        }
        else if (groupObject.transform.localPosition.y + contentsY < -scrollHeight - (_itemHeight))
        {
            int idx = (int)(groupObject.transform.localPosition.x / _itemWidth);

            groupObject.transform.localPosition += new Vector3(idx * _itemWidth, itemList.Count * _itemHeight);
            UpdateItemPostion(groupObject, contentsY, scrollHeight);
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
        item.SetData(ItemDataManager.GetInstance().GetMail(idx));
    }

    void Update()
    {
        float contentsY = _scrollRect.content.anchoredPosition.y;
        float scrollHeight = _scrollRect.GetComponent<RectTransform>().rect.height;

        foreach (GameObject obj in itemList)
        {
            //var item = obj.GetComponent<InfiniteItemBase>();

            bool isChanged = UpdateItemPostion(obj, contentsY, scrollHeight);
            if (isChanged)
            {
                for(int i = 0; i < obj.transform.childCount; ++i)
                {
                    var child = obj.transform.GetChild(i);
                    if (child == null) continue;
                    var item = child.GetComponent<InfiniteItemBase>();

                    int idx = (int)(-obj.transform.localPosition.y / (_itemHeight )); // 이게 그 그룹의 n번째
                    idx = (idx * _itemRowCount) + i;

                    SetData(item, idx);
                }
            }
        }
    }
}
