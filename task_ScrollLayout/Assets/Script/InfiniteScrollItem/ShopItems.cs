using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItems : InfiniteItemBase
{
    [SerializeField] TextMeshProUGUI _textMailIndex;

    public override float GetItemHeight() => this.GetComponent<RectTransform>().rect.height;

    public override float GetItemWidth() => this.GetComponent<RectTransform>().rect.width;

    public override void SetData(string contents)
    {
        ClientUtility.SetText(_textMailIndex, contents);
    }
}
