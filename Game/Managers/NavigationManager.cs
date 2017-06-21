using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;




public enum FadeState
{
	FadeIn,
	FadeOut,
	Inactive
}


public class NavigationManager : MonoBehaviour {


	// Singleton //

	public static NavigationManager instance { get; protected set; }

	void Awake ()
	{		
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);	
		}

		DontDestroyOnLoad (gameObject);
	}

	// Singleton //


	GameObject fadeCanvas;
	Image fadeImage;

	public static float fadeSpeed = 0.5f;
	static Color lastColor;

	public static bool navigationInProcess;


	// Use this for initialization

	void Start () 
	{	
		fadeCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/FadeCanvas"));
		fadeImage = fadeCanvas.transform.Find ("Image").GetComponent<Image> ();

		fadeImage.material.SetFloat ("_Fade", 0);

		SceneManager.sceneLoaded += StartFadeIn;
	}


	void OnDestroy()
	{
		SceneManager.sceneLoaded -= StartFadeIn;
	}


	public void NavigateToScene(string scene, Color color)
	{
		navigationInProcess = true;
		EventsHandler.Invoke_cb_inputStateChanged ();

		lastColor = color;
		StartCoroutine(FadeOut (scene, color));	
	}


	// Fading from scene to black / white screen

	// -- FADE OUT -- //

	IEnumerator FadeOut(string scene, Color color)
	{
		SoundManager.Invoke_cb_leftRoom_start ();

		fadeImage.color = color;
		float i = 0;

		while (i < 1) 
		{
			fadeImage.material.SetFloat ("_Fade", i);
			i += Time.deltaTime / fadeSpeed;

			yield return new WaitForFixedUpdate();					
		}

		fadeImage.material.SetFloat ("_Fade", 1);

		// While screen is black, reload the scene

		navigationInProcess = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

		SceneManager.LoadScene (scene);
	}



	// -- FADE IN -- //

	IEnumerator FadeIn()
	{
		SoundManager.Invoke_cb_enteredRoom_start ();

		fadeImage.color = lastColor;
		float i = 1;

		while (i > 0) 
		{
			fadeImage.material.SetFloat ("_Fade", i);
			i -= Time.deltaTime / fadeSpeed;

			yield return new WaitForFixedUpdate();
		}

		fadeImage.material.SetFloat ("_Fade", 0);

		EventsHandler.Invoke_cb_entered_room (RoomManager.instance.myRoom);
	}


	public void StartFadeIn(Scene scene, LoadSceneMode mode)
	{
		fadeCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/FadeCanvas"));
		fadeImage = fadeCanvas.transform.Find ("Image").GetComponent<Image> ();
		StartCoroutine(FadeIn());	
	}

}

