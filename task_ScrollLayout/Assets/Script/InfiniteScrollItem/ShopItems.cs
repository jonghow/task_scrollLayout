using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class ShopItems : InfiniteItemBase
{
    [SerializeField] TextMeshProUGUI _textMailIndex;
    [SerializeField] Image _imgItemPortrait;
    [SerializeField] Image _imgUsedGoodsIndicator;

    public override float GetItemHeight() => this.GetComponent<RectTransform>().rect.height;

    public override float GetItemWidth() => this.GetComponent<RectTransform>().rect.width;

    public override void SetData(object data)
    {
        var parseItemData = data as ItemData;
        if (parseItemData == null) return;

        ClientUtility.SetText(_textMailIndex, parseItemData.Gold.ToString());

        // Wait
        var resource = (Sprite)ResourceManager.GetInstance().GetResourceCache(ClientGlobal.LoadedResourceCategory.Sprite, parseItemData.Item);
        ClientUtility.SetSprite(_imgItemPortrait, resource);

        resource = (Sprite)ResourceManager.GetInstance().GetResourceCache(ClientGlobal.LoadedResourceCategory.Sprite, parseItemData.UseGoods);
        ClientUtility.SetSprite(_imgUsedGoodsIndicator, resource);
    }
}
