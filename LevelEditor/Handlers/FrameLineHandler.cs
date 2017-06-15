using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLineHandler : MonoBehaviour {



	List<GameObject> lineContainer = new List<GameObject>();




	// Use this for initialization
	void Start () 
	{
		
		EventsHandler.cb_tileLayoutChanged += CreateLines;

	}



	void OnDestroy()
	{

		EventsHandler.cb_tileLayoutChanged -= CreateLines;

	}

	
	// Update is called once per frame

	void Update () 
	{
		
	}




	// Draw Line


	public void CreateLine(PhysicalInteractable physicalInteractable)
	{
		
		GameObject lineObj = new GameObject ("frameline_" + physicalInteractable.identificationName);
		lineObj.transform.SetParent (this.transform);

		LineRenderer lr = lineObj.AddComponent<LineRenderer> ();
		lr.loop = true;
		lr.positionCount = 4;
		lr.widthMultiplier = 0.1f;


		List<Vector3> positionList = Utilities.EditorGetPhysicalInteractableFrameBounds (physicalInteractable);
		//Debug.Log ("center" + positionList [0]);


		Vector3[] posArray = new Vector3[4];


		for (int i = 1; i < positionList.Count; i++) 
		{
			posArray [i - 1] = positionList [i] + positionList [0] + new Vector3 (physicalInteractable.currentGraphicState.frameOffsetX, physicalInteractable.currentGraphicState.frameOffsetY,-5);
		}


		lr.SetPositions (posArray);

		lineContainer.Add (lineObj);

	}





	public void CreateLines()
	{

		lineContainer.ForEach (obj => Destroy (obj));
		lineContainer.Clear ();

		if (EditorRoomManager.instance.furnitureGameObjectMap != null) 
		{
			foreach (Furniture furn in EditorRoomManager.instance.furnitureGameObjectMap.Keys) 
			{
				CreateLine (furn);
			}
		}

		if (EditorRoomManager.instance.characterGameObjectMap != null) 
		{			
			foreach (Character character in EditorRoomManager.instance.characterGameObjectMap.Keys) 
			{
				CreateLine (character);
			}
		}


	}




}
