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


	// ------  TEXT ------ //

	public void DisplayText(List<DialogueSentence> sentenceList, bool isImportant = true)
	{
		if (currentTextBox != null) 		
		{			
			return;
		}

		currentTextBox = Instantiate (TextBoxPrefab);	
		DialogueTextObject textObject = currentTextBox.AddComponent<DialogueTextObject> ();
		textObject.AddTextList (sentenceList);
		textObject.isImportant = isImportant;

		GameManager.textBoxActive = true;
		EventsHandler.Invoke_cb_inputStateChanged ();
	}


	public void DisplayDialogueOption(string optionTitle)
	{
		DialogueOption dialogueOption = GameManager.gameData.nameDialogueOptionMap [optionTitle];
		DisplayText (dialogueOption.sentenceList);
	}


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
			
		PlayerManager.myPlayer.currentRoom = roomName;

		// checking if should be placed in the same position as the player was before
		Vector2 newPos;
		if (entrancePoint.x == -1)
		{
			newPos.x = PlayerManager.myPlayer.myPos.x;
		}
		else
		{
			newPos.x = entrancePoint.x;
		}

		if (entrancePoint.y == -1)
		{
			newPos.y = PlayerManager.myPlayer.myPos.y;
		}
		else
		{
			newPos.y = entrancePoint.y;
		}

		PlayerManager.myPlayer.myPos = newPos;

		GameManager.userData.GetPlayerDataByPlayerName (PlayerManager.myPlayer.identificationName).currentRoom = PlayerManager.myPlayer.currentRoom;

		NavigationManager.instance.NavigateToScene (SceneManager.GetActiveScene ().name, Color.black);
	}


	// Shadows

	public void ChangeShadowState(bool inTheShadow)
	{		
		Debug.Log ("interactionmanager:ChangeShadowState");

		EventsHandler.Invoke_cb_inputStateChanged ();

		if (RoomManager.instance.myRoom.myMirrorRoom == null) 
		{
			Debug.LogError ("this really shouldn't be happening - mirrorRoom is null");
			return;
		}

		if (RoomManager.instance.myRoom.myMirrorRoom.inTheShadow == inTheShadow) 
		{
			Debug.Log ("the same");

			return;
		}

		RoomManager.instance.myRoom.myMirrorRoom.inTheShadow = inTheShadow;

		if (inTheShadow == true) 
		{			
			GameManager.userData.AddToRoomsInShadow (RoomManager.instance.myRoom.myName);

		} else {

			GameManager.userData.RemoveFromRoomsInShadow (RoomManager.instance.myRoom.myName);
		}

		RoomManager.instance.SwitchObjectByShadowState (false);

		EventsHandler.Invoke_cb_shadowStateChanged (inTheShadow);
	}



	// Pick up item

	public void PickUpItem (InventoryItem inventoryItem)
	{		
		GameManager.userData.GetCurrentPlayerData().inventory.AddItem (inventoryItem);
	}


	// Remove item

	public void RemoveItem (string itemToRemove)
	{		
		GameManager.userData.GetCurrentPlayerData().inventory.RemoveItem (itemToRemove);
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





	// Pick up item

	public void ResetRoom (string roomToReset)
	{	

		switch (roomToReset) 
		{
			case "maze_room_6_mirror":


				// -- GREEN DOOR -- //

				PhysicalInteractable door_abandoned_6_green_mirror = RoomManager.instance.getFurnitureByName ("door_abandoned_6_green_mirror");

				if (GameManager.userData.CheckIfEventExists ("green_door_opened") == true) 
				{
					GameManager.userData.AddEventToList("green_door_mirror_opened");

					PI_Handler.instance.SetPIAnimationState (door_abandoned_6_green_mirror.identificationName, "Open");
					EventsHandler.Invoke_cb_inputStateChanged ();

				} else {

					GameManager.userData.RemoveEventFromList("green_door_mirror_opened");

					PI_Handler.instance.SetPIAnimationState (door_abandoned_6_green_mirror.identificationName, "Closed");
					EventsHandler.Invoke_cb_inputStateChanged ();
				}
					

				// -- BENCH -- //

				PhysicalInteractable bench_abandoned_6_mirror_books = RoomManager.instance.getFurnitureByName ("bench_abandoned_6_mirror");

				// remove all events

				GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_0_books");
				GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_1_book");
				GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_2_books");
				GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_3_books");

				if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_3_books") == true) {

					// add event

					GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_3_books");
					ResetBenchAbandoned6MirrorBookList ();

					// change animation state

					PI_Handler.instance.SetPIAnimationState (bench_abandoned_6_mirror_books.identificationName, "3_books");
					EventsHandler.Invoke_cb_inputStateChanged ();

				} else {

					if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_2_books") == true) {

						// add event

						GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_2_books");
						ResetBenchAbandoned6MirrorBookList ();

						// change animation state

						PI_Handler.instance.SetPIAnimationState (bench_abandoned_6_mirror_books.identificationName, "2_books");
						EventsHandler.Invoke_cb_inputStateChanged ();

					} else {

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_1_book") == true) {							

							// add event

							GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_1_book");
							ResetBenchAbandoned6MirrorBookList ();

							// change animation state

							PI_Handler.instance.SetPIAnimationState (bench_abandoned_6_mirror_books.identificationName, "1_book");
							EventsHandler.Invoke_cb_inputStateChanged ();

						} else {

							if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_0_books") == true) 
							{										
								// add event

								GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_0_books");
								ResetBenchAbandoned6MirrorBookList ();

								// change animation state

								PI_Handler.instance.SetPIAnimationState (bench_abandoned_6_mirror_books.identificationName, "Idle");
								EventsHandler.Invoke_cb_inputStateChanged ();
							
							} else {

								Debug.LogError ("how many books?");
							}
						}
					}
				}

				break;


			default:

				Debug.Log ("resetRoom");

				break;
		}
	}



	// Reset book list

	void ResetBenchAbandoned6MirrorBookList ()
	{
		GameManager.userData.benchAbandoned6MirrorBookList.Clear ();

		if (GameManager.userData.benchAbandoned6BookList.Count > 0) 
		{
			foreach (string bookName in GameManager.userData.benchAbandoned6BookList) 
			{
				if (bookName == "book_real") 
				{
					GameManager.userData.benchAbandoned6MirrorBookList.Add ("book_mirror");
				} 

				if(bookName == "book_mirror")
				{
					GameManager.userData.benchAbandoned6MirrorBookList.Add ("book_real");
				}
			}
		}
	}


	// Special

	public void SpecialInteraction (string specialInteraction)
	{		
		switch(specialInteraction)
		{
			case "bench_abandoned_6_pick_up_book":

				PhysicalInteractable bench_abandoned_6 = RoomManager.instance.getFurnitureByName ("bench_abandoned_6");

				if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_3_books") == true) 
				{
					Debug.LogError ("not supposed to happen");

				} else {

					if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_2_books") == true) 
					{
						// had 2 books, now has 1 book

						// remove event
						GameManager.userData.RemoveEventFromList ("bench_abandoned_6_2_books");

						// add event
						GameManager.userData.AddEventToList ("bench_abandoned_6_1_book");

						// change animation state

						PI_Handler.instance.SetPIAnimationState (bench_abandoned_6.identificationName, "1_book");
						EventsHandler.Invoke_cb_inputStateChanged ();

					} else {

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_1_book") == true) 
						{
							// had 1 book, now has no books

							// remove event
							GameManager.userData.RemoveEventFromList ("bench_abandoned_6_1_book");

							// add event
							GameManager.userData.AddEventToList ("bench_abandoned_6_0_books");

							// change animation state

							PI_Handler.instance.SetPIAnimationState (bench_abandoned_6.identificationName, "Idle");
							EventsHandler.Invoke_cb_inputStateChanged ();
						} 
					}


					// add item

					if (GameManager.userData.benchAbandoned6BookList [GameManager.userData.benchAbandoned6BookList.Count - 1] == "book_real") 
					{
						// if top book is real 

						GameManager.userData.RemoveBookFromBench ("book_real");

						InventoryItem book = new InventoryItem ("book", "Old Book"); 
						GameManager.userData.GetCurrentPlayerData ().inventory.AddItem (book);

					} else {							

						// if top book is mirror 

						if (GameManager.userData.benchAbandoned6BookList [GameManager.userData.benchAbandoned6BookList.Count - 1] == "book_mirror") 
						{
							GameManager.userData.RemoveBookFromBench ("book_mirror");

							InventoryItem book_mirror = new InventoryItem ("book_mirror", "Old Book"); 
							GameManager.userData.GetCurrentPlayerData ().inventory.AddItem (book_mirror);
						}
					}

					// add event

					GameManager.userData.AddEventToList ("book_taken");		
				}

				break;


			case "bench_abandoned_6_mirror_pick_up_book":

				PhysicalInteractable bench_abandoned_6_mirror = RoomManager.instance.getFurnitureByName ("bench_abandoned_6_mirror");

				if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_3_books") == true) 
				{
					DialogueSentence sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, "I think I'm done moving books around.", false);
					List<DialogueSentence> list = new List<DialogueSentence> ();
					list.Add (sentence);

					DisplayText (list);

					return;
				}


				if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_3_books") == true) 
				{							
					// had 3 books, now has 2 books

					// remove event
					GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_3_books");

					// add event
					GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_2_books");

					// change animation state
					PI_Handler.instance.SetPIAnimationState (bench_abandoned_6_mirror.identificationName, "2_books");
					EventsHandler.Invoke_cb_inputStateChanged ();

				} else {

					if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_2_books") == true) 
					{
						// had 2 books, now has 1 book

						// remove event
						GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_2_books");

						// add event
						GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_1_book");

						// change animation state

						PI_Handler.instance.SetPIAnimationState (bench_abandoned_6_mirror.identificationName, "1_book");
						EventsHandler.Invoke_cb_inputStateChanged ();

					} else {

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_1_book") == true) 
						{
							// had 1 book, now has no books

							// remove event
							GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_1_book");

							// add event
							GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_0_books");


							// change animation state

							PI_Handler.instance.SetPIAnimationState (bench_abandoned_6_mirror.identificationName, "Idle");
							EventsHandler.Invoke_cb_inputStateChanged ();
						} 
					}						
				}

				// add item

				if (GameManager.userData.benchAbandoned6MirrorBookList.Count > 0)
				{
					if (GameManager.userData.benchAbandoned6MirrorBookList [GameManager.userData.benchAbandoned6MirrorBookList.Count - 1] == "book_real") 
					{
						// if top book is real 

						GameManager.userData.RemoveBookFromBenchMirror ("book_real");

						InventoryItem book = new InventoryItem ("book", "Old Book"); 
						GameManager.userData.GetCurrentPlayerData ().inventory.AddItem (book);

					} else {							

						// if top book is mirror 

						if (GameManager.userData.benchAbandoned6MirrorBookList [GameManager.userData.benchAbandoned6MirrorBookList.Count - 1] == "book_mirror") 
						{
							GameManager.userData.RemoveBookFromBenchMirror ("book_mirror");

							InventoryItem book_mirror = new InventoryItem ("book_mirror", "Old Book"); 
							GameManager.userData.GetCurrentPlayerData ().inventory.AddItem (book_mirror);
						}
					}
				}

				// add event

				GameManager.userData.AddEventToList ("book_taken");	

				break;


			default:

				Debug.Log("no special interaction");

				break;
			}
	}
}
