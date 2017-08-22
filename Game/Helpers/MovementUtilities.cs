using UnityEngine;
using System.Collections;

public class MovementUtilities
{

	public static IEnumerator EaseIn (GameObject obj, Vector3 target, float maxSpeed, bool destroyOnArrival, bool invokeClearToContinue) 
	{
		Vector3 startPos = obj.transform.position;
		float speed = maxSpeed;

		while (Vector3.Distance(obj.transform.position, target) > 0.01f) 
		{
			speed = Vector3.Distance(obj.transform.position, target) * 2.5f;

			if (speed > maxSpeed)
			{
				speed = maxSpeed;
			}

			if (speed < 0.2f)
			{
				speed = 0.2f;
			}

			obj.transform.position = Vector3.MoveTowards (obj.transform.position, target, Time.deltaTime * speed);

			yield return new WaitForFixedUpdate ();
		}

		obj.transform.position = target;

		if (destroyOnArrival == true) 
		{
			GameObject.Destroy (obj);
		}

		if (invokeClearToContinue == true) 
		{
			EventsHandler.Invoke_Callback (EventsHandler.cb_isClearToContinue);
		}


	}


	public static IEnumerator MoveConstantSpeed (GameObject obj, Vector3 target, float maxSpeed, bool destroyOnArrival, bool invokeClearToContinue) 
	{
		Vector3 startPos = obj.transform.position;
		float speed = maxSpeed;

		while (Vector3.Distance(obj.transform.position, target) > 0.01f) 
		{
			obj.transform.position = Vector3.MoveTowards (obj.transform.position, target, Time.deltaTime * speed);

			yield return new WaitForFixedUpdate ();
		}

		obj.transform.position = target;

		if (destroyOnArrival == true) 
		{
			GameObject.Destroy (obj);
		}

		if (invokeClearToContinue == true) 
		{
			EventsHandler.Invoke_Callback (EventsHandler.cb_isClearToContinue);
		}
	}

	public static IEnumerator EaseOut (GameObject obj, Vector3 target, float maxSpeed, bool destroyOnArrival, bool invokeClearToContinue) 
	{
		Vector3 startPos = obj.transform.position;
		float speed = 0;

		while (Vector3.Distance(obj.transform.position, target) > 0.01f) 
		{
			speed += Time.deltaTime * maxSpeed;

			if (speed > maxSpeed)
			{
				speed = maxSpeed;
			}

			obj.transform.position = Vector3.MoveTowards (obj.transform.position, target, Time.deltaTime * speed);

			yield return new WaitForFixedUpdate ();
		}

		obj.transform.position = target;

		if (destroyOnArrival == true) 
		{
			GameObject.Destroy (obj);
		}

		if (invokeClearToContinue == true) 
		{
			EventsHandler.Invoke_Callback (EventsHandler.cb_isClearToContinue);
		}
	}



}

