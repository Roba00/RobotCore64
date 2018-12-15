using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquidBossController : MonoBehaviour {

    Vector3 squashHitRayVector;
    public float squashHitRayDistance;
    RaycastHit2D squashHit;
    Vector3 sensePlayerRayVector;
    public float sensePlayerRayDistance;
    RaycastHit2D sensePlayer;
    public int numberOfLives = 100;
    public float subtractDelay;
    public GameObject DialougeBackground;
    public Text DialougeText;
    public GameObject DialougeTextObject;
    public bool hasStartedTalking;
    public Camera MainCamera;
    public SpriteRenderer spriteRenderer;
    public GameObject groundIgnore1;
    public GameObject groundIgnore2;
    public GameObject groundIgnore3;
    public PolygonCollider2D bossCollider;
    public GameObject miniBossTitle;
    public GameObject miniBossHealthTextObject;
    public Text miniBossHealthText;
    public GameObject player;
    int attackNumber;


    void Start () {
        miniBossTitle.SetActive(false);
        miniBossHealthTextObject.SetActive(false);
        DialougeBackground.SetActive(false);
        DialougeTextObject.SetActive(false);
        hasStartedTalking = false;

        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(9, 8, true);
    }

	void Update () {
        miniBossHealthText.text = numberOfLives.ToString() + "%";

        squashHitRayVector.x = transform.position.x + 2.3f; 
        squashHitRayVector.y = transform.position.y + 2.4f;
        squashHitRayVector.z = transform.position.z;

        sensePlayerRayVector.x = transform.position.x - 1f;
        sensePlayerRayVector.y = transform.position.y - 2.4f;
        sensePlayerRayVector.z = transform.position.z;


        squashHit = Physics2D.Raycast(squashHitRayVector, Vector2.left, squashHitRayDistance);
        sensePlayer = Physics2D.Raycast(sensePlayerRayVector, Vector2.left, sensePlayerRayDistance);

        Debug.DrawRay(squashHitRayVector, Vector2.left * squashHitRayDistance, Color.green);
        Debug.DrawRay(sensePlayerRayVector, Vector2.left * sensePlayerRayDistance, Color.red);

        if (sensePlayer.collider != null && !hasStartedTalking && MainCamera.transform.position.x >= 41)
        {
            hasStartedTalking = true;
            MainCamera.GetComponent<CameraController>().isFrozen = true;
            Debug.Log("Sensed player!");
            DialougeBackground.SetActive(true);
            DialougeTextObject.SetActive(true);
            StartCoroutine(StartTalking());
        }

         if (squashHit.collider != null && player.GetComponent<PicoController>().onTopOfEnemy == false)
        {
            player.GetComponent<PicoController>().onTopOfEnemy = true;
            Debug.Log("I've been hit!");
            StartCoroutine(SubtractLife());
        }

        if (numberOfLives <= 50)
        {
            StopCoroutine(BossRun());
            StartCoroutine(BossAttack());
        }

        if (numberOfLives <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*Physics2D.IgnoreCollision(groundIgnore1.GetComponent<BoxCollider2D>(), gameObject.GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(groundIgnore2.GetComponent<BoxCollider2D>(), gameObject.GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(groundIgnore3.GetComponent<BoxCollider2D>(), gameObject.GetComponent<PolygonCollider2D>());*/

        if (collision.gameObject.layer == 9)
        {
            Physics2D.IgnoreCollision(collision.collider, bossCollider);
        }
    }

    IEnumerator SubtractLife()
    {
        player.GetComponent<PicoController>().onTopOfEnemy = true;
        numberOfLives -= 10;
        yield return new WaitForSecondsRealtime(subtractDelay);
        player.GetComponent<PicoController>().onTopOfEnemy = false;
    }

    IEnumerator StartTalking()
    {
        Debug.Log("Started Talking!");
        
        DialougeText.text = "";
        
        string dialouge = "Squid Boss: So it's you... you're the robot that everyone has been talking about...";
        foreach (char letter in dialouge.ToCharArray())
        {
            DialougeText.text += letter;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        
        yield return new WaitForSecondsRealtime(3);
        DialougeText.text = "";

        dialouge =  "Pico Robot: And you must be the beast that the humans have ordered me to eliminate!";
        foreach (char letter in dialouge.ToCharArray())
        {
            DialougeText.text += letter;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        yield return new WaitForSecondsRealtime(3);
        DialougeText.text = "";

        dialouge =  "Squid Boss: I can't let you continue, or you'll end all of the innocent species on Earth! Come at me!";
        foreach (char letter in dialouge.ToCharArray())
        {
            DialougeText.text += letter;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    
        yield return new WaitForSecondsRealtime(3);
        DialougeText.text = "";

        DialougeBackground.SetActive(false);
        DialougeTextObject.SetActive(false);
        
        //Play epic boss music.

        StartCoroutine(BossRun());
    }

    IEnumerator BossRun()
    {
        miniBossTitle.SetActive(true);
        miniBossHealthTextObject.SetActive(true);
        
        MainCamera.GetComponent<CameraController>().isFrozen = false;
        spriteRenderer.flipX = true;
        for (int i = 0; i < 2000; i++)
        {
            gameObject.transform.Translate(MainCamera.GetComponent<CameraController>().cameraSpeed, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator BossAttack()
    {
        DialougeBackground.SetActive(true);
        DialougeTextObject.SetActive(true);
        DialougeText.text = "";
        string dialouge =  "Squid Boss: I have you now!!!";
        foreach (char letter in dialouge.ToCharArray())
        {
            DialougeText.text += letter;
            yield return new WaitForSecondsRealtime(0.01f);
        }   
        yield return new WaitForSecondsRealtime(3);
        DialougeText.text = "";
        DialougeBackground.SetActive(false);
        DialougeTextObject.SetActive(false);


        gameObject.transform.position = new Vector3(125.62f, -1.11f, 0);
        player.transform.position = new Vector3(111.04f, -2.28f, 0);
        MainCamera.transform.position = new Vector3(119.15f, 1, 0);

        for (int i = 0; i < 100; i++)
        {
            attackNumber = Random.Range(1, 4);
            
            if (attackNumber == 1) //Slam Attack
            {

            }

            else if (attackNumber == 2) //Spray Ink
            {

            }

            else if (attackNumber == 3) //Spawn Minions
            {

            }

            yield return new WaitForEndOfFrame();
        }
    }
}