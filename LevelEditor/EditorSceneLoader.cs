using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSceneLoader : MonoBehaviour {


	public GameObject managers;



	// Use this for initialization
	void Start () {

		CreateManagers ();

	}




	// Update is called once per frame
	void Update () {

	}



	void CreateManagers()
	{

		Instantiate (managers);	

		// Initializing

		EditorTileManager.instance.Initialize ();
		BuildController.instance.Initialize ();
		EditorRoomManager.instance.Initialize ();


		// Starting Functions

		if (EditorRoomManager.loadRoomFromMemory == true) 
		{
			//Debug.Log ("loading room");
			EditorRoomManager.instance.InitializeRoom (EditorRoomManager.instance.room.bgName);

		} else {

			//Debug.Log ("creating room");
			EditorRoomManager.instance.InitializeRoom ("abandoned_lobby_bg");
		}
	

		EditorUI.instance.CreateUI ();

		BuildController.instance.mode = BuildController.Mode.inspect;

		EditorTileManager.instance.ColorTiles ();

		gameObject.AddComponent<FrameLineHandler> ();
	
	
	
	
	
	}



}
