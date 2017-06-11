using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTextObject : MonoBehaviour {


	public List<string> textList;
	int currentEntry;
	Text inventoryText;


	// Use this for initialization

	public void AddTextList (List<string> list) 
	{
		
	 	inventoryText = gameObject.transform.Find ("TextCanvas").Find ("Text").GetComponent<Text> ();

		this.textList = list;
		inventoryText.text = textList [0];

	}



	// Update is called once per frame

	void Update () 
	{
		if (Input.anyKeyDown) 
		{
			NextEntry ();
		}	

	}


	// Show next entry, and if you ran out of entries, destroy

	public void NextEntry()
	{
		currentEntry++;

		if (currentEntry < textList.Count) 
		{
			inventoryText.text = textList [currentEntry];
		
		} else {

			Destroy (gameObject);  
		}
	}





}
