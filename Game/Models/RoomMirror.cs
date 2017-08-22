using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



[Serializable]
public class RoomMirror {


	// declerations 


	public bool inTheShadow;
	public bool inTheShadow_initial;

	public string bgName_Shadow;
	public bool bgFlipped_Shadow = false;

	public List<Furniture> myFurnitureList_Persistant;
	public List<TileInteraction> myTileInteractionList_Persistant;

	public List<Furniture> myFurnitureList_Shadow;
	public List<TileInteraction> myTileInteractionList_Shadow;

	public Grid shadowGrid;
	public string myShadowMusic;


	// Shallow Room - for editor

	public RoomMirror() 
	{

		this.myFurnitureList_Persistant = new List<Furniture> ();
		this.myTileInteractionList_Persistant = new List<TileInteraction> ();

		this.myFurnitureList_Shadow = new List<Furniture> ();
		this.myTileInteractionList_Shadow = new List<TileInteraction> ();

	}



	/*

	// Real Room - for game 

	public RoomMirror(RoomMirror clone)
	{

		this.bgName_Shadow = clone.bgName_Shadow;

		this.myFurnitureList_Shadow = clone.myFurnitureList_Shadow;
		this.myTileInteractionList_Shadow = clone.myTileInteractionList_Shadow;


	}

	*/






}
