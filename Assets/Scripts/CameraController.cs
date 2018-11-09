using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool autoScroll;

	void Start () {
		
	}
	
	void Update () {
		if (autoScroll)
        {
            gameObject.transform.Translate(Vector3.right * 0.01f);
        }
        else
        {

        }
	}
}
