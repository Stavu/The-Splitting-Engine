using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorLoadRoom : MonoBehaviour {


	public GameObject roomButtonPrefab;

	Button loadButton;
	GameObject scrollViewObject;
	GameObject contentObject;




	// Use this for initialization

	void Start () 
	{
		loadButton = gameObject.transform.Find ("Button").GetComponent<Button> ();
		loadButton.onClick.AddListener (CreateButtons);

		scrollViewObject = gameObject.transform.Find ("ScrollView").gameObject;
		contentObject = scrollViewObject.transform.Find ("Viewport").Find ("Content").gameObject;
		
	}
	
	// Update is called once per frame

	void Update () 
	{
		
	}


	// Create buttone

	public void CreateButtons()
	{
		

		for (int i = 0; i < contentObject.transform.childCount; i++) 
		{
			Destroy (contentObject.transform.GetChild(i).gameObject);
		}


		Object[] myTextAssets = Resources.LoadAll ("Jsons/Rooms");

		foreach (TextAsset txt in myTextAssets) 
		{

			Room room = JsonUtility.FromJson<Room> (txt.text);

			Button roomButton = Instantiate(roomButtonPrefab).GetComponent<Button>();
			roomButton.transform.SetParent (contentObject.transform);

			roomButton.GetComponentInChildren<Text> ().text = room.myName;		
			roomButton.onClick.AddListener (() => LoadRoomClicked(txt.text));

		}
	}






	// Load room

	public void LoadRoomClicked(string roomString)
	{

		EditorRoomManager.roomToLoad = roomString;

		EditorRoomManager.loadRoomFromMemory = true;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);


	}




}
