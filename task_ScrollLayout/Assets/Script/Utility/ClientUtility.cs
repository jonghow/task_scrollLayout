using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public static partial class ClientUtility 
{
    public static void SetText(object obj, string contents)
    {
        if(obj is Text textComponent)
            textComponent.text = contents;

        if (obj is TextMeshPro textMeshPro)
            textMeshPro.text = contents;

        if (obj is TextMeshProUGUI textMeshProUGUI)
            textMeshProUGUI.text = contents;
    }

    public static void SetSprite(object obj, Sprite sprite)
    {
        if (obj is Image spriteComponent)
            spriteComponent.sprite = sprite;
    }
    public static void OnSetPosByScrollGroup(ref GameObject rGameObject)
    {
        if (rGameObject != null)
        {
            var localPosComponent = rGameObject.GetComponent<RectTransform>();

            if (localPosComponent == null) return;

            localPosComponent.anchorMin = new Vector2(0f, 1f);
            localPosComponent.anchorMax = new Vector2(0f, 1f);

            localPosComponent.pivot = new Vector3(0f, 1f);

            localPosComponent.localPosition = Vector3.zero;
        }
    }
    public static void OnResetLocalPos(ref GameObject rGameObject)
    {
        if (rGameObject != null)
        {
            var localPosComponent = rGameObject.GetComponent<RectTransform>();
            if (localPosComponent == null) return;

            localPosComponent.localPosition = Vector3.zero;
        }
    }
    public static void SetActive(ref GameObject rGameObject, bool isActive)
    {
        if (rGameObject != null) rGameObject.SetActive(isActive);
    }

    public static void SetActive(GameObject gameObject, bool isActive)
    {
        if (gameObject != null) gameObject.SetActive(isActive);
    }
}
