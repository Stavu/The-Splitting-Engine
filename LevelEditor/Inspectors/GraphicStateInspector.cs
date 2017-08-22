using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GraphicStateInspector : MonoBehaviour {

	// Declerations

	GameObject graphicStateInspectorObject;

	Transform panel;

	Dropdown graphicStateDropdown;

	InputField frameExtentsXInput;
	InputField frameExtentsYInput;
	InputField frameOffsetXInput;
	InputField frameOffsetYInput;

	Text frameExtentsXPlaceholder;
	Text frameExtentsYPlaceholder;
	Text frameOffsetXPlaceholder;
	Text frameOffsetYPlaceholder;

	Toggle changeCoordsToggle;
	Button closeButton;


	public void CreateGraphicStatePanel(PhysicalInteractable physicalInteractable, int i = 0)
	{		
		if (graphicStateInspectorObject != null) 
		{
			return;
		}

		graphicStateInspectorObject = Instantiate(Resources.Load<GameObject> ("Prefabs/Editor/InteractionPanelPrefabs/GraphicStatePanel"));

		panel = graphicStateInspectorObject.transform.Find ("Panel");

		graphicStateDropdown = panel.Find ("GraphicStateDropdown").GetComponent<Dropdown> ();

		frameExtentsXInput = panel.Find ("FrameExtentsXInput").GetComponent<InputField> ();
		frameExtentsYInput = panel.Find ("FrameExtentsYInput").GetComponent<InputField> ();
		frameOffsetXInput = panel.Find ("FrameOffsetXInput").GetComponent<InputField> ();
		frameOffsetYInput = panel.Find ("FrameOffsetYInput").GetComponent<InputField> ();

		frameExtentsXPlaceholder = panel.Find ("FrameExtentsXInput").Find("Placeholder").GetComponent<Text> ();
		frameExtentsYPlaceholder = panel.Find ("FrameExtentsYInput").Find("Placeholder").GetComponent<Text> ();
		frameOffsetXPlaceholder = panel.Find ("FrameOffsetXInput").Find("Placeholder").GetComponent<Text> ();
		frameOffsetYPlaceholder = panel.Find ("FrameOffsetYInput").Find("Placeholder").GetComponent<Text> ();

		changeCoordsToggle = panel.Find ("ChangeCoordsToggle").GetComponent<Toggle> ();
		closeButton = panel.Find ("CloseButton").GetComponent<Button> ();
	
		// OPENING //

		GameObject obj = EditorRoomManager.instance.GetPhysicalInteractableGameObject (physicalInteractable);

		List<string> optionList = Utilities.GetAnimationClipNames (obj);

		if (optionList.Count == 0) 
		{
			optionList.Add ("default");		
		} 

		graphicStateDropdown.AddOptions(optionList);
		graphicStateDropdown.value = i;

		frameExtentsXPlaceholder.text = physicalInteractable.currentGraphicState.frameExtents.x.ToString ();
		frameExtentsYPlaceholder.text = physicalInteractable.currentGraphicState.frameExtents.y.ToString();

		frameOffsetXPlaceholder.text = physicalInteractable.currentGraphicState.frameOffsetX.ToString();
		frameOffsetYPlaceholder.text = physicalInteractable.currentGraphicState.frameOffsetY.ToString();


		// Listeners

		graphicStateDropdown.onValueChanged.AddListener (ChangeCurrentGraphicState);

		frameExtentsXInput.onEndEdit.AddListener (ChangeFrameWidth);
		frameExtentsYInput.onEndEdit.AddListener (ChangeFrameHeight);

		frameOffsetXInput.onEndEdit.AddListener (ChangeFrameOffsetX);
		frameOffsetYInput.onEndEdit.AddListener (ChangeFrameOffsetY);

		changeCoordsToggle.onValueChanged.AddListener (SetChangeCoordsMode);
		closeButton.onClick.AddListener (DestroyGraphicStatePanel);
	}

	public void ChangeCurrentGraphicState(int i)
	{		
		string stateName = graphicStateDropdown.options [i].text;

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			EditorRoomManager.instance.ChangeInteractableCurrentGraphicState (stateName, InspectorManager.instance.chosenFurniture);
			DestroyGraphicStatePanel ();
			CreateGraphicStatePanel (InspectorManager.instance.chosenFurniture , i);

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{	
			EditorRoomManager.instance.ChangeInteractableCurrentGraphicState (stateName, InspectorManager.instance.chosenCharacter);
			DestroyGraphicStatePanel ();
			CreateGraphicStatePanel (InspectorManager.instance.chosenCharacter , i);

		}
	}

	// chane coords 

	public void SetChangeCoordsMode(bool isInCoordsMode)
	{

		if (isInCoordsMode) 
		{			
			BuildController.instance.mode = BuildController.Mode.changeCoords;
		
		} else {

			BuildController.instance.mode = BuildController.Mode.inspect;
		}

	}

	// frame size

	public void ChangeFrameWidth(string width)
	{
		float newWidth;

		bool validFormat = float.TryParse (width, out newWidth);

		if (validFormat == true)
		{
			if (InspectorManager.instance.chosenFurniture != null)
			{
				EditorRoomManager.instance.ChangeInteractableFrameWidth (newWidth, InspectorManager.instance.chosenFurniture);
			}
			else if (InspectorManager.instance.chosenCharacter != null)
			{			
				EditorRoomManager.instance.ChangeInteractableFrameWidth (newWidth, InspectorManager.instance.chosenCharacter);
			}	

			frameExtentsXInput.text = width;
		}
	}

	public void ChangeFrameHeight(string height)
	{

		float newHeight;
		bool validFormat = float.TryParse (height, out newHeight);

		if (validFormat == true)
		{
			if (InspectorManager.instance.chosenFurniture != null)
			{
				EditorRoomManager.instance.ChangeInteractableFrameHeight (newHeight, InspectorManager.instance.chosenFurniture);

			}
			else if (InspectorManager.instance.chosenCharacter != null)
			{			
				EditorRoomManager.instance.ChangeInteractableFrameHeight (newHeight, InspectorManager.instance.chosenCharacter);
			}	

			frameExtentsYInput.text = height;
		}

	}

	// change offset


	public void ChangeFrameOffsetX(string x)
	{

		float newX;

		bool validFormat = float.TryParse (x, out newX);

		if (validFormat)
		{
			if (InspectorManager.instance.chosenFurniture != null)
			{
				EditorRoomManager.instance.ChangeInteractableFrameOffsetX (newX, InspectorManager.instance.chosenFurniture);

			}
			else if (InspectorManager.instance.chosenCharacter != null)
			{			
				EditorRoomManager.instance.ChangeInteractableFrameOffsetX (newX, InspectorManager.instance.chosenCharacter);
			}

			frameOffsetXInput.text = x;
		}
	}

	public void ChangeFrameOffsetY(string y)
	{
		float newY;
		bool validFormat = float.TryParse (y, out newY);

		if (validFormat)
		{
			if (InspectorManager.instance.chosenFurniture != null)
			{
				EditorRoomManager.instance.ChangeInteractableFrameOffsetY (newY, InspectorManager.instance.chosenFurniture);

			}
			else if (InspectorManager.instance.chosenCharacter != null)
			{			
				EditorRoomManager.instance.ChangeInteractableFrameOffsetY (newY, InspectorManager.instance.chosenCharacter);
			}	

			frameOffsetYInput.text = y;
		}
	}

	// Close //

	public void DestroyGraphicStatePanel()
	{
		if (graphicStateInspectorObject != null) 
		{
			Destroy (graphicStateInspectorObject);	
			BuildController.instance.mode = BuildController.Mode.inspect;
			graphicStateInspectorObject = null;
		}
	}

}


