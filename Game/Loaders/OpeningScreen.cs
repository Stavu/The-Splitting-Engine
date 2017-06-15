using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class OpeningScreen : MonoBehaviour {


	GameObject newGameText;
	GameObject continueText;


	// Use this for initialization

	void Start () 
	{
		newGameText = GameObject.Find ("NewGameText");
		continueText = GameObject.Find ("ContinueText");	

		if (PlayerPrefs.HasKey ("PlayerData")) 
		{
			newGameText.SetActive (false);
			continueText.SetActive (true);

		} else {

			newGameText.SetActive (true);
			continueText.SetActive (false);
		}
	}

	
	// Update is called once per frame

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.N)) 
		{
			PlayerPrefs.DeleteKey ("PlayerData");
			NavigationManager.instance.NavigateToScene ("Main", Color.black);
			Destroy (this);
		}

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			NavigationManager.instance.NavigateToScene ("Main", Color.black);
			Destroy (this);
		}		
	}





}
