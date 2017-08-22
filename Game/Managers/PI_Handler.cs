using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_Handler : MonoBehaviour {



	// Singleton //

	public static PI_Handler instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public Dictionary<PhysicalInteractable,GameObject> PI_gameObjectMap;
	public Dictionary<string,PhysicalInteractable> name_PI_map;


	// Use this for initialization

	public void Initialize () 
	{
		EventsHandler.cb_newAnimationState += ChangeCurrentGraphicState;

		EventsHandler.cb_furnitureChanged += CreatePIGameObject;
		EventsHandler.cb_characterChanged += CreatePIGameObject;
		EventsHandler.cb_playerChanged += CreatePIGameObject;

		PI_gameObjectMap = new Dictionary<PhysicalInteractable, GameObject> ();
		name_PI_map = new Dictionary<string, PhysicalInteractable> ();
	}


	void OnDestroy()
	{
		EventsHandler.cb_newAnimationState -= ChangeCurrentGraphicState;

		EventsHandler.cb_furnitureChanged -= CreatePIGameObject;
		EventsHandler.cb_characterChanged -= CreatePIGameObject;
		EventsHandler.cb_playerChanged -= CreatePIGameObject;

		PI_gameObjectMap.Clear ();
		name_PI_map.Clear ();
	}


	public void AddPIToMap(PhysicalInteractable physicalInteractable, GameObject obj, string name)
	{
		// FIXME: find out why it crashes here in the starting scene
		try
		{
			PI_gameObjectMap.Add (physicalInteractable, obj);
			name_PI_map.Add (name, physicalInteractable);

			if (physicalInteractable is Player) 
			{
				Player player = (Player)physicalInteractable;

				if (PlayerManager.instance.playerGameObjectMap.ContainsKey (player) == false) 
				{
					PlayerManager.instance.playerGameObjectMap.Add (player, obj);
				}
			}
		}
		catch (System.Exception ex)
		{
			
		}


	}


	public void CreatePIGameObject (PhysicalInteractable myPhysicalInteractable)
	{
		// if the furniture has an identification name, use it as the name. If it doesn't, use the file name.

		bool useIdentifiactionName = ((myPhysicalInteractable.identificationName != null) && (myPhysicalInteractable.identificationName != string.Empty));
		string PI_name = useIdentifiactionName ? myPhysicalInteractable.identificationName : myPhysicalInteractable.fileName;			

		// for an active player, we do not calculate a position
		// it is done by the MoveToRoom function
		// for inactive players and other PIs we do
		if ((myPhysicalInteractable is Player) == false)
		{
			myPhysicalInteractable.myPos = new Vector3 (myPhysicalInteractable.x + myPhysicalInteractable.mySize.x / 2, myPhysicalInteractable.y, 0);
		}
		if (myPhysicalInteractable is Player) 
		{	
			Player player = (Player)myPhysicalInteractable;

			if (player.isActive == false) 
			{
				myPhysicalInteractable.myPos = new Vector3 (myPhysicalInteractable.x + myPhysicalInteractable.mySize.x / 2, myPhysicalInteractable.y, 0);
			}				
		}

		GameObject obj = null;
		SpriteRenderer sr = null;

		// Animated Object
	
		if (GameManager.stringPrefabMap.ContainsKey (myPhysicalInteractable.fileName)) 
		{
			obj = Instantiate (GameManager.stringPrefabMap [myPhysicalInteractable.fileName]);
			obj.name = myPhysicalInteractable.identificationName;

			sr = obj.GetComponentInChildren<SpriteRenderer> ();
			string state = GameManager.userData.GetAnimationState (myPhysicalInteractable.identificationName);

			PI_Handler.instance.AddPIToMap (myPhysicalInteractable, obj, PI_name);

			if (state != string.Empty) 
			{
				PI_Handler.instance.SetPIAnimationState (myPhysicalInteractable.identificationName, state, obj);
			} 

			AnimationEvent animationEvent = obj.GetComponent<AnimationEvent> ();

			if (animationEvent != null) 
			{
				animationEvent.physicalInteractable = myPhysicalInteractable;
			}

			if (myPhysicalInteractable is Player) 
			{	
				PlayerObject playerObj = obj.AddComponent<PlayerObject> ();

				Player player = (Player)myPhysicalInteractable;

				if (player.isActive == true) 
				{
					PlayerManager.instance.playerObject = playerObj;
				}				
			}

		} else {
			
			// if not animated object (furniture)

			obj = new GameObject (myPhysicalInteractable.fileName);
			GameObject childObj = new GameObject ("Image");
			childObj.transform.SetParent (obj.transform);

			PI_Handler.instance.AddPIToMap (myPhysicalInteractable, obj, PI_name);

			sr = childObj.AddComponent<SpriteRenderer>();
			sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myPhysicalInteractable.fileName); 
		}

		obj.transform.SetParent (this.transform);
		if (myPhysicalInteractable is Furniture)
		{
			Furniture furn = (Furniture)myPhysicalInteractable;
			obj.transform.localScale = furn.imageFlipped ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
		}

		if (myPhysicalInteractable is Character)
		{
			Character character = (Character)myPhysicalInteractable;
			obj.transform.localScale = character.imageFlipped ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
		}



		if (myPhysicalInteractable is Player) 
		{
			obj.transform.position = myPhysicalInteractable.myPos;

		} else {
			
			obj.transform.position = new Vector3 (myPhysicalInteractable.myPos.x + myPhysicalInteractable.offsetX, myPhysicalInteractable.myPos.y + 0.5f + myPhysicalInteractable.offsetY, myPhysicalInteractable.myPos.z);

		}


		// sorting order 

		Utilities.SetPISortingOrder (myPhysicalInteractable, obj);
//
//		if (myPhysicalInteractable.walkable == true) 
//		{
//			sr.sortingOrder = (int) -(myPhysicalInteractable.y + myPhysicalInteractable.mySize.y) * 10;
//		}
//
//		if (myPhysicalInteractable.aboveFrame == true)
//		{
//			sr.sortingLayerName = Constants.aboveFrame_layer;
//		}
//		else
//		{
//			sr.sortingLayerName = Constants.furniture_character_layer;
//		}
	}


	// Setting furniture animation state

	public void SetPIAnimationState(string PI_name, string state, GameObject obj = null)
	{

	//	Debug.Log ("SetPIAnimationState " + PI_name + state);


		if (name_PI_map.ContainsKey (PI_name)) 
		{
			PhysicalInteractable physicalInteractable = name_PI_map [PI_name];

			if(obj == null)
			{				
				obj = PI_gameObjectMap [physicalInteractable];
			}

			Animator animator = obj.GetComponent<Animator> ();

			ChangeCurrentGraphicState (physicalInteractable, state);

			animator.PlayInFixedTime (state);
			Utilities.SetPISortingOrder (physicalInteractable, obj);

		} else {

			Debug.LogError ("I don't have this title name " + PI_name);
		}
	}


	// Change Current Graphic State

	public void ChangeCurrentGraphicState(PhysicalInteractable physicalInteractable, string state)
	{
		//Debug.Log ("change current graphic state " + physicalInteractable.identificationName + " " + state);

		ActionBoxManager.instance.CloseFurnitureFrame ();

		foreach (GraphicState graphicState in physicalInteractable.graphicStates) 
		{
			if (graphicState.graphicStateName == state) 
			{
				Room room = RoomManager.instance.myRoom;

				// get the associated grids
				List<Grid> grids = new List<Grid>();

				if (physicalInteractable is Furniture) 
				{
					Furniture furn = (Furniture)physicalInteractable;

					if (room.myFurnitureList.Contains(furn))
					{
						grids.Add (room.myGrid);
					}

					if (room.myMirrorRoom != null)
					{
						if (room.myMirrorRoom.myFurnitureList_Shadow.Contains(furn))
						{
							grids.Add (room.myMirrorRoom.shadowGrid);
						}

						if (room.myMirrorRoom.myFurnitureList_Persistant.Contains(furn))
						{
							grids.Clear (); // maybe this is unnecessary, but whatever
							grids.Add (room.myGrid);
							grids.Add (room.myMirrorRoom.shadowGrid);
						}
					}


				}



				
				RoomManager.instance.myRoom.ChangePIInTiles (physicalInteractable, graphicState, grids);
				physicalInteractable.currentGraphicState = graphicState;
				GameManager.userData.AddAnimationState (physicalInteractable.identificationName, state);	
				//Debug.Log ("save state");

			}
		}
	}





	public void UnHide_PI (PhysicalInteractable physicalInteractable) 
	{

		physicalInteractable.hidden = false;


		if (physicalInteractable is Furniture)
		{
			Furniture furn = (Furniture) physicalInteractable;
			EventsHandler.Invoke_cb_furnitureChanged (furn);
		}

		if (physicalInteractable is Character)
		{
			Character character = (Character) physicalInteractable;
			EventsHandler.Invoke_cb_characterChanged (character);
		}


		// Remove from hidden list

		if (physicalInteractable is Furniture) 
		{
			if (GameManager.userData.hiddenFurnitureList.Contains (physicalInteractable.identificationName) == true) 
			{
				GameManager.userData.hiddenFurnitureList.Remove (physicalInteractable.identificationName);
			}
		}

		if (physicalInteractable is Character) 
		{
			if (GameManager.userData.hiddenCharacterList.Contains (physicalInteractable.identificationName) == true) 
			{
				GameManager.userData.hiddenCharacterList.Remove (physicalInteractable.identificationName);
			}
		}


		RoomManager.instance.myRoom.CreateRoomInteractables ();

	}



	public void RoomStarter_Unhide_PI (PhysicalInteractable physicalInteractable) 
	{
		
		physicalInteractable.hidden = false;

		// Remove from hidden list

		if (physicalInteractable is Furniture) 
		{
			if (GameManager.userData.hiddenFurnitureList.Contains (physicalInteractable.identificationName) == true) 
			{
				GameManager.userData.hiddenFurnitureList.Remove (physicalInteractable.identificationName);
			}
		}

		if (physicalInteractable is Character) 
		{
			if (GameManager.userData.hiddenCharacterList.Contains (physicalInteractable.identificationName) == true) 
			{
				GameManager.userData.hiddenCharacterList.Remove (physicalInteractable.identificationName);
			}
		}
	}





	public void Hide_PI (string PI_name) 
	{
		PhysicalInteractable physicalInteractable = name_PI_map [PI_name];

		if (physicalInteractable == null) 
		{
			Debug.LogError ("can't find PI by name");
			return;
		}

		physicalInteractable.hidden = true;

		if (PI_gameObjectMap.ContainsKey (physicalInteractable)) 
		{			
			Destroy (PI_gameObjectMap [physicalInteractable]);
			PI_gameObjectMap.Remove (physicalInteractable);
		}

		foreach (Tile tile in RoomManager.instance.myRoom.MyGrid.gridArray) 
		{
			if (tile.myFurnitureList != null) 
			{				
				if (tile.myFurnitureList.Contains ((Furniture)physicalInteractable)) 
				{
					tile.myFurnitureList.Remove ((Furniture)physicalInteractable);
				}
			}
		}

		if (physicalInteractable is Furniture) 
		{			
			if (GameManager.userData.hiddenFurnitureList.Contains (PI_name) == false) 
			{
				GameManager.userData.hiddenFurnitureList.Add (PI_name);
			}
		}

		if (physicalInteractable is Character) 
		{
			if (GameManager.userData.hiddenCharacterList.Contains (PI_name) == false) 
			{
				GameManager.userData.hiddenCharacterList.Add (PI_name);
			}
		}


	}



	public void RoomStarter_Hide_Character (string PI_name) 
	{

		PhysicalInteractable physicalInteractable = RoomManager.instance.getCharacterByName (PI_name);

		if (physicalInteractable == null) 
		{
			Debug.LogError ("can't find PI by name");
			return;
		}

		physicalInteractable.hidden = true;

		if (physicalInteractable is Character) 
		{

			if (GameManager.userData.hiddenCharacterList.Contains (PI_name) == false) 
			{
				GameManager.userData.hiddenCharacterList.Add (PI_name);
			}
		}


	}



	public void RoomStarter_Hide_Furniture (string PI_name) 
	{

		PhysicalInteractable physicalInteractable = RoomManager.instance.getFurnitureByName (PI_name);

		if (physicalInteractable == null) 
		{
			Debug.LogError ("can't find PI by name");
			return;
		}

		physicalInteractable.hidden = true;

		if (physicalInteractable is Furniture) 
		{

			if (GameManager.userData.hiddenFurnitureList.Contains (PI_name) == false) 
			{
				GameManager.userData.hiddenFurnitureList.Add (PI_name);
			}
		}


	}


	/*
	// Get PI by Name

	public PhysicalInteractable getPIbyName(string PI_Name)
	{
		PhysicalInteractable pi = null;

		if(name_PI_map.ContainsKey(PI_Name))
		{
			pi = name_PI_map [PI_Name];
		}
	
		return name_PI_map [PI_Name];
	}

	*/

}
