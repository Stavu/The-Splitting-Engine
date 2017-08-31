using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene  {


	public string myName;
	public bool isClearToContinue;
	public virtual IEnumerator MyCutscene()
	{
		return null;
	}

	public Cutscene (string myName)
	{
		this.myName = myName;
	}

}



public class OpeningScene : Cutscene
{
	public OpeningScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{
		///////////
		yield return new WaitForSeconds (2);
		///////////

		PlayerManager.instance.PlayerEntersRoom ("Daniel", new Vector2(18, 10));

		Character llehctiM = RoomManager.instance.getCharacterByName ("llehctiM");
		PI_Handler.instance.UnHide_PI (llehctiM);

		//PlayerManager.instance.PlayerEntersRoom ("llehctiM", new Vector2(18, 12));

		///////////

		GameManager.userData.AddEventToList ("opening_scene_done");


		/*

		// -- WHITE BUS ENTERS -- //

		GameObject bus_white = GameObject.Instantiate( Resources.Load<GameObject>("Prefabs/cutscene_objects/bus_white") );

		bus_white.transform.position = new Vector2 (24, -17);

		// Bus EASE-IN
		CutsceneManager.instance.EaseInObject 
		(
			bus_white,					// object to move
			new Vector3 (24, 1, 0), 	// target position
			7f,							// speed
			false,						// destroy on arrival
			true						// invoke clearToContinue
		);

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}


		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 
	


		// --- CHARACTERS ENTER --- //


		Character woman_bus = null;
		foreach (Character character in RoomManager.instance.myRoom.myCharacterList)
		{
			if (character.identificationName == "woman_bus")
			{
				woman_bus = character;
			}
		}

		// Unhiding the character
		PI_Handler.instance.UnHide_PI (woman_bus);

		// woman walks left

		List<Vector2> womanBusPosList = new List<Vector2> 
		{
			new Vector2 (-2, 2)
		};

		Character_Handler.instance.MoveByPath ((Character)PI_Handler.instance.name_PI_map["woman_bus"], womanBusPosList);


		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 




		// --- MAN ENTER --- //


		Character man_bus = null;
		foreach (Character character in RoomManager.instance.myRoom.myCharacterList)
		{
			if (character.identificationName == "man_bus")
			{
				man_bus = character;
			}
		}

		// Unhiding the character
		PI_Handler.instance.UnHide_PI (man_bus);

		// man walks left

		List<Vector2> manBusPosList = new List<Vector2> 
		{
			new Vector2 (-2, 3)
		};

		Character_Handler.instance.MoveByPath ((Character)PI_Handler.instance.name_PI_map["man_bus"], manBusPosList);


		// ------------------- 
		yield return new WaitForSeconds (2);
		// ------------------- 



		// --- DANIEL ENTERS --- //

			
		// Daniel walks down

		List<Vector2> danielPosList = new List<Vector2> 
		{
			new Vector2 (18, 6)
		};

		PlayerManager.instance.PlayerEntersRoom ("Daniel", new Vector2(18, 10));

		Character_Handler.instance.MoveByPath (PlayerManager.instance.GetPlayerByName("Daniel"), danielPosList);

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 
		 
		// Daniel changes animation state


		PI_Handler.instance.SetPIAnimationState ("Daniel", "Idle_right");


		// --- LLEHCTIM ENTERS --- //

		PlayerManager.instance.PlayerEntersRoom ("llehctiM", new Vector2(18, 12));
		PI_Handler.instance.SetPIAnimationState ("llehctiM", "Idle_right");


		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		 	
		// Bus EASE-OUT

		CutsceneManager.instance.EaseOutObject 
		(
			bus_white,					// object to move
			new Vector3 (24, 20, 0), 	// target position
			5f,							// speed
			true,						// if should be destroyed on arrival
			false
		);


		// ------------------- 
		yield return new WaitForSeconds (4);
		// ------------------- 

		// into the shadows

		InteractionManager.instance.ChangeShadowState (true);

		// ------------------- 
		yield return new WaitForSeconds (2);
		// ------------------- 


		// dialogue between daniel and llehctiM
		Debug.Log("list" + RoomManager.instance.nameSpeakerMap.Count);

		InteractionManager.instance.DisplayDialogueOption ("opening_dialogue1");


		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// FIXME - this doesn't work for some reason, it doesn't stop

		Debug.Log ("clear");

		// *** UN-HIDING Physical Interactable ***

		// getting the tree in a very expensive way
		// should make some function inside room manager
		// that gets the furniture by its name
		// because it is not yet created in PI_Handler's namePIMap

		Furniture tree = null;
		foreach (Furniture furn in RoomManager.instance.myRoom.myMirrorRoom.myFurnitureList_Shadow)
		{
			if (furn.identificationName == "tree_shadow5")
			{
				tree = furn;
			}
		}



		// ****************************************


		// out of the shadows

		InteractionManager.instance.ChangeShadowState (false);


		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		// -- RED BUS ENTERS -- //

		GameObject bus_red = GameObject.Instantiate (Resources.Load<GameObject>("Prefabs/cutscene_objects/bus_red"));

		bus_red.transform.position = new Vector2 (24, -17);
		Vector3 nextPos_bus_red = new Vector3 (24, 20, 0);

		float bus_red_speed = 12;

		CutsceneManager.instance.MoveInConstantSpeed (bus_red, nextPos_bus_red, bus_red_speed, true, false);

		// ANIMATION //


		// ------------------- 
		yield return new WaitForSeconds (3);
		// -------------------


		// into the shadows

		InteractionManager.instance.ChangeShadowState (true);


		// ------------------- 
		yield return new WaitForSeconds (1);
		// -------------------

		// dialogue 2

		InteractionManager.instance.DisplayDialogueOption ("opening_dialogue2");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////
		yield return new WaitForSeconds (1);
		///////////
		 

		// out of the shadows

		InteractionManager.instance.ChangeShadowState (false);


		InteractionManager.instance.DisplayInfoText ("3 Hours Later");

		// -- RED BUS ENTERS AGAIN -- //

		bus_red = GameObject.Instantiate (Resources.Load<GameObject>("Prefabs/cutscene_objects/bus_red"));

		bus_red.transform.position = new Vector2 (24, -17);

		CutsceneManager.instance.MoveInConstantSpeed (bus_red, nextPos_bus_red, bus_red_speed, true, false);

		///////////
		yield return new WaitForSeconds (3);
		///////////

		// into the shadows

		InteractionManager.instance.ChangeShadowState (true);


		// dialogue 3

		InteractionManager.instance.DisplayDialogueOption ("opening_dialogue3");
			

		*/

		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

	}
}



// ---------------------------------------------------------------------------------------------- //



public class WaterCanScene : Cutscene
{
	public WaterCanScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{


		///////////

		PI_Handler.instance.SetPIAnimationState ("water_can_mirror", "Spilled");
		PI_Handler.instance.SetPIAnimationState ("man_water_reflection", "Wet");


		InteractionManager.instance.DisplayDialogueOption ("water_can_spilled_dialogue1");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////


		PI_Handler.instance.SetPIAnimationState ("air_conditioner_mirror", "AC_Off");

		// missing - sound

		//////////


		InteractionManager.instance.DisplayDialogueOption ("water_can_spilled_dialogue2");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////

		GameManager.userData.AddEventToList ("water_can_spilled");


		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();
	}
}


// ---------------------------------------------------------------------------------------------- //



public class CoverManScene : Cutscene
{
	public CoverManScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{		

		// -- COVER MAN -- //

		GameManager.userData.GetCurrentPlayerData().inventory.RemoveItem ("blanket");

		PI_Handler.instance.SetPIAnimationState ("man_water_reflection", "With_blanket");


		// -- DIALOGUE -- //

		InteractionManager.instance.DisplayDialogueOption ("cover_man_with_blanket_dialogue1");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////


		PI_Handler.instance.SetPIAnimationState ("air_conditioner_mirror", "AC_On");

		// missing - sound

		//////////


		InteractionManager.instance.DisplayDialogueOption ("cover_man_with_blanket_dialogue2");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////


		PI_Handler.instance.SetPIAnimationState ("air_conditioner_mirror", "AC_Off");

		InteractionManager.instance.DisplayDialogueOption ("cover_man_with_blanket_dialogue3");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////

		// --- TECHNICIAN ENTERS --- //


		InteractionManager.instance.DisplayInfoText ("30 Minutes Later");

		Character technician_reflection = null;
		foreach (Character character in RoomManager.instance.myRoom.myCharacterList)
		{
			if (character.identificationName == "technician_reflection")
			{
				technician_reflection = character;
			}
		}

		// Unhiding the character

		PI_Handler.instance.UnHide_PI (technician_reflection);


		// technician walks up

		PI_Handler.instance.SetPIAnimationState ("technician_reflection", "Walk_back");

		List<Vector2> technicianPosList = new List<Vector2> 
		{
			new Vector2 (16, 13),
			new Vector2 (3, 13)
		};

		Character_Handler.instance.MoveByPath ((Character)PI_Handler.instance.name_PI_map["technician_reflection"], technicianPosList);

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////

		// --- TECHNICIAN DISAPPEARS --- //

		PI_Handler.instance.Hide_PI ("technician_reflection");


		// EVENTS //

		GameManager.userData.AddEventToList ("man_water_covered");
		GameManager.userData.AddEventToList ("air_vent_turned_off");
		GameManager.userData.AddEventToList ("technician_arrived");


		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();




	}

}




// ---------------------------------------------------------------------------------------------- //



public class ToolboxScene : Cutscene
{
	public ToolboxScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{


		// adding an item 


		InventoryItem pipe_wrench = new InventoryItem ("pipe_wrench", "Pipe Wrench");
		GameManager.userData.GetCurrentPlayerData().inventory.AddItem (pipe_wrench);


		///////////

		InteractionManager.instance.DisplayDialogueOption ("technician_leaves_dialogue");

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////


		PI_Handler.instance.SetPIAnimationState ("boxes_mirror", "Idle");


		// technician walks down

		PI_Handler.instance.SetPIAnimationState ("technician_reflection", "Walk_down");


		List<Vector2> technicianPosList = new List<Vector2> 
		{
			new Vector2 (6, 1),
		};

		Character_Handler.instance.MoveByPath ((Character)PI_Handler.instance.name_PI_map["technician_reflection"], technicianPosList);

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////

		// technician disappears

		PI_Handler.instance.Hide_PI ("technician_reflection");	

		//////////


		GameManager.userData.AddEventToList ("technician_left");



		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();


	}

}




// ---------------------------------------------------------------------------------------------- //


public class BusToAsylumScene : Cutscene
{
	public BusToAsylumScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{


		// -- RED BUS ENTERS -- //


		GameObject bus_red = GameObject.Instantiate( Resources.Load<GameObject>("Prefabs/cutscene_objects/bus_red") );

		bus_red.transform.position = new Vector2 (5, -17);

		// Bus EASE-IN
		CutsceneManager.instance.EaseInObject 
		(
			bus_red,					// object to move
			new Vector3 (5, 1, 0), 	    // target position
			7f,							// speed
			false,						// destroy on arrival
			true						// invoke clearToContinue
		);

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 



		// --- DANIEL WALKS TO BUS --- //


		List<Vector2> danielPosList = new List<Vector2> 
		{
			new Vector2 (10, 4),
			new Vector2 (10, 14)
		};
			
		Character_Handler.instance.MoveByPath (PlayerManager.instance.GetPlayerByName("Daniel"), danielPosList);

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		// daniel disappears



		// -- RED BUS LEAVES -- //

		CutsceneManager.instance.EaseOutObject 
		(
			bus_red,					// object to move
			new Vector3 (5, 20, 0), 	// target position
			5f,							// speed
			true,						// if should be destroyed on arrival
			false
		);


		// ------------------- 
		yield return new WaitForSeconds (4);
		// ------------------- 


		// -- MOVE ROOM: TO ASYLUM GATE -- //

		InteractionManager.instance.MoveToRoom ("asylum_gate", new Vector2 (10, 10));


		// -- BUS ENTERS -- //


		// -- DANIEL APPEARS -- //


		// -- BUS LEAVES -- //


		// -- DIALOGUE -- //


	
		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();


	}

}



// ---------------------------------------------------------------------------------------------- //



public class OpenGreenDoorScene : Cutscene
{
	public OpenGreenDoorScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{
		// -- Text -- //

		///////////

		InteractionManager.instance.DisplayDialogueOption ("open_green_door");

		//////////

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		// --- Open Green Door --- //

		PI_Handler.instance.SetPIAnimationState ("door_abandoned_6_green", "Opening");


		// missing - sound




		// add event

		GameManager.userData.AddEventToList ("green_door_opened");



		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

	}

}



// ---------------------------------------------------------------------------------------------- //


public class OpenGreenDoorMirrorScene : Cutscene
{
	public OpenGreenDoorMirrorScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{
		// -- Text -- //

		///////////

		InteractionManager.instance.DisplayDialogueOption ("open_green_door_mirror");

		//////////

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		// --- Open Green Door --- //

		PI_Handler.instance.SetPIAnimationState ("door_abandoned_6_green_mirror", "Opening");


		// missing - sound




		// add event

		GameManager.userData.AddEventToList ("green_door_mirror_opened");



		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

	}

}



// ---------------------------------------------------------------------------------------------- //




public class MeetgeMScene : Cutscene
{
	public MeetgeMScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
		EventsHandler.cb_isClearToContinue += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{


		// -- Dialogue -- //



		InteractionManager.instance.DisplayDialogueOption ("meet_geM_dialogue1");

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		////////// ------------------- ///////////



		// --- geM leaves --- //

		List<Vector2> geMPosList = new List<Vector2> 
		{
			new Vector2 (10, 4),
			new Vector2 (10, 14)
		};

		Character_Handler.instance.MoveByPath (PlayerManager.instance.GetPlayerByName("geM"), geMPosList);


		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		////////// ------------------- ///////////


		// dialogue with llehctiM


		InteractionManager.instance.DisplayDialogueOption ("meet_geM_dialogue2");


		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		////////// ------------------- ///////////


		// -- llechtiM leaves -- //


		List<Vector2> llehctiMPosList = new List<Vector2> 
		{
			new Vector2 (10, 4),
			new Vector2 (10, 14)
		};

		Character_Handler.instance.MoveByPath (PlayerManager.instance.GetPlayerByName("llehctiM"), llehctiMPosList);


		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 


		////////// ------------------- ///////////


		// -- llehctiM disappears -- // 



		// ------------------- 
		yield return new WaitForSeconds (1);
		// ------------------- 

		////////// ------------------- ///////////



		// add event

		GameManager.userData.AddEventToList ("met_geM");


		// END OF CUTSCENE //

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

	}
}



// ---------------------------------------------------------------------------------------------- //




public class DanielScene : Cutscene
{
	public DanielScene (string myName) : base (myName)
	{
		EventsHandler.cb_dialogueEnded += (() => isClearToContinue = true);
		EventsHandler.cb_characterFinishedPath += (() => isClearToContinue = true);
	}

	public override IEnumerator MyCutscene()
	{
		// Declerations

		//Character llehctiM = (Character)PI_Handler.instance.name_PI_map ["llehctiM"];
		Player Daniel = PlayerManager.myPlayer;

		// dialogue between daniel and llehctiM

		InteractionManager.instance.DisplayDialogueOption ("daniel_scene_dialogue1");


		//InteractionManager.instance.MoveToRoom ("test_mom", new Vector2 (15, 15));

		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////
		yield return new WaitForSeconds (2);
		///////////

		// llechtiM walks somewhere


		List<Vector2> geMMPosList = new List<Vector2> 
		{
			new Vector2 (24, 9),
			new Vector2 (1, 9),
			new Vector2 (24, 9),
			new Vector2 (1, 9),
			//new Vector2 (5, 10)
		};

		List<Vector2> llehctiMPosList = new List<Vector2> 
		{
			new Vector2 (18, 9),
			//new Vector2 (5, 13),
			//new Vector2 (5, 10)
		};

		Character_Handler.instance.MoveByPath (Daniel, llehctiMPosList);

		//CharacterManager.instance.MoveByPath (PlayerManager.instance.GetPlayerByName("llehctiM"), geMMPosList);


		/////////// 

		isClearToContinue = false;

		while (isClearToContinue == false) 
		{
			yield return new WaitForFixedUpdate ();
		}

		///////////
		yield return new WaitForSeconds (1);
		///////////

		SoundManager.Invoke_cb_playSound ("pottery_break", 1);

		///////////
		yield return new WaitForSeconds (2);
		///////////

		//InteractionManager.instance.MoveToRoom ("abandoned_lobby_mirror", new Vector2 (10, 10));

		//InteractionManager.instance.DisplayInfoText ("5 Minutes Later");

		InteractionManager.instance.DisplayInfoBlackScreen ("5 Minutes Later");



		///////////
		yield return new WaitForSeconds (2);
		///////////


		// End of Cutscne

		CutsceneManager.inCutscene = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

	}


}