using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Character : PhysicalInteractable, ISpeaker, IWalker {


	public float speed = 4f;
	public Tile targetTile;
	public Vector2 targetPos;

	[NonSerialized]
	public Queue<Vector2> path;

	public Color myTextColor;

	public bool imageFlipped = false;



	public string speakerName 
	{
		get	{ return identificationName; }

		set { identificationName = value; }
	}


	public Vector2 speakerSize 
	{
		get { return mySize; }

		set { mySize = value; }
	}


	public Vector3 speakerPos
	{
		get	{ return myPos; }

		set { myPos = value; }
	}


	public Color speakerTextColor
	{
		get	{ return GameManager.speakerColorMap [speakerName]; }

		set { myTextColor = value; }
	}


	// -- Walker -- // 



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
		get	{ return offsetX; }
	}


	public float walkerOffsetY
	{
		get	{ return offsetY; }
	}


	public GameObject walkerGameObject
	{
		get	{ return PI_Handler.instance.PI_gameObjectMap[this]; }

		set { PI_Handler.instance.PI_gameObjectMap[this] = value; }
	}


	public Queue<Vector2> walkerPath
	{
		get	{ return path; }

		set	{ path = value; }
	}




	// ---- CHARACTER ---- //


	public Character(string myName, int x, int y)
	{		
		// Constructor

		this.identificationName = myName;
		this.fileName = myName;
			
		this.x = x;
		this.y = y;

		myInteractionList = new List<Interaction> ();
		graphicStates = new List<GraphicState> ();

		myTextColor = Color.cyan;


	}


	// Constructor for flipped character

	public Character(Room room, Character character)
	{				
		this.identificationName = character.identificationName;
		this.fileName = character.fileName;
		this.x = room.MyGrid.myWidth - 1 - character.x - ((int)character.mySize.x - 1);
		this.y = character.y;

		this.imageFlipped = !character.imageFlipped;

		this.offsetX = -character.offsetX;
		this.offsetY = character.offsetY;
		this.mySize = character.mySize;
		this.graphicStates = character.graphicStates;

		this.walkable = character.walkable;
	}


	public void ChangePos(Vector2 newPos)
	{
		this.x = (int)newPos.x;
		this.y = (int)newPos.y;

		this.myPos = new Vector3 (newPos.x, newPos.y, 0);

		Tile newTile = RoomManager.instance.myRoom.MyGrid.GetTileAt((int)newPos.x, (int)newPos.y);
			
		// Stav: new thing - if there is no tile (the character left the room) the character's object will be destroyed

		if (newTile != null) 
		{
			newTile.myCharacter = this;

		} else {

			// destroy character


		}
	}
}





// Interface

public interface ISpeaker
{	
	
	string speakerName {get; set;}
	Vector2 speakerSize {get; set;}
	Vector3 speakerPos {get; set;}
	Color speakerTextColor {get; set;}

}


public interface IWalker
{
	string speakerName {get; set;}
	Vector2 speakerSize {get; set;}
	Vector3 speakerPos {get; set;}

	float walkerOffsetX {get;}
	float walkerOffsetY {get;}

	float walkerSpeed{get; set;}
	Vector2 walkerTargetPos {get; set;}
	Queue<Vector2> walkerPath {get; set;}

	GameObject walkerGameObject {get; set;}

	// functions
	void ChangePos (Vector2 newPos);


}