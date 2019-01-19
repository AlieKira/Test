using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPFollow : MonoBehaviour
{
    private float m_ObjectHeight;
    public GameObject followTarget;
    private RectTransform m_SliderRectTransform;
	
	void Start () {
	    m_SliderRectTransform=GetComponent<Slider>().transform as RectTransform;
	    m_ObjectHeight = followTarget.GetComponent<CapsuleCollider>().height;
	    Debug.Log("object height." + m_ObjectHeight);
	    //m_Slider = gameObject.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
	    Vector3 worldPosition = new Vector3(followTarget.transform.position.x,
	        followTarget.transform.position.y + m_ObjectHeight, followTarget.transform.position.z);
	    // 根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
	    Vector2 position = Camera.main.WorldToScreenPoint(worldPosition);
	    m_SliderRectTransform.position = position;

	}
}
