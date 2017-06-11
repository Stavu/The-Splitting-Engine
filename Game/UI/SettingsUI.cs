using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {


	// Singleton //

	public static SettingsUI instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	GameObject settingsObject;

	GameObject musicText;
	GameObject soundText;

	GameObject chosenText;



	// Use this for initialization

	public void Initialize () 
	{		
		CreateSettings();
		CloseSettings();	

		EventsHandler.cb_key_p_pressed += OnSettingsKeyPressed;
		EventsHandler.cb_keyPressedDown += BrowseSettings;
	}


	void OnDestroy()
	{
		EventsHandler.cb_key_p_pressed -= OnSettingsKeyPressed;
		EventsHandler.cb_keyPressedDown -= BrowseSettings;
	}


	
	// Update is called once per frame

	void Update () 
	{
		
	}


	public void CreateSettings()
	{
		// If there's already an inventory, display error

		if (settingsObject != null) 
		{
			Debug.LogError ("There's already a settings object");
		}
			
		settingsObject = Instantiate (Resources.Load<GameObject> ("Prefabs/settingsUI"));

		musicText = settingsObject.transform.Find ("MusicText").gameObject;
		soundText = settingsObject.transform.Find ("SoundText").gameObject;
	}



	// When pressing p

	public void OnSettingsKeyPressed()
	{
		if (GameManager.actionBoxActive) 
		{
			return;
		}

		if (GameManager.inventoryOpen) 
		{
			return;
		}

		if (GameManager.mapOpen) 
		{
			return;
		}

		if (GameManager.settingsOpen == false)
		{		
			OpenSettings ();

		} else {

			CloseSettings ();
		}
	}



	public void OpenSettings()
	{
		settingsObject.SetActive (true);

		GameManager.settingsOpen = true;

		chosenText = musicText; 
		soundText.transform.Find ("Arrow").gameObject.SetActive (false);
		musicText.transform.Find ("Arrow").gameObject.SetActive (true);			

		EventsHandler.Invoke_cb_inputStateChanged ();
	}



	// Browsing inventory

	public void BrowseSettings(Direction direction)
	{

		if (GameManager.instance.inputState != InputState.Settings) 
		{
			return;
		}

		// Catch the index of the old chosen item, before changing it

	
		switch (direction)
		{

			case Direction.up:

			case Direction.down:

				if (chosenText == musicText) 
				{
					chosenText = soundText;
					soundText.transform.Find ("Arrow").gameObject.SetActive (true);
					musicText.transform.Find ("Arrow").gameObject.SetActive (false);
				
				} else {

					chosenText = musicText;
					soundText.transform.Find ("Arrow").gameObject.SetActive (false);
					musicText.transform.Find ("Arrow").gameObject.SetActive (true);

				}

				break;


			case Direction.left:

			case Direction.right:

				if (chosenText == soundText) 
				{
					if (SoundManager.soundIsOff == true) 
					{
						SoundManager.Invoke_cb_setSound (false);
						soundText.transform.Find ("SoundToggleText").GetComponent<Text> ().text = "On";
					
					} else {

						SoundManager.Invoke_cb_setSound (true);
						soundText.transform.Find ("SoundToggleText").GetComponent<Text> ().text = "Off";
					}
				}

				if (chosenText == musicText) 
				{
					if (SoundManager.musicIsOff == true) 
					{
						SoundManager.Invoke_cb_setMusic (false);
						musicText.transform.Find ("MusicToggleText").GetComponent<Text> ().text = "On";

					} else {

						SoundManager.Invoke_cb_setMusic (true);
						musicText.transform.Find ("MusicToggleText").GetComponent<Text> ().text = "Off";
					}
				}

				break;
		}
	}




	// ----- CLOSE SETTINGS ----- //


	public void CloseSettings()
	{
		settingsObject.SetActive (false);

		GameManager.settingsOpen = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

	}



}
