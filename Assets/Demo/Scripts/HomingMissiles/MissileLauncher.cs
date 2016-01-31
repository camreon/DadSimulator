using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileLauncher : Puffy_MultiSpawner {
	
	static List<HomingMissile> missiles = new List<HomingMissile>();
	
	public GameObject MissilePrefab;
	public Transform target;

	public int launchCount = 1;
	public float missileAccuracy = 0.5f;
	public float missileCrazyness = 0.2f;
	
	void Update () {
		
		HomingMissile.globalFreeze = Puffy_Emitter.globalFreeze;
		
		if(Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height - 70){
			HomingMissile missileScript = null;
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 5));
			
			for(int m =0; m < launchCount;m++){
				missileScript = null;
				
				// try to find a missile to recycle
				for(int i = 0; i<missiles.Count; i++){
					if(missiles[i].ReadyForRecycle()){
						missileScript = missiles[i];
						break;
					}
				}
				
				if(launchCount > 1) pos += Random.onUnitSphere;
				
				// no missile has been found, create a new one
				if(missileScript == null){
					
					GameObject missile = Instantiate(MissilePrefab , pos , Quaternion.identity) as GameObject;
					missileScript = missile.GetComponent<HomingMissile>();
					missiles.Add(missileScript);
					
					// Puffy Smoke important line :
					// Add this missile to the particles spawners list.
					MakeSpawner(missile,pos,Vector3.zero);
					
					/*
					 * // parent the camera to the last missile launched
					Camera.main.transform.parent = missile.transform;
					Camera.main.transform.localPosition = new Vector3(6,0,-4);
					Camera.main.transform.localEulerAngles = new Vector3(0,-90,0);
					*/
				}
				
				missileScript.angularThreshold = missileAccuracy;
				missileScript.craziness = missileCrazyness;
				
				// init the missile
				missileScript.Spawn(pos,Camera.main.transform.forward , target, missileScript.speed);
			}
		}
	}
	
	
}
