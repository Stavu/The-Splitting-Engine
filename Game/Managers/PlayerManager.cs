using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class PlayerManager : MonoBehaviour {


	// Singleton //

	public static PlayerManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	public GameObject CharacterPrefab;
	public static Player myPlayer;
	public PlayerObject playerObject;

	public Dictionary<Player,GameObject> playerGameObjectMap;
	public static Vector2 entrancePoint = new Vector2(15,6);

	public static List<Player> playerList;


	// Use this for initialization

	public void Initialize () 
	{		
		//EventsHandler.cb_roomCreated 	+= CreatePlayer;
		//EventsHandler.cb_playerCreated 	+= CreatePlayerObject;
		EventsHandler.cb_keyPressed 	+= MovePlayer;
		EventsHandler.cb_noKeyPressed 	+= StopPlayer;
		EventsHandler.cb_playerMove		+= SavePlayerPosition;

		playerGameObjectMap = new Dictionary<Player, GameObject> ();
	}


	public void OnDestroy()
	{	
		//EventsHandler.cb_roomCreated 	-= CreatePlayer;
		//EventsHandler.cb_playerCreated 	-= CreatePlayerObject;
		EventsHandler.cb_keyPressed 	-= MovePlayer;
		EventsHandler.cb_noKeyPressed 	-= StopPlayer;
		EventsHandler.cb_playerMove 	-= SavePlayerPosition;
	}

	
	// Update is called once per frame

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.A)) 
		{
			if (myPlayer.identificationName == "Daniel") 
			{
				SwitchPlayer ("geM");
			
			} else {
				
				SwitchPlayer ("Daniel");
			}
		}
	}


	// Create Players

	public void CreatePlayers()
	{
		// When first loading the game - create list and assign player

		if (playerList == null) 
		{		
			Debug.Log ("nullllll");
			
			playerList = new List<Player> ();


			System.Object[] myPlayers = Resources.LoadAll ("Jsons/Players");

			foreach (TextAsset txt in myPlayers) 
			{
				Player player = JsonUtility.FromJson<Player> (txt.text);
				playerList.Add (player);

			}

		}
	}




	// -------- MOVE PLAYER --------- // 


	public void MovePlayer(Direction myDirection)
	{
		if (GameManager.instance.inputState != InputState.Character) 
		{
			return;			
		}

		// if player is not in the room, return

		if (myPlayer.currentRoom != RoomManager.instance.myRoom.myName) 
		{
			return;
		}

		// 4 tiles in one second

		float playerSpeed = 4f * Time.deltaTime;
		float offsetX = 0;
		float offsetY = 0;

		Vector3 newPos = new Vector3 (-1000, -1000, -1000); 

		// check the new position according to the diretion 

		switch (myDirection) 
		{
			case Direction.left:

				newPos = new Vector3 ((myPlayer.myPos.x - playerSpeed), myPlayer.myPos.y, myPlayer.myPos.z);
				offsetX = -0.5f;
				//playerGameObjectMap [myPlayer].transform.localScale = new Vector3(1,1,1);

				break;

			case Direction.right:

				newPos = new Vector3 ((myPlayer.myPos.x + playerSpeed), myPlayer.myPos.y, myPlayer.myPos.z);
				offsetX = 0.5f;
				//playerGameObjectMap [myPlayer].transform.localScale = new Vector3(-1,1,1);

				break;

			case Direction.up:

				newPos = new Vector3 (myPlayer.myPos.x, (myPlayer.myPos.y + playerSpeed), myPlayer.myPos.z);
				offsetY = 0.5f;

				break;

			case Direction.down:

				newPos = new Vector3 (myPlayer.myPos.x, (myPlayer.myPos.y - playerSpeed), myPlayer.myPos.z);

				break;
		}


		Tile tile = RoomManager.instance.myRoom.MyGrid.GetTileAt(new Vector3 (newPos.x + offsetX, newPos.y + offsetY, newPos.z));

		if (tile == null) 
		{			
			StopPlayer (InputManager.instance.lastDirection);
			return;
		}

		if (tile != null)
		{
			// FURNITURE - if there a furniture at this tile

			if (tile.myFurniture != null) 
			{
				if (tile.myFurniture.walkable == false) 
				{		
					EventsHandler.Invoke_cb_playerHitPhysicalInteractable (tile.myFurniture, tile);
					StopPlayer (InputManager.instance.lastDirection);

					return;
				}
			}				

			// CHARACTER - if there a character at this tile

			if (tile.myCharacter != null) 
			{
				if (tile.myCharacter.walkable == false) 
				{		
					EventsHandler.Invoke_cb_playerHitPhysicalInteractable (tile.myCharacter, tile);
					StopPlayer (InputManager.instance.lastDirection);

					return;
				}
			}		


			// INACTIVE PLAYER - if there an inactive player at this tile

			if (tile.myInactivePlayer != null) 
			{
				if (tile.myInactivePlayer.walkable == false) {		
					EventsHandler.Invoke_cb_playerHitPhysicalInteractable (tile.myInactivePlayer, tile);
					StopPlayer (InputManager.instance.lastDirection);

					return;
				}
			} 


			// if there's no character at this tile

			if (ActionBoxManager.instance.currentPhysicalInteractable != null) 
			{
				EventsHandler.Invoke_cb_playerLeavePhysicalInteractable ();
			}	

			// TILE INTERACTION - If the next tile is interactable

			if (tile.myTileInteraction != null) 
			{
				if (tile.myTileInteraction.walkable == false) 
				{
					StopPlayer (InputManager.instance.lastDirection);
				}
			
				EventsHandler.Invoke_cb_playerHitTileInteraction (tile);

				if (tile.myTileInteraction.walkable == false) 
				{							
					return;
				}
			}

			if (GameActionManager.instance.currentTileInteraction != null) 
			{
				EventsHandler.Invoke_cb_playerLeaveTileInteraction ();

			}
										
			// Walk to new pos

			myPlayer.ChangePosition (newPos);
			myPlayer.myDirection = myDirection;
			UpdatePlayerObjectPosition (myPlayer, myDirection);
		}
	}



	// When character has stopped 

	public void StopPlayer(Direction lastDirection)
	{		
		if (playerObject != null) 
		{	
			playerObject.StopCharacter (lastDirection);

		} else {

			//Debug.LogError ("player object is null");
		}
	
		
	}


	// Updating the character object position

	public void UpdatePlayerObjectPosition(Player myPlayer, Direction myDirection)
	{		
		if (playerObject != null) 
		{	
			playerObject.MovePlayerObject (myPlayer, myDirection);

		} else {

			Debug.LogError ("player object is null");
		}
	}


	// Updating the character sorting layer

	public void UpdatePlayerSortingLayer(Player myPlayer)
	{		
		Tile currentTile = RoomManager.instance.myRoom.MyGrid.GetTileAt(myPlayer.myPos);
		playerGameObjectMap[myPlayer].GetComponent<SpriteRenderer> ().sortingOrder = -currentTile.y * 10;
	}
			


	// --- SWITCH PLAYER --- //


	public void ChangeCharacterToPlayer()
	{
		
	}


	public void SwitchPlayer(string newPlayer)
	{
		if (myPlayer.identificationName == newPlayer) 
		{
			Debug.LogError ("switched to the same player");
			return;
		}

		Player player = GetPlayerByName (newPlayer);
						
		//Debug.Log ("switch player");


		// ---- switcharoo ---- //

		myPlayer.isActive = false;

		// Park Player 

		ParkPlayerInTiles (myPlayer, myPlayer.myPos);

		// switch

		myPlayer = player;

		// new player is active

		myPlayer.isActive = true;
		GameManager.userData.currentActivePlayer = myPlayer.identificationName;

		//Debug.Log (myPlayer.identificationName);

		// check if player is already in the room

		if (myPlayer.currentRoom == RoomManager.instance.myRoom.myName) 
		{			
			// remove new player from tiles 

			RemovePlayerFromTiles (myPlayer);


			// if new player is in room

			//Debug.Log ("player exists in room");

			if (playerGameObjectMap [myPlayer].GetComponent<PlayerObject> () == null) {
				playerGameObjectMap [myPlayer].AddComponent<PlayerObject> ();
			}

			playerObject = playerGameObjectMap [myPlayer].GetComponent<PlayerObject> ();

		} else {

			InteractionManager.instance.MoveToRoom (myPlayer.currentRoom, new Vector2 (myPlayer.myPos.x, myPlayer.myPos.y));

		}

		//Debug.Log ("after else");

		EventsHandler.Invoke_cb_playerSwitched (player);
		return;	
	}


	// Get player by name

	public Player GetPlayerByName(string name)
	{
		foreach (Player player in playerList) 
		{
			if (player.identificationName == name) 
			{
				return player;
			}
		}

		return null;
	}


	// Save player position to player data when player has moved

	public void SavePlayerPosition(Player player)
	{
		GameManager.userData.GetPlayerDataByPlayerName (player.identificationName).currentPos = player.myPos;
		GameManager.instance.SaveData ();
	}




	public void ParkPlayerInTiles(Player player, Vector3 currentPos)
	{
		Room myRoom = RoomManager.instance.myRoom;

		player.x = Mathf.FloorToInt(currentPos.x);
		player.y = Mathf.FloorToInt(currentPos.y);		

		List<Tile> PlayerTiles = myRoom.GetMyTiles(myRoom.myGrid,player.mySize, player.x, player.y);

		//PlayerTiles.ForEach(tile => Debug.Log(tile.x + "," + tile.y));
		PlayerTiles.ForEach (tile => tile.PlaceInactivePlayerInTile (player));

		if (myRoom.roomState == RoomState.Mirror) 
		{
			List<Tile> PlayerTiles_shadow = myRoom.GetMyTiles(myRoom.myMirrorRoom.shadowGrid,player.mySize, player.x, player.y);
			PlayerTiles_shadow.ForEach (tile => tile.PlaceInactivePlayerInTile (player));
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}




	public void RemovePlayerFromTiles(Player player)
	{		
		Room myRoom = RoomManager.instance.myRoom;

		foreach (Tile tile in myRoom.myGrid.gridArray) 
		{
			if(tile.myInactivePlayer == player)
			{
				tile.myInactivePlayer = null;
			}
		}

		if (myRoom.roomState == RoomState.Mirror) 
		{
			foreach (Tile tile_shadow in myRoom.myMirrorRoom.shadowGrid.gridArray) 
			{
				if(tile_shadow.myInactivePlayer == player)
				{
					tile_shadow.myInactivePlayer = null;
				}
			}
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();

	}





	public void PlayerEntersRoom(string playerName)
	{
		Player player = GetPlayerByName (playerName);

		// change current room of player

		player.currentRoom = RoomManager.instance.myRoom.myName;
		GameManager.userData.GetPlayerDataByPlayerName (playerName).currentRoom = player.currentRoom;

		// create game object 

		EventsHandler.Invoke_cb_playerChanged (player);


		// park in tiles

		if (player.isActive == false) 
		{
			ParkPlayerInTiles (player, player.myPos);
		}
	}



	public void PlayerExitsRoom(string playerName, string nextRoom)
	{
		Player player = GetPlayerByName (playerName);

		// change current room of player

		player.currentRoom = nextRoom;
		GameManager.userData.GetPlayerDataByPlayerName (playerName).currentRoom = player.currentRoom;


		// destroy game object 

		GameObject playerObj = playerGameObjectMap [player];

		Destroy (playerObj);


		// remove from maps 
					
		playerGameObjectMap.Remove (player);		
		PI_Handler.instance.name_PI_map.Remove (player.identificationName);
		PI_Handler.instance.PI_gameObjectMap.Remove (player);


		// remove from tiles 

		RemovePlayerFromTiles (player);
	}


}
