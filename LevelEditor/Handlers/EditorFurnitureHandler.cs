using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EditorFurnitureHandler : MonoBehaviour 
{
	


	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_editorNewRoomCreated += FurnitureFactory;
		EventsHandler.cb_editorFurnitureModelChanged += CreateFurnitureObject;
	}


	public void OnDestroy()
	{
		EventsHandler.cb_editorNewRoomCreated -= FurnitureFactory;
		EventsHandler.cb_editorFurnitureModelChanged -= CreateFurnitureObject;
	}



	// Update is called once per frame

	void Update () 
	{
		
	}



	// Placing Furniture in the editor - when building new furniture

	public void PlaceFurniture(Tile tile, string furnitureName)
	{		
		if(furnitureName == null)
		{
			return;
		}

		// If there's already a furniture on this tile, destroy it before creating a new furniture

		Room myRoom = EditorRoomManager.instance.room;

		if (tile.myFurniture != null)
		{
			Furniture oldFurn = tile.myFurniture;
		
			if (myRoom.RoomState == RoomState.Real) 
			{			
				// Real
				myRoom.myFurnitureList.Remove (oldFurn);

			} else {

				if (myRoom.myMirrorRoom.inTheShadow == true) 
				{			
					// Shadow
					myRoom.myMirrorRoom.myFurnitureList_Shadow.Remove (oldFurn);

				} else {

					// Mirror
					myRoom.myFurnitureList.Remove (oldFurn);
				}
			}

			Destroy(EditorRoomManager.instance.furnitureGameObjectMap [oldFurn]);
			EditorRoomManager.instance.furnitureGameObjectMap.Remove (oldFurn);

			foreach (Tile oldTile in myRoom.MyGrid.gridArray) 
			{
				if (oldTile.myFurniture == oldFurn) 
				{
					oldTile.myFurniture = null;
				}
			}
		}

		// create furniture

		Furniture furn = new Furniture (furnitureName, tile.x, tile.y);


		// set default size

		Sprite furnitureSprite = Resources.Load <Sprite> ("Sprites/Furniture/" + furnitureName);
		furn.mySize = new Vector2 (Mathf.Ceil(furnitureSprite.bounds.size.x), 1f);


		// According to room state, add to list


		if (myRoom.RoomState == RoomState.Real) 
		{			
			// Real

			myRoom.myFurnitureList.Add (furn);
		
		} else {

			if (myRoom.myMirrorRoom.inTheShadow == true) 
			{			
				// Shadow

				myRoom.myMirrorRoom.myFurnitureList_Shadow.Add (furn);

			} else {

				// Mirror

				myRoom.myFurnitureList.Add (furn);
			}
		}

		EventsHandler.Invoke_cb_editorFurnitureModelChanged (furn);
		PlaceFurnitureInTiles (furn, myRoom, myRoom.MyGrid);	

	}




	// -- FACTORY -- //

	// when loading a room


	public void FurnitureFactory(Room room)
	{
		//Debug.Log ("FurnitureFactory");

		if (room.RoomState == RoomState.Real) 
		{			
			// -- REAL ROOM -- //

			foreach (Furniture furn in room.myFurnitureList) 
			{				
				EventsHandler.Invoke_cb_editorFurnitureModelChanged (furn);
				PlaceFurnitureInTiles (furn, room, room.myGrid);
			}

		} else {

			// -- MIRROR ROOM GENERAL -- //

			if (room.myMirrorRoom.inTheShadow == true) 
			{
				// SHADOW ROOM

				foreach (Furniture furn in room.myMirrorRoom.myFurnitureList_Shadow) 
				{
					EventsHandler.Invoke_cb_editorFurnitureModelChanged (furn);
					PlaceFurnitureInTiles (furn, room, room.myMirrorRoom.shadowGrid);
				}

			} else {

				// MIRROR ROOM

				foreach (Furniture furn in room.myFurnitureList) 
				{
					EventsHandler.Invoke_cb_editorFurnitureModelChanged (furn);					
					PlaceFurnitureInTiles (furn, room, room.myGrid);
				}
			}

			// PERSISTANT FURNITURE

			foreach (Furniture furn in room.myMirrorRoom.myFurnitureList_Persistant) 
			{
				EventsHandler.Invoke_cb_editorFurnitureModelChanged (furn);				
				PlaceFurnitureInTiles (furn, room, room.myMirrorRoom.shadowGrid);
				PlaceFurnitureInTiles (furn, room, room.myGrid);
			}
		}	
	}




	public void PlaceFurnitureInTiles(Furniture furn, Room room, Grid grid)
	{
		List<Tile> tempTileList = room.GetMyTiles (grid, furn.GetMyCoordsList ());
		Debug.Log ("tile list count" + tempTileList.Count);

		foreach (Tile myTile in tempTileList) 
		{			
			Debug.Log ("tile x " + myTile.x + "y " + myTile.y);
			myTile.myFurniture = furn;
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();

	}




	public void CreateFurnitureObject(Furniture furn)
	{

		//Debug.Log ("CreateFurnitureObject");

		GameObject obj = Utilities.CreateEditorFurnitureGameObject (furn, this.transform);


		if (furn == null) 
		{
			Debug.Log ("furn = null");
		}

		if (EditorRoomManager.instance.furnitureGameObjectMap == null) 
		{
			EditorRoomManager.instance.furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		}

		EditorRoomManager.instance.furnitureGameObjectMap.Add (furn, obj);	


		// populate list of graphic states

		if (furn.graphicStates.Count == 0) 
		{
			furn.graphicStates = Utilities.GetGraphicStateList (furn);
		}

		furn.currentGraphicState = furn.graphicStates [0];


	}

}
