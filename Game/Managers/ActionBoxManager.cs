using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBoxManager : MonoBehaviour {



	// Singleton //

	public static ActionBoxManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public GameObject FurnitureFramePrefab;
	public GameObject ActionBoxPrefab;
	public GameObject ActionPrefab;

	GameObject currentPhysicalInteractableFrame;
	public GameObject currentActionBox;

	public PhysicalInteractable currentPhysicalInteractable;



	Interaction _currentInteraction;

	public Interaction currentInteraction
	{
		
		get {return _currentInteraction;} 
		set {

				if (_currentInteraction != null) 
				{
					myInteractionObjectDictionary [_currentInteraction].transform.GetChild(0).gameObject.SetActive (false);
				}

				_currentInteraction = value;

				if (_currentInteraction != null) 
				{		
					// Interaction frame
					myInteractionObjectDictionary [_currentInteraction].transform.GetChild (0).gameObject.SetActive (true);
					myInteractionObjectDictionary [_currentInteraction].transform.GetChild (0).GetComponent<Image> ().color = myInteractionObjectDictionary [_currentInteraction].GetComponent<Text> ().color;

				}

			} 
	}


	List<Interaction> activeInteractionList;
	Dictionary<Interaction,GameObject> myInteractionObjectDictionary;



	// Use this for initialization

	public void Initialize () 
	{

		EventsHandler.cb_playerHitPhysicalInteractable += SelectPhysicalInteractable;
		EventsHandler.cb_playerLeavePhysicalInteractable += CloseFurnitureFrame;	

		EventsHandler.cb_keyPressedDown += BrowseInteractions;

		currentPhysicalInteractable = null;
		
	}


	public void OnDestroy()
	{
		
		EventsHandler.cb_playerHitPhysicalInteractable -= SelectPhysicalInteractable;
		EventsHandler.cb_playerLeavePhysicalInteractable -= CloseFurnitureFrame;

		EventsHandler.cb_keyPressedDown -= BrowseInteractions;
			
	}


	public void SelectPhysicalInteractable(PhysicalInteractable myPhysicalInt)
	{

		if (myPhysicalInt.myInteractionList.Count == 0) 
		{		
			return;		
		}


		if (currentPhysicalInteractableFrame != null) 		
		{			
			return;
		}

		currentPhysicalInteractable = myPhysicalInt;

		currentPhysicalInteractableFrame = CreateFrame (myPhysicalInt);
	}


	public static GameObject CreateFrame(PhysicalInteractable myPhysicalInt, bool inEditor = false)
	{

		GameObject frame = Instantiate (Resources.Load<GameObject>("Prefabs/FurnitureFrameObject"));

		List<Vector3> positionsList;


		positionsList = Utilities.GetPhysicalInteractableFrameBounds (myPhysicalInt, inEditor);

		frame.GetComponent<RectTransform> ().anchoredPosition = positionsList [0];

		frame.transform.Find ("FramePiece_DL").GetComponent<RectTransform> ().anchoredPosition = positionsList [1];
		frame.transform.Find ("FramePiece_DR").GetComponent<RectTransform> ().anchoredPosition = positionsList [2];
		frame.transform.Find ("FramePiece_UL").GetComponent<RectTransform> ().anchoredPosition = positionsList [3];
		frame.transform.Find ("FramePiece_UR").GetComponent<RectTransform> ().anchoredPosition = positionsList [4];

		return frame;
	}


	public void CloseFurnitureFrame ()
	{	
		if (currentPhysicalInteractableFrame != null) 
		{
			Destroy (currentPhysicalInteractableFrame.gameObject);
			currentPhysicalInteractable = null;
		}

	}



	// Create actionbox

	public void OpenActionBox()
	{
		currentActionBox = Instantiate (ActionBoxPrefab, PositionActionBox() ,Quaternion.identity);

		setInteractionButtons ();

		GameManager.actionBoxActive = true;
		//GameManager.instance.inputState = InputState.ActionBox;

		EventsHandler.Invoke_cb_inputStateChanged ();
	}


	Vector3 PositionActionBox()
	{
				
		Player activePlayer = PlayerManager.myPlayer;

		Tile playerTile = RoomManager.instance.myRoom.MyGrid.GetTileAt (activePlayer.myPos);
		Tile currentTile = RoomManager.instance.myRoom.MyGrid.GetTileAt (currentPhysicalInteractable.myPos);

		int x = 0;
		int y = 0;

		if (playerTile.y == currentTile.y)
		{
			
			// character is left of object

			if (playerTile.x < currentTile.x)
			{
				x = -3;
				y = 3;
			}

			// character is right of object

			if (playerTile.x > currentTile.x)
			{
				x = 2;
				y = 1;
			}
		}


	
		if (playerTile.x == currentTile.x)
		{

			// character is above object

			if (playerTile.y > currentTile.y) 
			{
				x = -2;
				y = 1;
			}


			// character is below object

			if (playerTile.y < currentTile.y)
			{
				x = -1;
				y = 0;
			}

		}


		return new Vector3 (playerTile.x + x, playerTile.y + y, 0);


	}




	// --------- SET INTERACTION BUTTONS --------- // 


	public void setInteractionButtons()
	{

		if (currentPhysicalInteractable.myInteractionList.Count == 0) 
		{
			return;
		}

		activeInteractionList = new List<Interaction> ();
		myInteractionObjectDictionary = new Dictionary<Interaction, GameObject> ();


		for (int i = 0; i < currentPhysicalInteractable.myInteractionList.Count; i++) 
		{

			// Check if passed the conditions, if not, continute to next interaction 
					
			bool passedConditions = Utilities.EvaluateConditions (currentPhysicalInteractable.myInteractionList [i].conditionList);

			Debug.Log ("passedConditions " + passedConditions);

			if (passedConditions == false) 
			{
				Debug.Log ("continue");
				continue;
			}


			// y pos is -1 because there is an image among the parent's children

			int yPos = currentActionBox.transform.childCount - 1;

			GameObject obj = Instantiate (ActionPrefab, currentActionBox.transform);

			obj.transform.localPosition = new Vector3 (0, 1 - yPos, 0);
			obj.GetComponent<Text> ().text = currentPhysicalInteractable.myInteractionList[i].myVerb;

			activeInteractionList.Add(currentPhysicalInteractable.myInteractionList[i]);
			myInteractionObjectDictionary.Add (currentPhysicalInteractable.myInteractionList[i], obj);


			// if the interaction is use item, and there are no items in the inventory, it will be in 0.5 alpha

			if ((currentPhysicalInteractable.myInteractionList[i].myVerb == "Use Item") && (GameManager.userData.GetCurrentPlayerData().inventory.items.Count == 0)) 
			{
				obj.GetComponent<Text> ().color = new Color (1f, 1f, 1f, 0.5f); 
			}


			if (currentInteraction == null) 
			{
				currentInteraction = currentPhysicalInteractable.myInteractionList[i];
			}
		}
	}



	public void BrowseInteractions(Direction myDirection)
	{

		if (GameManager.instance.inputState != InputState.ActionBox) 
		{
			return;
		}

		if (currentActionBox == null) 		
		{			
			return;
		}

		if (currentInteraction == null) 
		{
			return;
		}

		int i =	activeInteractionList.IndexOf (currentInteraction);


		switch (myDirection) 
		{

		case Direction.down:

				if (i < activeInteractionList.Count - 1) 
			{
				currentInteraction = activeInteractionList [i + 1];
			}

				if (i == activeInteractionList.Count - 1) 
			{
				currentInteraction = activeInteractionList [0];
			}


			break;


		case Direction.up:

			if (i > 0) 
			{
				currentInteraction = activeInteractionList [i - 1];
			}

			if (i == 0) 
			{
				currentInteraction = activeInteractionList [activeInteractionList.Count - 1];
			}

			break;

		}
	}



	public void CloseActionBox ()
	{
		if (currentActionBox != null) 
		{
			Destroy (currentActionBox.gameObject);

			GameManager.actionBoxActive = false;
			currentInteraction = null;

			EventsHandler.Invoke_cb_inputStateChanged ();
		}
	}



	// Activate interaction


	public void ActivateInteraction ()
	{

		if (currentInteraction == null) 
		{
			return;
		}

			
		if (currentInteraction.subInteractionList.Count == 0) 
		{
			Debug.LogError ("SubInteract: There are no subinteractions.");

			CloseFurnitureFrame ();
			CloseActionBox ();

			return;
		}


		// check if subinteractions passed the conditions

		List<SubInteraction> subinteractionsToDo = Utilities.GetPassedSubinteractions (currentInteraction.subInteractionList);
	
		subinteractionsToDo.ForEach (subInt => subInt.SubInteract ());			



		CloseActionBox ();

	}





}
