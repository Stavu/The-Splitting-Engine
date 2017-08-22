using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Furniture : PhysicalInteractable 
{

	public bool imageFlipped = false;
	public int myPriority = 0;

	public Furniture(string myName, int x, int y)
	{
		// Constructor

		this.fileName = myName;
		this.identificationName = myName;

		this.x = x;
		this.y = y;	

		myInteractionList = new List<Interaction> ();
		graphicStates = new List<GraphicState> ();	

	
	}


	// Constructor for flipped furniture

	public Furniture(Room room, Furniture furn)
	{				
		this.identificationName = furn.identificationName;
		this.fileName = furn.fileName;
		this.x = room.MyGrid.myWidth - 1 - furn.x - ((int)furn.mySize.x - 1);
		this.y = furn.y;

		this.imageFlipped = !furn.imageFlipped;

		this.offsetX = -furn.offsetX;
		this.offsetY = furn.offsetY;
		this.mySize = furn.mySize;
		this.graphicStates = furn.graphicStates;

		this.walkable = furn.walkable;
	}

}



