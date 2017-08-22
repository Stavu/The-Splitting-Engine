using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public enum InventoryState
{
	Browse,
	UseItem,
	Combine,
	Closed
}


[Serializable]

public class Inventory {

	public List<InventoryItem> items;

	[NonSerialized]
	public InventoryState myState = InventoryState.Closed;


	public Inventory ()
	{
		// fresh inventory

		items = new List<InventoryItem> ();	
	}


	// Adding Item

	public void AddItem(InventoryItem item)
	{
		// if there's already an item with the same name, return

		foreach (InventoryItem inventoryItem in items) 
		{
			if (inventoryItem.fileName == item.fileName) 
			{
				Debug.LogError ("There's already an item with the same name");
				return;
			}			
		}

		items.Add (item);
		item.Initialize ();

		EventsHandler.Invoke_cb_inventoryChanged (this);
		EventsHandler.Invoke_cb_itemAddedToInventory (item);

		GameManager.instance.SaveData ();
	}


	// Removing Item

	public void RemoveItem(InventoryItem item)
	{
		if (items.Contains (item)) 
		{
			items.Remove (item);
		}

		EventsHandler.Invoke_cb_inventoryChanged (this);

		GameManager.instance.SaveData ();		
	}


	// Removing Item

	public void RemoveItem(string itemName)
	{
		for (int i = items.Count - 1; i >= 0; i--) 
		{			
			if (items[i].fileName == itemName) 
			{				
				items.RemoveAt (i);
			}
		}

		EventsHandler.Invoke_cb_inventoryChanged (this);

		GameManager.instance.SaveData ();		
	}


}
