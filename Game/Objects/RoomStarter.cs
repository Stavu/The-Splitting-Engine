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


	// -------- PREPARE ROOM --------//

	public void PrepareRoom(Room room)
	{

		//Debug.Log ("room in the shadow " + room.myMirrorRoom.inTheShadow);


		// If it's a mirror room, check if it's in the shadow

		if (room.myMirrorRoom != null) 
		{
			if (GameManager.userData.roomsVisitedList.Contains (room.myName) == false) 
			{
				// if it's the first time in the room

				//Debug.Log ("first time in room: " + room.myName);

				if (room.myMirrorRoom.inTheShadow_initial) 
				{
					GameManager.userData.AddToRoomsInShadow (room.myName);
				}				
			} 

			if (GameManager.userData.roomsInShadowList.Contains (room.myName) == true) 
			{
				//Debug.Log ("room " + room.myName + "in the shadow list");
				room.myMirrorRoom.inTheShadow = true;
				
			} else {

				//Debug.Log ("room " + room.myName + "not in the shadow list");
				room.myMirrorRoom.inTheShadow = false;
			}

		}


		switch (room.myName) 
		{
			case "doorTest":

				if (GameManager.userData.CheckIfEventExists ("pottery_broke") == true) 
				{
					SoundManager.instance.PlaySound ("pottery_break", 0);
				}
						
				break;

			

			case "bus_stop_mirror":							
					

				if (GameManager.userData.CheckIfEventExists ("opening_scene_done")) 
				{
					if (GameManager.userData.roomsVisitedList.Contains ("pub_inside_mirror")) 
					{
						//Character llehctiM = RoomManager.instance.getCharacterByName ("llehctiM");
						PI_Handler.instance.RoomStarter_Hide_Character ("llehctiM");

					} else {

						Character llehctiM = RoomManager.instance.getCharacterByName ("llehctiM");
						PI_Handler.instance.RoomStarter_Unhide_PI (llehctiM);
					}

				} else {

					PI_Handler.instance.RoomStarter_Hide_Character ("llehctiM");

				}
			
				break;			


			case "pub_inside_mirror":


				if (GameManager.userData.roomsVisitedList.Contains ("pub_inside_mirror")) 
				{
					Character llehctiM = RoomManager.instance.getCharacterByName ("llehctiM");
					PI_Handler.instance.RoomStarter_Unhide_PI (llehctiM);

					PI_Handler.instance.ChangeCurrentGraphicState (llehctiM, "Sitting_with_beer");
				}

				if (GameManager.userData.roomsVisitedList.Contains ("pub_storeroom_mirror")) 
				{
					Debug.Log ("water can");

					// unhide water can

					Furniture water_can_mirror = RoomManager.instance.getFurnitureByName ("water_can_mirror");
					PI_Handler.instance.RoomStarter_Unhide_PI (water_can_mirror);
				}

				if (GameManager.userData.CheckIfEventExists ("pipe_clogged")) 
				{
					Furniture door_toilets_mirror = RoomManager.instance.getFurnitureByName ("door_toilet_mirror");
					PI_Handler.instance.ChangeCurrentGraphicState (door_toilets_mirror, "Out_of_order");

					GameManager.userData.RemoveEventFromList ("door_toilet_mirror_opened");
				}

				break;



			case "pub_storeroom":

				Debug.Log ("storeroom");


				// physical interactables

				Furniture air_vent = RoomManager.instance.getFurnitureByName ("air_vent");
				Character technician = RoomManager.instance.getCharacterByName ("technician");
				Furniture boxes = RoomManager.instance.getFurnitureByName ("boxes");
				Furniture pipe_storeroom = RoomManager.instance.getFurnitureByName ("pipe_storeroom");

				// events

				if (GameManager.userData.CheckIfEventExists ("water_can_spilled")) 
				{
					Debug.Log ("water_spilled");
					PI_Handler.instance.ChangeCurrentGraphicState (air_vent, "Turned_off");
				}

				if (GameManager.userData.CheckIfEventExists ("technician_arrived")) 
				{					
					// un hide technician

					PI_Handler.instance.RoomStarter_Unhide_PI (technician);
					PI_Handler.instance.ChangeCurrentGraphicState (technician, "Working");

					PI_Handler.instance.ChangeCurrentGraphicState (boxes, "With_toolbox");
				}


				if (GameManager.userData.CheckIfEventExists ("technician_left")) 
				{
					// hide technician

					PI_Handler.instance.RoomStarter_Hide_Character ("technician");

					// boxes

					PI_Handler.instance.ChangeCurrentGraphicState (boxes, "Idle");
				}


				if (GameManager.userData.CheckIfEventExists ("pipe_opened")) 
				{
					// pipe

					PI_Handler.instance.ChangeCurrentGraphicState (pipe_storeroom, "Open");
				}


				if (GameManager.userData.CheckIfEventExists ("pipe_clogged")) 
				{
					// pipe

					PI_Handler.instance.ChangeCurrentGraphicState (pipe_storeroom, "Open_with_goo");
				}


				if (GameManager.userData.CheckIfEventExists ("pipe_closed_for_good")) 
				{
					// pipe

					PI_Handler.instance.ChangeCurrentGraphicState (pipe_storeroom, "Closed");
				}


			break;



			case "pub_storeroom_mirror":

				// physical interactables

				Furniture air_vent_mirror = RoomManager.instance.getFurnitureByName ("air_vent_mirror");
				Character technician_reflection = RoomManager.instance.getCharacterByName ("technician_reflection");
				Furniture boxes_mirror = RoomManager.instance.getFurnitureByName ("boxes_mirror");

				// events

				if (GameManager.userData.CheckIfEventExists ("water_can_spilled")) 
				{				
					PI_Handler.instance.ChangeCurrentGraphicState (air_vent_mirror, "Turned_off");
				}

				if (GameManager.userData.CheckIfEventExists ("technician_arrived")) 
				{	
					// unhide technician

					PI_Handler.instance.RoomStarter_Unhide_PI (technician_reflection);
					PI_Handler.instance.ChangeCurrentGraphicState (technician_reflection, "Working");

					PI_Handler.instance.ChangeCurrentGraphicState (boxes_mirror, "With_toolbox");
				}

				if (GameManager.userData.CheckIfEventExists ("technician_left")) 
				{
					// hide technician

					PI_Handler.instance.RoomStarter_Hide_Character ("technician_reflection");

					// boxes

					PI_Handler.instance.ChangeCurrentGraphicState (boxes_mirror, "Idle");
				}

				break;
							

			case "pub_toilet_mirror":
				
				Character man_toilets_mirror = RoomManager.instance.getCharacterByName ("man_toilets_mirror_reflection");
				Character woman_toilets_reflection = RoomManager.instance.getCharacterByName ("woman_toilets_reflection");
				Character old_man_toilets_reflection = RoomManager.instance.getCharacterByName ("old_man_toilets_reflection");

				if (GameManager.userData.CheckIfEventExists ("pipe_clogged")) 
				{
					// hide technician

					PI_Handler.instance.RoomStarter_Hide_Character ("man_toilets_mirror_reflection");
					PI_Handler.instance.RoomStarter_Hide_Character ("woman_toilets_reflection");
					PI_Handler.instance.RoomStarter_Hide_Character ("old_man_toilets_reflection");
				}

				GameManager.instance.inputState = InputState.Dialogue;

				break;



			case "pub_toilet":

				if (GameManager.userData.roomsVisitedList.Contains ("pub_toilet") == false) 
				{
					GameManager.instance.inputState = InputState.Dialogue;
				}

				break;



			case "abandoned_wing_entrance_shadow":

				break;


			case "abandoned_wing_entrance":

				break;	


			case "abandoned_wing_side":
				
				Furniture window_abandoned_to_break = RoomManager.instance.getFurnitureByName ("window_abandoned_to_break");

				// events

				if (GameManager.userData.CheckIfEventExists ("window_broken")) 
				{				
					PI_Handler.instance.ChangeCurrentGraphicState (window_abandoned_to_break, "Broken");
				}

				break;


			case "maze_room_1":

				if (GameManager.userData.CheckIfEventExists ("closet_abandoned_1_opened")) 
				{
					Furniture mirror_closet_abandoned_1 = RoomManager.instance.getFurnitureByName ("mirror_closet_abandoned_1");
					PI_Handler.instance.RoomStarter_Unhide_PI (mirror_closet_abandoned_1);
				}

				if (GameManager.userData.CheckIfEventExists ("map_taken")) 
				{
					PI_Handler.instance.RoomStarter_Hide_Furniture ("map_on_bed");
				}


				break;


			case "maze_room_1_mirror":
				
				if (room.myMirrorRoom != null) // it's not supposed to be null anyway
				{
					if (room.myMirrorRoom.inTheShadow == false) 
					{	
						Furniture closet_abandoned_1_mirror = RoomManager.instance.getFurnitureByName ("closet_abandoned_1_mirror");
						Furniture mirror_closet_abandoned_1_mirror = RoomManager.instance.getFurnitureByName ("mirror_closet_abandoned_1_mirror");

						PI_Handler.instance.ChangeCurrentGraphicState (closet_abandoned_1_mirror, "Open_with_mirror");
						PI_Handler.instance.RoomStarter_Unhide_PI (mirror_closet_abandoned_1_mirror);

					} else {

						Furniture closet_abandoned_1_shadow = RoomManager.instance.getFurnitureByName ("closet_abandoned_1_shadow");
						Furniture mirror_closet_abandoned_1_shadow = RoomManager.instance.getFurnitureByName ("mirror_closet_abandoned_1_shadow");

						PI_Handler.instance.ChangeCurrentGraphicState (closet_abandoned_1_shadow, "Closed");
						PI_Handler.instance.RoomStarter_Hide_Furniture ("mirror_closet_abandoned_1_shadow");
					}
				}

				break;


			case "maze_room_2_mirror":

				Furniture mirror_rotating_mirror = RoomManager.instance.getFurnitureByName ("mirror_rotating_mirror");

				if (room.myMirrorRoom != null) 
				{
					if (room.myMirrorRoom.inTheShadow == false) 
					{ 						
						PI_Handler.instance.ChangeCurrentGraphicState (mirror_rotating_mirror, "Front");

					} else {

						PI_Handler.instance.ChangeCurrentGraphicState (mirror_rotating_mirror, "Back");
					}
				}

				break;



			case "maze_room_3_mirror":

				Furniture mirror_abandoned_3_mirror = RoomManager.instance.getFurnitureByName ("mirror_abandoned_3_mirror");

				if (room.myMirrorRoom != null) 
				{
					if (room.myMirrorRoom.inTheShadow == false) 
					{ 		
						// room is not in the shadows

						Furniture table_abandoned_vase_mirror = RoomManager.instance.getFurnitureByName ("table_abandoned_vase_mirror");

						PI_Handler.instance.ChangeCurrentGraphicState (mirror_abandoned_3_mirror, "Uncovered");
					
						if ((GameManager.userData.CheckIfEventExists ("tablecloth_taken")) && (GameManager.userData.CheckIfEventExists ("tablecloth_back") == false))
						{
							PI_Handler.instance.ChangeCurrentGraphicState (table_abandoned_vase_mirror, "Broken");

						} else {

							PI_Handler.instance.ChangeCurrentGraphicState (table_abandoned_vase_mirror, "Idle");

						}

					} else {

						// room is in the shadows

						Furniture table_abandoned_vase_shadow = RoomManager.instance.getFurnitureByName ("table_abandoned_vase_shadow");

						PI_Handler.instance.ChangeCurrentGraphicState (mirror_abandoned_3_mirror, "Covered");

						if ((GameManager.userData.CheckIfEventExists ("tablecloth_taken")) && (GameManager.userData.CheckIfEventExists ("tablecloth_back") == false))
						{
							PI_Handler.instance.ChangeCurrentGraphicState (table_abandoned_vase_shadow, "Broken");
						
						} else {
							
							PI_Handler.instance.ChangeCurrentGraphicState (table_abandoned_vase_shadow, "Without_map");

						}
					}
				}

				break;


			case "maze_room_4_mirror":

				Debug.Log ("cleaning_fluid_taken " + GameManager.userData.CheckIfEventExists ("cleaning_fluid_taken"));

				Furniture mirror_abandoned_4_mirror = RoomManager.instance.getFurnitureByName ("mirror_abandoned_4_mirror");

				if (room.myMirrorRoom != null) 
				{
					if (room.myMirrorRoom.inTheShadow == false) 
					{ 
						// --- not in the shadows --- //

						// mirror

						PI_Handler.instance.ChangeCurrentGraphicState (mirror_abandoned_4_mirror, "Idle");

						// shelves

						Furniture shelves_abandoned_4_mirror = RoomManager.instance.getFurnitureByName ("mirror_abandoned_4_mirror");

						if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_back_again")) == true) 
						{							
							// cleaning fluid back again

							PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_mirror, "With_cleaning_fluid");

						} else {
													
							if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_taken_again")) == true) 
							{
								// cleaning fluid taken twice

								PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_mirror, "Empty");

							} else {

								if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_back")) == true) 
								{
									// cleaning fluid back once

									PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_mirror, "With_cleaning_fluid");
								
								} else {

									if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_taken")) == true) 
									{
										// cleaning fluid taken once

										Debug.Log("taken once");

										PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_mirror, "Empty");

									} else {

										// cleaning fluid not taken

										PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_mirror, "With_cleaning_fluid");
									}
								}
							}
						}					

					} else {
						
						// --- in the shadows --- //

						Debug.Log("shadow");


						// mirror

						PI_Handler.instance.ChangeCurrentGraphicState (mirror_abandoned_4_mirror, "Covered");

						// shelves

						Furniture shelves_abandoned_4_shadow = RoomManager.instance.getFurnitureByName ("shelves_abandoned_4_shadow");

						if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_back_again")) == true) {							
							// cleaning fluid back again

							PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_shadow, "With_cleaning_fluid_shadow");

						} else {

							if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_taken_again")) == true) 
							{
								// cleaning fluid taken twice

								Debug.Log ("taken twice shadow");

								PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_shadow, "Empty");

							} else {

								if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_back")) == true) {
									
									Debug.Log ("back once shadow");

									// cleaning fluid back once

									PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_shadow, "With_cleaning_fluid_shadow");

								} else {

									if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_taken")) == true) {
										// cleaning fluid taken once

										Debug.Log ("taken once shadow");

										PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_shadow, "Empty");

									} else {

										// cleaning fluid not taken

										Debug.Log ("not taken once shadow");

										PI_Handler.instance.ChangeCurrentGraphicState (shelves_abandoned_4_shadow, "With_cleaning_fluid_shadow");
									}
								}
							}
						}
					}
				}

			break;



			case "maze_room_6":

				if (GameManager.userData.roomsVisitedList.Contains (room.myName) == false) 
				{
					GameManager.userData.AddEventToList ("bench_abandoned_6_0_books");
				}

				Furniture bench_abandoned_6 = RoomManager.instance.getFurnitureByName ("bench_abandoned_6");

				if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_3_books")) 
				{
					PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6, "3_books");
				
				} else {
					
					if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_2_books")) 
					{
						PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6, "2_books");

					} else {

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_1_book")) 
						{
							PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6, "1_book");

						} else {

							if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_0_books")) 
							{
								PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6, "Idle");

							} else {

								Debug.LogError("how many books?");
							}
						}
					}
				}


				// closet

				//Furniture closet_abandoned_6 = RoomManager.instance.getFurnitureByName ("closet_abandoned_6");
				Furniture mirror_closet_abandoned_6 = RoomManager.instance.getFurnitureByName ("mirror_closet_abandoned_6");

				if (GameManager.userData.CheckIfEventExists ("closet_abandoned_6_opened")) 
				{
					PI_Handler.instance.RoomStarter_Unhide_PI (mirror_closet_abandoned_6);
			
				} else {

					PI_Handler.instance.RoomStarter_Hide_Furniture ("mirror_closet_abandoned_6");
				}

			break;



			case "maze_room_6_mirror":

				if (GameManager.userData.roomsVisitedList.Contains (room.myName) == false) 
				{
					GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_0_books");
				}

				Furniture bench_abandoned_6_mirror = RoomManager.instance.getFurnitureByName ("bench_abandoned_6_mirror");

				if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_3_books")) {
					PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6_mirror, "3_books");

				} else {

					if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_2_books")) {
						PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6_mirror, "2_books");

					} else {

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_1_book")) {
							PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6_mirror, "1_book");

						} else {

							if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_0_books")) 
							{
								PI_Handler.instance.ChangeCurrentGraphicState (bench_abandoned_6_mirror, "Idle");

							} else {

								Debug.LogError("how many books?");
							}
						}
					}
				}


				// closet

				Furniture closet_abandoned_6_mirror = RoomManager.instance.getFurnitureByName ("closet_abandoned_6_mirror");
				Furniture mirror_closet_abandoned_6_mirror = RoomManager.instance.getFurnitureByName ("mirror_closet_abandoned_6_mirror");

				if (GameManager.userData.roomsVisitedList.Contains (room.myName) == false) 
				{
					GameManager.userData.AddEventToList("closet_abandoned_6_mirror_opened");
				}

				if (GameManager.userData.CheckIfEventExists ("closet_abandoned_6_mirror_opened")) 
				{
					PI_Handler.instance.ChangeCurrentGraphicState (closet_abandoned_6_mirror, "Open_button");
					PI_Handler.instance.RoomStarter_Unhide_PI (mirror_closet_abandoned_6_mirror);

				} else {
					
					PI_Handler.instance.ChangeCurrentGraphicState (closet_abandoned_6_mirror, "Closed_button");
					PI_Handler.instance.RoomStarter_Hide_Furniture ("mirror_closet_abandoned_6_mirror");
				}

			break;




			case "abandoned_lobby_mirror":			




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
			case "bus_stop_mirror":	

				if (firstTimeinRoom) 
				{
					CutsceneManager.instance.PlayCutscene ("opening_scene");	
				}

				break;



			case "field_shadow2":
				
				if (firstTimeinRoom) 
				{					
					RoomStarterMonologue ("There's nothing in this direction.");
				}

				break;



			case "field_shadow3":
				
				if (firstTimeinRoom) 
				{					
					RoomStarterMonologue ("There's nothing in this direction.");
				}

				break;


			case "abandoned_lobby":

			
				break;


			case "pub_storeroom_mirror":

				//GameManager.userData.AddEventToList ("saw_air_vent");

				break;


			case "pub_toilet_mirror":

				if (firstTimeinRoom)
				{						
					List<string> textList = new List<string> ();
					textList.Add ("Look At all of these people...");
					textList.Add ("I can't possibly pass through the mirror. They'll see me!");

					RoomStarterMonologue (textList);
				}

				if ((GameManager.userData.CheckIfEventExists ("pipe_clogged")) && (GameManager.userData.CheckIfEventExists ("saw_empty_toilets") == false)) 
				{					
					RoomStarterMonologue ("It worked!");

					GameManager.userData.AddEventToList ("saw_empty_toilets");
				}


				break;


			case "pub_toilet":

				if (firstTimeinRoom)
				{						
					List<string> textList = new List<string> ();
					textList.Add ("Oh my god... That smells awful...");
					textList.Add ("It seems like clogging the pipe caused a flood.");

					RoomStarterMonologue (textList);
				}

				break;

		
			case "abandoned_wing_outside_shadow":

				break;


			case "abandoned_wing_entrance":

				break;


			case "maze_room_1":

				if (firstTimeinRoom) 
				{	
					List<string> textList = new List<string> ();
					textList.Add ("Good, I'm inside.");
					textList.Add ("It looks like a patient room.");
					textList.Add ("But this place has been deserted for a long time now.");

					RoomStarterMonologue (textList);
				}

				break;
		


			case "abandoned_lobby_mirror":			
				
				if (GameManager.userData.CheckIfEventExists("met_geM") == false)
				{
					CutsceneManager.instance.PlayCutscene ("meet_geM_scene");
				}


				break;
		
		}
	}




	// no subint

	public static void RoomStarterMonologue(string myText)
	{
		DialogueSentence sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, myText, false);

		List<DialogueSentence> list = new List<DialogueSentence> ();
		list.Add (sentence);

		InteractionManager.instance.DisplayText (list);
	}



	// with list instead of one text

	public static void RoomStarterMonologue(List<string> myTextList)
	{
		List<DialogueSentence> list = new List<DialogueSentence> ();

		foreach (string str in myTextList) 
		{
			DialogueSentence sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, str, false);
			list.Add (sentence);
		}

		InteractionManager.instance.DisplayText (list);
	}




}
