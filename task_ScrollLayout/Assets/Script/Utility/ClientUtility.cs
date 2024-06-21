using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

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
}
