using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStarter : MonoBehaviour {


	// Singleton //

	public static RoomStarter instance { get; protected set; }

	void Awake ()
	{		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //




	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_entered_room += StartRoom;
		EventsHandler.cb_roomCreated += PrepareRoom;
	}
	

	void OnDestroy () 
	{
		EventsHandler.cb_entered_room -= StartRoom;
		EventsHandler.cb_roomCreated -= PrepareRoom;

	}


	// Update is called once per frame

	void Update () 
	{

	}




	// -------- PREPARE ROOM --------//

	public void PrepareRoom(Room room)
	{

		switch (room.myName) 
		{
			case "doorTest":

				if (GameManager.userData.CheckIfEventExists ("pottery_broke") == true) 
				{
					SoundManager.instance.PlaySound ("pottery_break", 0);
				}
						
				break;


			case "abandoned_lobby_mirror":			

				break;


			case "abandoned_wing_outside_shadow":

				break;


			case "abandoned_wing_outside":

				break;

		}

	}





	// -------- START ROOM -------- // 


	public void StartRoom(Room room)
	{

		// adding the room to rooms visited list

		bool firstTimeinRoom = GameManager.userData.AddToRoomsVisited (room.myName);

		switch (room.myName) 
		{
			case "abandoned_lobby":

				InteractionManager.instance.DisplayDialogueOption("enter_room_abandoned_lobby");

				break;


			case "abandoned_lobby_mirror":

				if (firstTimeinRoom) 
				{					
					InteractionManager.instance.DisplayDialogueOption ("enter_room_abandoned_lobby");
				}

				break;


			case "abandoned_wing_outside_shadow":

				break;


			case "abandoned_wing_outside":

				break;

		}
	}








}
