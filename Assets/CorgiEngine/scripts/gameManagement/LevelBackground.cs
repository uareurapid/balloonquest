﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]

/// <summary>
/// Add this class to a background image so it will act as your level's background
/// </summary>
public class LevelBackground : MonoBehaviour 
{
	/// If true, the background will stick to the camera
	public bool FollowCamera=true;
	/// If true, the background will stick to the camera even in edit mode
	public bool FollowCameraEditMode=true;
		
	private CameraController _cameraController ;
	
	/// <summary>
	/// On enable, we get the main camera
	/// </summary>
	void OnEnable () 
	{		
		// we get the camera				
		_cameraController = FindObjectOfType<CameraController>();			
	}
	
	/// <summary>
	/// Every update, we make the level follow the camera's position
	/// </summary>
	void Update()
	{
		if (_cameraController==null)
			return;
			
		if (FollowCamera)
		{
			// if we're in editor mode and if the background is not supposed to follow the camera in edit mode, we exit
			if ( (!FollowCameraEditMode) && (Application.isEditor) )
				return;
				
			// we set the actual transform's position
			transform.position=new Vector3(_cameraController.transform.position.x,_cameraController.transform.position.y,transform.position.z);
		}
	}
}
