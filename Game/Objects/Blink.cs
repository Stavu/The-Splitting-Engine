using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {

	SpriteRenderer mySpriteRenderer;
	float speed = 1.5f;

	// Use this for initialization

	void Start () 
	{
		mySpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		StartCoroutine (PlayBlink ());
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}


	public IEnumerator PlayBlink()
	{
		
		float a = 0.5f;
		mySpriteRenderer.color = new Color (1f, 1f, 1f, a);

		while(a < 1f)
		{
			a += Time.deltaTime * speed;
			mySpriteRenderer.color = new Color (1f, 1f, 1f, a);
			yield return new WaitForFixedUpdate ();
		}

		a = 1f;
		mySpriteRenderer.color = Color.white;

		while(a > 0.5f)
		{
			a -= Time.deltaTime * speed;
			mySpriteRenderer.color = new Color (1f, 1f, 1f, a);
			yield return new WaitForFixedUpdate ();

		}

		a = 0.5f;
		mySpriteRenderer.color = new Color (1f, 1f, 1f, a);

		Destroy (this);

	}

}
