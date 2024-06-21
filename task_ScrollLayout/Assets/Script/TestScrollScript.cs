using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TestScrollScript : MonoBehaviour
{
    [SerializeField]
    public GameObject orinalScrollItem;

    public Transform m_Parent;

    private void Start()
    {
        for (int i = 0; i < 500; i++)
        {
            Instantiate(orinalScrollItem, m_Parent.transform);
        }
    }



}
