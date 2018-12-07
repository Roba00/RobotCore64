﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneController : MonoBehaviour {

	public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PicoController>().isDead == true)
		{
			EditorSceneManager.LoadScene(0);
		}
	}
}
