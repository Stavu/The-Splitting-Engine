using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utilities {


	public static int myVar;
	public int myVar2;
	int myVar3; 




	public static void AdjustOrthographicCamera(Room room)
	{
		Camera.main.orthographicSize = 12; 

		Camera.main.transform.position = new Vector3 ((room.myWidth / 2), (room.myHeight / 2), -10);
	}


	public static Vector2 GetObjectScreenPosition (Vector3 myPos, RectTransform myTransform, float offsetX, float offsetY)
	{

		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(myPos);
		Vector2 WorldObject_ScreenPosition = new Vector2
			(
				((ViewportPosition.x * myTransform.sizeDelta.x) - (myTransform.sizeDelta.x*0.5f)) + offsetX,
				((ViewportPosition.y * myTransform.sizeDelta.y) - (myTransform.sizeDelta.y*0.5f)) + offsetY
			);

		return WorldObject_ScreenPosition;

	}


	public static List<string> SeparateText(string rawText)
	{

		if(rawText == string.Empty)
		{
			return null;
		}
	
		List<string> textList = new List<string> ();

		char delimiter = '|';
		string[] result = rawText.Split (delimiter);

		foreach (string str in result)
		{
			textList.Add (str);
		}

		//Debug.Log (textList);


		return textList;



	}


	// Editor function

	public static GameObject CreateEditorFurnitureGameObject (Furniture myFurniture, Transform parent)
	{		

		myFurniture.myPos = new Vector3 (myFurniture.x + myFurniture.mySize.x/2, myFurniture.y, 0);

		GameObject obj = null;
		SpriteRenderer sr = null;

		if (EditorRoomManager.stringPrefabMap.ContainsKey (myFurniture.fileName)) 
		{
			obj = GameObject.Instantiate (EditorRoomManager.stringPrefabMap[myFurniture.fileName]);
			obj.transform.SetParent (parent);

			sr = obj.GetComponentInChildren<SpriteRenderer>();
		}
		

		if (obj == null) 
		{
			obj = new GameObject (myFurniture.fileName);
			obj.transform.SetParent (parent);

			GameObject childObj = new GameObject ("Image");
			childObj.transform.SetParent (obj.transform);

			sr = childObj.AddComponent<SpriteRenderer>();
			sr.sprite = Resources.Load<Sprite> ("Sprites/Furniture/" + myFurniture.fileName); 
			sr.flipX = myFurniture.imageFlipped;
		}

		/*
		if (myFurniture.currentGraphicState.frameExtents == Vector2.zero) 
		{
			Debug.Log ("frameExtents " + myFurniture.currentGraphicState.frameExtents);
			myFurniture.currentGraphicState.frameExtents = sr.sprite.bounds.extents;
		}
		*/	

		obj.transform.position = new Vector3 (myFurniture.myPos.x + myFurniture.offsetX, myFurniture.myPos.y + 0.5f + myFurniture.offsetY, myFurniture.myPos.z);
		Debug.Log (obj.transform.position);


		// sorting order 

		sr.sortingOrder = -myFurniture.y * 10;

		if (myFurniture.walkable == true) 
		{
			sr.sortingOrder = (int) -(myFurniture.y + myFurniture.mySize.y) * 10;

		}

		sr.sortingLayerName = Constants.furniture_character_layer;

		return obj;

	}





	public static void SetPISortingOrder(PhysicalInteractable myPI, GameObject obj)
	{

		// sorting order 

		SpriteRenderer[] sr_list = obj.GetComponentsInChildren<SpriteRenderer> ();
		//Debug.Log ("length" + obj.transform.childCount);


		for (int i = 0; i < obj.transform.childCount; i++)
		{
			obj.transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingOrder = i + (-myPI.y * 10);			
		}

		/*

		// exceptions

		if (myPI.fileName == "busStop") 
		{
			SpriteRenderer sr;
			sr = obj.transform.Find ("Back").GetComponent<SpriteRenderer> ();
			sr.sortingOrder = sr.sortingOrder - 65;
		}

		if (myPI.fileName == "busStop_shadow") 
		{
			SpriteRenderer sr;
			sr = obj.transform.Find ("Back").GetComponent<SpriteRenderer> ();
			sr.sortingOrder = sr.sortingOrder - 65;
		}
		*/

	}




	public static GameObject CreateCharacterGameObject (Character myCharacter, Transform parent)
	{
		//Debug.Log ("Assign Furniture Image");

		myCharacter.myPos = new Vector3 (myCharacter.x + myCharacter.mySize.x/2, myCharacter.y, 0);

		GameObject obj = GameObject.Instantiate(Resources.Load<GameObject> ("Prefabs/Characters/" + myCharacter.fileName)); 			
		obj.transform.SetParent (parent);


		obj.transform.position = new Vector3 (myCharacter.myPos.x + myCharacter.offsetX, myCharacter.myPos.y + 0.5f + myCharacter.offsetY, myCharacter.myPos.z);

		//Debug.Log ("object position" + myFurniture.myName + obj.transform.position + sr.sprite.bounds);


		// sorting order 

		obj.GetComponentInChildren<SpriteRenderer>().sortingOrder  = -myCharacter.y * 10;
		obj.GetComponentInChildren<SpriteRenderer>().sortingLayerName = Constants.furniture_character_layer;

		return obj;


	}



	public static Vector2 GetCharacterPosOnTile(IWalker character, Vector2 myPos)
	{	
		
		Vector2 pos = new Vector2 (myPos.x + character.speakerSize.x / 2 + character.walkerOffsetX, myPos.y + character.walkerOffsetY + 0.5f);

		return pos;

	}




	// When it's the player	


	public static List<DialogueSentence> CreateSentenceList (ISpeaker speaker, List<string> textlist)
	{

		List<DialogueSentence> sentenceList = new List<DialogueSentence> ();


		foreach (string str in textlist) 
		{
			DialogueSentence tempSentence = new DialogueSentence (speaker.speakerName, str, false);
			sentenceList.Add (tempSentence);
		}

		return sentenceList;

	}



	public static List<DialogueSentence> CreateSentenceList (ISpeaker speaker, string text)
	{

		List<DialogueSentence> sentenceList = new List<DialogueSentence> ();

		DialogueSentence tempSentence = new DialogueSentence (speaker.speakerName, text, false);
		sentenceList.Add (tempSentence);

		return sentenceList;

	}


	public static bool EvaluateConditions(List<Condition> conditionList)
	{

		if (conditionList.Count == 0) 
		{
			return true;
		}


		bool evaluation = true;

		foreach (Condition cond in conditionList) 
		{
			
			evaluation = cond.EvaluateCondition ();

		}

		return evaluation;


	}


	// FADE IN AND OUT - from shadow to mirror and vice versa //


	public static IEnumerator FadeBetweenSprites(List<SpriteRenderer> fadeOutSprites, List<SpriteRenderer> fadeInSprites)
	{		
		float speed = 1f;
		float inter = 0;

		while(inter < 1)
		{
			foreach (SpriteRenderer sr in fadeOutSprites) 
			{
				sr.color = new Color (1,1,1,1-inter);
			}

			foreach (SpriteRenderer sr in fadeInSprites) 
			{
				sr.color = new Color (1,1,1,inter);
			}

			// speed - how many seconds it will take 

			inter += Time.deltaTime / speed;
		
			yield return new WaitForFixedUpdate ();		
		}

		SwitchBetweenSprites (fadeOutSprites, fadeInSprites);
	}



	public static void SwitchBetweenSprites(List<SpriteRenderer> outSprites, List<SpriteRenderer> inSprites)
	{
		foreach (SpriteRenderer sr in outSprites) 
		{
			sr.color = Color.clear;
		}

		foreach (SpriteRenderer sr in inSprites) 
		{
			sr.color = Color.white;
		}

		EventsHandler.Invoke_cb_tileLayoutChanged ();
	}


	/// <summary>
	/// Gets the passed subinteractions.
	/// Returns a list of the subinteractions that passed the conditions
	/// </summary>
	/// <returns>The passed subinteractions.</returns>
	/// <param name="subIntList">Sub int list.</param>

	public static List<SubInteraction> GetPassedSubinteractions (List<SubInteraction> subIntList)
	{		
		// check if subinteractions passed the conditions

		List<SubInteraction> subinteractionsToDo = new List<SubInteraction> ();

		foreach (SubInteraction subInt in subIntList) 
		{
			bool passedConditions = Utilities.EvaluateConditions (subInt.conditionList);

			if (passedConditions == true) 
			{
				subinteractionsToDo.Add (subInt);
			}
		}

		return subinteractionsToDo;
	}




	public static List<Vector3> GetPhysicalInteractableFrameBounds(PhysicalInteractable myPhysicalInt)
	{
		
		// declerations 

		Vector2 frameBounds = myPhysicalInt.CurrentGraphicState().frameExtents;

		List<Vector3> positions = new List<Vector3> ();

		SpriteRenderer sr = PI_Handler.instance.PI_gameObjectMap [myPhysicalInt].GetComponentInChildren<SpriteRenderer>();

		Vector3 center = sr.bounds.center + new Vector3 (myPhysicalInt.currentGraphicState.frameOffsetX, myPhysicalInt.currentGraphicState.frameOffsetY, 0);

		// center 

		positions.Add(center);

		// positioning frame pieces

		if (frameBounds == Vector2.zero) 
		{
			frameBounds = sr.bounds.extents;
		}

		//down left
		positions.Add(new Vector3 (-frameBounds.x, -frameBounds.y,0));

		//down right
		positions.Add(new Vector3 (frameBounds.x, -frameBounds.y,0));

		//up left
		positions.Add(new Vector3 (-frameBounds.x, frameBounds.y,0));

		//up right
		positions.Add(new Vector3 (frameBounds.x, frameBounds.y,0));



		return positions;
	}





	public static List<Vector3> EditorGetPhysicalInteractableFrameBounds(PhysicalInteractable myPhysicalInt)
	{
		// declerations 

		Vector2 frameBounds = myPhysicalInt.currentGraphicState.frameExtents;

		List<Vector3> positions = new List<Vector3> ();

		if (myPhysicalInt is Furniture) 		
		{

			Furniture myFurniture = (Furniture)myPhysicalInt;


			SpriteRenderer sr = EditorRoomManager.instance.furnitureGameObjectMap[myFurniture].GetComponentInChildren<SpriteRenderer>();

			if (sr == null) 
			{
				Debug.Log ("sr is null");
			}



			Vector3 center = sr.bounds.center;

			// center 

			positions.Add(center);

			// positioning frame pieces

			if (frameBounds == Vector2.zero) 
			{
				frameBounds = sr.bounds.extents;
			}			

		}


		if (myPhysicalInt is Character) 		
		{
			Character myCharacter = (Character)myPhysicalInt;

			GameObject myObject = EditorRoomManager.instance.characterGameObjectMap [myCharacter];
			Vector3 center = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.center;

			// center 
			positions.Add(center);

			// positioning frame pieces

			if (frameBounds == Vector2.zero) 
			{
				frameBounds = myObject.GetComponentInChildren<SpriteRenderer> ().bounds.extents;
			}
		}


		//down left
		positions.Add(new Vector3 (-frameBounds.x, -frameBounds.y,0));

		//down right
		positions.Add(new Vector3 (frameBounds.x, -frameBounds.y,0));

		//up right
		positions.Add(new Vector3 (frameBounds.x, frameBounds.y,0));

		//up left
		positions.Add(new Vector3 (-frameBounds.x, frameBounds.y,0));

		return positions;
	}




	public static List<GraphicState> GetGraphicStateList (PhysicalInteractable physicalInteractable)
	{

		GameObject obj = null;
		List<GraphicState> graphicStateList = new List<GraphicState> ();


		if (physicalInteractable is Furniture) 
		{
			Furniture furn = (Furniture)physicalInteractable;

			obj = EditorRoomManager.instance.furnitureGameObjectMap [furn];
		}


		if (physicalInteractable is Character) 
		{
			Character character = (Character)physicalInteractable;

			obj = EditorRoomManager.instance.characterGameObjectMap [character];
		}


		Animator animator = obj.GetComponent<Animator> ();

		if (animator != null) 
		{
			if (animator.runtimeAnimatorController.animationClips.Length == 0) 
			{
				Debug.LogError ("the animator has no clips");
			}

			foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) 
			{
				GraphicState graphicState = new GraphicState ();
				graphicState.graphicStateName = clip.name;

				graphicState.frameExtents = obj.GetComponentInChildren<SpriteRenderer>().bounds.extents; // FIXME
				graphicState.frameOffsetX = 0;
				graphicState.frameOffsetY = 0;

				graphicState.coordsList = new List<Coords> ();

				graphicStateList.Add (graphicState);

				Debug.Log ("animation list " + graphicStateList.Count);
			}

		} else {

			// If there's no animator

			GraphicState defaultGraphicState = new GraphicState ();

			defaultGraphicState.graphicStateName = "default";

			defaultGraphicState.frameExtents = obj.GetComponentInChildren<SpriteRenderer>().bounds.extents;
			defaultGraphicState.frameOffsetX = 0;
			defaultGraphicState.frameOffsetY = 0;

			defaultGraphicState.coordsList = new List<Coords> ();

			graphicStateList.Add (defaultGraphicState);

		}

		return graphicStateList;


	}





	public static List<string> GetAnimationClipNames(GameObject obj)
	{
		
		List<string> animationList = new List<string> ();

		if (obj != null) 
		{			
	
			Animator animator = obj.GetComponent<Animator> ();

			if (animator == null) 
			{
				return animationList;
			}

			foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) 
			{
				animationList.Add (clip.name);
				//Debug.Log ("animation list " + animationList.Count);
			}

		}

		return animationList;
	}




}

