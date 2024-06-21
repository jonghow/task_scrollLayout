using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class ExtremeScrollItem : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_Text;
    public int index;
    public void SetData(int index)
    {
        this.index = index;

        StringBuilder sb = new StringBuilder();
        sb.Append(index);
        if (m_Text != null) m_Text.text = $"Rank.{sb.ToString()}";
    }
}
