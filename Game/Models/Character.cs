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


	public void ChangePos(Vector2 newPos)
	{
		this.x = (int)newPos.x;
		this.y = (int)newPos.y;

		this.myPos = new Vector3 (newPos.x, newPos.y, 0);

		Tile newTile = RoomManager.instance.myRoom.MyGrid.GetTileAt((int)newPos.x, (int)newPos.y);
			
		newTile.myCharacter = this;
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