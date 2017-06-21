using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {



	// Singleton //

	public static BuildController instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

	}

	// Singleton //


	public string furnitureName;
	public string characterName;


	public enum Mode
	{
		buildFurniture,
		buildCharacter,
		buildTileInteraction,
		changeCoords,
		inspect
	}

	public Mode mode = Mode.inspect;



	// Use this for initialization
	public void Initialize () 
	{
		EventsHandler.cb_editorTilesSelected += OnTilesSelected;
	}


	public void OnDestroy()
	{
		EventsHandler.cb_editorTilesSelected -= OnTilesSelected;
	}


	
	// Update is called once per frame
	void Update () 
	{
		
	}



	// ---- TILES SELECTED ---- //


	public void OnTilesSelected(Tile tile)
	{
		/*
		if (tileList.Count == 0) {
			return;
		}
		*/

		if (tile == null) {
			return;
		}


		switch (mode) 
		{		
			case Mode.inspect:

				InspectTiles (tile);
				return;

			
			case Mode.buildFurniture:

				EditorRoomManager.editorFurnitureHandler.PlaceFurniture (tile, furnitureName);

				break;


			case Mode.buildCharacter:

				EditorRoomManager.editorCharacterHandler.PlaceCharacter (tile, characterName);

				break;


			case Mode.buildTileInteraction:

				EditorRoomManager.editorTileInteractionHandler.PlaceTileInteraction (tile);

				break;

			
			case Mode.changeCoords:

				EditorRoomManager.instance.SetPICoords (tile);

				return;		
		}

		// return to inspect mode

		mode = Mode.inspect;
	}



	public void InspectTiles(Tile tile)
	{
		// everyone is null

		InspectorManager.instance.chosenFurniture = null;
		InspectorManager.instance.chosenCharacter = null;
		InspectorManager.instance.chosenTileInteraction = null;


		// inspect tile

		InspectorManager.instance.chosenFurniture = tile.myFurniture;

		if (InspectorManager.instance.chosenFurniture != null) 
		{			
			return;
		}

		InspectorManager.instance.chosenCharacter = tile.myCharacter;

		if (InspectorManager.instance.chosenCharacter != null) 
		{			
			return;
		}

		InspectorManager.instance.chosenTileInteraction = tile.myTileInteraction;
	}


}
