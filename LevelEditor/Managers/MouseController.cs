using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.EventSystems;



public class MouseController : MonoBehaviour {



	// Singleton //

	public static MouseController instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	Vector3 lastFrameposition;
	Vector3 currentFramePosition;
	Vector3 dragStartPosition;


	bool isDragging = false;

	Plane plane = new Plane(new Vector3(0,1,0), new Vector3(1,0,0), new Vector3(-1,0,0));

	public GameObject tileMarker;
	List<GameObject> tileMarkerGameObjects;




	// Use this for initialization
	void Start () 
	{

		tileMarkerGameObjects = new List<GameObject>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		currentFramePosition = GetCurrentMousePosition();
		CheckClickOnTile ();
		//UpdateDragging ();
		
	}




	Vector3 GetCurrentMousePosition()
	{

		//Debug.Log ("GetCurrentMousePosition");


		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction * -Camera.main.transform.position.z, Color.yellow);

		float rayDistance;
		plane.Raycast (ray, out rayDistance);

		Vector3 pos = ray.GetPoint (rayDistance);

		return pos;

	}


	void DestroyMarkers()
	{

		while (tileMarkerGameObjects.Count > 0) 
		{

			GameObject go = tileMarkerGameObjects[0];

			tileMarkerGameObjects.RemoveAt(0);

			SimplePool.Despawn (go);


		}

	}



	// CLICK ON TILE //

	void CheckClickOnTile()
	{

		if ((EventSystem.current.IsPointerOverGameObject()) && (isDragging == false))
		{
			return;
		}


		if (Input.GetMouseButtonDown (0)) 
		{			

			int posX = Mathf.FloorToInt (currentFramePosition.x);
			int posY = Mathf.FloorToInt (currentFramePosition.y);


			// Destroying old markers

			DestroyMarkers ();


			// Creation of the marker

			Tile tile = EditorRoomManager.instance.room.MyGrid.GetTileAt (posX, posY);

			if (tile != null) 
			{

				GameObject obj = SimplePool.Spawn (tileMarker, new Vector3 (posX, posY, -1), Quaternion.identity);
				obj.transform.SetParent (this.transform, true);

				tileMarkerGameObjects.Add (obj);
			}

			EventsHandler.Invoke_cb_editorTilesSelected (tile);

		}

	}





	void UpdateDragging()
	{

		if ((EventSystem.current.IsPointerOverGameObject()) && (isDragging == false))
		{
			return;
		}


		if (Input.GetMouseButtonDown (0)) 
		{

			dragStartPosition = currentFramePosition;
			isDragging = true;

		}

		// The tile we started at

		int startX = Mathf.FloorToInt(dragStartPosition.x);
		int startY =  Mathf.FloorToInt(dragStartPosition.y);

		// The tile we are on right now

		int endX = Mathf.FloorToInt(currentFramePosition.x);
		int endY = Mathf.FloorToInt(currentFramePosition.y);

		//Debug.Log (currentFramePosition);

		//Debug.Log (endX.ToString () + "," + endY.ToString ());


		// Making it friendly :)


		if (endX < startX) 
		{
			int temp = startX;
			startX = endX;
			endX = temp;
		
		}


		if (endY < startY) 
		{
			int temp = startY;
			startY = endY;
			endY = temp;


		}


		// Destroying old markers

		DestroyMarkers ();



		// Creation of the markers

		if ((Input.GetMouseButton (0)) && (isDragging)) 
		{

			for (int x = startX; x <= endX; x++) 
			{
				for (int y = startY; y <= endY; y++) 
				{

					Tile tile = EditorRoomManager.instance.room.MyGrid.GetTileAt (x, y);

					if (tile != null) 
					{

						GameObject obj = SimplePool.Spawn (tileMarker, new Vector3 (x, y, -1), Quaternion.identity);
						obj.transform.SetParent (this.transform, true);

						tileMarkerGameObjects.Add (obj);

					}

				}

			}

		}



		// Creating objects


		if ((Input.GetMouseButtonUp (0)) && (isDragging)) 
		{

			isDragging = false;

			List<Tile> tileList = new List<Tile> ();

			for (int x = startX; x <= endX; x++) 
			{
				for (int y = startY; y <= endY; y++) 
				{

					Tile tile = EditorRoomManager.instance.room.MyGrid.GetTileAt (x, y);

					if (tile != null) 
					{
					
						tileList.Add (tile);
					
					}
				}
			}


			//EventsHandler.Invoke_cb_editorTilesSelected (tileList);

		}
	}





}
