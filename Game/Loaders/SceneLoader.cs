using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour {


	public GameObject managers;
	public GameObject staticManagers;




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

		if(GameObject.Find("StaticManagers") == null)
		{
			GameObject obj = Instantiate (staticManagers);
			obj.name = "StaticManagers";
		}			

		//PlayerManager.instance.CreatePlayers ();
		GameManager.instance.CreateInventoryItemData ();
		GameManager.instance.CreateUserData ();

		GameManager.instance.Initialize ();
		RoomManager.instance.Initialize ();

		PI_Handler.instance.Initialize ();
		FurnitureManager.instance.Initialize ();
		CharacterManager.instance.Initialize ();

		InputManager.instance.Initialize ();
		TileManager.instance.Initialize ();
		PlayerManager.instance.Initialize ();
		ActionBoxManager.instance.Initialize ();
		InteractionManager.instance.Initialize ();	
		GameActionManager.instance.Initialize ();
		DialogueManager.instance.Initialize ();
		//SoundManager.instance.Initialize ();
		RoomStarter.instance.Initialize();
			

		RoomManager.instance.BuildRoom ();

		FindObjectOfType<DebugHelper> ().Initialize ();
		InventoryUI.instance.Initialize ();
		SettingsUI.instance.Initialize ();
		MapUI.instance.Initialize ();

		// Testing 

		//Testing.TestAll ();

	}



}
