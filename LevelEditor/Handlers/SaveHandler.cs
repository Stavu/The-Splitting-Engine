﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SaveHandler : MonoBehaviour 
{

	public void SaveToFile()
	{
		EditorRoomManager.instance.SerializeRoom ();	
	}

}
