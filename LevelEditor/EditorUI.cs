using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;



public class EditorUI : MonoBehaviour 
{

	// Singleton //

	public static EditorUI instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	Dropdown backgroundDropdown;
	Dropdown musicDropdown;
	Dropdown mapAreaDropdown;

	public GameObject interactableSelectPrefab;
	public GameObject interactableButtonPrefab;

	GameObject interactableSelect;

	InputField roomNameInput;

	Button toGameButton;
	Button furnButton;
	Button characterButton;
	Button tileIntButton;

	Button newRoomButton;

	Button flipRoomButton;
	Dropdown roomStateDropdown;

	Toggle shadowToggle;
	Toggle flippedToggle;

	Slider zoomSlider;

	List<string> mapAreaList;


	// Use this for initialization

	public void CreateUI () {

		mapAreaList = new List<string> 
		{
			"None",
			"Asylum",
			"Asylum_mirror",
			"Asylum_outside",
			"Asylum_outside_shadow",
			"Abandoned_wing",
			"Abandoned_wing_mirror"
		};

		CreateBgSelect ();
	}


	public void CreateBgSelect()
	{	

		Room myRoom = EditorRoomManager.instance.room;

		// Lists

		List<string> bgNameList = new List<string> ();
		Sprite[] bgSpriteList = Resources.LoadAll <Sprite> ("Sprites/Rooms/");

		foreach (Sprite spr in bgSpriteList) 
		{
			bgNameList.Add (spr.name);
		}


		List<string> clipNameList = new List<string> ();
		AudioClip[] musicClipList = Resources.LoadAll <AudioClip> ("Audio/Music/");

		foreach (AudioClip clip in musicClipList) 
		{
			clipNameList.Add (clip.name);
		}


		// Assign

		toGameButton = transform.Find ("ToGameButton").GetComponent<Button> ();

		roomNameInput = transform.Find ("RoomNameInput").GetComponent<InputField> ();
		backgroundDropdown = transform.Find ("BackgroundDropdown").GetComponent<Dropdown> ();
		musicDropdown = transform.Find ("MusicDropdown").GetComponent<Dropdown> ();
		mapAreaDropdown = transform.Find ("MapAreaDropdown").GetComponent<Dropdown> ();
		furnButton = transform.Find ("FurnitureButton").GetComponent<Button>();
		characterButton = transform.Find ("CharacterButton").GetComponent<Button>();
		tileIntButton = transform.Find ("TileInteractionButton").GetComponent<Button> ();
		newRoomButton = transform.Find ("NewRoomButton").GetComponent<Button> ();			
		flipRoomButton = transform.Find ("FlipRoomButton").GetComponent<Button>();
		roomStateDropdown = transform.Find ("RoomStateDropdown").GetComponent<Dropdown>();
		shadowToggle = transform.Find ("ShadowToggle").GetComponent<Toggle>();
		flippedToggle = transform.Find ("FlippedToggle").GetComponent<Toggle>();

		zoomSlider = transform.Find ("ZoomSlider").GetComponent<Slider>();

		// Listeners

		toGameButton.onClick.AddListener(() => SceneManager.LoadScene("Main"));

		// BACKGROUND DROPDOWN

		backgroundDropdown.AddOptions (bgNameList);
		backgroundDropdown.onValueChanged.AddListener (NewBackgroundSelected);

		// Zoom Slider
		zoomSlider.onValueChanged.AddListener ( i => Camera.main.orthographicSize = i );

		// set bg dropdown value 

		string roomBgName = myRoom.bgName;
			
		if (myRoom.roomState == RoomState.Mirror) 			
		{
			if (myRoom.myMirrorRoom.inTheShadow == true) 
			{
				roomBgName = myRoom.myMirrorRoom.bgName_Shadow;
			}
		}

		if (bgNameList.Contains (roomBgName) == true) 
		{
			backgroundDropdown.value = bgNameList.IndexOf (roomBgName);

		} else {

			Debug.LogError ("can't find bg name in list");
		}
			
		// MUSIC DROPDOWN

		musicDropdown.AddOptions (clipNameList);
		musicDropdown.onValueChanged.AddListener (NewMusicSelected);

		// set music dropdown value 

		string roomMusicName = myRoom.myMusic;

		if (myRoom.roomState == RoomState.Mirror) 			
		{
			if (myRoom.myMirrorRoom.inTheShadow == true) 
			{
				roomMusicName = myRoom.myMirrorRoom.myShadowMusic;
			}
		}

		if (clipNameList.Contains (roomMusicName) == true) 
		{
			musicDropdown.value = clipNameList.IndexOf (roomMusicName);

		} else if (roomMusicName != string.Empty) 
		{
			Debug.LogError ("can't find clip name in list");
		}

		// MAP AREA DROPDOWN

		mapAreaDropdown.AddOptions (mapAreaList);
		mapAreaDropdown.onValueChanged.AddListener (NewMapAreaSelected);

		// set map area dropdown value 

		string roomMapArea = myRoom.mapArea;

		if (mapAreaList.Contains (roomMapArea) == true) 
		{
			mapAreaDropdown.value = mapAreaList.IndexOf (roomMapArea);

		} else if (roomMapArea != string.Empty) 
		{
			Debug.LogError ("can't find map area in list");
		}

		furnButton.onClick.AddListener (CreateFurnitureSelect);

		characterButton.onClick.AddListener (CreateCharacterSelect);
		tileIntButton.onClick.AddListener (SetTileInteractionMode);	

		newRoomButton.onClick.AddListener (CreateBackgroundSelect);
		flipRoomButton.onClick.AddListener (FlipRoom);
		roomStateDropdown.onValueChanged.AddListener (SetRoomState);
		roomStateDropdown.value = (int)myRoom.RoomState;

		roomNameInput.onEndEdit.AddListener (RoomNameChanged);

		// Room name 

		if (myRoom.myName != null) 
		{
			roomNameInput.text = myRoom.myName;
		}

		// Shadow state 

		if (myRoom.RoomState == RoomState.Real) 
		{
			shadowToggle.interactable = false;
		
		} else {

			shadowToggle.interactable = true;
			shadowToggle.isOn = myRoom.myMirrorRoom.inTheShadow;
			roomStateDropdown.interactable = !shadowToggle.isOn;
		}

		shadowToggle.onValueChanged.AddListener (SetShadowState);

		// flipped background
		flippedToggle.isOn = myRoom.bgFlipped;
		flippedToggle.interactable = false; // FIXME: if you want to make it interactable, add a function which will change it and reload the background
	}


	// Dropdown - on value changed //

	public void NewMusicSelected(int optionNum)
	{
		string spriteName = musicDropdown.options [optionNum].text;
		EditorRoomManager.instance.ChangeRoomMusic (spriteName);
	}

	public void NewBackgroundSelected(int optionNum)
	{
		string clipName = backgroundDropdown.options [optionNum].text;
		EditorRoomManager.instance.ChangeRoomBackground (clipName);
	}

	public void NewMapAreaSelected(int optionNum)
	{
		string mapArea = mapAreaDropdown.options [optionNum].text;
		EditorRoomManager.instance.ChangeMapArea (mapArea);

	}


	// CREATING BACKGROUND //

	public void CreateBackgroundSelect()
	{
		Destroy (interactableSelect);
		interactableSelect = Instantiate (interactableSelectPrefab);

		GameObject content = interactableSelect.GetComponentInChildren<GridLayoutGroup> ().gameObject;				
		Sprite[] bgSpriteList = Resources.LoadAll<Sprite> ("Sprites/Rooms/");

		foreach (Sprite sprite in bgSpriteList) 
		{
			GameObject button = Instantiate (interactableButtonPrefab);
			button.transform.SetParent (content.transform);

			button.GetComponent<Image> ().sprite = sprite;
			button.GetComponentInChildren<Text> ().text = sprite.name;

			button.GetComponent<Button> ().onClick.AddListener (() => EditorRoomManager.instance.CreateNewRoomFromSprite(sprite.name));
		}

		// Assigning the close button
		GameObject.Find("CloseButton").GetComponentInChildren<Button>().onClick.AddListener ( () => Destroy (interactableSelect) );
	}


	// CREATING FURNITURE //

	public void CreateFurnitureSelect()
	{
		Destroy (interactableSelect);
		interactableSelect = Instantiate (interactableSelectPrefab);

		GameObject content = interactableSelect.GetComponentInChildren<GridLayoutGroup> ().gameObject;				
		Sprite[] furnitureSpriteList = Resources.LoadAll<Sprite> ("Sprites/Furniture/");

		foreach (Sprite sprite in furnitureSpriteList) 
		{
			GameObject button = Instantiate (interactableButtonPrefab);
			button.transform.SetParent (content.transform);

			button.GetComponent<Image> ().sprite = sprite;
			button.GetComponentInChildren<Text> ().text = sprite.name;

			button.GetComponent<Button> ().onClick.AddListener (() => SetFurnitureBuildMode(sprite.name));
		}

		// Assigning the close button
		GameObject.Find("CloseButton").GetComponentInChildren<Button>().onClick.AddListener ( () => Destroy (interactableSelect) );
	}


	// CREATING CHARACTER //

	public void CreateCharacterSelect()
	{
		Destroy (interactableSelect);
		interactableSelect = Instantiate (interactableSelectPrefab);

		GameObject content = interactableSelect.GetComponentInChildren<GridLayoutGroup> ().gameObject;
		GameObject[] characterGameObjectList = Resources.LoadAll<GameObject> ("Prefabs/Characters/");

		foreach (GameObject gameObject in characterGameObjectList) 
		{
			GameObject button = Instantiate (interactableButtonPrefab);
			button.transform.SetParent (content.transform);

			button.GetComponent<Image> ().sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
			button.GetComponentInChildren<Text> ().text = gameObject.name;

			button.GetComponent<Button> ().onClick.AddListener (() => SetCharacterBuildMode(gameObject.name));
		}

		// Assigning the close button
		GameObject.Find("CloseButton").GetComponentInChildren<Button>().onClick.AddListener ( () => Destroy (interactableSelect) );
	}


	public void SetFurnitureBuildMode(string furnitureName)
	{
		BuildController.instance.furnitureName = furnitureName;

		Destroy (interactableSelect);

		// set to build Furniture mode

		BuildController.instance.mode = BuildController.Mode.buildFurniture;
	}


	public void SetCharacterBuildMode(string characterName)
	{
		BuildController.instance.characterName = characterName;

		Destroy (interactableSelect);

		// set to build Furniture mode

		BuildController.instance.mode = BuildController.Mode.buildCharacter;
	}


	// CREATE TILE INTERACTION //

	public void SetTileInteractionMode()
	{
		// set to build tileInteraction mode

		BuildController.instance.mode = BuildController.Mode.buildTileInteraction;
	}


	// ROOM NAME

	public void RoomNameChanged(string name)
	{
		EditorRoomManager.instance.room.myName = name;
	}


	// --------------- MIRROR ROOM ---------------- //


	public void FlipRoom ()
	{
		Room newRoom = EditorRoomHelper.CreateFlippedRoom (EditorRoomManager.instance.room);

		string roomString = JsonUtility.ToJson (newRoom);

		// Load room

		EditorRoomManager.roomToLoad = roomString;
		EditorRoomManager.loadRoomFromMemory = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void SetRoomState(int i)
	{
		EditorRoomManager.instance.room.RoomState = (RoomState)i;

		if (EditorRoomManager.instance.room.RoomState == RoomState.Real) 
		{
			shadowToggle.interactable = false;

		} else {
			
			shadowToggle.interactable = true;
		}		
	}


	public void SetShadowState(bool inShadow)
	{
		EditorRoomManager.instance.room.myMirrorRoom.inTheShadow = inShadow;
		EditorRoomManager.roomToLoad = JsonUtility.ToJson (EditorRoomManager.instance.room);

		EditorRoomManager.loadRoomFromMemory = true;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	// ----- ALERT ----- //

	public static void DisplayAlert(string textString, UnityEngine.Events.UnityAction action)
	{
		GameObject alertObject = Instantiate(Resources.Load<GameObject>("Prefabs/Editor/Alert"));

		Text alertText = alertObject.transform.Find ("Panel").Find ("Text").GetComponent<Text> ();
		alertText.text = textString;

		Button yesButton = alertObject.transform.Find ("Panel").Find ("YesButton").GetComponent<Button> ();
		Button noButton = alertObject.transform.Find ("Panel").Find ("NoButton").GetComponent<Button> ();

		yesButton.onClick.AddListener (action);
		yesButton.onClick.AddListener (() => Destroy(alertObject));
		noButton.onClick.AddListener (() => Destroy(alertObject));
	}


	public static void DisplayAlert_B(string textString)
	{
		GameObject alertObject = Instantiate(Resources.Load<GameObject>("Prefabs/Editor/Alert_B"));

		Text alertText = alertObject.transform.Find ("Panel").Find ("Text").GetComponent<Text> ();
		alertText.text = textString;

		Button okButton = alertObject.transform.Find ("Panel").Find ("OkButton").GetComponent<Button> ();

		okButton.onClick.AddListener (() => Destroy(alertObject));
	}

}
