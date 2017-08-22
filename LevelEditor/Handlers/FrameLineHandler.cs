using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLineHandler : MonoBehaviour {



	List<GameObject> lineContainer = new List<GameObject>();
	Transform parent;



	// Use this for initialization
	void Start () 
	{
		
		EventsHandler.cb_tileLayoutChanged += CreateFrames;
		parent = new GameObject ("Frames").transform;

	}



	void OnDestroy()
	{

		EventsHandler.cb_tileLayoutChanged -= CreateFrames;

	}


	public void CreateFrames()
	{

		lineContainer.ForEach (obj => Destroy (obj));
		lineContainer.Clear ();

		if (EditorRoomManager.instance.furnitureGameObjectMap != null) 
		{
			foreach (Furniture furn in EditorRoomManager.instance.furnitureGameObjectMap.Keys) 
			{
				GameObject obj = ActionBoxManager.CreateFrame (furn, true);
				obj.transform.SetParent (parent);
				lineContainer.Add(obj);
			}
		}

		if (EditorRoomManager.instance.characterGameObjectMap != null) 
		{			
			foreach (Character character in EditorRoomManager.instance.characterGameObjectMap.Keys) 
			{
				GameObject obj = ActionBoxManager.CreateFrame (character, true);

				lineContainer.Add(obj);
			}
		}
	}

}
