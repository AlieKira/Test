using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToolTip :MonoBehaviour { 
    private Text toopTipText;
    private Text contentText;
    private CanvasGroup canvasGroup;
    private bool isShow=false;
    private Canvas canvas;
    private Vector2 offset;
    private Vector2 origionOffset= new Vector2(10, -10);

    private void Start()
    {
        toopTipText = GetComponent<Text>();
        contentText = transform.Find("Content").GetComponent<Text>();
        canvasGroup = transform.GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup.alpha = 0;
        Hide();
    }


    private void Update()
    {
        if (isShow)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);
            this.transform.localPosition = position + offset;
        }
    }

    public void Show(string data,Vector2 offset)
    {
        this.transform.SetAsLastSibling();
        isShow = true;
        canvasGroup.alpha = 1;
        toopTipText.text = data;
        contentText.text = data;
        if (offset==Vector2.zero)
        {
            this.offset = origionOffset;
        }
        else
        {
            this.offset = offset;
        }
    }

    //public void Show(string data)
    //{
    //    this.transform.SetAsLastSibling();
    //    isShow = true;
    //    canvasGroup.alpha = 1;
    //    toopTipText.text = data;
    //    contentText.text = data;
    //    this.offset = origionOffset;
    //}

    public void Hide()
    {
        if (canvasGroup==null)
        {
            canvasGroup = transform.GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        isShow = false;
    }
    
    
}
