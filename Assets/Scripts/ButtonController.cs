using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Sprite greenButton;
	public Sprite redButton;
	public GameObject levelOneInteractableGround;
	public bool hasBeenPressed;
	public AudioSource SoundEffectsSource;
	public AudioClip buttonPressSound;
	
	void Start () {
		spriteRenderer.sprite = redButton;
		hasBeenPressed = false;
	}

	void Update () {
		if (spriteRenderer.sprite == greenButton && !hasBeenPressed)
		{
			hasBeenPressed = true;
			SoundEffectsSource.PlayOneShot(buttonPressSound);
			StartCoroutine(moveLevelOneInteractableGround());
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == "Player")
		{
			spriteRenderer.sprite = greenButton;
		}
	}

	IEnumerator moveLevelOneInteractableGround()
	{
		for (int i = 0; i < 400; i++)
		{
			levelOneInteractableGround.transform.Translate(0, 0.01f, 0);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSecondsRealtime(1);
		for (int i = 0; i < 400; i++)
		{
			levelOneInteractableGround.transform.Translate(0, -0.01f, 0);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSecondsRealtime(1);
	}
}
