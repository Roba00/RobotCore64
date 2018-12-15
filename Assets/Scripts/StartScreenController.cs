using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour {

	public Button startButton;
	public Camera mainCamera;
	//int red = 0;
	//int green = 0;
	//int blue = 0;
	//Color color;
	Color bgColor;
	bool blooping = true;

	void Start () {
		startButton.onClick.AddListener(OnClick);
		StartCoroutine(bgColorShifter());
	}
	

	void OnClick()
	{
		SceneManager.LoadScene(1);
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
