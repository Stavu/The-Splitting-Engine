using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;


public class SoundManager : MonoBehaviour {



	// Singleton //

	public static SoundManager instance { get; protected set; }

	void Awake ()
	{		
		if (instance == null) 
		{
			instance = this;

		} else if (instance != this) 
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		LoadAllSounds ();
		RegisterSounds ();
	}
		
	// Singleton //


	public static bool soundIsOff = false;
	public static bool musicIsOff = false;

	Dictionary<string,AudioClip> stringAudioClipMap;
	Dictionary<string,AudioSource> stringAudioSourceLoopMap;
	Dictionary<string,AudioClip> stringAudioClipMusicMap;

	AudioSource[] audioSources = new AudioSource[10];
	int nextAudioSource = 0;

	AudioSource[] audioSources_loop = new AudioSource[5];
	int nextAudioSource_loop = 0;

	AudioSource[] audioSources_music = new AudioSource[2];
	int currentMusicChannel = 0;


	void OnDestroy()
	{
		cb_setMusic -= SetMusic;
		cb_setSound -= SetSound;

		cb_playSound -= PlaySound;
		cb_stopSound -= StopSound;
		cb_leftRoom_start -= StopLoopSounds;
		cb_enteredRoom_start -= ActivateLoopSounds;

		EventsHandler.cb_roomCreated -= SwitchMusic;
		EventsHandler.cb_shadowStateChanged -= SwitchShadowStateMusic;
	}

	
	// Update is called once per frame

	void Update () 
	{
		
	}

	public void RegisterSounds()
	{		
		cb_setMusic += SetMusic;
		cb_setSound += SetSound;

		cb_playSound += PlaySound;
		cb_stopSound += StopSound;
		cb_leftRoom_start += StopLoopSounds;
		cb_enteredRoom_start += ActivateLoopSounds;

		EventsHandler.cb_roomCreated += SwitchMusic;
		EventsHandler.cb_shadowStateChanged += SwitchShadowStateMusic;
	}


	// Loading Sounds //

	public void LoadAllSounds()
	{
		for (int i = 0; i < audioSources.Length; i++) 
		{
			audioSources [i] = gameObject.AddComponent<AudioSource> ();			
		}	

		for (int i = 0; i < audioSources_loop.Length; i++) 
		{
			audioSources_loop [i] = gameObject.AddComponent<AudioSource> ();			
		}

		for (int i = 0; i < audioSources_music.Length; i++) 
		{			
			audioSources_music [i] = gameObject.AddComponent<AudioSource> ();	
			audioSources_music [i].loop = true;
		}

		AudioClip[] clipList_sound = Resources.LoadAll<AudioClip> ("Audio/SoundEffects");
		AudioClip[] clipList_music = Resources.LoadAll<AudioClip> ("Audio/Music");

		stringAudioClipMap = new Dictionary<string, AudioClip> ();
		stringAudioSourceLoopMap = new Dictionary<string, AudioSource> ();
		stringAudioClipMusicMap = new Dictionary<string, AudioClip> ();


		// Populate clipList

		foreach (AudioClip clip in clipList_sound) 
		{
			stringAudioClipMap.Add (clip.name, clip);
		}

		foreach (AudioClip clip in clipList_music) 
		{
			stringAudioClipMusicMap.Add (clip.name, clip);
		}
	}





	// ------ SET SOUND AND MUSIC ------ //


	// Set music on/off in settings

	public void SetMusic(bool musicOff)
	{
		Debug.Log ("SetMusic");

		foreach (AudioSource audioSource in audioSources_music) 
		{
			audioSource.mute = musicOff;
		}

		musicIsOff = musicOff;
	}


	// Set sound on/off in settings

	public void SetSound(bool soundOff)
	{
		Debug.Log ("SetSound");

		foreach (AudioSource audioSource in audioSources) 
		{
			audioSource.mute = soundOff;
		}

		foreach (AudioSource audioSource in audioSources_loop) 
		{
			audioSource.mute = soundOff;
		}

		soundIsOff = soundOff;
	}


	// ------ MUSIC ------ // 


	// SWITCH BETWEEN ROOMS MUSIC

	public void SwitchMusic(Room nextRoom)
	{	

		if (audioSources_music [currentMusicChannel].clip == null) 
		{
			if (stringAudioClipMusicMap.ContainsKey(nextRoom.myMusic))
			{
				audioSources_music [currentMusicChannel].clip = stringAudioClipMusicMap [nextRoom.myMusic];
				audioSources_music [currentMusicChannel].Play ();
			}

			return;
		}		

		if (nextRoom.roomState == RoomState.Mirror) 
		{
			if (nextRoom.myMirrorRoom.inTheShadow == true) 
			{
				// SHADOW ROOM

				if (audioSources_music [currentMusicChannel].clip.name == nextRoom.myMirrorRoom.myShadowMusic) 
				{
					Debug.Log ("same music");
					return;
				}

			} else {

				// MIRROR ROOM

				if (audioSources_music [currentMusicChannel].clip.name == nextRoom.myMusic) 
				{
					Debug.Log ("same music");
					return;
				}
			}

		} else 
		{
			// REAL ROOM

			if (audioSources_music [currentMusicChannel].clip.name == nextRoom.myMusic) 
			{
				Debug.Log ("same music");
				return;
			}
		}

		string clipName = nextRoom.myMusic;
		float fadeSpeed = NavigationManager.fadeSpeed;

		// if next room is in shadow state

		if (nextRoom.roomState == RoomState.Mirror) 
		{
			if (nextRoom.myMirrorRoom.inTheShadow == true) 
			{				
				clipName = nextRoom.myMirrorRoom.myShadowMusic;
			}
		}

		if (stringAudioClipMusicMap.ContainsKey (clipName) == false) 
		{
			Debug.LogError ("couldn't find clip name");
			return;
		}

		StartCoroutine (CrossFadeMusic (clipName,fadeSpeed));
	}



	// SWITCH SHADOW STATE MUSIC

	public void SwitchShadowStateMusic(bool intoShadows)
	{	
		Room myRoom = RoomManager.instance.myRoom;

		string clipName;
		float fadeSpeed = NavigationManager.fadeSpeed * 2;

		if (myRoom.roomState == RoomState.Real) 
		{
			Debug.Log ("the room is real.");
			return;
		}

		if (myRoom.myMirrorRoom == null) 
		{
			Debug.Log ("mirror room is null.");
			return;
		}

		if (myRoom.myMusic == myRoom.myMirrorRoom.myShadowMusic) 
		{
			Debug.Log ("same music");
			return;
		}

		if (intoShadows == true) 
		{
			clipName = myRoom.myMirrorRoom.myShadowMusic;

		} else {		

			clipName = myRoom.myMusic;
		}

		if (stringAudioClipMusicMap.ContainsKey (clipName) == false) 
		{
			Debug.LogError ("couldn't find clip name");
			return;
		}

		StartCoroutine (CrossFadeMusic (clipName,fadeSpeed));
	}



	// -- CROSS FADE MUSIC -- //

	IEnumerator CrossFadeMusic(string nextClip, float fadeSpeed)
	{
		int nextMusicChannel = currentMusicChannel == 0 ? 1 : 0;
		audioSources_music [nextMusicChannel].clip = stringAudioClipMusicMap [nextClip];
		audioSources_music [nextMusicChannel].Play ();

		float i = 0;

		while (i < 1) 
		{	
			audioSources_music[currentMusicChannel].volume = 1-i;
			audioSources_music [nextMusicChannel].volume = i;

			i += Time.deltaTime / fadeSpeed;

			yield return new WaitForFixedUpdate ();
		}

		audioSources_music[currentMusicChannel].volume = 0;
		audioSources_music [currentMusicChannel].Stop ();

		audioSources_music [nextMusicChannel].volume = 1;

		currentMusicChannel = nextMusicChannel;
	}



	// ------ SOUND ------ //

	public void PlaySound(string soundName, int numberOfPlays)
	{
		Debug.Log ("play sound " + soundName);

		if (numberOfPlays == 0) 
		{
			audioSources_loop[nextAudioSource_loop].clip = stringAudioClipMap [soundName];
			StartCoroutine(PlaySoundList (audioSources_loop [nextAudioSource_loop], numberOfPlays));

			nextAudioSource_loop ++;

		} else {

			audioSources[nextAudioSource].clip = stringAudioClipMap [soundName];
			StartCoroutine(PlaySoundList (audioSources [nextAudioSource], numberOfPlays));

			nextAudioSource ++;
		}
	}


	public void StopSound(string soundName)
	{
		Debug.Log ("stop sound " + soundName);

		if (stringAudioSourceLoopMap.ContainsKey (soundName)) 
		{
			stringAudioSourceLoopMap [soundName].Stop ();
			stringAudioSourceLoopMap.Remove (soundName);
		}
	}


	// play sound a unmber of times. If the number is 0, play sound in loop.

	IEnumerator PlaySoundList(AudioSource audioSource, int n)
	{
		if (n == 0) 
		{		
			// PLAY IN LOOP

			Debug.Log ("n = 0");

			audioSource.loop = true;
			audioSource.Play ();

			stringAudioSourceLoopMap.Add (audioSource.clip.name, audioSource);

			yield return null;
		
		} else {

			Debug.Log ("n != 0");

			int i = 0;

			while (i < n) 
			{
				audioSource.Play ();
				i++;

				yield return new WaitForSeconds (audioSource.clip.length);
			}
		}
	}


	// When leaving a room, stopping loop sounds

	public void StopLoopSounds()
	{		
		StartCoroutine(FadeOutLoopSounds());
	}


	// ---- FADE IN & OUT ---- //

	IEnumerator FadeOutLoopSounds()
	{
		float i = 1;

		while (i > 0) 
		{	
			foreach (AudioSource audioSource in audioSources_loop) 
			{
				audioSource.volume = i;				
			}

			i -= Time.deltaTime / NavigationManager.fadeSpeed;

			yield return new WaitForFixedUpdate ();
		}

		foreach (AudioSource audioSource in audioSources_loop) 
		{
			audioSource.Stop();
		}

		stringAudioSourceLoopMap.Clear();
	}


	public void ActivateLoopSounds()
	{
		StartCoroutine(FadeInLoopSounds());
	}


	IEnumerator FadeInLoopSounds()
	{
		float i = 0;

		while (i < 1) 
		{	
			foreach (AudioSource audioSource in audioSources_loop) 
			{
				audioSource.volume = i;				
			}

			i += Time.deltaTime / NavigationManager.fadeSpeed;

			yield return new WaitForFixedUpdate ();
		}

		foreach (AudioSource audioSource in audioSources_loop) 
		{
			audioSource.volume = 1;	
		}
	}





	// ------------- STATIC EVENTS ------------- //



	public static Action<string,int> cb_playSound; 

	public static void Invoke_cb_playSound(string soundName, int numberOfPlays)
	{
		if(cb_playSound != null)
		{
			cb_playSound (soundName,numberOfPlays);
		}
	}


	public static Action<string> cb_stopSound; 

	public static void Invoke_cb_stopSound(string soundName)
	{
		if(cb_stopSound != null)
		{
			cb_stopSound (soundName);
		}
	}


	public static Action cb_leftRoom_start;

	public static void Invoke_cb_leftRoom_start()
	{
		if(cb_leftRoom_start != null)
		{
			cb_leftRoom_start ();
		}
	}


	public static Action cb_enteredRoom_start;

	public static void Invoke_cb_enteredRoom_start()
	{
		if(cb_enteredRoom_start != null)
		{
			cb_enteredRoom_start ();
		}
	}


	public static Action<bool> cb_setSound;

	public static void Invoke_cb_setSound(bool soundOff)
	{
		if(cb_setSound != null)
		{
			cb_setSound (soundOff);
		}
	}


	public static Action<bool> cb_setMusic;

	public static void Invoke_cb_setMusic(bool musicOff)
	{
		if(cb_setMusic != null)
		{
			cb_setMusic (musicOff);
		}
	}



}






