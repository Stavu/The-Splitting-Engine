using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextObject : MonoBehaviour {


	public List<DialogueSentence> sentenceList;
	int currentEntry;
	Text textComponent;

	public bool isImportant = true;


	// Use this for initialization

	public void AddTextList (List<DialogueSentence> list) 
	{
		textComponent = gameObject.transform.Find ("Image").Find ("Text").GetComponent<Text> ();

		this.sentenceList = list;
	
		PopulateTextBox (sentenceList [0]);

		//currentTextBox.GetComponent<RectTransform> ().anchoredPosition = PositionTextBox (speaker);
	}


	// Update is called once per frame

	void Update () 
	{	
		if ((Input.GetKeyDown(KeyCode.Space) || ((isImportant == false) && (Input.anyKeyDown)))) 
		{
			if ((sentenceList [currentEntry].mySubIntList != null) && (sentenceList[currentEntry].subinteractImmediately == false))
			{

				// check if subinteractions passed the conditions

				List<SubInteraction> subinteractionsToDo = Utilities.GetPassedSubinteractions (sentenceList [currentEntry].mySubIntList);

				subinteractionsToDo.ForEach (subInt => subInt.SubInteract ());	
			}

			NextEntry ();
		}
	}


	// Show next entry, and if you ran out of entries, destroy

	public void NextEntry()
	{
		currentEntry++;

		if (currentEntry < sentenceList.Count) 
		{
			if ((sentenceList [currentEntry].mySubIntList != null) && (sentenceList[currentEntry].subinteractImmediately == true))
			{

				// check if subinteractions passed the conditions

				List<SubInteraction> subinteractionsToDo = Utilities.GetPassedSubinteractions (sentenceList [currentEntry].mySubIntList);

				subinteractionsToDo.ForEach (subInt => subInt.SubInteract ());	

			}

			PopulateTextBox (sentenceList [currentEntry]);

		} else {

			CloseTextBox ();

		}
	}



	public void CloseTextBox ()
	{		
		InteractionManager.instance.currentTextBox = null;

		if (DialogueManager.instance.dialogueTreeObject != null) 
		{
			DialogueManager.instance.SetDialogueTreeActive (true);		
			//GameManager.instance.inputState = InputState.DialogueBox;

		} else {

			EventsHandler.Invoke_Callback (EventsHandler.cb_dialogueEnded);
			//GameManager.instance.inputState = InputState.Character;	
		}

		GameManager.textBoxActive = false;
		EventsHandler.Invoke_cb_inputStateChanged ();

		Destroy (gameObject);
	
	}



	public Vector3 PositionTextBox(ISpeaker speaker)
	{
		int offsetX = 0;
		int offsetY = 5;

		Vector3 newPos = new Vector3 (speaker.speakerPos.x + offsetX, speaker.speakerPos.y + offsetY,0);

		//now you can set the position of the ui element
		return newPos;
	}



	public void PopulateTextBox(DialogueSentence sentence)
	{
		textComponent.text = sentence.myText;

		ISpeaker speaker = RoomManager.instance.nameSpeakerMap [sentence.speakerName];
		textComponent.color = speaker.speakerTextColor;
		transform.position = PositionTextBox (speaker);
	}



}
