using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

	//This script must be attached to the Main Camera.

	public GameObject player;
	public Button playButton;
	public Button quitButton;
	public Button continueButton;
	public Text introText;
	public RawImage[] transparentBackgrounds;

	bool hasPlayedIntro = false;
	public AudioSource introTypingSound;

	Color bgColor;
	bool blooping = true;

	void Start () 
	{
		switch (SceneManager.GetActiveScene().name)
		{
			case "StartScreen":
				playButton.onClick.AddListener(Play);
				quitButton.onClick.AddListener(Quit);
				StartCoroutine(bgColorShifter());
				break;

			case "IntroScreen":
				continueButton.onClick.AddListener(Continue);
				break;

			case "WinScreen":
				playButton.onClick.AddListener(Play);
				quitButton.onClick.AddListener(Quit);
				StartCoroutine(bgColorShifter());
				break;

			case "GameOverScreen":
				playButton.onClick.AddListener(Play);
				quitButton.onClick.AddListener(Quit);
				StartCoroutine(bgColorShifter());
				break;

			case "Level1":
				break;
		}

		//The credit for this camera-aspect ratio script goes to Adrian Lopez!
		
		// set the desired aspect ratio (the values in this example are
    	// hard-coded for 16:9, but you could make them into public
    	// variables instead so you can set them at design time)
    	float targetaspect = 16.0f / 9.0f;
    	// determine the game window's current aspect ratio
		float windowaspect = (float)Screen.width / (float)Screen.height;
		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;
		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();
		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{  
			Rect rect = camera.rect;
			rect.width = 1.0f;
			rect.height = scaleheight;
			rect.x = 0;
			rect.y = (1.0f - scaleheight) / 2.0f;
			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;
			Rect rect = camera.rect;
			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;
			camera.rect = rect;
		}
	}
	

	void Update () 
	{
		switch (SceneManager.GetActiveScene().name)
		{
			case "StartScreen":
				break;

			case "IntroScreen":
				if (!hasPlayedIntro)
				{
					StartCoroutine(playDialougeText());
				}
				break;

			case "WinScreen":
				break;

			case "GameOverScreen":
				break;

			case "Level1":
				if (player.GetComponent<PicoController>().isDead == true)
				{
					StartCoroutine(LoadScene3());
				}
				break;
		}
	}

	void Play()
	{
		SceneManager.LoadScene(1); //Loads Intro Screen
	}

	void Quit()
	{
		Application.Quit(); //Quits Game
	}

	void Continue()
	{
		SceneManager.LoadScene(4); //Loads Continue Screen
	}

	IEnumerator LoadScene3()
	{
		yield return new WaitForSecondsRealtime(3);
		SceneManager.LoadScene(3);
	}

	IEnumerator playDialougeText()
	{
		hasPlayedIntro = true;
		introText.text = "";
        
        string dialouge = "June 11, 2390 \n After a progressive 1000 months, a team of engineers funded by the United Nations was able to create a superior life-form to surpass the race of Homo-Sapiens. However, the life-form, named Pico, has escaped the laboratory in which it was created, and set loose on its purpose to end all life-forms on Earth, starting with innocent creatures that have been exposed to a chemical radiation leak caused by humans.";
        foreach (char letter in dialouge.ToCharArray())
        {
        	introText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }
		introTypingSound.Stop();
	}

	//Partial credit for this IEnumerator goes to Berenger, I edited it to work in C#.
	IEnumerator bgColorShifter()
	{
		 while (blooping)
     	{
         	bgColor.r = Random.value; // value is already between 0 and 1
         	bgColor.g = Random.value;
         	bgColor.b = Random.value;
         	bgColor.a = 1.0f;    
         	Debug.Log("bgColor: "+bgColor);
         	float t = 0f;
         	var currentColor = Camera.main.backgroundColor;
         	while(t < 3.0)
         	{
             	Camera.main.backgroundColor = Color.Lerp(currentColor, bgColor, t);
             	yield return new WaitForEndOfFrame();
             	t += Time.deltaTime;
         	}
     	}
	}
}
