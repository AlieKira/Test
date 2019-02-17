using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    private ScrollRect scrollRect;
    private float[] floats=new float[3]{0,0.5f,1};
    private bool isDrag = false;
    private float targetFloat;
    private float offset;
    private int index = 0;
    public float smoothSpeed = 5;
    public Toggle[] toggles;

    // Use this for initialization
    void Start ()
	{
	    scrollRect = GetComponent<ScrollRect>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!isDrag)
	    {
	        scrollRect.horizontalNormalizedPosition=Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetFloat,Time.deltaTime*smoothSpeed);
	        if (Mathf.Abs(targetFloat- scrollRect.horizontalNormalizedPosition)<0.01f)
	        {
	            scrollRect.horizontalNormalizedPosition = targetFloat;
                isDrag = true;
	        }
	    }
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        index = 0;
        offset = Mathf.Abs(scrollRect.horizontalNormalizedPosition - floats[index]);
        float tempOffset = 0;
        for (int i = 1; i < floats.Length; i++)
        {
            tempOffset = Mathf.Abs(scrollRect.horizontalNormalizedPosition - floats[i]);
            if (tempOffset < offset)
            {
                index = i;
                offset = tempOffset;
            }
        }
        targetFloat = floats[index];
        toggles[index].isOn = true;
    }

    public void OnToggle_0Click(bool isOn)
    {
        if (isOn)
        {
            scrollRect.horizontalNormalizedPosition = floats[0];
        }
    }
    public void OnToggle_1Click(bool isOn)
    {
        if (isOn)
        {
            scrollRect.horizontalNormalizedPosition = floats[1];
        }
    }
    public void OnToggle_2Click(bool isOn)
    {
        if (isOn)
        {
            scrollRect.horizontalNormalizedPosition = floats[2];
        }
    }

}
