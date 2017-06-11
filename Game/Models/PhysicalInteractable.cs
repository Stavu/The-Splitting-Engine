using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PhysicalInteractable : Interactable {


	public string identificationName;
	public string fileName;

	public Vector3 myPos {get; set;}

	//public Vector2 frameExtents;

	//public float frameOffsetX;
	//public float frameOffsetY;

	public float offsetX = 0;
	public float offsetY = 0;

	public List<Interaction> myInteractionList;

	public List<GraphicState> graphicStates;

	[NonSerialized]
	public GraphicState currentGraphicState;

	public GraphicState CurrentGraphicState()
	{
		if (currentGraphicState == null) 
		{
			currentGraphicState = graphicStates [0];
		} 

		return currentGraphicState;
	}


	// -- Coords List -- //

	public List<Coords> GetMyCoordsList()
	{	
		Debug.Log ("GetMyCoordsList");

		if (currentGraphicState == null) 
		{			
			Debug.LogError ("graphic state is null");
		} 


		if (CurrentGraphicState ().coordsList.Count > 0) 
		{			
			Debug.Log ("getting coords list");
			return CurrentGraphicState ().coordsList;

		} else {

			List<Coords> coordsList = new List<Coords> ();

			for (int i = 0; i < mySize.x; i++) 
			{
				for (int j = 0; j < mySize.y; j++) 
				{
					Coords coords = new Coords (x+i,y+j);
					coordsList.Add (coords);
				}
			}

			return coordsList;
		}

	}

}



[Serializable]
public class GraphicState {

	public string graphicStateName;

	public Vector2 frameExtents;

	public float frameOffsetX;
	public float frameOffsetY;

	public List<Coords> coordsList;


}



[Serializable]
public struct Coords {

	public int x;
	public int y;

	// Constructor
	public Coords(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}