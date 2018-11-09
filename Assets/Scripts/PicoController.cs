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
    public int candyCollected = 0;
    public Text livesText;
    public int livesCollected = 1;
    public bool fullSize = true;

    void Start () {
        fullSize = true;
	}

	void Update () {
        
        candyText.text = candyCollected.ToString();
        livesText.text = livesCollected.ToString();

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

        if (Input.GetKeyDown("up") && isGrounded)
        {
            isGrounded = false;
            playerRb.AddForce(new Vector2(0,jumpPower));
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
            StartCoroutine(Shrink());
        }

        if(Input.GetKeyDown("space") && !fullSize)
        {
            StartCoroutine(Enlarge());
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        if (collision.collider.tag == "Candy")
        {
            candyCollected += 1;
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Giftbox")
        {
            candyCollected += 10;
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Chest")
        {
            livesCollected += 1;
            Destroy(collision.collider.gameObject);
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
}
