using UnityEngine;
using System.Collections;
using System.Timers;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class FreeMove : MonoBehaviour {
	
	
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public GameObject followMe;
	
	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	
	public float speed = 15;
	private double realDeltaTime = 0;
	
//	private Vector3 lookAt = Vector3.zero;
	
	
	void Start(){
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
		realDeltaTime = Time.realtimeSinceStartup;
	}
	
	
	void Update ()
	{
		realDeltaTime = Time.realtimeSinceStartup - realDeltaTime;
		
		if(!Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift) ){
			if (axes == RotationAxes.MouseXAndY)
			{
				//float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				//transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
				
				transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y")* sensitivityY,Input.GetAxis("Mouse X")* sensitivityX,0));
				
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}
		
		Vector3 directionVector = transform.forward * ((Input.GetKey(KeyCode.DownArrow)?0:1) - (Input.GetKey(KeyCode.UpArrow)?0:1)) + transform.right * ((Input.GetKey(KeyCode.LeftArrow)?0:1) - (Input.GetKey(KeyCode.RightArrow)?0:1)) ;
	
		if (directionVector != Vector3.zero) {
				
			
			directionVector *= (float)realDeltaTime * speed;
			
			if(Input.GetKey(KeyCode.RightControl)) directionVector *= 0.1f;
			
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			var directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		
		transform.position += directionVector;
		
		realDeltaTime = Time.realtimeSinceStartup;
	}
	
	
}