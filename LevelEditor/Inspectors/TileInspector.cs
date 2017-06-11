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

	Text sizeXPlaceholder;
	Text sizeYPlaceholder;
	Text posXPlaceholder;
	Text posYPlaceholder;

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

		Debug.Log("CreateTileInspector");

		InspectorManager.physicalInteractableInspector.DestroyInspector ();
		DestroyTileInspector ();

		tileInspectorObject = Instantiate (InspectorManager.instance.tileInspectorObjectPrefab);


		// Assign 

		panel = tileInspectorObject.transform.Find ("Panel");

		sizeXInput = panel.Find ("SizeX").GetComponent<InputField> ();
		sizeYInput = panel.Find ("SizeY").GetComponent<InputField> ();
		posXInput = panel.Find ("PosX").GetComponent<InputField> ();
		posYInput = panel.Find ("PosY").GetComponent<InputField> ();

		sizeXPlaceholder = panel.Find ("SizeX").Find ("Placeholder").GetComponent<Text> ();
		sizeYPlaceholder = panel.Find ("SizeY").Find ("Placeholder").GetComponent<Text> ();
		posXPlaceholder = panel.Find ("PosX").Find ("Placeholder").GetComponent<Text> ();
		posYPlaceholder = panel.Find ("PosY").Find ("Placeholder").GetComponent<Text> ();

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


		// dialogue

		if (currentTileInteraction.mySubInt != null) 
		{			
			if (currentTileInteraction.mySubInt.RawText != string.Empty) 
			{
				//Debug.Log ("CreateTileInspector: insert raw text" + currentTileInteraction.mySubInt.RawText);

				textInputCheckBox.isOn = true;
				interactionTextInput.interactable = true;

				interactionTextInput.text = currentTileInteraction.mySubInt.RawText;

			} else {

				Debug.Log ("RawText = null");
			}
		}


		// move to room

		if (currentTileInteraction.mySubInt != null) 
		{			
			if (currentTileInteraction.mySubInt.destinationRoomName != string.Empty) 
			{
				enterRoomCheckBox.isOn = true;
				destinationRoomInput.interactable = true;

				destinationRoomInput.text = currentTileInteraction.mySubInt.destinationRoomName;
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

		if (interactionTextInput.interactable == true) 		
		{
			SubInteraction subInteraction = new SubInteraction ("showMonologue");
			subInteraction.RawText = interactionTextInput.text;
			Debug.Log ("raw " + subInteraction.RawText);

			InspectorManager.instance.chosenTileInteraction.mySubInt = subInteraction;
		}


		// create enter room


		if (destinationRoomInput.interactable == true)		
		{
			SubInteraction subInteraction = new SubInteraction ("moveToRoom");
			subInteraction.destinationRoomName = destinationRoomInput.text;

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
		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeInteractableWidth (newX, InspectorManager.instance.chosenTileInteraction);
	}


	public void ChangeTileInteractionHeight(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeInteractableHeight (newY, InspectorManager.instance.chosenTileInteraction);
	}


	// change position

	public void ChangeTileInteractionX(string x)
	{
		int newX = int.Parse (x);
		EditorRoomManager.instance.ChangeInteractableTileX (newX, InspectorManager.instance.chosenTileInteraction);
	}


	public void ChangeTileInteractionY(string y)
	{
		int newY = int.Parse (y);
		EditorRoomManager.instance.ChangeInteractableTileY (newY, InspectorManager.instance.chosenTileInteraction);
	}


	public void DeleteTileInteraction()
	{
		Debug.Log ("delete tile interaction");

		TileInteraction tileInt = InspectorManager.instance.chosenTileInteraction;

		Tile tile = EditorRoomManager.instance.room.MyGrid.GetTileAt (tileInt.x, tileInt.y);

		EditorRoomManager.instance.room.myTileInteractionList.Remove (tileInt);
		tile.myTileInteraction = null;

		InspectorManager.instance.chosenTileInteraction = null;

		EventsHandler.Invoke_cb_tileLayoutChanged ();
		//DestroyTileInspector ();
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
