using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.UI;


public class CareerItem:MonoBehaviour
{
    public RoleType roleType;
    public GameObject gamePanel;
    private ChooseCareerRequest request;

    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(OnClick);
        request = gamePanel.GetComponent<ChooseCareerRequest>();
    }

    private void OnClick()
    {
        request.SendRequest(roleType);
        gamePanel.GetComponent<GamePanel>().HideCareerPanel();
    }
   
}
