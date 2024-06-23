using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class InfiniteItemBase : MonoBehaviour
{
    public abstract void SetData(string contents);
    public abstract float GetItemHeight();
    public abstract float GetItemWidth();
}




