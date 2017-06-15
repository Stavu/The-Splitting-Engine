using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class DialogueTree {


	public string myName;
	public List<Conversation> conversationList;

	[NonSerialized]
	public Conversation currentConversation;




	// Find conversation by name

	public Conversation GetConversationByName (string conversationName)
	{

		foreach (Conversation conversation in conversationList) 
		{
			if (conversation.myName == conversationName) 
			{
				return conversation;
			}			
		}

		Debug.LogError ("Can't find conversation");
		return null;

	}

}



// Conversation - What the conversation is about


[Serializable]
public class Conversation {


	public string myName;
	public List<DialogueOption> optionList;


}


// Dialogue option

[Serializable]
public class DialogueOption: IConditionable {
	
	public string myTitle;

	public List<DialogueSentence> sentenceList;

	public List<Condition> conditionList;
	public List<Condition> ConditionList 
	{
		get
		{ 
			return conditionList;
		}

		set 
		{
			conditionList = value;
		}
	}

	public void RemoveConditionFromList(Condition condition)
	{
		if (condition == null) 
		{

			Debug.LogError ("condition is null");
			return;
		}

		if (conditionList.Contains (condition) == false) 
		{
			Debug.LogError ("condition is not in list");
			return;
		}

		conditionList.Remove (condition);
	}
}





// Dialogue Sentence

[Serializable]
public class DialogueSentence : ISubinteractable {
	
	public string speakerName;
	public string myText;
	public bool subinteractImmediately = false;

	public List<SubInteraction> mySubIntList;
	public List<SubInteraction> SubIntList 
	{
		get
		{ 
			return mySubIntList;
		}

		set 
		{
			mySubIntList = value;
		}
	}



	public DialogueSentence(string speaker, string text, bool subIntIm, List<SubInteraction> subIntList = null)
	{

		speakerName = speaker;
		myText = text;
		mySubIntList = subIntList;
		subinteractImmediately = subIntIm;

	}

}