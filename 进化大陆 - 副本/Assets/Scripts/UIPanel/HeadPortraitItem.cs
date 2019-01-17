using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


public class HeadPortraitItem:MonoBehaviour, IPointerDownHandler
{
    private string headPortraitPath;

    public string HeadPortraitPath
    {
        get { return headPortraitPath; }
    }
    public HeadPortraitPanel headPortraitPanel;
    public void OnPointerDown(PointerEventData eventData)
    {
        headPortraitPanel.SetCurrtenItem(this);
    }
    public void SetPath(string path)
    {
        this.headPortraitPath = path;
    }

    
}

