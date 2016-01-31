using UnityEngine;
using System.Collections;

public class HomingMissile : Puffy_ParticleSpawner {
	
	public static bool globalFreeze = false;
	
	public Transform target;
	public float speed = 1f;
	
	public float angularThreshold = 0.05f;
	public float angularThresholdRandomOffset = 0.01f;
	
	private float _angularThreshold = 0.05f;
	
	public float craziness = 0.2f;
	public float crazinessRandomOffset = 0.1f;
	private float _craziness = 0.1f;
	
	public float crazinessFrequency = 0.05f;
	
	private float crazynessFreqElapsed = 0f;
	
	public float rearOffset = 0.2f;
	public float straightLaunchTime = 2f;
	public int lifeTime = 5;
	
	[HideInInspector]
	public Vector3 moveVector = Vector3.up;
			
	private Vector3 launchDirection;
	
	private float targetSize = 1f;
	
	[HideInInspector]
	public float age = 0f;
	
	public void Spawn(Vector3 startPosition, Vector3 startDirection, Transform targetObject, float missileSpeed = 10f){
		if(_transform == null) _transform = transform;
		
		_transform.position = startPosition;
		moveVector = startDirection;
		launchDirection = startDirection;
		
		target = targetObject;
		
		targetSize = 1f;
		
		// guess target size automatically
		if(target){
			if(target.GetComponent<Renderer>()){
				targetSize = target.GetComponent<Renderer>().bounds.size.magnitude * 0.5f;
			}
		}
				
		speed = missileSpeed;
		GetComponent<Renderer>().enabled = true;
		enabled = true;
		age = 0f;
				
		_angularThreshold = angularThreshold + Random.Range(0,angularThresholdRandomOffset);
		
		_craziness = craziness + Random.Range(0,crazinessRandomOffset);
		crazynessFreqElapsed = 0f;
		
		_transform.forward = startDirection;
		
		// Puffy Smoke important line :
		// init particles data
		InitSpawnPoint(startPosition, startDirection * -1);
	}
	
	void Update () {
	
		if(!globalFreeze){
			
			float delta = Time.deltaTime;
			
			Vector3 direction;
			float distance = 1000;
			
			if(target == null){
				// no target defined, the missile go straight forward
				direction.x = launchDirection.x;	
				direction.y = launchDirection.y;
				direction.z = launchDirection.z;
			}else{
				if(_angularThreshold == 0){
					direction.x = launchDirection.x;	
					direction.y = launchDirection.y;
					direction.z = launchDirection.z;
				}else{
					direction = target.position - _transform.position;
				}
				distance = direction.sqrMagnitude;
			}
			
			crazynessFreqElapsed += delta;
			age += delta;
			
			// launch the missile straight forward, and gradually orient it to its target
			if(age < straightLaunchTime){
				direction = direction * age/straightLaunchTime + launchDirection * (1f-age/straightLaunchTime);
			}
			
			// define the movement vector toward the target
			moveVector = (moveVector + direction * _angularThreshold * delta).normalized;
			
			// randomize trajectory
			if(_craziness!=0 && crazynessFreqElapsed >= crazinessFrequency){
				moveVector += Random.insideUnitSphere * _craziness;
				crazynessFreqElapsed = 0f;
			}
			
			// align the missile to its path
			_transform.forward = moveVector.normalized;
			
			// move the missile
			if(age > 0) _transform.position += _transform.forward * speed * delta;
			
			// kill the missile if it's too old or have reached the target
			if(age > lifeTime || distance < targetSize) Kill();
			
		
			// Puffy Smoke important line :
			// update particles emission data
			UpdateSpawnPoint(_transform.position - _transform.forward * rearOffset , _transform.forward * -1);
			
		}
	}
	
	public void Kill(){
		enabled = false;
		GetComponent<Renderer>().enabled = false;
	}
	
	public bool ReadyForRecycle(){
		return age >= lifeTime;
	}
}
