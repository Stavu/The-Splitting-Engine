using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Player : PhysicalInteractable, ISpeaker, IWalker {


	//public string identificationName;
	//public string myName;
	//public Vector2 mySize;

	//public Vector3 myPos;

	public Color myTextColor;

	[NonSerialized]
	public Direction myDirection;
	public float speed = 4f;

	public Queue<Vector2> path;
	public Vector2 targetPos;

	public bool isActive = false;
	public string startingRoom;
	public Vector3 startingPos;

	public string currentRoom;


	// Speaker

	public string speakerName 
	{
		get { return identificationName; }

		set { identificationName = value; }
	}


	public Vector2 speakerSize 
	{
		get	{ return mySize; }

		set { mySize = value; }
	}


	public Vector3 speakerPos
	{
		get { return myPos; }

		set { myPos = value; }
	}


	public Color speakerTextColor
	{
		get { return GameManager.speakerColorMap [speakerName]; }

		set { myTextColor = value; }
	}


	// Walker 

	public float walkerSpeed
	{
		get	{ return speed; }

		set { speed = value; }
	}


	public Vector2 walkerTargetPos
	{
		get { return targetPos;	}

		set { targetPos = value; }
	}


	public float walkerOffsetX
	{
		get	{ return 0; }
	}


	public float walkerOffsetY
	{
		get	{ return 0; }
	}


	public GameObject walkerGameObject
	{
		get	{ return PlayerManager.instance.playerGameObjectMap[this]; }

		set { PlayerManager.instance.playerGameObjectMap[this] = value; }
	}


	public Queue<Vector2> walkerPath
	{
		get	{ return path; }

		set	{ path = value; }
	}



	// Constructor

	public Player(string myName, Vector2 mySize)
	{
		this.identificationName = myName;
		this.fileName = myName;

		this.mySize = mySize;	

		/*
		if (myName == "Daniel") 
		{
			//Debug.Log ("color");
			myTextColor = GameManager.instance.danielColor;
		}
		*/

		myTextColor = Color.white;

		myInteractionList = new List<Interaction> ();
		graphicStates = new List<GraphicState> ();

	}



	// When user is moving the player

	public void ChangePosition(Vector3 myPos)
	{
		this.myPos = myPos;
		EventsHandler.Invoke_cb_playerMove (this);
	}


	// When cutscene is moving the player

	public void ChangePos(Vector2 newPos)
	{		
		this.myPos = Utilities.GetCharacterPosOnTile (this, newPos);
		EventsHandler.Invoke_cb_playerMove (this);
	}

}
