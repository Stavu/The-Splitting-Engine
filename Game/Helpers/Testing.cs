using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

	// Use this for initialization

	void Start () 
	{
		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
		
	}





	// ------- TEST ALL ------- //



	public static void TestAll()
	{
		
		TestRoom (RoomManager.instance.myRoom);

	}



	public static void TestRoom (Room room)
	{

		// Check if all interactions contain subinteractions


		foreach (Furniture furn in room.myFurnitureList) 
		{
			foreach (Interaction interaction in furn.myInteractionList) 
			{				
				if (interaction.subInteractionList.Count == 0) 
				{
					Debug.LogError (string.Format("TestRoom: {0} {1} has no subinteractions.", furn.identificationName, interaction.myVerb));
				}
			}
		}


		foreach (Character character in room.myCharacterList) 
		{
			foreach (Interaction interaction in character.myInteractionList) 
			{				
				if (interaction.subInteractionList.Count == 0) 
				{
					Debug.LogError (string.Format("TestRoom: {0} {1} has no subinteractions.", character.identificationName, interaction.myVerb));
				}
			}
		}


		foreach (TileInteraction tileInt in room.myTileInteractionList) 
		{
			if (tileInt.mySubInt == null) 
			{
				Debug.LogError ("TestRoom: tileInt has no subinteractions.");
			}
		}

		if(room.myMirrorRoom != null)
		{

			List<Vector2> existingPositions = new List<Vector2> ();


			room.myMirrorRoom.myTileInteractionList_Persistant.ForEach (tileInt => {
				if (tileInt.mySubInt == null) {
					Debug.LogError ("TestRoom: tileInt_persistent has no subinteractions.");
				}
				Vector2 position = new Vector2(tileInt.x, tileInt.y);
				if(existingPositions.Contains(position))
				{
					Debug.LogError("there is already a tile interaction at this position");
				}
				existingPositions.Add(position);
			});






		}






	}












}
