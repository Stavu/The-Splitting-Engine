using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class TileInteraction : Interactable {


	//public Vector2 mySize;
	//public int x;
	//public int y;

	//public bool walkable = false;


	public SubInteraction mySubInt;


	public TileInteraction(int x, int y)
	{

		this.x = x;
		this.y = y;

	}




	// Constructor for flipped furniture

	public TileInteraction(Room room, TileInteraction tileInt)
	{		
		
		this.x = room.MyGrid.myWidth - 1 - tileInt.x - ((int)tileInt.mySize.x - 1);
		this.y = tileInt.y;

		this.mySize = tileInt.mySize;
		this.walkable = tileInt.walkable;

	}





}
