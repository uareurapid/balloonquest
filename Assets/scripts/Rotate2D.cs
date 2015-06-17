using UnityEngine;
using System.Collections;


public static class Rotation2D {
	
	private static Quaternion rotation = new Quaternion();
	
	/// <summary>
	/// Smoothly looks at target in 2D. Return Quaternion to be applied to transform.rotation
	/// </summary>
	/// <returns>
	/// The <see cref=”Quaternion”/>.
	/// </returns>
	/// <param name=’transform’>
	/// Transform of the rotating object
	/// </param>
	/// <param name=’target’>
	/// Target being looked at
	/// </param>
	/// <param name=’axis’>
	/// Global axis of rotation, must be single axis
	/// </param>
	/// <param name=’damping’>
	/// Rotation speed.
	/// </param>
	public static Quaternion SmoothLookAt(Transform transform, Vector3 target, Vector3 axis, float speed){
		if(axis == Vector3.up || axis == -Vector3.up){
			target.y = transform.position.y;
			if(target.y >= transform.position.y) {
				rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
			}
			else {
				rotation = Quaternion.LookRotation(target - transform.position, -Vector3.up);
			}
		}else if(axis == Vector3.right || axis == -Vector3.right){
			target.x = transform.position.x;
			if(target.x >= transform.position.x)
				rotation = Quaternion.LookRotation(target - transform.position, Vector3.right);
			else
				rotation = Quaternion.LookRotation(target - transform.position, -Vector3.right);
		}else if(axis == Vector3.forward || axis == -Vector3.forward){
			target.z = transform.position.z;
			if(target.z >= transform.position.z)
				rotation = Quaternion.LookRotation(target - transform.position, Vector3.forward);
			else
				rotation = Quaternion.LookRotation(target - transform.position, -Vector3.forward);
		}
		return Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
	}
	
	/// <summary>
	/// Looks at target in 2D
	/// </summary>
	/// <param name=’transform’>
	/// Transform of the rotating object
	/// </param>
	/// <param name=’target’>
	/// Target being looked at
	/// </param>
	/// <param name=’axis’>
	/// Global axis of rotation, a single axis
	/// </param>
	public static void LookAt(Transform transform, Vector3 target, Vector3 axis){
		if(axis == Vector3.up || axis == -Vector3.up){
			target.y = transform.position.y;
		}else if(axis == Vector3.right || axis == -Vector3.right){
			target.x = transform.position.x;
		}else if(axis == Vector3.forward || axis == -Vector3.forward){
			target.z = transform.position.z;
		}
		transform.LookAt(target, axis);
	}
}
