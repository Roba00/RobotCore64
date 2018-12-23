using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject greenSquidClone;
    public GameObject sludge;
    bool hasAttacked;
    public bool isDead = false;
    public AudioSource musicSource;
    public AudioSource soundEffectsSource;
    public AudioClip winSound;


    void Start () {
        miniBossTitle.SetActive(false);
        miniBossHealthTextObject.SetActive(false);
        DialougeBackground.SetActive(false);
        DialougeTextObject.SetActive(false);
        hasStartedTalking = false;
        hasAttacked = false;

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

        if (squashHit.collider == player.GetComponent<Collider2D>() && player.GetComponent<PicoController>().onTopOfEnemy == false)
        {
            player.GetComponent<PicoController>().onTopOfEnemy = true;
            player.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 250);
            Debug.Log("I've been hit!");
            StartCoroutine(SubtractLife());
        }

        if (numberOfLives <= 50 && !hasAttacked)
        {
            StopCoroutine(BossRun());
            StartCoroutine(BossAttack());
            hasAttacked = true;
        }

        if (numberOfLives <= 0 && !isDead)
        {
            Destroy(musicSource);
            soundEffectsSource.PlayOneShot(winSound);
            StartCoroutine(NextLevel());
            isDead = true;
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

    IEnumerator NextLevel()
    {
        spriteRenderer.enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        player.GetComponent<PicoController>().speed = 0;
        player.GetComponent<PicoController>().fullSize = true;
        StartCoroutine(player.GetComponent<PicoController>().Enlarge());
        player.GetComponent<SpriteRenderer>().flipX = false;
        player.GetComponent<PicoController>().allowedToMove = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PicoController>().playerSprite.sprite = player.GetComponent<PicoController>().playerCheerUp;
        yield return new WaitForSecondsRealtime(1);
        player.GetComponent<PicoController>().playerSprite.sprite = player.GetComponent<PicoController>().playerCheerDown;
        yield return new WaitForSecondsRealtime(1);
        player.GetComponent<PicoController>().playerSprite.sprite = player.GetComponent<PicoController>().playerCheerUp;
        yield return new WaitForSecondsRealtime(1);
        player.GetComponent<PicoController>().playerSprite.sprite = player.GetComponent<PicoController>().playerCheerDown;
        yield return new WaitForSecondsRealtime(1);
        player.GetComponent<PicoController>().playerSprite.sprite = player.GetComponent<PicoController>().playerCheerUp;
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(2);
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
        while (!hasAttacked)
        {
            //for (int i = 0; i < 2000; i++)
            //{
                gameObject.transform.Translate(MainCamera.GetComponent<CameraController>().cameraSpeed, 0, 0);
                yield return new WaitForEndOfFrame();
            //}
        }       
        /* else
        {
            gameObject.transform.Translate(new Vector3(0,0,0));
        }*/
    }

    IEnumerator BossAttack()
    {  
        StopCoroutine(StartTalking());
        StopCoroutine(BossRun());
        gameObject.transform.position = new Vector3(125.62f, -1.11f, 0);
        spriteRenderer.flipX = false;
        player.transform.position = new Vector3(115.04f, -2.28f, 0);
        MainCamera.transform.position = new Vector3(119.15f, 1, -10f);
        MainCamera.GetComponent<CameraController>().isFrozen = true;

        gameObject.transform.Translate(new Vector3(0, 0, 0));

        yield return new WaitForSecondsRealtime(0.5f);

        DialougeText.text = "";
        DialougeBackground.SetActive(true);
        DialougeTextObject.SetActive(true);
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

        for (int i = 0; i < 100; i++)
        {
            attackNumber = Random.Range(1, 4);
            
            if (attackNumber == 1) //Slam Attack
            {
                for (int l = 0; l < 50; l++)
                {
                    gameObject.transform.Translate(0, 0.1f, 0);
                    yield return new WaitForSecondsRealtime(0.01f);
                }
                int moveDistance = Random.Range (30, 125);
                for (int l = 0; l < moveDistance; l++)
                {
                    gameObject.transform.Translate(-0.1f, 0, 0);
                    yield return new WaitForSecondsRealtime(0.01f);
                }
                for (int l = 0; l < 70; l++)
                {
                    gameObject.transform.Translate(0, -0.1f, 0);
                    yield return new WaitForSecondsRealtime(0.01f);
                }
                for (int l = 0; l < moveDistance; l++)
                {
                    gameObject.transform.Translate(0.1f, 0, 0);
                    yield return new WaitForSecondsRealtime(0.01f);
                }
                for (int l = 0; l < 20; l++)
                {
                    gameObject.transform.Translate(0, 0.1f, 0);
                    yield return new WaitForSecondsRealtime(0.01f);
                }
            }

            if (attackNumber == 2) //Spray Ink
            {
                GameObject sludge1 = Instantiate(sludge, new Vector3(Random.Range(110.8246f, 115.69f), 2.5f, 0f), Quaternion.identity);
                yield return new WaitForSecondsRealtime(0.5f);
                GameObject sludge2 = Instantiate(sludge, new Vector3(Random.Range(117.78f, 121.34f), 2.5f, 0f), Quaternion.identity);
                yield return new WaitForSecondsRealtime(0.5f);
                GameObject sludge3 = Instantiate(sludge, new Vector3(Random.Range(123.92f, 127.6f), 2.5f, 0f), Quaternion.identity);
                yield return new WaitForSecondsRealtime(3);
                Destroy(sludge1);
                Destroy(sludge2);
                Destroy(sludge3);
            }

            else if (attackNumber == 3) //Spawn Minions
            {
                GameObject minion1 = Instantiate(greenSquidClone, new Vector3(112.5f, 2.5f, 0f), Quaternion.identity);
                minion1.GetComponent<GreenSquidController>().player = GameObject.Find("/Pico");
                yield return new WaitForSecondsRealtime(0.5f);
                GameObject minion2 = Instantiate(greenSquidClone, new Vector3(119, 2.5f, 0f), Quaternion.identity);
                minion2.GetComponent<GreenSquidController>().player = GameObject.Find("/Pico");
                yield return new WaitForSecondsRealtime(0.5f);
                GameObject minion3 = Instantiate(greenSquidClone, new Vector3(125, 2.5f, 0f), Quaternion.identity);
                minion3.GetComponent<GreenSquidController>().player = GameObject.Find("/Pico");
                yield return new WaitForSecondsRealtime(3);
            }

            yield return new WaitForSecondsRealtime(1);
        }
    }
}