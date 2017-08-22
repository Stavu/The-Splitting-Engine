using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTileManager : MonoBehaviour {



	// Singleton //

	public static EditorTileManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	public GameObject TilePrefab;
	public Dictionary<Tile,GameObject> tileGameObjectMap;

	GameObject tilesParent;




	// Use this for initialization
	public void Initialize () {

		EventsHandler.cb_editorNewRoomCreated += CreateTileObject;
		EventsHandler.cb_tileLayoutChanged += ColorTiles;

		tileGameObjectMap = new Dictionary<Tile, GameObject>();
	}


	public void OnDestroy()
	{

		EventsHandler.cb_editorNewRoomCreated -= CreateTileObject;
		EventsHandler.cb_tileLayoutChanged -= ColorTiles;
	}




	// Update is called once per frame
	void Update () {

	}


	void CreateTileObject(Room myRoom)
	{		
		
		// Destroy old tiles


		foreach (GameObject obj in tileGameObjectMap.Values) 
		{			
			Destroy (obj);
		}

		if (tilesParent != null) 
		{
			Destroy (tilesParent);
		}

		tileGameObjectMap.Clear ();


		// Create new tiles

		tilesParent = new GameObject ("Tiles");

		foreach (Tile tile in myRoom.MyGrid.gridArray) 
		{
			GameObject obj = Instantiate (TilePrefab, tilesParent.transform);

			obj.transform.position = new Vector3 (tile.x,tile.y,0);
			obj.GetComponent<SpriteRenderer> ().sortingLayerName = Constants.tiles_layer;

			tileGameObjectMap.Add(tile, obj);

			// adding object to hirarchy under TileManager

			obj.name = "Tile " + tile.x + "," + tile.y;

		}
	}



	public void ColorTiles()
	{
		// First - Clean tile layout

		foreach (GameObject obj in tileGameObjectMap.Values) 
		{
			obj.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.1f);

		}

		// Color furniture tiles

		foreach (Tile tile in EditorRoomManager.instance.room.MyGrid.gridArray) 
		{
			if (tile.myTileInteraction != null) 
			{
				TileInteraction tileInt = tile.myTileInteraction;

				for (int x = 0; x < tileInt.mySize.x; x++) 
				{
					for (int y = 0; y < tileInt.mySize.y; y++) 
					{
						Tile tempTile = EditorRoomManager.instance.room.MyGrid.GetTileAt (tileInt.x + x, tileInt.y + y);
						tileGameObjectMap [tile].GetComponent<SpriteRenderer> ().color = new Color (0, 1f, 0, 0.2f);
					}
				}
			}

			if (tile.myCharacter != null) 
			{
				tileGameObjectMap [tile].GetComponent<SpriteRenderer> ().color = new Color (1f, 0, 0, 0.5f);
			}

			if (tile.myFurniture != null) 
			{
				tileGameObjectMap [tile].GetComponent<SpriteRenderer> ().color = new Color (0, 0, 1f, 0.5f);
			}
		}
	}
}
