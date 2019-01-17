using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    private Transform canvasTransform;
    public Slider hpSlider;
    public Vector3 pos;
    //void Start () {
	   // Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + 1,
	   //     this.transform.position.z));

    //}

    private void Start()
    {
        canvasTransform = GameObject.Find("Canvas/Sliders").GetComponent<Transform>();
        GameObject hpSlider = Instantiate(Resources.Load<GameObject>("HPSlider"));
        hpSlider.transform.SetParent(canvasTransform,false);
        hpSlider.GetComponent<HPFollow>().followTarget = this.gameObject;
        //hpSlider.transform.GetComponent<RoleHP>().hpSlider = hpSlider.transform.GetComponent<Slider>();
    }

}
