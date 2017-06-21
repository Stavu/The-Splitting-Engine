using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConditionInspector : MonoBehaviour {


	// Declerations


	GameObject conditionPanelObject;

	Transform panel;

	Dropdown conditionTypeDropdown;
	InputField stringInput;

	Button cancelButton;
	Button submitButton;

	IConditionable conditionable;





	// Use this for initialization

	void Start () 
	{
		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}


	// Creating condition panel 


	public void CreateConditionPanel(IConditionable iConditionable)
	{

		if (conditionPanelObject != null) 
		{		
			return;
		}

		conditionable = iConditionable;

		conditionPanelObject = Instantiate(Resources.Load<GameObject> ("Prefabs/Editor/InteractionPanelPrefabs/ConditionPanel"));

		panel = conditionPanelObject.transform.Find ("Panel");

		conditionTypeDropdown = panel.Find ("ConditionTypeDropdown").GetComponent<Dropdown> ();
		stringInput = panel.Find ("StringInput").GetComponent<InputField> ();

		cancelButton = panel.Find ("CancelButton").GetComponent<Button> ();
		submitButton = panel.Find ("SubmitButton").GetComponent<Button> ();


		// Populate values


		// Dropdown

		List<string> conditionTypeList = new List<string>();

		foreach (ConditionType condType in Enum.GetValues(typeof(ConditionType))) 
		{
			conditionTypeList.Add (condType.ToString ());

		}

		conditionTypeDropdown.AddOptions (conditionTypeList);


		// Buttons

		cancelButton.onClick.AddListener (DestroyConditionInspector);
		submitButton.onClick.AddListener (SubmitCondition);

	
	}




	// Cancel //

	public void DestroyConditionInspector()
	{
		if (conditionPanelObject != null) 
		{
			Destroy (conditionPanelObject);
			conditionable = null;
		}
	}





	// Submit //

	public void SubmitCondition()
	{


		if (conditionable == null) 
		{
			Debug.LogError ("There is no conditionable");
			return;	
		
		}


		// Creating condition

		ConditionType condType = (ConditionType)conditionTypeDropdown.value;

		string condString = stringInput.text;

		Condition condition = new Condition (condType, condString);

		conditionable.ConditionList.Add (condition);
		EventsHandler.Invoke_cb_conditionAdded ();
		Destroy (conditionPanelObject);

	}





}
