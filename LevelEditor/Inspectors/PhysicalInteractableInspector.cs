using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalInteractableInspector : MonoBehaviour {


	GameObject inspectorObjectPrefab;
	GameObject inspectorObject;

	Transform panel;

	Text fileNameText;
	InputField identificationText;

	Toggle imageFlippedToggle;
	Toggle persistentToggle;

	// inputs

	InputField sizeXInput;
	InputField sizeYInput;

	InputField posXInput;
	InputField posYInput;

	InputField offsetXInput;
	InputField offsetYInput;


	// placeholders

	Text sizeXPlaceholder;
	Text sizeYPlaceholder;

	Text posXPlaceholder;
	Text posYPlaceholder;

	Text offsetXPlaceholder;
	Text offsetYPlaceholder;


	// grpahic state panel button

	Button graphicStatePanelButton;

	// delete button

	Button deleteButton;



	// Use this for initialization

	void Start () 
	{
		inspectorObjectPrefab = Resources.Load<GameObject> ("Prefabs/Editor/InteractionPanelPrefabs/Inspector");		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}


	// INSPECTOR //


	public void CreateInspector(PhysicalInteractable currentPhysicalInteractable)
	{
		Debug.Log ("createInspector");

		DestroyInspector ();
		inspectorObject = Instantiate (inspectorObjectPrefab);


		// Assign 

		panel = inspectorObject.transform.Find ("Panel");

		fileNameText = panel.Find ("Name").GetComponent<Text> ();
		identificationText = panel.Find ("IdentificationName").GetComponent<InputField> ();

		imageFlippedToggle = panel.Find ("ImageFlippedToggle").GetComponent<Toggle> ();
		persistentToggle = panel.Find ("PersistentToggle").GetComponent<Toggle> ();

		graphicStatePanelButton = panel.Find ("OpenGraphicStatePanelButton").GetComponent<Button> ();
		deleteButton = panel.Find ("DeleteButton").GetComponent<Button> ();

		sizeXInput = panel.Find ("SizeX").GetComponent<InputField> ();
		sizeYInput = panel.Find ("SizeY").GetComponent<InputField> ();

		posXInput = panel.Find ("PosX").GetComponent<InputField> ();
		posYInput = panel.Find ("PosY").GetComponent<InputField> ();

		offsetXInput = panel.Find ("OffsetX").GetComponent<InputField> ();
		offsetYInput = panel.Find ("OffsetY").GetComponent<InputField> ();


		// placeholders 

		sizeXPlaceholder = panel.Find ("SizeX").Find ("Placeholder").GetComponent<Text> ();
		sizeYPlaceholder = panel.Find ("SizeY").Find("Placeholder").GetComponent<Text> ();

		posXPlaceholder = panel.Find ("PosX").Find ("Placeholder").GetComponent<Text> ();
		posYPlaceholder = panel.Find ("PosY").Find ("Placeholder").GetComponent<Text> ();

		offsetXPlaceholder = panel.Find ("OffsetX").Find ("Placeholder").GetComponent<Text> ();
		offsetYPlaceholder = panel.Find ("OffsetY").Find ("Placeholder").GetComponent<Text> ();


		// Text

		fileNameText.text = currentPhysicalInteractable.fileName;
		identificationText.text = currentPhysicalInteractable.identificationName;

		sizeXPlaceholder.text = currentPhysicalInteractable.mySize.x.ToString();
		sizeYPlaceholder.text = currentPhysicalInteractable.mySize.y.ToString();

		posXPlaceholder.text = currentPhysicalInteractable.x.ToString();
		posYPlaceholder.text = currentPhysicalInteractable.y.ToString();
			
		offsetXPlaceholder.text = currentPhysicalInteractable.offsetX.ToString();
		offsetYPlaceholder.text = currentPhysicalInteractable.offsetY.ToString();


		// Listeners

		identificationText.onEndEdit.AddListener (ChangeIdentificationName);

		sizeXInput.onEndEdit.AddListener (changeWidth);
		sizeYInput.onEndEdit.AddListener (changeHeight);
	
		posXInput.onEndEdit.AddListener (changeX);
		posYInput.onEndEdit.AddListener (changeY);

		offsetXInput.onEndEdit.AddListener (changeOffsetX);
		offsetYInput.onEndEdit.AddListener (changeOffsetY);

		//int graphicState_i = currentPhysicalInteractable.graphicStates.IndexOf (currentPhysicalInteractable.currentGraphicState);

		graphicStatePanelButton.onClick.AddListener (() => InspectorManager.graphicStateInspector.CreateGraphicStatePanel (currentPhysicalInteractable, currentPhysicalInteractable.graphicStates.IndexOf (currentPhysicalInteractable.currentGraphicState)));
		deleteButton.onClick.AddListener (() => EditorUI.DisplayAlert("Are you sure?", DeletePhysicalInteractable));


		// Toggle 

		if (currentPhysicalInteractable is Furniture) 
		{
			Furniture furn = (Furniture)currentPhysicalInteractable;

			imageFlippedToggle.interactable = true;
			imageFlippedToggle.isOn = furn.imageFlipped;

			imageFlippedToggle.onValueChanged.AddListener (SetImageFlipped);
						
		} else {

			imageFlippedToggle.interactable = false;
		}


		// Persistent - only if furniture

		if (currentPhysicalInteractable is Furniture) 
		{

			Furniture furn = (Furniture)currentPhysicalInteractable;

			if (EditorRoomManager.instance.room.RoomState == RoomState.Real) 
			{
				persistentToggle.interactable = false;

			} else {

				persistentToggle.interactable = true;

				if (EditorRoomManager.instance.room.myMirrorRoom.myFurnitureList_Persistant.Contains (furn)) 
				{
					persistentToggle.isOn = true;

				} else {

					persistentToggle.isOn = false;
				}
			}

			persistentToggle.onValueChanged.AddListener (FurniturePersistantToggleClicked);

		} else {
			
			persistentToggle.interactable = false;
		}


		// create existing interactions

		for (int i = 0; i < 6; i++) {

			Button button = panel.Find ("AddInteraction" + i.ToString ()).GetComponent<Button> ();

			if (currentPhysicalInteractable.myInteractionList.Count > i) 
			{
				button.transform.Find ("Text").GetComponent<Text> ().text = currentPhysicalInteractable.myInteractionList [i].myVerb;
				Interaction interaction = currentPhysicalInteractable.myInteractionList [i];
				button.onClick.AddListener (() => LoadInteractionAndOpenPanel (interaction));	

			} else {

				button.onClick.AddListener (() => LoadInteractionAndOpenPanel (null));
			}
		}
	}



	public void LoadInteractionAndOpenPanel(Interaction interaction)
	{
		InspectorManager.interactionInspector.loadedInteraction = interaction;

		InspectorManager.interactionInspector.CreateInteractionPanel ();
	}


	public void DestroyInspector()
	{	
		if (inspectorObject != null) 
		{
			Destroy (inspectorObject);
		}

		InspectorManager.interactionInspector.DestroyInteractionPanel ();
	}




	// --------- EDITING --------- // 



	// identification

	public void ChangeIdentificationName (string name)
	{
		if (InspectorManager.instance.chosenFurniture != null) 
		{
			if (name == string.Empty) 
			{
				InspectorManager.instance.chosenFurniture.identificationName = InspectorManager.instance.chosenFurniture.fileName;

			} else {			

				InspectorManager.instance.chosenFurniture.identificationName = name;
			}

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			if (name == string.Empty) 
			{
				InspectorManager.instance.chosenCharacter.identificationName = InspectorManager.instance.chosenCharacter.fileName;

			} else {			

				InspectorManager.instance.chosenCharacter.identificationName = name;
			}
		}
	}


	// image flipping 

	public void SetImageFlipped(bool isFlipped)
	{
		Furniture furn = InspectorManager.instance.chosenFurniture;

		furn.imageFlipped = isFlipped;
		EditorRoomManager.instance.furnitureGameObjectMap [furn].GetComponent<SpriteRenderer> ().flipX = isFlipped;

		changeOffsetX ((-furn.offsetX).ToString ());
	}



	// persistency

	public void FurniturePersistantToggleClicked(bool isPersistent)
	{
		Furniture furn = InspectorManager.instance.chosenFurniture;

		EditorRoomHelper.SetFurniturePersistency (isPersistent,furn);
	}


	// change size

	public void changeWidth(string x)
	{
		int newX = int.Parse (x);

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableWidth (newX, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableWidth (newX, InspectorManager.instance.chosenCharacter);
		}
	}



	public void changeHeight(string y)
	{
		int newY = int.Parse (y);

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableHeight (newY, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableHeight (newY, InspectorManager.instance.chosenCharacter);
		}
	}



	// change position

	public void changeX(string x)
	{
		int newX = int.Parse (x);

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileX (newX, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileX (newX, InspectorManager.instance.chosenCharacter);
		}
	}


	public void changeY(string y)
	{
		int newY = int.Parse (y);

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileY (newY, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableTileY (newY, InspectorManager.instance.chosenCharacter);
		}
	}



	// change offset

	public void changeOffsetX(string x)
	{
		float newX = float.Parse (x);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableOffsetX (newX, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableOffsetX (newX, InspectorManager.instance.chosenCharacter);
		}

		offsetXInput.text = x;
	}


	public void changeOffsetY(string y)
	{
		float newY = float.Parse (y);

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableOffsetY (newY, InspectorManager.instance.chosenFurniture);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			EditorRoomManager.instance.ChangeInteractableOffsetY (newY, InspectorManager.instance.chosenCharacter);
		}	

		offsetYInput.text = y;
	}




	// ----- DELETE ----- //

	public void DeletePhysicalInteractable()
	{
		Room myRoom;

		// Furniture

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			myRoom = EditorRoomManager.instance.room;
			Furniture furn = InspectorManager.instance.chosenFurniture;
			GameObject obj = EditorRoomManager.instance.furnitureGameObjectMap[furn];
			//Tile tile = myRoom.MyGrid.GetTileAt (furn.x, furn.y);
		
			Destroy (obj.gameObject);
			EditorRoomManager.instance.furnitureGameObjectMap.Remove (furn);


			// Removing from Furniture Lists 

			if (myRoom.myFurnitureList.Contains (furn)) 
			{
				// REAL & MIRROR

				myRoom.myFurnitureList.Remove (furn);
			}

			if (myRoom.roomState == RoomState.Mirror) 
			{
				if (myRoom.myMirrorRoom.myFurnitureList_Persistant.Contains(furn))
				{
					// PERSISTENT

					myRoom.myMirrorRoom.myFurnitureList_Persistant.Remove (furn);
				}

				if (myRoom.myMirrorRoom.myFurnitureList_Shadow.Contains(furn))
				{
					// SHADOW

					myRoom.myMirrorRoom.myFurnitureList_Shadow.Remove (furn);
				}

				// Removing from Tiles

				if (myRoom.myMirrorRoom.shadowGrid != null) 
				{
					foreach (Tile oldTile in myRoom.myMirrorRoom.shadowGrid.gridArray) 
					{
						if (oldTile.myFurniture == furn) {
							oldTile.myFurniture = null;
						}
					}
				}
			}

			foreach (Tile oldTile in myRoom.myGrid.gridArray) 
			{
				if (oldTile.myFurniture == furn) 
				{
					oldTile.myFurniture = null;
				}
			}

			InspectorManager.instance.chosenFurniture = null;
		}



		// Character 

		if (InspectorManager.instance.chosenCharacter != null) 
		{
			myRoom = EditorRoomManager.instance.room;
			Character character = InspectorManager.instance.chosenCharacter;
			GameObject obj = EditorRoomManager.instance.characterGameObjectMap[character];

			Destroy (obj.gameObject);
			EditorRoomManager.instance.characterGameObjectMap.Remove (character);
			myRoom.myCharacterList.Remove (character);
	

			// Removing from Furniture Lists 

			if (myRoom.myCharacterList.Contains (character)) 
			{
				// REAL & MIRROR

				myRoom.myCharacterList.Remove (character);
			}


			if (myRoom.roomState == RoomState.Mirror) 
			{
				// Removing from Tiles

				foreach (Tile oldTile in myRoom.myMirrorRoom.shadowGrid.gridArray) 
				{
					if (oldTile.myCharacter == character) 
					{
						oldTile.myCharacter = null;
					}
				}
			}

			foreach (Tile oldTile in myRoom.myGrid.gridArray) 
			{
				if (oldTile.myCharacter == character) 
				{
					oldTile.myCharacter = null;
				}
			}

			InspectorManager.instance.chosenCharacter = null;
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
		//DestroyInspector ();
	}



}
