using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool autoScroll;
    public bool isFrozen = false;
    public float cameraSpeed;
    

	void Start () {
		
	}
	
	void Update () {
        if (!isFrozen)
        {
		    if (autoScroll)
            {
                gameObject.transform.Translate(Vector3.right * cameraSpeed);
            }
            else
            {

            }
        }
	}
}
