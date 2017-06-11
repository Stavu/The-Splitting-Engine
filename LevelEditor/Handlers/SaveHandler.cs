using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SaveHandler : MonoBehaviour {





	// Use this for initialization
	void Start () 
	{
		
	}


	// Update is called once per frame
	void Update () 
	{
		
	}


	public void SaveToFile()
	{

		//Debug.Log (EditorRoomManager.instance.room.bgName);

		EditorRoomManager.instance.SerializeRoom ();
	
	}





}
