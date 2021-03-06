﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemHelper {



	public static void UseItemOnPhysicalInteractable (InventoryItem item, PhysicalInteractable physicalInt)
	{
		switch (item.fileName) 
		{
			
			
			/* -------- COMPASS -------- */

			case "compass":

				switch (physicalInt.identificationName) 
				{
					case "door_abandoned_main_shadow":
						
						DialogueSentence sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, "This item is no good.", false);
						List<DialogueSentence> list = new List<DialogueSentence> ();
						list.Add (sentence);

						InteractionManager.instance.DisplayText (list);


						// adding an item 

						InventoryItem picture = new InventoryItem ("missing_picture", "Missing Picture");

						GameManager.userData.GetCurrentPlayerData().inventory.AddItem (picture);

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

			break;



			/* -------- ORDER DETAILS -------- */

			case "order_details":				

				switch (physicalInt.identificationName) 
				{

					case "air_vent_mirror":
					case "air_vent":

						ItemOnObjectMonologue ("I don't wanna shred it. My boss is counting on me.");

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;



			
			/* -------- POCKET KNIFE -------- */


			case "pocket_knife":

				switch (physicalInt.identificationName) 
				{
					case "man_water_reflection":

						ItemOnObjectMonologue ("I don't wanna hurt him...");

						break;

					case "air_vent_mirror":

						ItemOnObjectMonologue ("I don't wanna destroy a perfectly good pocket knife.");

						break;

					case "air_vent":

						ItemOnObjectMonologue ("I don't wanna destroy a perfectly good pocket knife.");

						break;

					
					case "pipe_storeroom":						
					case "pipe_storeroom_mirror":
						
						ItemOnObjectMonologue ("I don't think this is the right tool for the job.");

						break;


					case "mirror_abandoned_4_mirror":

						//PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Covered");
						//EventsHandler.Invoke_cb_inputStateChanged ();

						if ((GameManager.userData.CheckIfEventExists ("mirror_abandoned_4_mirror_covered") == true) && (GameManager.userData.CheckIfEventExists ("cloth_cut") == false))
						{

							// monologue

							ItemOnObjectMonologue ("I cut a piece of the cloth.");

							// event

							GameManager.userData.AddEventToList ("cloth_cut");

							// adding an item 

							InventoryItem piece_oh_cloth = new InventoryItem ("piece_of_cloth", "Piece Of Cloth");

							GameManager.userData.GetCurrentPlayerData ().inventory.AddItem (piece_oh_cloth);
						
						} else {

							if (GameManager.userData.CheckIfEventExists ("cloth_cut") == true) 
							{
								ItemOnObjectMonologue ("I already cut a piece of cloth.");
							}

							if (GameManager.userData.CheckIfEventExists ("mirror_abandoned_4_mirror_covered") == false) 
							{
								ItemOnObjectMonologue ("I think the knife would just go through the mirror...");
							}
						}

						break;


						 
					case "fuse_box":						
					case "fuse_box_mirror":

						ItemOnObjectMonologue ("The knife is too big for the keyhole.");

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;
			

			/* -------- MISSING PICTURE -------- */

			case "missing_picture":				

				switch (physicalInt.identificationName) 
				{

					case "air_vent_mirror":

						ItemOnObjectMonologue ("This picture might be a clue. I don't wanna shred it.");

						break;


					case "air_vent":

						ItemOnObjectMonologue ("This picture might be a clue. I don't wanna shred it.");

						break;


					case "door_abandoned_main_shadow":

						DialogueSentence sentence2 = new DialogueSentence (PlayerManager.myPlayer.identificationName, "pic on door.", false);
						List<DialogueSentence> list2 = new List<DialogueSentence> ();
						list2.Add (sentence2);

						InteractionManager.instance.DisplayText (list2);

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;


			

			/* -------- STOREROOM KEY -------- */

			case "key":

				switch (physicalInt.identificationName) 
				{
					
					case "air_vent":
					case "air_vent_mirror":

						ItemOnObjectMonologue ("On the off chance I might need it... I prefer not to.");

						break;


					case "door_kitchen_shadow":

						ItemOnObjectMonologue ("The key Fits!");

						PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Open");
						EventsHandler.Invoke_cb_inputStateChanged ();

						GameManager.userData.AddEventToList ("door_kitchen_shadow_unlocked");
						GameManager.userData.AddEventToList ("door_kitchen_shadow_opened");

						break;


					case "door_storeroom_down":

						List<string> textList = new List<string> ();
						textList.Add ("It doesn't fit.");
						textList.Add ("This key is for the mirror storeroom.");

						ItemOnObjectMonologue (textList);

						break;


					case "pipe_storeroom":						
					case "pipe_storeroom_mirror":

						ItemOnObjectMonologue ("I don't think it's supposed to be opened with a key.");

						break;



					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;



			/* -------- BLANKET -------- */


			case "blanket":

				switch (physicalInt.identificationName) 
				{
					case "air_vent":

						ItemOnObjectMonologue ("It'll just tear the blanket to pieces.");

						break;


					case "air_vent_mirror":
						
						ItemOnObjectMonologue ("It'll just tear the blanket to pieces.");

						break;


					case "man_water_reflection":

						if(GameManager.userData.CheckIfEventExists("water_can_spilled"))
						{
							if (GameManager.userData.CheckIfEventExists ("fork_in_vent")) 
							{						
								Debug.Log ("cover_man_scene");

								CutsceneManager.instance.PlayCutscene ("cover_man_scene");
							
							} else {

								// fork not in vent

								List<string> textList = new List<string> ();
								textList.Add ("If I cover him, he'll ask to turn the AC back on.");
								textList.Add ("I just feel there's something I have to do first.");

								ItemOnObjectMonologue (textList);
							}
						
						} else {

								ItemOnObjectMonologue ("He looks warm enough.");
						}
						
						break;


					default:
						
						RandomItemOnRandomObjectText ();

						break;
				}

				break;



			/* -------- RUSTY FORK -------- */

			case "fork":

				switch (physicalInt.identificationName) 
				{			

					case "door_kitchen_shadow":

						ItemOnObjectMonologue ("Rusty forks don't usually open doors.");

						break;


					case "air_vent":

						ItemOnObjectMonologue ("I placed the fork inside the vent.");

						//PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Open");
						//EventsHandler.Invoke_cb_inputStateChanged ();

						GameManager.userData.AddEventToList ("fork_in_vent");
						GameManager.userData.GetCurrentPlayerData().inventory.RemoveItem ("fork");

						// fork in vent

						PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "With_fork");
						EventsHandler.Invoke_cb_inputStateChanged ();

						break;


					case "air_vent_mirror":

						ItemOnObjectMonologue ("I should place it in the real vent for it to have any effect.");

						break;


					case "pipe_storeroom":						
					case "pipe_storeroom_mirror":
						
						ItemOnObjectMonologue ("I really don't think I could open this pipe with a fork.");

						break;			

					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;





			/* -------- GOO -------- */

			case "goo":

				switch (physicalInt.identificationName) 
				{

					case "pipe_storeroom_mirror":

						List<string> textList = new List<string> ();
						textList.Add ("I would need to open it first if I wanted to clog it.");
						textList.Add ("In the real world, that is.");

						ItemOnObjectMonologue (textList);

						break;


					case "air_vent":						
					case "air_vent_mirror":

						ItemOnObjectMonologue ("The blades are too sharp, They'll cut right through it.");

						break;


					case "pipe_storeroom":
					
						List<string> textList2 = new List<string> ();
						textList2.Add ("There! The pipe is clogged.");
						textList2.Add ("It should have an effect on the toilets here.");

						ItemOnObjectMonologue (textList2);

						PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Open_with_goo");
						EventsHandler.Invoke_cb_inputStateChanged ();

						GameManager.userData.AddEventToList ("pipe_clogged");

						GameManager.userData.GetCurrentPlayerData().inventory.RemoveItem ("goo");

						// missing - sound

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;




			/* -------- PIPE WRENCH -------- */

			case "pipe_wrench":

				switch (physicalInt.identificationName) 
				{

					case "pipe_storeroom_mirror":

						ItemOnObjectMonologue ("I should open the real pipe.");

						break;


					case "pipe_storeroom":

						ItemOnObjectMonologue ("I opened the pipe with the wrench.");

						PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Open");
						EventsHandler.Invoke_cb_inputStateChanged ();

						GameManager.userData.AddEventToList ("pipe_opened");
						GameManager.userData.GetCurrentPlayerData().inventory.RemoveItem ("pipe_wrench");


						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;





			/* -------- STONE -------- */

			case "stone":

				switch (physicalInt.identificationName) 
				{

					case "window_abandoned_to_break":

						ItemOnObjectMonologue ("I feel kinda guilty about this.");

						PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Broken");
						EventsHandler.Invoke_cb_inputStateChanged ();

						GameManager.userData.AddEventToList ("window_broken");
						GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("stone");

						// missing sound

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;



			/* -------- TABLECLOTH -------- */

			case "tablecloth":

				switch (physicalInt.identificationName) 
				{

					case "mirror_abandoned_3_mirror":
						
						PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Covered");
						PI_Handler.instance.SetPIAnimationState ("dresser_abandoned_3_shadow", "With_key");

						if((GameManager.userData.CheckIfEventExists("tablecloth_taken")) && ((GameManager.userData.CheckIfEventExists("tablecloth_back")) == false))
						{
							PI_Handler.instance.SetPIAnimationState ("table_abandoned_vase_shadow", "Broken");

						} else {
							
							PI_Handler.instance.SetPIAnimationState ("table_abandoned_vase_shadow", "Without_map");
						}

						EventsHandler.Invoke_cb_inputStateChanged ();

						GameManager.userData.AddEventToList ("mirror_abandoned_3_mirror_covered");
						GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("tablecloth");

						InteractionManager.instance.ChangeShadowState (true);

						break;


					case "mirror_abandoned_4_mirror":

						PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Covered");

						if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_back_again")) == true) {							

							// cleaning fluid back again

							PI_Handler.instance.SetPIAnimationState ("shelves_abandoned_4_shadow", "With_cleaning_fluid_shadow");

						} else {

							if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_taken_again")) == true) 
							{
								// cleaning fluid taken twice

								PI_Handler.instance.SetPIAnimationState ("shelves_abandoned_4_shadow", "Empty");

							} else {

								if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_back")) == true) {

									// cleaning fluid back once

									PI_Handler.instance.SetPIAnimationState ("shelves_abandoned_4_shadow", "With_cleaning_fluid_shadow");

								} else {

									if ((GameManager.userData.CheckIfEventExists ("cleaning_fluid_taken")) == true) {

										// cleaning fluid taken once

										PI_Handler.instance.SetPIAnimationState ("shelves_abandoned_4_shadow", "Empty");

									} else {

										// cleaning fluid not taken

										PI_Handler.instance.SetPIAnimationState ("shelves_abandoned_4_shadow", "With_cleaning_fluid_shadow");
									}
								}
							}
						}


						EventsHandler.Invoke_cb_inputStateChanged ();

						GameManager.userData.AddEventToList ("mirror_abandoned_4_mirror_covered");
						GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("tablecloth");

						InteractionManager.instance.ChangeShadowState (true);

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;




			/* -------- KEY COPPER MIRROR -------- */

			case "key_copper_mirror":

				switch (physicalInt.identificationName) 
				{

					case "door_abandoned_2_mirror":

						if (GameManager.userData.CheckIfEventExists ("door_abandoned_2_mirror_unlocked") == false) 
						{						

							ItemOnObjectMonologue ("The key fits!");

							PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Open");
							EventsHandler.Invoke_cb_inputStateChanged ();

							GameManager.userData.AddEventToList ("door_abandoned_2_mirror_unlocked");
							GameManager.userData.AddEventToList ("door_abandoned_2_mirror_opened");

							// remove item

							GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("key_copper_mirror");

							// missing - sound

						} else {

							Debug.LogError ("this shouldn't happen - we shoudln't have the key.");

						}

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;



			/* -------- KEY SILVER MIRROR -------- */

			case "key_silver_mirror":

				switch (physicalInt.identificationName) 
				{
					case "door_right_down_maze_corridor_down_shadow":
						
						if (GameManager.userData.CheckIfEventExists ("door_right_down_maze_corridor_down_shadow_unlocked") == false) 
						{	
							ItemOnObjectMonologue ("The key fits!");

							PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Open");
							EventsHandler.Invoke_cb_inputStateChanged ();

							GameManager.userData.AddEventToList ("door_right_down_maze_corridor_down_shadow_unlocked");
							GameManager.userData.AddEventToList ("door_right_down_maze_corridor_down_shadow_opened");

							// remove item

							GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("key_silver_mirror");

							// missing - sound

						} else {

							Debug.LogError ("this shouldn't happen - we shoudln't have the key.");

						}

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;




			/* -------- KEY GOLD MIRROR -------- */

			case "key_gold_mirror":

				switch (physicalInt.identificationName) 
				{
					case "door_abandoned_7_down":

						if (GameManager.userData.CheckIfEventExists ("door_abandoned_7_down_unlocked") == false) 
						{	
							ItemOnObjectMonologue ("It doesn't fit. I think this key is for the mirror room.");


						} else {

							Debug.LogError ("this shouldn't happen - we shoudln't have the key.");

						}

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;





			/* -------- KEY GOLD -------- */

			case "key_gold":

				switch (physicalInt.identificationName) 
				{
					case "door_abandoned_7_down":

						if (GameManager.userData.CheckIfEventExists ("door_abandoned_7_down_unlocked") == false) 
						{	
							ItemOnObjectMonologue ("The key fits!");

							PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Open");
							EventsHandler.Invoke_cb_inputStateChanged ();

							GameManager.userData.AddEventToList ("door_abandoned_7_down_unlocked");
							GameManager.userData.AddEventToList ("door_abandoned_7_down_opened");

							// remove item

							GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("key_gold");

							// missing - sound

						} else {

							Debug.LogError ("this shouldn't happen - we shoudln't have the key.");

						}

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;



			


			/* -------- CLEANING FLUID -------- */


			case "cleaning_fluid":

				switch (physicalInt.identificationName) 
				{

					case "mirror_abandoned_5_mirror":
						
						List<string> textList = new List<string> ();
						textList.Add ("I can't just pour the liquid on the mirror.");
						textList.Add ("I need some kind of cloth to clean with.");

						ItemOnObjectMonologue (textList);

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;




			/* -------- PIECE OF CLOTH -------- */
			

			case "piece_of_cloth":

				switch (physicalInt.identificationName) 
				{
					
					case "mirror_abandoned_5_mirror":
	
						ItemOnObjectMonologue ("I need some kind of cleaning fluid to clean it.");

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;


		
			/* -------- PIECE OF CLOTH WITH CLEANING FLUID -------- */


			case "piece_of_cloth_cleaning_fluid":

				switch (physicalInt.identificationName) 
				{

					case "mirror_abandoned_5_mirror":
						
						if (GameManager.userData.CheckIfEventExists ("mirror_cleaned_once") == false) 
						{	
							// monologue 

							List<string> textList = new List<string> ();
							textList.Add ("Oh no...");
							textList.Add ("There wasn't enough cleaning fluid for the whole mirror.");
							textList.Add ("I need to get some more somehow.");

							ItemOnObjectMonologue (textList);

							// change animation state

							PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "With_half_mold");
							EventsHandler.Invoke_cb_inputStateChanged ();

							// add event

							GameManager.userData.AddEventToList ("mirror_cleaned_once");

							// remove item

							GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("piece_of_cloth_cleaning_fluid");

							// adding an item 

							InventoryItem picture = new InventoryItem ("piece_of_cloth", "Piece Of Cloth");
							GameManager.userData.GetCurrentPlayerData().inventory.AddItem (picture);

						} else {
							
							if (GameManager.userData.CheckIfEventExists ("mirror_cleaned_twice") == false) 
							{									
								ItemOnObjectMonologue ("Great! The mirror is all clean!");

								PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "Idle");
								EventsHandler.Invoke_cb_inputStateChanged ();

								GameManager.userData.AddEventToList ("mirror_cleaned_twice");

								InteractionManager.instance.ChangeShadowState (false);

								// remove item

								GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("piece_of_cloth_cleaning_fluid");
							
							} else {

								Debug.LogError ("the mirror should be clean by now");
							}
						}

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;



			/* -------- BOOK -------- */


			case "book":

				switch (physicalInt.identificationName) 
				{
					case "bench_abandoned_6":

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_3_books") == true) 
						{
							Debug.LogError ("not supposed to happen");

						} else {
							
							if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_2_books") == true) 
							{
								// had 2 books, now has 3 books

								// remove event
								GameManager.userData.RemoveEventFromList ("bench_abandoned_6_2_books");

								// add event
								GameManager.userData.AddEventToList ("bench_abandoned_6_3_books");

								// remove item

								GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book");
								GameManager.userData.AddBookToBench ("book_real");

								// change animation state

								PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "3_books");
								EventsHandler.Invoke_cb_inputStateChanged ();
																							
							} else {

								if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_1_book") == true) 
								{								
									// had 1 book, now has 2 books

									// remove event
									GameManager.userData.RemoveEventFromList ("bench_abandoned_6_1_book");

									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_2_books");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book");
									GameManager.userData.AddBookToBench ("book_real");
								
									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "2_books");
									EventsHandler.Invoke_cb_inputStateChanged ();

								} else {

									// no event - had no books, now has 1 book

									// remove event
									GameManager.userData.RemoveEventFromList ("bench_abandoned_6_0_books");

									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_1_book");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book");
									GameManager.userData.AddBookToBench ("book_real");

									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "1_book");
									EventsHandler.Invoke_cb_inputStateChanged ();
								}
							}

							// remove event
							Debug.Log ("remove book_taken");
							GameManager.userData.RemoveEventFromList ("book_taken");
						}

						break;


					case "bench_abandoned_6_mirror":

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_3_books") == true) 
						{
							Debug.LogError ("not supposed to happen");

						} else {

							if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_2_books") == true) 
							{
								// had 2 books, now has 3 books

								// remove event
								GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_2_books");

								// add event
								GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_3_books");

								// remove item

								GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book");
								GameManager.userData.AddBookToBenchMirror ("book_real");

								// change animation state

								PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "3_books");
								EventsHandler.Invoke_cb_inputStateChanged ();


							} else {

								if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_1_book") == true) {

									// had 1 book, now has 2 books

									// remove event
									GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_1_book");

									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_2_books");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book");
									GameManager.userData.AddBookToBenchMirror ("book_real");

									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "2_books");
									EventsHandler.Invoke_cb_inputStateChanged ();

								} else {

									// had no books, now has 1 book

									// remove event
									GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_0_books");


									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_1_book");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book");
									GameManager.userData.AddBookToBenchMirror ("book_real");

									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "1_book");
									EventsHandler.Invoke_cb_inputStateChanged ();
								}
							}

							// remove event

							Debug.Log ("remove book_taken");
							GameManager.userData.RemoveEventFromList ("book_taken");
						}

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;




			/* -------- BOOK MIRROR -------- */


			case "book_mirror":

				switch (physicalInt.identificationName) 
				{
					case "bench_abandoned_6":

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_3_books") == true) 
						{
							Debug.LogError ("not supposed to happen");

						} else {

							if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_2_books") == true) 
							{
								// had 2 books, now has 3 books

								// remove event
								GameManager.userData.RemoveEventFromList ("bench_abandoned_6_2_books");

								// add event
								GameManager.userData.AddEventToList ("bench_abandoned_6_3_books");

								// remove item

								GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book_mirror");
								GameManager.userData.AddBookToBench ("book_mirror");

								// change animation state

								PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "3_books");
								EventsHandler.Invoke_cb_inputStateChanged ();

							} else {

								if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_1_book") == true) 
								{
									// had 1 book, now has 2 books

									// remove event
									GameManager.userData.RemoveEventFromList ("bench_abandoned_6_1_book");

									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_2_books");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book_mirror");
									GameManager.userData.AddBookToBench ("book_mirror");

									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "2_books");
									EventsHandler.Invoke_cb_inputStateChanged ();

								} else {

									// no event

									Debug.Log ("had 0 book now has 1");

									// had no books, now has 1 book

									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_1_book");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book_mirror");
									GameManager.userData.AddBookToBench ("book_mirror");

									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "1_book");
									EventsHandler.Invoke_cb_inputStateChanged ();

								}
							}

							// remove event

							GameManager.userData.RemoveEventFromList ("book_taken");				

						}

						break;


					case "bench_abandoned_6_mirror":

						if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_3_books") == true) 
						{
							Debug.LogError ("not supposed to happen");

						} else {

							if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_2_books") == true) 
							{
								// had 2 books, now has 3 books

								// remove event
								GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_2_books");

								// add event
								GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_3_books");

								// remove item

								GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book_mirror");
								GameManager.userData.AddBookToBenchMirror ("book_mirror");

								// change animation state

								PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "3_books");
								EventsHandler.Invoke_cb_inputStateChanged ();

							} else {

								if (GameManager.userData.CheckIfEventExists ("bench_abandoned_6_mirror_1_book") == true) {

									// had 1 book, now has 2 books

									// remove event
									GameManager.userData.RemoveEventFromList ("bench_abandoned_6_mirror_1_book");

									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_2_books");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book_mirror");
									GameManager.userData.AddBookToBenchMirror ("book_mirror");

									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "2_books");
									EventsHandler.Invoke_cb_inputStateChanged ();

								} else {

									// no event

									// had no books, now has 1 book

									// add event
									GameManager.userData.AddEventToList ("bench_abandoned_6_mirror_1_book");

									// remove item

									GameManager.userData.GetCurrentPlayerData ().inventory.RemoveItem ("book_mirror");
									GameManager.userData.AddBookToBenchMirror ("book_mirror");

									// change animation state

									PI_Handler.instance.SetPIAnimationState (physicalInt.identificationName, "1_book");
									EventsHandler.Invoke_cb_inputStateChanged ();
								}
							}

							// remove event

							GameManager.userData.RemoveEventFromList ("book_taken");				

						}

						break;


					default:

						RandomItemOnRandomObjectText ();

						break;
				}

				break;















		}

	}










	/* ------ TEXT FUNCTIONS ------ */



	// no subint

	public static void ItemOnObjectMonologue(string myText)
	{
		DialogueSentence sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, myText, false);

		List<DialogueSentence> list = new List<DialogueSentence> ();
		list.Add (sentence);

		InteractionManager.instance.DisplayText (list);
	}


	// with list instead of one text

	public static void ItemOnObjectMonologue(List<string> myTextList)
	{
		List<DialogueSentence> list = new List<DialogueSentence> ();

		foreach (string str in myTextList) 
		{
			DialogueSentence sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, str, false);
			list.Add (sentence);
		}

		InteractionManager.instance.DisplayText (list);
	}



	// randomized sentences for "unassigned" combinations

	public static void RandomItemOnRandomObjectText()
	{
		DialogueSentence sentence = null;
		int rando = UnityEngine.Random.Range (0, 6);

		Debug.Log ("rando" + rando);


		switch (rando) 
		{

			case 1:

				sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, "I don't think it will do any good.", false);


				break;

			case 2:

				sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, "It doesn't make any sense.", false);

				break;


			case 3:
				
				sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, "No.", false);

				break;


			case 4:

				sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, "Why?", false);

				break;


			case 5:

				sentence = new DialogueSentence (PlayerManager.myPlayer.identificationName, "It's not gonna work.", false);

				break;
		}

		List<DialogueSentence> list = new List<DialogueSentence> ();
		list.Add (sentence);

		InteractionManager.instance.DisplayText (list);
	}
}
