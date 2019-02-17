using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class RecordItem:MonoBehaviour
{
    public Text level;

    public Text time;

    private void Awake()
    {
        level = transform.Find("Level").GetComponent<Text>();
        time = transform.Find("Time").GetComponent<Text>();
    }
}
