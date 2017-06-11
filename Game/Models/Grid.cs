using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Grid {


	public Tile[,] gridArray;

	public int myWidth;
	public int myHeight;


	public Grid(int myWidth, int myHeight)
	{
		this.myWidth = myWidth;
		this.myHeight = myHeight;

		createGrid ();

	}


	void createGrid()
	{

		gridArray = new Tile[myWidth,myHeight];

		for (int i = 0; i < myWidth; i++) 
		{
			for (int j = 0; j < myHeight; j++) 
			{

				gridArray [i, j] = new Tile (i, j);

			}
			
		}
	}



	public Tile GetTileAt(int x, int y)
	{

		if ((x >= myWidth) || (x < 0) || (y >= myHeight) || (y < 0))
		{

			return null;
			
		}

		return gridArray [x, y];

	}




	public Tile GetTileAt(Vector3 myPos)
	{

		int x = Mathf.FloorToInt (myPos.x);
		int y = Mathf.FloorToInt (myPos.y);

		return GetTileAt(x,y);

	}




}
