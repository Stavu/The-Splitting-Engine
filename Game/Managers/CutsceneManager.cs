using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {


	// Singleton //

	public static CutsceneManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	// Singleton //



	public Dictionary <string,IEnumerator> stringCutsceneMap;
	public static bool inCutscene = false;


	// Use this for initialization

	void Start () 
	{
		stringCutsceneMap = new Dictionary<string, IEnumerator> ();

		Cutscene openingCutScene = new OpeningScene ("opening_scene");
		stringCutsceneMap.Add (openingCutScene.myName, openingCutScene.MyCutscene());

		Cutscene waterCanCutScene = new WaterCanScene ("water_can_scene");
		stringCutsceneMap.Add (waterCanCutScene.myName, waterCanCutScene.MyCutscene());

		Cutscene coverManCutscene = new CoverManScene ("cover_man_scene");
		stringCutsceneMap.Add (coverManCutscene.myName, coverManCutscene.MyCutscene());
	
		Cutscene toolboxCutscene = new ToolboxScene ("technician_toolbox_scene");
		stringCutsceneMap.Add (toolboxCutscene.myName, toolboxCutscene.MyCutscene());

		Cutscene busToAsylumCutscene = new BusToAsylumScene ("bus_to_asylum_scene");
		stringCutsceneMap.Add (busToAsylumCutscene.myName, busToAsylumCutscene.MyCutscene());

		Cutscene OpenGreenDoorScene = new OpenGreenDoorScene ("open_green_door_scene");
		stringCutsceneMap.Add (OpenGreenDoorScene.myName, OpenGreenDoorScene.MyCutscene());

		Cutscene OpenGreenDoorMirrorScene = new OpenGreenDoorMirrorScene ("open_green_door_mirror_scene");
		stringCutsceneMap.Add (OpenGreenDoorMirrorScene.myName, OpenGreenDoorMirrorScene.MyCutscene());
	}
	
	// Update is called once per frame

	void Update () 
	{
		/*
		if(Input.GetKeyDown(KeyCode.G))
		{
			PlayCutscene ("switch_player_cutscene");	
		}
		*/
	}



	public void PlayCutscene(string cutsceneName)
	{
		if (stringCutsceneMap.ContainsKey (cutsceneName) == false) 
		{
			Debug.LogError ("no cutscene with this name " + cutsceneName);
			return;
		}

		inCutscene = true;
		EventsHandler.Invoke_cb_inputStateChanged ();

		StartCoroutine (stringCutsceneMap [cutsceneName]);	

	}

	public void EaseInObject (GameObject obj, Vector3 target, float maxSpeed, bool destroyOnArrival, bool invokeClearToContinue) 
	{
		StartCoroutine ( MovementUtilities.EaseIn(obj, target, maxSpeed, destroyOnArrival, invokeClearToContinue) );
	}

	public void EaseOutObject (GameObject obj, Vector3 target, float maxSpeed, bool destroyOnArrival, bool invokeClearToContinue) 
	{
		StartCoroutine ( MovementUtilities.EaseOut(obj, target, maxSpeed, destroyOnArrival, invokeClearToContinue) );
	}

	public void MoveInConstantSpeed (GameObject obj, Vector3 target, float maxSpeed, bool destroyOnArrival, bool invokeClearToContinue) 
	{
		StartCoroutine ( MovementUtilities.MoveConstantSpeed(obj, target, maxSpeed, destroyOnArrival, invokeClearToContinue) );
	}


}

