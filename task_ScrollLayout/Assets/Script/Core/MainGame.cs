using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    void Start()
    {
        ResourceManager.GetInstance();
        ItemDataManager.GetInstance();
        UIWindowManager.GetInstance();
    }
}
