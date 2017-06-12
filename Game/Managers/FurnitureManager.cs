using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureManager : MonoBehaviour {


	// Singleton //

	public static FurnitureManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	//public Dictionary<Furniture,GameObject> furnitureGameObjectMap;
	//public Dictionary<string,Furniture> nameFurnitureMap;



	// Use this for initialization

	public void Initialize () 
	{		
	

		//furnitureGameObjectMap = new Dictionary<Furniture, GameObject> ();
		//nameFurnitureMap = new Dictionary<string, Furniture> ();

	}


	public void OnDestroy()
	{	
	
		//EventsHandler.cb_furnitureChanged -= CreateFurnitureGameObject;
			
	}




	// Update is called once per frame

	void Update () 
	{
		
	}





}
