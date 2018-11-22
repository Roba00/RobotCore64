using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSquidController : MonoBehaviour {

    public Vector3 rayVector;
    public float rayDistance;
    public RaycastHit2D squashHit;

    public float deathDelay;
    public GameObject player;

    void Start () {
        
    }

	void Update () {
        rayVector.x = transform.position.x + 0.5f; 
        rayVector.y = transform.position.y + 0.6f;
        rayVector.z = transform.position.z;

        squashHit = Physics2D.Raycast(rayVector, Vector2.left, rayDistance);

        if (squashHit.collider != null)
        {
            player.GetComponent<PicoController>().enemyDeathDelay = true;
            Debug.Log("I've been hit!");
            StartCoroutine(Death());
        }
        Debug.DrawRay(rayVector, Vector2.left * rayDistance, Color.red);
    }

    IEnumerator Death()
    {
        player.GetComponent<PicoController>().enemyDeathDelay = true;
        yield return new WaitForSecondsRealtime(deathDelay);
        player.GetComponent<PicoController>().enemyDeathDelay = false;
        Destroy(gameObject);
    }
}
