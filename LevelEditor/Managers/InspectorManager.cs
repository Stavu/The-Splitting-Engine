using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;



public class InspectorManager : MonoBehaviour {



	// Singleton //

	public static InspectorManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public GameObject interactionPanelObjectPrefab;
	public GameObject tileInspectorObjectPrefab;


	//GameObject interactionPanelObject;
	//GameObject tileInspectorObject;


	public static InteractionInspector interactionInspector;
	public static PhysicalInteractableInspector physicalInteractableInspector;
	public static ConditionInspector conditionInspector;
	public static SubinteractionInspector subinteractionInspector;
	public static TileInspector tileInspector;
	public static GraphicStateInspector graphicStateInspector;

	// public Interaction loadedInteraction;



	// Chosen furniture

	Furniture _chosenFurniture;
	public Furniture chosenFurniture
	{
		get 
		{
			return _chosenFurniture;
		}

		set 
		{
			_chosenFurniture = value;

			if ((_chosenFurniture == null) && (chosenCharacter == null))
			{
				physicalInteractableInspector.DestroyInspector ();
				graphicStateInspector.DestroyGraphicStatePanel ();

			} else if (_chosenFurniture != null)
			{
				physicalInteractableInspector.CreateInspector (_chosenFurniture);
			}
		}
	}



	// Chosen character

	Character _chosenCharacter;
	public Character chosenCharacter
	{
		get 
		{
			return _chosenCharacter;
		}

		set 
		{
			_chosenCharacter = value;

			if ((_chosenCharacter == null) && (chosenFurniture == null))
			{
				physicalInteractableInspector.DestroyInspector ();
				graphicStateInspector.DestroyGraphicStatePanel ();

			} else if (_chosenCharacter != null)
			{
				physicalInteractableInspector.CreateInspector (_chosenCharacter);
			}
		}
	}





	// chosen tile interaction property

	TileInteraction _chosenTileInteraction;
	public TileInteraction chosenTileInteraction
	{
		get 
		{
			return _chosenTileInteraction;
		}

		set 
		{
			_chosenTileInteraction = value;

			if (_chosenTileInteraction == null)  
			{
				//Debug.Log ("destroy tile inspector");
				tileInspector.DestroyTileInspector ();

			} else {

				//Debug.Log ("create tile inspector");
				tileInspector.CreateTileInspector (_chosenTileInteraction);
			}
		}
	}




	// Use this for initialization

	void Start () 
	{	
		
		if (interactionInspector == null) 
		{
			interactionInspector = gameObject.AddComponent<InteractionInspector> ();
		}

		if (physicalInteractableInspector == null) 
		{
			physicalInteractableInspector = gameObject.AddComponent<PhysicalInteractableInspector> ();
		}

		if (conditionInspector == null) 
		{
			conditionInspector = gameObject.AddComponent<ConditionInspector> ();
		}

		if (subinteractionInspector == null) 
		{
			subinteractionInspector = gameObject.AddComponent<SubinteractionInspector> ();
		}

		if (tileInspector == null) 
		{
			tileInspector = gameObject.AddComponent<TileInspector> ();
		}

		if (graphicStateInspector == null) 
		{
			graphicStateInspector = gameObject.AddComponent<GraphicStateInspector> ();
		}

	}


	// Update is called once per frame

	void Update () 
	{
		
	}







	public PhysicalInteractable GetChosenPI()
	{
		
		if (InspectorManager.instance.chosenFurniture != null) 
		{
			return InspectorManager.instance.chosenFurniture;
		
		} else if (InspectorManager.instance.chosenCharacter != null) 
		{			
			return InspectorManager.instance.chosenCharacter;
		}	
			
		return null;

	}




}
