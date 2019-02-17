using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyToggle : MonoBehaviour
{
    public GameObject switchOn;
    public GameObject switchOff;
    // Use this for initialization
    void Start ()
	{
        ChangeValue(GetComponent<Toggle>().isOn);
	}

    public void ChangeValue(bool isOn)
    {
        Debug.Log(isOn);
        switchOff.SetActive(isOn);
        switchOn.SetActive(!isOn);
    }
}
