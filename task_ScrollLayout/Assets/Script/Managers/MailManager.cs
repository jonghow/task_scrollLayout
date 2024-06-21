using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MailManager 
{
    public static MailManager Instance;

    public List<string> MailList = new List<string>();

    public static MailManager GetInstance()
    {
        if(Instance == null)
        {
            Instance = new MailManager();
            Instance.Initialize();
        }

        return Instance;
    }

    public void Initialize()
    {
        MailList.Clear();

        for (int i = 0; i < 100; i++)
        {
            MailList.Add($"{i}번째 보상 우편");
        }
    }

    public string GetMail(int index) => MailList[index];
}
