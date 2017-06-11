using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour {



	// Singleton //

	public static MapUI instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	GameObject mapObject;

	GameObject bg_asylumOutside;
	GameObject bg_asylumOutside_shadow;
	GameObject bg_abandonedWing;
	GameObject bg_abandonedWing_mirror;
	GameObject bg_asylum;
	GameObject bg_asylum_mirror;


	// Use this for initialization

	public void Initialize () 
	{		
		CreateMap();
		CloseMap();	

		EventsHandler.cb_key_m_pressed += OnMapKeyPressed;
	}


	void OnDestroy()
	{
		EventsHandler.cb_key_m_pressed -= OnMapKeyPressed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	public void CreateMap()
	{
		// If there's already an inventory, display error

		if (mapObject != null) 
		{
			Debug.LogError ("There's already a map object");
		}

		mapObject = Instantiate (Resources.Load<GameObject> ("Prefabs/MapUI"));

		bg_asylumOutside = mapObject.transform.Find ("BG").Find ("BG_AsylumOutside").gameObject;
		bg_asylumOutside_shadow = mapObject.transform.Find ("BG").Find ("BG_AsylumOutside_Shadow").gameObject;
		bg_abandonedWing = mapObject.transform.Find ("BG").Find ("BG_AbandonedWing").gameObject;
		bg_abandonedWing_mirror = mapObject.transform.Find ("BG").Find ("BG_AbandonedWing_Mirror").gameObject;
		bg_asylum = mapObject.transform.Find ("BG").Find ("BG_Asylum").gameObject;
		bg_asylum_mirror = mapObject.transform.Find ("BG").Find ("BG_Asylum_Mirror").gameObject;
	}


	// When pressing p

	public void OnMapKeyPressed()
	{
		if (GameManager.actionBoxActive) 
		{
			return;
		}

		if (GameManager.inventoryOpen) 
		{
			return;
		}

		if (GameManager.settingsOpen) 
		{
			return;
		}

		if (GameManager.mapOpen == false)
		{		
			if ((RoomManager.instance.myRoom.mapArea == string.Empty) || (RoomManager.instance.myRoom.mapArea == "None"))
			{
				Debug.Log ("no map");
			}

			OpenMap ();

		} else {

			CloseMap ();
		}
	}


	public void OpenMap()
	{
		mapObject.SetActive (true);

		GameManager.mapOpen = true;

		SetMapBG ();

		EventsHandler.Invoke_cb_inputStateChanged ();
	}
		

	void SetMapBG()
	{
		bg_asylumOutside.SetActive (false);
		bg_asylumOutside_shadow.SetActive (false);
		bg_abandonedWing.SetActive (false);
		bg_abandonedWing_mirror.SetActive (false);
		bg_asylum.SetActive (false);
		bg_asylum_mirror.SetActive (false);

		switch (RoomManager.instance.myRoom.mapArea) 
		{

			case "Asylum":

				bg_asylum.SetActive (true);

				break;


			case "Asylum_mirror":

				bg_asylum_mirror.SetActive (true);

				break;


			case "Asylum_outside":

				bg_asylumOutside.SetActive (true);

				break;


			case "Asylum_outside_shadow":

				bg_asylumOutside_shadow.SetActive (true);

				break;


			case "Abandoned_wing":

				bg_abandonedWing.SetActive (true);

				break;


			case "Abandoned_wing_mirror":

				bg_abandonedWing_mirror.SetActive (true);

				break;
		}
	}


	// ----- CLOSE MAP ----- //

	public void CloseMap()
	{
		mapObject.SetActive (false);

		GameManager.mapOpen = false;
		EventsHandler.Invoke_cb_inputStateChanged ();
	}



}