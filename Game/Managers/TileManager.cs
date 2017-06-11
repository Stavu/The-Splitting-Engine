using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {



	// Singleton //

	public static TileManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	GameObject tiles;

	public GameObject TilePrefab;
	public Dictionary<Tile,GameObject> tileGameObjectMap;



	// Use this for initialization
	public void Initialize () 
	{	
		//EventsHandler.cb_furnitureChanged += ColorTiles;
		//EventsHandler.cb_characterChanged += ColorTiles;
		//EventsHandler.cb_tileInteractionChanged += ColorTiles;
		EventsHandler.cb_playerMove += FindPlayerTile;
		EventsHandler.cb_roomCreated += CreateTileObject;
		EventsHandler.cb_tileLayoutChanged += ColorTiles;

		tileGameObjectMap = new Dictionary<Tile, GameObject>();
					
	}


	public void OnDestroy()
	{
		//EventsHandler.cb_furnitureChanged -= ColorTiles;
		//EventsHandler.cb_characterChanged -= ColorTiles;
		//EventsHandler.cb_tileInteractionChanged -= ColorTiles;
		EventsHandler.cb_playerMove -= FindPlayerTile;
		EventsHandler.cb_roomCreated -= CreateTileObject;
		EventsHandler.cb_tileLayoutChanged -= ColorTiles;
	}



	// Update is called once per frame

	void Update () 
	{
		
	}


	void CreateTileObject(Room myRoom)
	{

		tiles = new GameObject ("Tiles");


		foreach (Tile tile in myRoom.myGrid.gridArray) 
		{
			GameObject obj = Instantiate (TilePrefab, tiles.transform);

			obj.transform.position = new Vector3 (tile.x,tile.y,0);
			obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.tiles_layer;

			tileGameObjectMap.Add(tile, obj);

			// adding object to hirarchy under TileManager
		
			obj.name = "Tile " + tile.x + "," + tile.y;

		}

	}


	// Coloring the tiles the player walks on

	public void FindPlayerTile(Player myPlayer)
	{

		Tile currentTile = RoomManager.instance.myRoom.MyGrid.GetTileAt(PlayerManager.myPlayer.myPos);

		// light the tile

		TileManager.instance.GetTileObject(currentTile.x, currentTile.y).GetComponent<SpriteRenderer> ().color = new Color (0.1f,0.1f,0.1f,0.2f);

	}



	public GameObject GetTileObject(int x, int y)
	{
		
		GameObject obj = tiles.transform.Find ("Tile " + x + "," + y).gameObject;

		if (obj == null) 
		{
			Debug.LogError ("obj is null");
		}

		return obj;

	}


	/*
	// Color furniture tiles inside game

	public void ColorTiles(Interactable interactable)
	{

		//Debug.Log ("ColorTiles");


		List<Tile> InteractableTiles = RoomManager.instance.myRoom.GetMyTiles(RoomManager.instance.myRoom.MyGrid, interactable.mySize, interactable.x, interactable.y);


		// light the tiles


		foreach (Tile tile in InteractableTiles) {

			TileManager.instance.tileGameObjectMap [tile].GetComponent<SpriteRenderer> ().color = new Color (0.1f, 0.3f, 0.2f, 0.4f);

		}		


	}
	*/




	public void ColorTiles()
	{

		//Debug.Log ("ColorTiles");

		// First - Clean tile layout

		foreach (GameObject obj in tileGameObjectMap.Values) 
		{
			obj.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.1f);
		}


		// Color furniture tiles

		foreach (Tile tile in RoomManager.instance.myRoom.MyGrid.gridArray) 
		{

			Tile mapTile = RoomManager.instance.myRoom.myGrid.GetTileAt (tile.x, tile.y);

		
			if (tile.myTileInteraction != null) 
			{
				TileInteraction tileInt = tile.myTileInteraction;

				for (int x = 0; x < tileInt.mySize.x; x++) 
				{
					for (int y = 0; y < tileInt.mySize.y; y++) 
					{
						Tile tempTile = RoomManager.instance.myRoom.myGrid.GetTileAt (tileInt.x + x, tileInt.y + y);
						tileGameObjectMap [tempTile].GetComponent<SpriteRenderer> ().color = Color.yellow;
					}
				}
			}

			if (tile.myCharacter != null) 
			{
				tileGameObjectMap [mapTile].GetComponent<SpriteRenderer> ().color = Color.magenta;
			}

			if (tile.myFurniture != null) 
			{
				tileGameObjectMap [mapTile].GetComponent<SpriteRenderer> ().color = Color.blue;
			}
		}
	}




}
