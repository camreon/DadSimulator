using UnityEngine;
using System.Collections;

public class faucetBehavior : Interactable {

    public EllipsoidParticleEmitter water;
    public GameObject planeWater;
    public EllipsoidParticleEmitter splash;
    public EllipsoidParticleEmitter waterfall;
    public GameObject puddle;
    public float fillSpeed = 0.005f;
    public float maxWaterHeight = 3;
	public float minWaterHeight = 0;
    public float fullWaterHeight = 2.5f;
    public bool isFull = false;

    public override void OnStart()
    {
        puddle.SetActive(false);
    }
	public override void Interact()
    {
        Debug.Log("faucet interacted!");
        water.emit = !water.emit;
        splash.emit = !splash.emit;

        if (!water.emit)
        {
            waterfall.emit = false;
        }
    }

    void Update()
    {
        if (water.emit && planeWater != null)
        {
            if(planeWater.transform.localPosition.y < maxWaterHeight)
            {
                planeWater.transform.Translate(0, fillSpeed, 0, Space.Self);
            }
            if (planeWater.transform.localPosition.y >= fullWaterHeight)
            {
                isFull = true;
            }
            if (planeWater.transform.localPosition.y >= maxWaterHeight)
            {
                waterfall.emit = true;
                puddle.SetActive(true);
            }
        }

		// drain
//		else if (planeWater != null && planeWater.transform.localPosition.y > minWaterHeight)
//			planeWater.transform.Translate(0, -fillSpeed, 0, Space.Self);
    }
}
