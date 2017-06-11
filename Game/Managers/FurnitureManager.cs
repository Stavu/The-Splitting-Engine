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



	/*
	public void CreateFurnitureGameObject (Furniture myFurniture)
	{
		// if the furniture has an identification name, use it as the name. If it doesn't, use the file name.

		bool useIdentifiactionName = ((myFurniture.identificationName != null) && (myFurniture.identificationName != string.Empty));
		string furnitureName = useIdentifiactionName ? myFurniture.identificationName : myFurniture.fileName;			

		myFurniture.myPos = new Vector3 (myFurniture.x + myFurniture.mySize.x/2, myFurniture.y, 0);

		GameObject obj = null;
		SpriteRenderer sr = null;


		// Animated Object

		GameObject[] animatedObjects = Resources.LoadAll<GameObject> ("Prefabs/Furniture");

		foreach (GameObject gameObj in animatedObjects) 
		{
			if (gameObj.name == myFurniture.fileName) 
			{
				obj = Instantiate (gameObj);
				sr = obj.GetComponentInChildren<SpriteRenderer>();

				string state = GameManager.playerData.GetAnimationState (myFurniture.identificationName);

				PI_Handler.instance.AddPIToMap (myFurniture, obj, furnitureName);

				if (state != string.Empty) 
				{
					PI_Handler.instance.SetPIAnimationState (myFurniture.identificationName, state, obj);
				} 

				break;
			}	
		}

		// if not animated object

		if (obj == null) 
		{
			obj = new GameObject (myFurniture.fileName);
			GameObject childObj = new GameObject ("Image");
			childObj.transform.SetParent (obj.transform);

			PI_Handler.instance.AddPIToMap (myFurniture, obj, furnitureName);
					
			sr = childObj.AddComponent<SpriteRenderer>();
			sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myFurniture.fileName); 
		}


		obj.transform.SetParent (this.transform);
		obj.transform.position = new Vector3 (myFurniture.myPos.x + myFurniture.offsetX, myFurniture.myPos.y + 0.5f + myFurniture.offsetY, myFurniture.myPos.z);


		// sorting order 
		Utilities.SetPISortingOrder (myFurniture, obj);


		if (myFurniture.walkable == true) 
		{
			sr.sortingOrder = (int) -(myFurniture.y + myFurniture.mySize.y) * 10;

		}

		sr.sortingLayerName = Constants.furniture_character_layer;


	}


	*/




}
