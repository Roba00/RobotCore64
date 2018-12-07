using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicoController : MonoBehaviour {

    public Transform playerTf;
    public Rigidbody2D playerRb;
    public SpriteRenderer playerSprite;
    public Sprite playerIdle;
    public Sprite playerWalking;
    public Sprite playerJumping;
    public Vector2 movementVect;

    float horizontalValue;
    float verticalValue;
    public int speed;
    public int jumpPower;
    public bool isGrounded;
    public bool isWalking;

    public Text candyText;
    public int candyCollected;
    public Text livesText;
    public int numberOfLives;
    public bool fullSize = true;
    public bool onTopOfEnemy = false;
    public bool isDead;

    public AudioSource MusicSource;
    public AudioSource SoundEffectsSource;
    public AudioClip jumpSound;
    public AudioClip collectCoinSound;
    public AudioClip collectGiftboxSound;
    public AudioClip collectTreasureSound;
    public AudioClip miniturizeSound;
    public AudioClip enlargeSound;
    public AudioClip killEnemySound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public Camera mainCamera;
    

    void Start () {
        fullSize = true;
        onTopOfEnemy = false;
        candyCollected = 0;
        numberOfLives = 3;
        isDead = false;
	}

	void Update () {
        
        candyText.text = candyCollected.ToString();
        livesText.text = numberOfLives.ToString();

        horizontalValue = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        playerTf.Translate(horizontalValue, 0, 0);
        if (horizontalValue != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (Input.GetKeyDown("left"))
        {
            playerSprite.flipX = true;
        }
        if (Input.GetKeyDown("right"))
        {
            playerSprite.flipX = false;
        }
        
        if (Input.GetKeyDown("up") && isGrounded)
        {
            isGrounded = false;
            playerRb.AddForce(new Vector2(0,jumpPower));
            SoundEffectsSource.PlayOneShot(jumpSound);
        }

        if (isGrounded && !isWalking)
        {
            playerSprite.sprite = playerIdle;
        }
        else if (isGrounded && isWalking)
        {
            playerSprite.sprite = playerWalking;
        }
        else
        {
            playerSprite.sprite = playerJumping;
        }

        if(Input.GetKeyDown("space") && fullSize)
        {
            SoundEffectsSource.PlayOneShot(miniturizeSound);
            StartCoroutine(Shrink());
        }

        if(Input.GetKeyDown("space") && !fullSize)
        {
            SoundEffectsSource.PlayOneShot(enlargeSound);
            StartCoroutine(Enlarge());
        }

        if (gameObject.transform.position.x < mainCamera.transform.position.x - 10 
        || gameObject.transform.position.x > mainCamera.transform.position.x + 10
        || gameObject.transform.position.y < mainCamera.transform.position.y - 7
        || gameObject.transform.position.y > mainCamera.transform.position.y + 10)
        {
            numberOfLives = 0;
        }

        if (numberOfLives == 0 && !isDead)
        {
            SoundEffectsSource.PlayOneShot(deathSound);
            mainCamera.GetComponent<CameraController>().notDead = false;
            Destroy(MusicSource);
            StartCoroutine(Death());
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        if (collision.collider.tag == "Candy")
        {
            candyCollected += 1;
            Destroy(collision.collider.gameObject);
            SoundEffectsSource.PlayOneShot(collectCoinSound);
        }

        if (collision.collider.tag == "Giftbox")
        {
            candyCollected += 10;
            Destroy(collision.collider.gameObject);
            SoundEffectsSource.PlayOneShot(collectGiftboxSound);
        }

        if (collision.collider.tag == "Chest")
        {
            numberOfLives += 1;
            Destroy(collision.collider.gameObject);
            SoundEffectsSource.PlayOneShot(collectTreasureSound);
        }

        if (collision.collider.tag == "Enemy" && !onTopOfEnemy)
        {
            numberOfLives -= 1;
            SoundEffectsSource.PlayOneShot(damageSound);
        }
        else if (collision.collider.tag == "Enemy" && onTopOfEnemy)
        {
            candyCollected += 10;
            SoundEffectsSource.PlayOneShot(killEnemySound);
        }
        
    }

    IEnumerator Shrink()
    {
        Debug.Log("Shrinking");
        transform.localScale = new Vector3(1.5f, 1.375f, 1);
        yield return new WaitForSecondsRealtime(0.0125f);
        fullSize = false;
    }

    IEnumerator Enlarge()
    {
        Debug.Log("Enlarging");
        transform.Translate(0, 1, 0);
        transform.localScale = new Vector3(3, 2.75f, 1);
        yield return new WaitForSecondsRealtime(0.0125f);
        fullSize = true;
    }

    IEnumerator Death()
    {
        SoundEffectsSource.PlayOneShot(deathSound);
        yield return new WaitForSeconds(1);
        isDead = true;
        gameObject.SetActive(false);
    }
}
