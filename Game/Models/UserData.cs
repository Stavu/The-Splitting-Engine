using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class UserData {
	
	List<string> gameEventsList;
	public List<string> roomsVisitedList;
	public List<PI_AnimationState> animationStateList;
	public List<PlayerData> playerDataList;

	public string currentActivePlayer;


	public UserData()
	{		
		gameEventsList = new List<string> ();
		roomsVisitedList = new List<string> ();
		animationStateList = new List<PI_AnimationState> ();
		playerDataList = new List<PlayerData> ();
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

		//Debug.Log ("already existed");
		return false;
	}


	// Add Event

	public void AddEventToList(string eventName)
	{
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


	// Add animation state

	public void AddAnimationState(string physicalInateractable,string animationState)
	{		
		//Debug.Log ("add animation state");

		foreach (PI_AnimationState state in animationStateList) 
		{
			if (physicalInateractable == state.myName) 
			{
				state.animationState = animationState;
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
		//Debug.Log ("get animation state");

		string animationState;

		foreach (PI_AnimationState state in animationStateList) 
		{
			if (physicalInateractable == state.myName) 
			{
				animationState = state.animationState;
				//Debug.Log ("return animation state " + animationState);
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