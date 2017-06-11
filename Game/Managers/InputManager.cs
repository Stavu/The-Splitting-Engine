using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



public enum Direction
{
	left,right,up,down
}



public class InputManager : MonoBehaviour {



	// Singleton //

	public static InputManager instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //



	// Direction List

	List<Direction> directionList;
	public Direction lastDirection;

	bool playerIdle = true;



	// Use this for initialization

	public void Initialize () 
	{

		directionList = new List<Direction> ();

	}


	
	// Update is called once per frame

	void Update () {


		CheckKeys ();
		CheckKeysDown ();

	}




	void CheckKey(Direction direction, KeyCode keycode)
	{
		

		if (Input.GetKey (keycode)) 		
		{
			if (directionList.Contains (direction) == false) 
			{
				directionList.Add (direction);
			}

		} else {

			if (directionList.Contains (direction) == true) 
			{
				directionList.Remove (direction);

			} 
		}

	}




	void CheckKeys()
	{

		CheckKey (Direction.left, KeyCode.LeftArrow);
		CheckKey (Direction.right, KeyCode.RightArrow);
		CheckKey (Direction.down, KeyCode.DownArrow);
		CheckKey (Direction.up, KeyCode.UpArrow);

		if (directionList.Count > 0) 
		{
			EventsHandler.Invoke_cb_keyPressed (directionList [0]);
			lastDirection = directionList [0];
			playerIdle = false;
		
		} else if (playerIdle == false)
		{				
			EventsHandler.Invoke_cb_noKeyPressed (lastDirection);
			playerIdle = true;
		}




		if (Input.GetKeyDown (KeyCode.Space))
		{			
			EventsHandler.Invoke_cb_spacebarPressed ();
		}

		if (Input.GetKeyDown (KeyCode.Escape))
		{			
			EventsHandler.Invoke_cb_escapePressed ();
		}


	}




	void CheckKeysDown()
	{
		
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{				
			EventsHandler.Invoke_cb_keyPressedDown (Direction.up);
		} 

		else if (Input.GetKeyDown (KeyCode.DownArrow)) 	
		{			
			EventsHandler.Invoke_cb_keyPressedDown (Direction.down);		
		} 

		else if (Input.GetKeyDown (KeyCode.LeftArrow)) 			
		{			
			EventsHandler.Invoke_cb_keyPressedDown (Direction.left);
		} 

		else if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			EventsHandler.Invoke_cb_keyPressedDown (Direction.right);
		} 	

		if (Input.GetKeyDown (KeyCode.I)) 
		{
			EventsHandler.Invoke_cb_key_i_pressed ();
		} 

		if (Input.GetKeyDown (KeyCode.P)) 
		{
			EventsHandler.Invoke_cb_key_p_pressed ();
		} 

		if (Input.GetKeyDown (KeyCode.M)) 
		{
			EventsHandler.Invoke_cb_key_m_pressed ();
		} 

	}


}
