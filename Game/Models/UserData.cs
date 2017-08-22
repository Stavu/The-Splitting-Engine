using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class UserData {
	
	public List<string> gameEventsList;
	public List<string> roomsVisitedList;
	public List<string> roomsInShadowList;
	public List<string> benchAbandoned6BookList;
	public List<string> benchAbandoned6MirrorBookList;

	public List<PI_AnimationState> animationStateList;
	public List<PlayerData> playerDataList;

	public List<string> hiddenFurnitureList;
	public List<string> hiddenCharacterList;

	public string currentActivePlayer;


	public UserData()
	{		
		gameEventsList = new List<string> ();
		roomsVisitedList = new List<string> ();
		roomsInShadowList = new List<string> ();
		benchAbandoned6BookList = new List<string> ();
		benchAbandoned6MirrorBookList = new List<string> ();

		animationStateList = new List<PI_AnimationState> ();
		playerDataList = new List<PlayerData> ();

		hiddenFurnitureList = new List<string> ();
		hiddenCharacterList = new List<string> ();
	}

	// Get current Player Data 

	public PlayerData GetCurrentPlayerData()
	{
		string myPlayer = PlayerManager.myPlayer.identificationName;

		foreach (PlayerData playerData in playerDataList) 
		{
			if (playerData.playerName == myPlayer) 
			{
				
				return playerData;
			}
		}

		Debug.LogError("playerData is null");
		return null;
	}


	public PlayerData GetPlayerDataByPlayerName(string playerName)
	{
		foreach (PlayerData playerData in GameManager.userData.playerDataList) 
		{
			if (playerData.playerName == playerName) 
			{
				return playerData;
			}			
		}

		return null;
	}


	// -- CONDITIONS -- //



	// check current player 

	public bool	CheckIfCurrentPlayer(string playerName)
	{
		return PlayerManager.myPlayer.identificationName == playerName;
	}


	// check if event exists

	public bool CheckIfEventExists(string eventName)
	{
		return gameEventsList.Contains (eventName);
	}


	// check if character exists

	public bool CheckIfCharacterExistsInRoom(string characterName)
	{
		foreach (Character character in RoomManager.instance.myRoom.myCharacterList) 
		{
			if (character.identificationName == characterName) 
			{
				return true;
			}
		}

		return false;
	}


	/// <summary>
	/// Adds to rooms visited.
	/// </summary>
	/// <returns><c>true</c>, if room didn't exist in roomsVisited, <c>false</c> otherwise.</returns>
	/// <param name="roomName">Room name.</param>

	public bool AddToRoomsVisited(string roomName)
	{
		if (roomsVisitedList.Contains (roomName) == false) 
		{
			Debug.Log ("first time");
			roomsVisitedList.Add (roomName);
			GameManager.instance.SaveData ();
			return true;
		}

		return false;
	}



	// --- SHADOWS --- //


	// Add to rooms in shadow list

	public void AddToRoomsInShadow(string roomName)
	{
		
		if (roomsInShadowList.Contains (roomName) == false) 
		{
			Debug.Log ("room in shadow");
			roomsInShadowList.Add (roomName);
			GameManager.instance.SaveData ();
		}
	}


	// Remove from rooms in shadow list

	public void RemoveFromRoomsInShadow(string roomName)
	{
		if (roomsInShadowList.Contains (roomName)) 
		{
			Debug.Log ("room not in shadow");
			roomsInShadowList.Remove (roomName);
			GameManager.instance.SaveData ();

		}	
	}


	// --- EVENTS --- //


	// Add Event

	public void AddEventToList(string eventName)
	{
		//Debug.Log ("add event to list");

		if (gameEventsList.Contains (eventName) == false) 
		{
			gameEventsList.Add (eventName);
			GameManager.instance.SaveData ();
		}
	}


	// Remove Event

	public void RemoveEventFromList(string eventName)
	{
		if (gameEventsList.Contains (eventName) == true) 
		{
			gameEventsList.Remove (eventName);
			GameManager.instance.SaveData ();
		}
	}




	// --- BOOKS --- //


	// Add book

	public void AddBookToBench(string bookName)
	{

		Debug.Log ("book " + bookName);

		benchAbandoned6BookList.Add (bookName);
		GameManager.instance.SaveData ();

		Debug.Log ("books " + benchAbandoned6BookList);

	}


	// Remove book

	public void RemoveBookFromBench(string eventName)
	{
		if (benchAbandoned6BookList.Count > 0)
		{
			benchAbandoned6BookList.RemoveAt (benchAbandoned6BookList.Count - 1);
			GameManager.instance.SaveData ();
		}
	}



	// Add book

	public void AddBookToBenchMirror(string bookName)
	{

		Debug.Log ("book " + bookName);

		benchAbandoned6MirrorBookList.Add (bookName);
		GameManager.instance.SaveData ();

		Debug.Log ("books " + benchAbandoned6MirrorBookList);

	}


	// Remove book

	public void RemoveBookFromBenchMirror(string eventName)
	{
		if (benchAbandoned6MirrorBookList.Count > 0)
		{
			benchAbandoned6MirrorBookList.RemoveAt (benchAbandoned6MirrorBookList.Count - 1);
			GameManager.instance.SaveData ();
		}
	}





	// Add animation state

	public void AddAnimationState(string physicalInateractable,string animationState)
	{	
		//Debug.Log ("AddAnimationState " + physicalInateractable + " " + animationState);

		foreach (PI_AnimationState state in animationStateList) 
		{
			if (physicalInateractable == state.myName) 
			{
				state.animationState = animationState;
				//Debug.Log ("animation state " + state.myName);
				return;
			}
		}

		PI_AnimationState pi_animationState = new PI_AnimationState();

		pi_animationState.myName = physicalInateractable;
		pi_animationState.animationState = animationState;

		animationStateList.Add (pi_animationState);

		GameManager.instance.SaveData ();
	}


	public string GetAnimationState(string physicalInateractable)
	{
		string animationState;

		foreach (PI_AnimationState state in animationStateList) 
		{
			if (physicalInateractable == state.myName) 
			{
				animationState = state.animationState;

				return animationState;
			}
		}

		return string.Empty;
	}
}



[Serializable]
public class PlayerData {
	
	public string playerName;
	public string currentRoom;
	public Vector3 currentPos;
	public Inventory inventory;

	public PlayerData(string playerName)
	{
		this.playerName = playerName;
		inventory = new Inventory ();
	}

	// check if item exists

	public bool CheckIfItemExists(string itemName)
	{
		foreach (InventoryItem item in inventory.items) 
		{
			if (item.fileName == itemName) 
			{
				return true;
			}
		}

		return false;
	}
}



[Serializable]
public class PI_AnimationState
{
	public string myName;
	public string animationState;

}