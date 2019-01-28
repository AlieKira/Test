using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {


	// Update is called once per frame
	void Update ()
	{
	    if (Camera.main==null)
	    {
	        return;
	    }
	    transform.rotation = Camera.main.transform.rotation;
	}
}
