using System.Collections;
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

						InventoryItem picture = new InventoryItem ("missing_picture", "Missing Picture");

						GameManager.userData.GetCurrentPlayerData().inventory.AddItem (picture);

						break;


					default:

						break;
				}

			break;


				/* -------- MISSING PICTURE -------- */

			case "missing_picture":				

				switch (physicalInt.identificationName) 
				{
					case "door_abandoned_main_shadow":

						DialogueSentence sentence2 = new DialogueSentence (PlayerManager.myPlayer.identificationName, "pic on door.", false);
						List<DialogueSentence> list2 = new List<DialogueSentence> ();
						list2.Add (sentence2);

						InteractionManager.instance.DisplayText (list2);
											
						break;


					default:

						break;
				}

				break;
		}
	}


}
