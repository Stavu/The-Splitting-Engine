using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InteractionManager : MonoBehaviour {



	// Singleton //

	public static InteractionManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public GameObject TextBoxPrefab;
	public GameObject currentTextBox;



	// Use this for initialization

	public void Initialize () 
	{
				
	}

	public void OnDestroy()
	{	

	}


	// Update is called once per frame

	void Update () 
	{
				
	}


	// ------  TEXT ------ //


	public void DisplayText(List<DialogueSentence> sentenceList)
	{
		if (currentTextBox != null) 		
		{			
			return;
		}

		currentTextBox = Instantiate (TextBoxPrefab);	
		DialogueTextObject textObject = currentTextBox.AddComponent<DialogueTextObject> ();
		textObject.AddTextList (sentenceList);

		GameManager.textBoxActive = true;
		EventsHandler.Invoke_cb_inputStateChanged ();
	}


	public void DisplayDialogueOption(string optionTitle)
	{
		DialogueOption dialogueOption = GameManager.gameData.nameDialogueOptionMap [optionTitle];
		DisplayText (dialogueOption.sentenceList);
	}



	/*
	// Change conversation


	public void ChangeConversation(string conversationName)
	{
		
	
	}
	*/

	// inventory text


	public void DisplayInventoryText(List<string> stringList)
	{
		
		// checking if one already exists

		InventoryTextObject tempObj = GameObject.FindObjectOfType<InventoryTextObject> ();
	
		if (tempObj != null) 
		{
			return;
		}

		// open inventory text box

		GameObject obj = Instantiate (Resources.Load<GameObject>("Prefabs/InventoryTextBox"));

		obj.GetComponent<InventoryTextObject> ().AddTextList (stringList);

	}


	public void DisplayInfoText(string textString)
	{
		GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/InfoText"));

		Text objText = obj.transform.Find("Text").GetComponent<Text> ();
		objText.text = textString;		

		StartCoroutine (TextFade (objText));
	}


	IEnumerator TextFade(Text text)
	{

		float a = 0;
		float speed = 3;

		text.color = new Color (1f, 1f, 1f, a);

		while (a < 1f) 
		{			
			a += Time.deltaTime * speed;
			text.color = new Color (1f, 1f, 1f, a);
			yield return new WaitForFixedUpdate ();
		}


		a = 1f;
		text.color = Color.white;

		yield return new WaitForSeconds (3);


		while(a > 0)
		{
			a -= Time.deltaTime * speed;
			text.color = new Color (1f, 1f, 1f, a);
			yield return new WaitForFixedUpdate ();

		}

		a = 0;
		text.color = new Color (1f, 1f, 1f, a);

		Destroy (text.transform.parent.gameObject);

	}


	// black screen info

	public void DisplayInfoBlackScreen(string textString)
	{			
		StartCoroutine (BlackScreenFade (textString));
	}


	IEnumerator BlackScreenFade(string textString)
	{
		
		GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/InfoBlackScreen"));

		Text objText = obj.transform.Find("Text").GetComponent<Text> ();
		objText.text = textString;		

		Image objImage = obj.transform.Find("Image").GetComponent<Image> ();


		// coroutine

		float a = 0;
		float speed = 5;

		objText.color = new Color (1f, 1f, 1f, a);
		objImage.color = new Color (0f, 0f, 0f, a);


		while (a < 1f) 
		{			
			a += Time.deltaTime * speed;
			objText.color = new Color (1f, 1f, 1f, a);
			objImage.color = new Color (0f, 0f, 0f, a);

			yield return new WaitForFixedUpdate ();
		}


		a = 1f;

		objText.color = Color.white;
		objImage.color = Color.black;

		yield return new WaitForSeconds (3);


		while(a > 0)
		{
			a -= Time.deltaTime * speed;
			objText.color = new Color (1f, 1f, 1f, a);
			objImage.color = new Color (0f, 0f, 0f, a);

			yield return new WaitForFixedUpdate ();

		}

		a = 0;
		objText.color = new Color (1f, 1f, 1f, a);
		objImage.color = new Color (0f, 0f, 0f, a);

		Destroy (obj);

	}





	// move to room


	public void MoveToRoom(string roomName, Vector2 entrancePoint)
	{

		GameManager.roomToLoad = GameManager.instance.stringRoomMap [roomName];

		Tile tempTile = RoomManager.instance.myRoom.MyGrid.GetTileAt ((int)entrancePoint.x, (int)entrancePoint.y);

		//Debug.Log (entrancePoint);

		// Errors with destination tile

		if (tempTile != null) 
		{			
			if (tempTile.IsWalkable() == false) 
			{
				Debug.LogError ("destination tile is not walkable");
			}

		} else {

			Debug.LogError ("destination tile is null");
		}

		PlayerManager.entrancePoint = entrancePoint;
		PlayerManager.myPlayer.currentRoom = roomName;
		NavigationManager.instance.NavigateToScene (SceneManager.GetActiveScene ().name, Color.black);

		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);	
	}



	// Shadows

	public void ChangeShadowState(bool inTheShadow)
	{		
		EventsHandler.Invoke_cb_inputStateChanged ();
		//GameManager.instance.inputState = InputState.Character;

		if (RoomManager.instance.myRoom.myMirrorRoom == null) 
		{
			Debug.LogError ("this really shouldn't be happening - mirrorRoom is null");
			return;
		}

		if (RoomManager.instance.myRoom.myMirrorRoom.inTheShadow == inTheShadow) 
		{
			return;
		}

		RoomManager.instance.myRoom.myMirrorRoom.inTheShadow = inTheShadow;
		RoomManager.instance.SwitchObjectByShadowState (false);

		EventsHandler.Invoke_cb_shadowStateChanged (inTheShadow);
	}



	// Pick up item

	public void PickUpItem (InventoryItem inventoryItem)
	{		
		GameManager.userData.GetCurrentPlayerData().inventory.AddItem (inventoryItem);
	}


	// Use item (we still don't know what item it is)

	public void OpenInventory_UseItem (PhysicalInteractable physicalInt)
	{
		InventoryUI.instance.OpenInventory (InventoryState.UseItem);
	}


	// Combine item (we have a current item, but need to chose an item to combine it with

	public void OpenInventory_CombineItem ()
	{
		InventoryUI.instance.OpenInventory (InventoryState.Combine);
	}


}
