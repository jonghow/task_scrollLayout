using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.UI;
using static UnityEditor.Progress;

[RequireComponent(typeof(ScrollRect))]
public class ExtremeScrollView : MonoBehaviour
{
    [Tooltip("Horizontal : true , Vertical : false")]
    [SerializeField]
    bool IsHorizontal = true;
    int InitItemCount;
    const int bufferCount = 3; // 갯수 여유분 3개 ( 화면에 조그맣게라도 보일 컨텐츠 1개 + 위아래 여유분 2개 )

    // Prefab Data
    public ExtremeScrollItem m_ListItem;
    [SerializeField]  float itemHeight;
    List<int> dataList = new List<int>();

    [SerializeField]
    ScrollRect scrollRect;
    private List<GameObject> itemList;

    float offset;

    private void Awake()
    {
        Init();

    }

    private void Init()
    {
        scrollRect = GetComponent<ScrollRect>();
        dataList.Clear();

        for(int i = 0; i < 1000; ++i)
            dataList.Add(i);

        CreateItems();
        InitContentsLayoutSetting();
    }

    private void CreateItems()
    {
        var rectTransform = scrollRect.GetComponent<RectTransform>();
        itemList = new List<GameObject>();

        int itemCount = (int)(rectTransform.rect.height / itemHeight) + bufferCount;

        for(int i = 0;i < itemCount; ++i)
        {
            GameObject item = Instantiate(m_ListItem.gameObject, scrollRect.content);

            var ItemComponent = item.GetComponent<ExtremeScrollItem>();
            itemList.Add(item);

            item.transform.localPosition = new Vector3(0f, -i * itemHeight);
            SetData(ItemComponent, i);
        }

        offset = itemList.Count * itemHeight;
    }

    public void InitContentsLayoutSetting()
    {
        if(scrollRect != null)
        {
            Vector2 newV = new Vector2(scrollRect.content.sizeDelta.x, (dataList.Count * itemHeight));
            scrollRect.content.sizeDelta = newV;
        }
    }

    public bool UpdateItemPostion(ExtremeScrollItem item, float contentsY, float scrollHeight)
    {
        if(item.transform.localPosition.y + contentsY > (itemHeight*2))
        {
            item.transform.localPosition -= new Vector3(0, itemList.Count * itemHeight);
            UpdateItemPostion(item, contentsY, scrollHeight);
            return true;
        }
        else if (item.transform.localPosition.y + contentsY < -scrollHeight - (itemHeight))
        {
            item.transform.localPosition += new Vector3(0, itemList.Count * itemHeight);
            UpdateItemPostion(item, contentsY, scrollHeight);
            return true;
        }

        return false;
    }

    private void SetData(ExtremeScrollItem item, int idx)
    {
        if (idx < 0 || idx >= dataList.Count)
        {
            item.gameObject.SetActive(false);
            return;
        }

        item.gameObject.SetActive(true);
        item.SetData(dataList[idx]);
    }

    void Update()
    {
        float contentsY = scrollRect.content.anchoredPosition.y;
        float scrollHeight = scrollRect.GetComponent<RectTransform>().rect.height;

        foreach(GameObject obj in itemList)
        {
            var item = obj.GetComponent<ExtremeScrollItem>();

            bool isChanged = UpdateItemPostion(item, contentsY, scrollHeight);
            if (isChanged)
            {
                int idx = (int)(-item.transform.localPosition.y / itemHeight);
                SetData(item, idx);
            }
        }
    }
}
