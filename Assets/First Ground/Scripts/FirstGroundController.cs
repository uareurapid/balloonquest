// First Ground version: 1.3.1
// Author: Gold Experience Team (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

#endregion

/***************
* FGArcticController class.
* 
* 	This class handles
* 		- Switch Camera and Player control
* 		- Init Render Setting
* 		- Display GUIs
* 
***************/

public class FirstGroundController : MonoBehaviour
{

#region Variables
	
	public Material m_SkyBoxMaterial = null;
	
	public GameObject m_FirstPerson = null;
	public GameObject m_OrbitCamera = null;
	
#endregion {Variables}
	
// ######################################################################
// MonoBehaviour Functions
// ######################################################################

#region Component Segments

	// Use this for initialization
	void Start ()
	{
		InitCamera();
		InitRenderSetting();
		
		// Update How to text
		UpdateHowToText();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyUp(KeyCode.E))
		{
			SwitchCamera();
		}
	}
	
	void OnTriggerExit(Collider other)
	{		
		// Reset player position when user move it away from terrain
		this.transform.localPosition = new Vector3(0,1,0);
    }
	
#endregion Component Segments
	
// ######################################################################
// Functions Functions
// ######################################################################

#region Functions
	
	void InitCamera()
	{
		m_FirstPerson.SetActive(false);
		m_OrbitCamera.SetActive(true);
	}
	
	void InitRenderSetting()
	{
		RenderSettings.skybox = m_SkyBoxMaterial;
	}

	void SwitchCamera()
	{
		m_FirstPerson.SetActive(!m_FirstPerson.activeSelf);
		m_OrbitCamera.SetActive(!m_OrbitCamera.activeSelf);
		
		if(m_OrbitCamera.activeSelf==true)
		{
			FirstGroundOrbitCamera pFirstGroundOrbitCamera = (FirstGroundOrbitCamera) Object.FindObjectOfType(typeof(FirstGroundOrbitCamera));
			pFirstGroundOrbitCamera.TargetLookAt.transform.localPosition = new Vector3(0,0,0);
		}

		UpdateHowToText();
	}
	
	// Update How to text
	void UpdateHowToText()
	{ 
		if(m_OrbitCamera.activeSelf==true)
		{
			// Update How to text
			GameObject Text_HowTo = GameObject.Find("Text_HowTo");
			if(Text_HowTo!=null)
			{ 				
				Text pText = Text_HowTo.GetComponent<Text>();
				pText.text = "Mouse Drags = Orbit | Mouse Wheel = Zoom | E = Switch to First Person";
			}
		}
		else
		{ 
			
			// Update How to text
			GameObject Text_HowTo = GameObject.Find("Text_HowTo");
			if(Text_HowTo!=null)
			{ 				
				Text pText = Text_HowTo.GetComponent<Text>();
				pText.text = "AWSD = Move | Spacebar = Jump | Mouse = Turn | E = Switch to Orbit";
			}
		}
	}

#endregion Functions
	
}
