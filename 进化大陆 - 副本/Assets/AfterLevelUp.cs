using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AfterLevelUp : MonoBehaviour
{

    public Text roleLv;
    public RoleData roleData;


	public void LevelUp ()
	{
        this.gameObject.SetActive(true);
        roleLv = GameObject.Find("LV").GetComponent<Text>();
        roleLv.text = roleData.Lv.ToString();
	    this.transform.DOLocalMoveY(10f, 1f);
	    GetComponent<Text>().DOFade(0, 2f).OnComplete(()=>this.gameObject.SetActive(false));
	}
	
}
