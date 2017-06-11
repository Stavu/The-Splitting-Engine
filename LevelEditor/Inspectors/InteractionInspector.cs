using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionInspector : MonoBehaviour {


	// Declarations

	GameObject interactionPanelObject;
	public Interaction loadedInteraction;

	// Prefabs

	GameObject rowObjectPrefab;
	GameObject conditionItemPrefab;

	Transform panel;
	Transform titleRow;
	Transform conditionContainer;

	InputField interactionTitleInput;

	Button interactionConditionButton;

	GameObject addSubIntPanel;
	GameObject intButtonsPanel;

	Button newSubIntButton;

	Button submitButton;
	Button cancelButton;




	// Use this for initialization

	void Start () 
	{

		EventsHandler.cb_conditionAdded += CreateInteractionPanel;
		EventsHandler.cb_subinteractionChanged += CreateInteractionPanel;

		rowObjectPrefab = Resources.Load<GameObject> ("Prefabs/Editor/Row");
		conditionItemPrefab = Resources.Load<GameObject> ("Prefabs/Editor/ConditionText");

		if ((rowObjectPrefab == null) || (conditionItemPrefab == null)) 
		{
			Debug.LogError ("Something is not quite right");
		}
	}


	void OnDestroy()
	{

		EventsHandler.cb_conditionAdded -= CreateInteractionPanel;
		EventsHandler.cb_subinteractionChanged -= CreateInteractionPanel;

	}


	// Update is called once per frame

	void Update () 
	{

	}




	// Create interaction panel

	public void CreateInteractionPanel()
	{

		Debug.Log ("CreateInteractionPanel");

		if (interactionPanelObject != null) 
		{
			Debug.Log ("destroy interactionpanel");
			DestroyInteractionPanel ();
		}

		interactionPanelObject = Instantiate (InspectorManager.instance.interactionPanelObjectPrefab);

		panel = interactionPanelObject.transform.Find ("ScrollView").Find ("Viewport").Find ("Content");

		titleRow = panel.Find ("Row_Title");
		interactionTitleInput = titleRow.Find ("InputTitle").GetComponent<InputField> ();
		conditionContainer = titleRow.Find ("Conditions");

		interactionConditionButton = titleRow.Find("AddConditionButton").GetComponent<Button> ();

		addSubIntPanel = panel.Find ("AddSubIntPanel").gameObject;
		intButtonsPanel = panel.Find ("InteractionButtonsPanel").gameObject;

		newSubIntButton = panel.Find("AddSubIntPanel").Find("AddButton").GetComponent<Button> ();
		cancelButton = panel.Find("InteractionButtonsPanel").Find("CancelButton").GetComponent<Button> ();
		submitButton = panel.Find("InteractionButtonsPanel").Find("SubmitButton").GetComponent<Button> ();


		OpenInteractionPanel ();

	}




	// Opening interaction panel after created

	void OpenInteractionPanel()
	{
		
		//interactionPanelObject.SetActive (true);
			
	
		if (loadedInteraction != null) 
		{


		//	Debug.Log ("list count" + loadedInteraction.subInteractionList.Count);


			// interaction title input field

			interactionTitleInput.text = loadedInteraction.myVerb;


			// insert interaction conditions 

			PopulateConditionContainer (loadedInteraction,conditionContainer);


			// Set title row height


			if (loadedInteraction.conditionList.Count >= 2) 
			{
				Debug.Log ("OpenInteractionPanel: set title row height");

				Rect tempRect = titleRow.GetComponent<RectTransform> ().rect;
				titleRow.GetComponent<RectTransform> ().sizeDelta = new Vector2 (tempRect.width, loadedInteraction.conditionList.Count * 30);
			}


			// Create row for each subinteraction in the interaction's subinterction list



			if (loadedInteraction.subInteractionList.Count > 0) 
			{

				for (int i = 0; i < loadedInteraction.subInteractionList.Count; i++) 
				{

					SubInteraction subInt = loadedInteraction.subInteractionList [i];

					GameObject row = Instantiate (rowObjectPrefab, panel);
					row.name = ("Row_" + i); 


					// Declerations

					Button removeSubIntButton = row.transform.Find("RemoveButton").GetComponent<Button>();
					Button addSubIntConditionButton = row.transform.Find("AddConditionButton").GetComponent<Button>();
					Button editSubIntButton = row.transform.Find("SubIntButton").GetComponent<Button>();

					Transform subIntConditionContainer = row.transform.Find("Conditions");


					// Populate condition container (with condition)

					PopulateConditionContainer (subInt, subIntConditionContainer);


					// Set row height

					if (subInt.conditionList.Count >= 2) 
					{

						Rect tempRect = row.GetComponent<RectTransform> ().rect;
						row.GetComponent<RectTransform> ().sizeDelta = new Vector2 (tempRect.width, subInt.conditionList.Count * 30);


					}


					// Remove subinteraction button

					removeSubIntButton.onClick.AddListener (() => RemoveSubinteraction(subInt));


					// Add Condition button

					addSubIntConditionButton.onClick.AddListener(() => InspectorManager.conditionInspector.CreateConditionPanel(subInt));


					// Edit subinteraction button

					editSubIntButton.transform.Find ("Text").GetComponent<Text> ().text = subInt.interactionType;
					editSubIntButton.onClick.AddListener (() => InspectorManager.subinteractionInspector.CreateSubinteractionPanel (loadedInteraction,subInt));

											
				}
			}


			// Take 2 last rows to bottom

			addSubIntPanel.transform.SetAsLastSibling();
			intButtonsPanel.transform.SetAsLastSibling ();


		} else {

						
			loadedInteraction = new Interaction ();
			Debug.Log ("list count" + loadedInteraction.subInteractionList.Count);


		}



		// If it's relevant both to null / not null interaction

		// Title input 

		interactionTitleInput.onValueChanged.AddListener (ChangeTitleText);



		// New Condition Button 

		interactionConditionButton.onClick.AddListener (() => InspectorManager.conditionInspector.CreateConditionPanel(loadedInteraction));


		// New subinteraction button

		newSubIntButton.onClick.AddListener (() => InspectorManager.subinteractionInspector.CreateSubinteractionPanel (loadedInteraction));
			

		// Cancel button

		cancelButton.onClick.AddListener (DestroyInteractionPanel);


		// Submit button

		submitButton.onClick.AddListener (SubmitInteraction);
	}



	public void ChangeTitleText(string titleName)
	{
		loadedInteraction.myVerb = titleName;
	}


	public void RemoveCondition (IConditionable conditionable, Condition condition)
	{
		conditionable.RemoveConditionFromList (condition);
		CreateInteractionPanel ();
	}


	public void RemoveSubinteraction (SubInteraction subInt)
	{
		loadedInteraction.RemoveSubinteractionFromList (subInt);
		CreateInteractionPanel ();
	}


	public void PopulateConditionContainer(IConditionable conditionable, Transform container)
	{
		for (int i = 0; i < conditionable.ConditionList.Count; i++) 
		{
			GameObject obj = Instantiate (conditionItemPrefab);
			obj.transform.SetParent (container);

			Condition cond = conditionable.ConditionList [i];
			string condString = "";

			switch (cond.myType) 
			{
				case ConditionType.HasItem:

					condString = cond.hasItem;

					break;


				case ConditionType.EventOccured:

					condString = cond.eventOccured;

					break;


				case ConditionType.CharacterInRoom:

					condString = cond.characterInRoom;

					break;


				case ConditionType.LacksItem:

					condString = cond.lacksItem;

					break;


				case ConditionType.EventDidntOccur:

					condString = cond.eventDidntOccur;

					break;


				case ConditionType.CharacterNotInRoom:

					condString = cond.characterNotInRoom;

					break;


				case ConditionType.IsCurrentPlayer:

					condString = cond.playerName;

					break;
			}

			obj.GetComponent<Text> ().text = string.Format ("{0} - {1}", cond.myType, condString);

			// Give the remove button (inside the condition) an event listener

			obj.transform.Find ("Button").GetComponent<Button> ().onClick.AddListener (() => RemoveCondition(conditionable,cond));
		}	
	}


	public void DestroyInteractionPanel()
	{
		if (interactionPanelObject != null) 
		{
			Destroy(interactionPanelObject);
		}
	}


	public List<string> LoadInventoryItems()
	{
		Sprite[] itemSprites = Resources.LoadAll<Sprite> ("Sprites/Inventory/Small_items");
		List<string> itemStringList = new List<string> ();

		//List<Dropdown.OptionData> itemDataList = new List<Dropdown.OptionData> ();

		foreach (Sprite spr in itemSprites) 
		{
			//Dropdown.OptionData data = new Dropdown.OptionData (spr.name, spr);		
			itemStringList.Add (spr.name);
		}

		return itemStringList;
	}


	public void SubmitInteraction()
	{
		// create interaction 

		Interaction interaction;
		interaction = loadedInteraction;

		PhysicalInteractable currentPhysicalInteractable = null;

		if (InspectorManager.instance.chosenFurniture != null) 
		{
			currentPhysicalInteractable = InspectorManager.instance.chosenFurniture;

		} else if (InspectorManager.instance.chosenCharacter != null) 
		{
			currentPhysicalInteractable = InspectorManager.instance.chosenCharacter;

		}

		// If this is a new interaction, add to interaction list

		if (currentPhysicalInteractable != null)
		{
			if (currentPhysicalInteractable.myInteractionList.Contains (interaction) == false) 
			{
				currentPhysicalInteractable.myInteractionList.Add (interaction);
			} 
		}

		DestroyInteractionPanel ();


		// Refreshing inspector 

		InspectorManager.physicalInteractableInspector.DestroyInspector ();
		InspectorManager.physicalInteractableInspector.CreateInspector (currentPhysicalInteractable);


	}




}
