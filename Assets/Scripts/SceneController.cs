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
        
        string dialouge = "June 11, 2390 \n After a progressive 1000 months, a team of engineers funded by the United Nations were able to create a superior life-form to surpass the race of Homo-Sapiens. However, the life-form, named Pico, has escaped the labratory in which it was created, and set loose on it's purpose to end all life-forms on Earth, starting with innocent creatures that have been exposed to a chemical radiation leak caused by humans.";
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
