using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;



[Serializable]
public class Interaction : IConditionable, ISubinteractable {


	public string myVerb;
	public List<SubInteraction> subInteractionList;
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


	public List<SubInteraction> SubIntList 
	{
		get
		{ 
			return subInteractionList;
		}

		set 
		{
			subInteractionList = value;
		}
	}




	//  Constructor

	public Interaction ()
	{
		
		subInteractionList = new List<SubInteraction>();
		conditionList = new List<Condition>();

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



	public void RemoveSubinteractionFromList(SubInteraction subInt)
	{

		if (subInt == null) 
		{

			Debug.LogError ("subInt is null");
			return;
		}


		if (subInteractionList.Contains (subInt) == false) 
		{
			Debug.LogError ("subInt is not in list");
			return;
		}

		subInteractionList.Remove (subInt);

	}




}






// Interface

public interface IConditionable
{	

	List<Condition> ConditionList { get; set; }


	// Empty Functions
	void RemoveConditionFromList (Condition condition);


}



