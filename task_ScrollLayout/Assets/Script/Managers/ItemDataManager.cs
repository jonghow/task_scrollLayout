using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDataManager 
{
    public static ItemDataManager Instance;

    public List<string> MailList = new List<string>();
    private bool _isInitialized = false;

    public static ItemDataManager GetInstance()
    {
        if(Instance == null)
        {
            Instance = new ItemDataManager();
            Instance.Initialize();
        }

        return Instance;
    }

    public void Initialize()
    {
        if(_isInitialized) return;

        MailList.Clear();

        for (int i = 0; i < 500; i++)
            MailList.Add($"{i * 100}");

        _isInitialized = true;
    }

    public string GetMail(int index) => MailList[index];
}
