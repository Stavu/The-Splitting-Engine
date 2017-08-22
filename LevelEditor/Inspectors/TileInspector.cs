using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInspector : MonoBehaviour {


	// Declarations

	GameObject tileInspectorObject;

	Transform panel;

	InputField sizeXInput;
	InputField sizeYInput;
	InputField posXInput;
	InputField posYInput;
	InputField entrancePointXInput;
	InputField entrancePointYInput;


	Text sizeXPlaceholder;
	Text sizeYPlaceholder;
	Text posXPlaceholder;
	Text posYPlaceholder;
	Text entrancePointXPlaceholder;
	Text entrancePointYPlaceholder;



	InputField interactionTextInput;
	Toggle textInputCheckBox;

	InputField destinationRoomInput;
	Toggle enterRoomCheckBox;

	Toggle walkableToggle;
	Toggle persistentToggle;

	Button deleteButton;



	// Use this for initialization

	void Start () 
	{
		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}


	// ----- TILE INSPECTOR ----- // 


	public void CreateTileInspector(TileInteraction currentTileInteraction)
	{
		InspectorManager.physicalInteractableInspector.DestroyInspector ();
		DestroyTileInspector ();

		tileInspectorObject = Instantiate (InspectorManager.instance.tileInspectorObjectPrefab);


		// Assign 

		panel = tileInspectorObject.transform.Find ("Panel");

		sizeXInput = panel.Find ("SizeX").GetComponent<InputField> ();
		sizeYInput = panel.Find ("SizeY").GetComponent<InputField> ();
		posXInput = panel.Find ("PosX").GetComponent<InputField> ();
		posYInput = panel.Find ("PosY").GetComponent<InputField> ();
		entrancePointXInput = panel.Find ("EntrancePointX").GetComponent<InputField> ();
		entrancePointYInput = panel.Find ("EntrancePointY").GetComponent<InputField> ();

		sizeXPlaceholder = panel.Find ("SizeX").Find ("Placeholder").GetComponent<Text> ();
		sizeYPlaceholder = panel.Find ("SizeY").Find ("Placeholder").GetComponent<Text> ();
		posXPlaceholder = panel.Find ("PosX").Find ("Placeholder").GetComponent<Text> ();
		posYPlaceholder = panel.Find ("PosY").Find ("Placeholder").GetComponent<Text> ();
		entrancePointXPlaceholder = panel.Find ("EntrancePointX").Find ("Placeholder").GetComponent<Text> ();
		entrancePointYPlaceholder = panel.Find ("EntrancePointY").Find ("Placeholder").GetComponent<Text> ();

		// Dialogue

		interactionTextInput = panel.Find("InteractionTextInput").GetComponent<InputField> ();
		textInputCheckBox = panel.Find ("TextInputCheckBox").GetComponent<Toggle> ();

		// Destination room

		destinationRoomInput = panel.Find("DestinationRoomInput").GetComponent<InputField>();
		enterRoomCheckBox = panel.Find ("EnterRoomCheckBox").GetComponent<Toggle> ();

		// walkable 

		walkableToggle = panel.Find ("Walkable").GetComponent<Toggle> ();

		// Persistent

		persistentToggle = panel.Find ("Persistent").GetComponent<Toggle> ();

		// delete ubtton

		deleteButton = panel.Find ("DeleteButton").GetComponent<Button> ();


		// SIZE AND POSITION //

		sizeXPlaceholder.text = currentTileInteraction.mySize.x.ToString();
		sizeYPlaceholder.text = currentTileInteraction.mySize.y.ToString();

		posXPlaceholder.text = currentTileInteraction.x.ToString();
		posYPlaceholder.text = currentTileInteraction.y.ToString();

		// Listeners

		sizeXInput.onEndEdit.AddListener (ChangeTileInteractionWidth);
		sizeYInput.onEndEdit.AddListener (ChangeTileInteractionHeight);
	
		posXInput.onEndEdit.AddListener (ChangeTileInteractionX);
		posYInput.onEndEdit.AddListener (ChangeTileInteractionY);

		deleteButton.onClick.AddListener (DeleteTileInteraction);



		// INTERACTIONS //




		if (currentTileInteraction.mySubInt != null) 
		{			
			// dialogue
			if (currentTileInteraction.mySubInt.interactionType == "showMonologue") 
			{
				textInputCheckBox.isOn = true;
				interactionTextInput.interactable = true;

				interactionTextInput.text = currentTileInteraction.mySubInt.RawText;

			} 

			// move to room

			if (currentTileInteraction.mySubInt.interactionType == "moveToRoom") 
			{
				enterRoomCheckBox.isOn = true;
				destinationRoomInput.interactable = true;

				destinationRoomInput.text = currentTileInteraction.mySubInt.destinationRoomName;

				entrancePointXInput.text = currentTileInteraction.mySubInt.entrancePoint.x.ToString();
				entrancePointYInput.text = currentTileInteraction.mySubInt.entrancePoint.y.ToString();
			}
		}


		// walkable 

		if (currentTileInteraction.walkable == true) 
		{
			walkableToggle.isOn = true;
			
		} else {
		
			walkableToggle.isOn = false;
		}



		// Persistent


		if (EditorRoomManager.instance.room.RoomState == RoomState.Real) 
		{
			persistentToggle.interactable = false;

		} else {

			persistentToggle.interactable = true;

			if (EditorRoomManager.instance.room.myMirrorRoom.myTileInteractionList_Persistant.Contains (currentTileInteraction)) 
			{
				persistentToggle.isOn = true;

			} else {

				persistentToggle.isOn = false;
			}

			persistentToggle.onValueChanged.AddListener (TileInteractionePersistantToggleClicked);
		} 

		// Inspector toggles 

		textInputCheckBox.onValueChanged.AddListener (CheckTileInspectorToggles);
		enterRoomCheckBox.onValueChanged.AddListener (CheckTileInspectorToggles);

		CheckTileInspectorToggles (true);


		// Submit button

		panel.Find("SubmitButton").GetComponent<Button> ().onClick.AddListener  (() => SubmitTileInteraction());

	}



	// Persistency

	public void TileInteractionePersistantToggleClicked(bool isPersistent)
	{	
		TileInteraction tileInt = InspectorManager.instance.chosenTileInteraction;

		EditorRoomHelper.SetTileInteractionPersistency (isPersistent,tileInt);
	}




	// CHECK TILE INSPECTOR TOGGLES //

	public void CheckTileInspectorToggles(bool boolean)
	{
		if (textInputCheckBox.isOn == true) 
		{
			enterRoomCheckBox.isOn = false;
			enterRoomCheckBox.interactable = false;
			destinationRoomInput.interactable = false;

		} else {

			enterRoomCheckBox.interactable = true;
		}

		if (enterRoomCheckBox.isOn == true) 
		{
			textInputCheckBox.isOn = false;
			textInputCheckBox.interactable = false;
			interactionTextInput.interactable = false;

		} else {

			textInputCheckBox.interactable = true;
		}
	}



	// ----- SUBMIT ----- //

	public void SubmitTileInteraction()
	{

		// create show dialogue

		if (textInputCheckBox.isOn == true && interactionTextInput.text != string.Empty) 		
		{
			SubInteraction subInteraction = new SubInteraction ("showMonologue");
			subInteraction.RawText = interactionTextInput.text;

			InspectorManager.instance.chosenTileInteraction.mySubInt = subInteraction;
		}
		else if (enterRoomCheckBox.isOn == true && destinationRoomInput.text != string.Empty)		
		{
			// create enter room
			// default is 0
			int x;
			int y;

			int.TryParse (entrancePointXInput.text, out x);
			int.TryParse (entrancePointYInput.text, out y);

			SubInteraction subInteraction = new SubInteraction ("moveToRoom");
			subInteraction.destinationRoomName = destinationRoomInput.text;

			Vector2 entrancePoint = new Vector2 (x, y);
			subInteraction.entrancePoint = entrancePoint;
			Debug.Log (entrancePoint.x + entrancePoint.y);

			InspectorManager.instance.chosenTileInteraction.mySubInt = subInteraction;
		}


		// walkable

		if (walkableToggle.isOn) 
		{
			InspectorManager.instance.chosenTileInteraction.walkable = true;
		
		} else {
			
			InspectorManager.instance.chosenTileInteraction.walkable = false;
		}

		DestroyTileInspector ();
		//CreateTileInspector (InspectorManager.instance.chosenTileInteraction);
	}



	// change size

	public void ChangeTileInteractionWidth(string x)
	{
		int newX;
		bool validFormat = int.TryParse (x, out newX);

		if (validFormat)
		{
			EditorRoomManager.instance.ChangeInteractableWidth (newX, InspectorManager.instance.chosenTileInteraction);
		}
	}


	public void ChangeTileInteractionHeight(string y)
	{
		int newY;
		bool validFormat = int.TryParse (y, out newY);

		if (validFormat)
		{
			EditorRoomManager.instance.ChangeInteractableHeight (newY, InspectorManager.instance.chosenTileInteraction);
		}
	}


	// change position

	public void ChangeTileInteractionX(string x)
	{
		int newX;
		bool validFormat = int.TryParse (x, out newX);

		if (validFormat)
		{
			EditorRoomManager.instance.ChangeInteractableTileX (newX, InspectorManager.instance.chosenTileInteraction);
		}
	}


	public void ChangeTileInteractionY(string y)
	{
		int newY;
		bool validFormat = int.TryParse (y, out newY);

		if (validFormat)
		{
			EditorRoomManager.instance.ChangeInteractableTileY (newY, InspectorManager.instance.chosenTileInteraction);
		}
	}


	public void DeleteTileInteraction()
	{
		Room room = EditorRoomManager.instance.room;
		TileInteraction tileInt = InspectorManager.instance.chosenTileInteraction;

		// remove from all lists
		room.myTileInteractionList.Remove (tileInt);
		if (room.myMirrorRoom != null)
		{
			room.myMirrorRoom.myTileInteractionList_Persistant.Remove (tileInt);
			room.myMirrorRoom.myTileInteractionList_Shadow.Remove (tileInt);
		}

		Tile tile = EditorRoomManager.instance.room.MyGrid.GetTileAt (tileInt.x, tileInt.y);
		tile.myTileInteraction = null;

		InspectorManager.instance.chosenTileInteraction = null;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}



	// ----- DESTROY ----- //


	public void DestroyTileInspector()
	{
		if (tileInspectorObject != null) 
		{
			Destroy (tileInspectorObject);
		}
	}



}
