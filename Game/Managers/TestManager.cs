using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TestManager : MonoBehaviour {


	/*
	 
	TestModel model;



	// Use this for initialization
	void Start () {

	
	

	}





	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.J)) 		
		{		
		
			RoomToJson ();
		
		}


	}





	public void WriteJson()
	{

		model = new TestModel();
		model.myList = new List<string> ();

		model.myList.Add ("string1");
		model.myList.Add ("string2");
		model.myList.Add ("string3");

		model.myInt = 5;
		model.myFloat = 0.5f;
		model.myString = "model string";

		string myJson = JsonUtility.ToJson (model);



	}




	public void ReadJson()
	{

		TextAsset myTextAsset = (TextAsset) Resources.Load ("Jsons/TestJson");

		model = JsonUtility.FromJson<TestModel> (myTextAsset.text);

		//Debug.Log (model.myList[2]);


	}


	public void RoomToJson()
	{
	
		string myJson = JsonUtility.ToJson (RoomManager.instance.myRoom);

		//Debug.Log (myJson);


	}


*/
}
