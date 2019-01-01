using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool autoScroll;
    public bool isFrozen = false;
    public float cameraSpeed;
    public GameObject player;
    Vector3 playerVector;
    Quaternion quat;
    

	void Start () {
		
	}
	
	void Update () {
        playerVector = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        
        if (!isFrozen)
        {
		    if (autoScroll)
            {
                gameObject.transform.Translate(Vector3.right * cameraSpeed);
            }
            if (!autoScroll)
            {
                gameObject.transform.SetPositionAndRotation(playerVector, quat);
            }
        }
	}
}
