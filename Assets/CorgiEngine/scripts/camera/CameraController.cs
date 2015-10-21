using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]
/// <summary>
/// The Corgi Engine's Camera Controller. Handles camera movement, shakes, player follow.
/// </summary>
public class CameraController : MonoBehaviour 
{
	
	/// True if the camera should follow the player
	public bool FollowsPlayer{get;set;}
	
	[Space(10)]	
	[Header("Distances")]
	/// How far ahead from the Player the camera is supposed to be		
	public float HorizontalLookDistance = 3;
	/// Vertical Camera Offset	
	public Vector3 CameraOffset ;
	/// Minimal distance that triggers look ahead
	public float LookAheadTrigger = 0.1f;
	/// How high (or low) from the Player the camera should move when looking up/down
	public float ManualUpDownLookDistance = 3;
	
	
	[Space(10)]	
	[Header("Movement Speed")]
	/// How fast the camera goes back to the Player
	public float ResetSpeed = 0.5f;
	/// How fast the camera moves
	public float CameraSpeed = 0.3f;
	
	[Space(10)]	
	[Header("Camera Zoom")]
	[Range (1, 20)]
	public float MinimumZoom=5f;
	[Range (1, 20)]
	public float MaximumZoom=10f;	
	public float ZoomSpeed=0.4f;
	
	// Private variables
	
	protected Transform _target;
	private CorgiController _targetController;
	protected LevelLimits _levelBounds;
	
	private float _xMin;
	private float _xMax;
	private float _yMin;
	private float _yMax;	 
	
	protected float _offsetZ;
	protected Vector3 _lastTargetPosition;
	private Vector3 _currentVelocity;
	private Vector3 _lookAheadPos;
		
	private float _shakeIntensity;
	private float _shakeDecay;
	private float _shakeDuration;
	
	protected float _currentZoom;	
	protected Camera _camera;
	
	private Vector3 _lookDirectionModifier = new Vector3(0,0,0);
	
	/// <summary>
	/// Initialization
	/// </summary>
	protected virtual void Start ()
	{		
		// we get the camera component
		_camera=GetComponent<Camera>();
	
		// We make the camera follow the player
		FollowsPlayer=true;
		_currentZoom=MinimumZoom;
		
		// player and level bounds initialization
		_target = GameManager.Instance.Player.transform;
		if (_target.GetComponent<CorgiController>()==null)
			return;
		_targetController=_target.GetComponent<CorgiController>();
		_levelBounds = GameObject.FindGameObjectWithTag("LevelBounds").GetComponent<LevelLimits>();		
		
		// we store the target's last position
		_lastTargetPosition = _target.position;
		_offsetZ = (transform.position - _target.position).z;
		transform.parent = null;
		
		//_lookDirectionModifier=new Vector3(0,0,0);
		
		Zoom();
	}
	
	
	/// <summary>
	/// Every frame, we move the camera if needed
	/// </summary>
	void LateUpdate () 
	{
		// if the camera is not supposed to follow the player, we do nothing
		if (!FollowsPlayer)
			return;
			
		Zoom();
			
		// if the player has moved since last update
		float xMoveDelta = (_target.position - _lastTargetPosition).x;
		
		bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > LookAheadTrigger;
		
		if (updateLookAheadTarget) 
		{
			_lookAheadPos = HorizontalLookDistance * Vector3.right * Mathf.Sign(xMoveDelta);
		} 
		else 
		{
			_lookAheadPos = Vector3.MoveTowards(_lookAheadPos, Vector3.zero, Time.deltaTime * ResetSpeed);	
		}
		
		Vector3 aheadTargetPos = _target.position + _lookAheadPos + Vector3.forward * _offsetZ + _lookDirectionModifier + CameraOffset;
				
		Vector3 newCameraPosition = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, CameraSpeed);
				
		Vector3 shakeFactorPosition = new Vector3(0,0,0);
		
		// If shakeDuration is still running.
		if (_shakeDuration>0)
		{
			shakeFactorPosition= Random.insideUnitSphere * _shakeIntensity * _shakeDuration;
			_shakeDuration-=_shakeDecay*Time.deltaTime ;
		}		
		newCameraPosition = newCameraPosition+shakeFactorPosition;		
		
		// Clamp to level boundaries
		float posX = Mathf.Clamp(newCameraPosition.x, _xMin, _xMax);
		float posY = Mathf.Clamp(newCameraPosition.y, _yMin, _yMax);
		float posZ = newCameraPosition.z;
		
		// We move the actual transform
		transform.position=new Vector3(posX, posY, posZ);
		
		_lastTargetPosition = _target.position;		
	}
	
	/// <summary>
	/// Handles the zoom of the camera based on the main character's speed
	/// </summary>
	protected void Zoom()
	{
	
		float characterSpeed=Mathf.Abs(_targetController.Speed.x);
		float currentVelocity=0f;
		
		_currentZoom=Mathf.SmoothDamp(_currentZoom,(characterSpeed/10)*(MaximumZoom-MinimumZoom)+MinimumZoom,ref currentVelocity,ZoomSpeed);
			
		_camera.orthographicSize=_currentZoom;
		GetLevelBounds();
	}
	
	/// <summary>
	/// Gets the levelbounds coordinates to lock the camera into the level
	/// </summary>
	private void GetLevelBounds()
	{
		// camera size calculation (orthographicSize is half the height of what the camera sees.
		float cameraHeight = Camera.main.orthographicSize * 2f;		
		float cameraWidth = cameraHeight * Camera.main.aspect;
		
		_xMin = _levelBounds.LeftLimit+(cameraWidth/2);
		_xMax = _levelBounds.RightLimit-(cameraWidth/2); 
		_yMin = _levelBounds.BottomLimit+(cameraHeight/2); 
		_yMax = _levelBounds.TopLimit-(cameraHeight/2);	
	}
	
	/// <summary>
	/// Use this method to shake the camera, passing in a Vector3 for intensity, duration and decay
	/// </summary>
	/// <param name="shakeParameters">Shake parameters : intensity, duration and decay.</param>
	public void Shake(Vector3 shakeParameters)
	{
		_shakeIntensity = shakeParameters.x;
		_shakeDuration=shakeParameters.y;
		_shakeDecay=shakeParameters.z;
	}

	/// <summary>
	/// Moves the camera up
	/// </summary>
	public void LookUp()
	{
		_lookDirectionModifier = new Vector3(0,ManualUpDownLookDistance,0);
	}	
	/// <summary>
	/// Moves the camera down
	/// </summary>
	public void LookDown()
	{
		_lookDirectionModifier = new Vector3(0,-ManualUpDownLookDistance,0);
	}
	/// <summary>
	/// Resets the look direction modifier
	/// </summary>
	public void ResetLookUpDown()
	{	
		_lookDirectionModifier = new Vector3(0,0,0);
	}
	
	
}