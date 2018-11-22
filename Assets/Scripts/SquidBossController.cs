using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidBossController : MonoBehaviour {

    Vector3 squashHitRayVector;
    public float squashHitRayDistance;
    RaycastHit2D squashHit;
    Vector3 sensePlayerRayVector;
    public float sensePlayerRayDistance;
    RaycastHit sensePlayer;
    public int numberOfHits = 3;

    public float subtractDelay;

    void Start () {
        
    }

	void Update () {
        squashHitRayVector.x = transform.position.x + 3f; 
        squashHitRayVector.y = transform.position.y + 2.5f;
        squashHitRayVector.z = transform.position.z;

        squashHit = Physics2D.Raycast(squashHitRayVector, Vector2.left, squashHitRayDistance);

        if (squashHit.collider != null)
        {
            Debug.Log("I've been hit!");
            StartCoroutine(SubtractLife());
        }
        Debug.DrawRay(squashHitRayVector, Vector2.left * squashHitRayDistance, Color.red);

        if (numberOfHits <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SubtractLife()
    {
        yield return new WaitForSecondsRealtime(subtractDelay);
        numberOfHits -= 1;
    }
}
