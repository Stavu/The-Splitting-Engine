using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Handler : MonoBehaviour {

	// Singleton //

	public static Character_Handler instance { get; protected set;}

	void Awake () {		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	// Singleton //


	// ---- MOVE CHARACTER ---- //


	public void MoveByPath (IWalker walker, List<Vector2> posList)
	{
		Queue<Vector2> path = new Queue<Vector2> ();

		foreach (Vector2 pos in posList) 
		{
			path.Enqueue (pos);					
		}

		walker.walkerPath = path;
		walker.walkerTargetPos = walker.walkerPath.Dequeue ();

		MoveToTargetTile (walker, walker.walkerTargetPos);
	}



	public void MoveToTargetTile(IWalker walker, Vector2 myTargetPos)
	{
		Tile currentTile = RoomManager.instance.myRoom.MyGrid.GetTileAt ((int)walker.speakerPos.x, (int)walker.speakerPos.y);

		if (currentTile.myCharacter == walker) 
		{
			currentTile.myCharacter = null;
		}

		walker.walkerTargetPos = myTargetPos;

		StartCoroutine (MoveCoroutine (walker));
	}



	public IEnumerator MoveCoroutine(IWalker walker)
	{
		// the walker's position and the target position

		Vector3 startPos = walker.walkerGameObject.transform.position;
		Vector3 endPos = Utilities.GetCharacterPosOnTile (walker, walker.walkerTargetPos);

		float distance = Vector3.Distance (startPos, endPos);

		float tempSpeed = walker.walkerSpeed / distance;

		// interpolation
		float inter = 0;

		// -- ANIMATIONS -- //

		// declarations

		Animator myAnimator = walker.walkerGameObject.GetComponent<Animator>();
		Direction lastDirection = Direction.left;

		// coords - for the animation 

		Coords startCoords = new Coords (walker.speakerPos.x, walker.speakerPos.y);
		Coords endCoords = new Coords (walker.walkerTargetPos.x, walker.walkerTargetPos.y);

		// Walk down

		if (startCoords.y > endCoords.y) 			
		{
			myAnimator.PlayInFixedTime ("Walk_front");
			lastDirection = Direction.down;
		}

		// Walk up

		if (startCoords.y < endCoords.y) 			
		{
			myAnimator.PlayInFixedTime ("Walk_back");
			lastDirection = Direction.up;
		}

		// Walk left

		if (startCoords.x > endCoords.x) 			
		{
			myAnimator.PlayInFixedTime ("Walk_left");
			lastDirection = Direction.left;

		}

		// Walk right

		if (startCoords.x < endCoords.x) 			
		{
			myAnimator.PlayInFixedTime ("Walk_right");
			lastDirection = Direction.right;
		}

		// while loop - updating the character object position

		while(startPos != endPos)
		{
			inter += tempSpeed * Time.deltaTime;

			if (inter >= 1) 
			{				
				walker.walkerGameObject.transform.position = startPos = endPos;
				break;

			} else {

				walker.walkerGameObject.transform.position = Vector3.Lerp (startPos, endPos, inter);
			}

			// changing the sorting layer
			walker.walkerGameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = Mathf.FloorToInt( -(walker.walkerGameObject.transform.position.y * 10) + 6f);

			yield return new WaitForFixedUpdate ();
		}

		// After while loop is done, change the character tile

		walker.ChangePos (walker.walkerTargetPos);

		switch (lastDirection) 
		{
			case Direction.left:

				myAnimator.PlayInFixedTime ("Idle_left");

				break;

			case Direction.right:

				myAnimator.PlayInFixedTime ("Idle_right");

				break;

			case Direction.down:

				myAnimator.PlayInFixedTime ("Idle_front");

				break;

			case Direction.up:

				myAnimator.PlayInFixedTime ("Idle_back");

				break;
		}

		// Check if I need to continue walking 

		if (walker.walkerPath != null) 
		{
			if (walker.walkerPath.Count > 0) 
			{
				walker.walkerTargetPos = walker.walkerPath.Dequeue ();
				MoveToTargetTile (walker, walker.walkerTargetPos);
			
			} else {

				EventsHandler.Invoke_Callback (EventsHandler.cb_characterFinishedPath);
			}
		}
	}

}
