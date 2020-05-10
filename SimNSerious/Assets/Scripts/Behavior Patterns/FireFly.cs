using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FireFly : Behaver
{

    private const float maxWingRate = 50;
    private float wingCycle;

    [SerializeField] private Transform leftWing, rightWing;
    [SerializeField] private Light glowLight;

    private static readonly Distribution GlowIntensity = new Distribution(2, 4);
    private static readonly Distribution AccelerationRate = new Distribution(6, 10);

    private float glowIntensity;
    private float maxAcceleration;

    public override void RandomizeTraits()
    {
        glowIntensity = GlowIntensity.Next();
        maxAcceleration = AccelerationRate.Next();
    }

    #region MonoBehaviour
    private void Start()
    {
        Initialize();
        wingCycle = 0;
    }
    private void Update()
    {
        glowLight.intensity = glowIntensity * Mathf.PerlinNoise(Time.time, 0);

        transform.LookAt(transform.position + body.velocity);

        float currentWingAngle = (50 + Mathf.Sin(wingCycle) * 30);
        leftWing.localEulerAngles = Vector3.back * currentWingAngle;
        rightWing.localEulerAngles = Vector3.forward * currentWingAngle;
    }
    private void FixedUpdate()
    {
        bool inLiquid = false;
        foreach(Liquid liquid in Liquid.SceneLiquids)
            if(liquid.IsObjectInside(transform))
            {
                inLiquid = true;
                break;
            }

        float xForce = 0;
        float yForce = 0;
        float zForce = 0;
        if(inLiquid)
        {
            yForce = maxAcceleration * 1.5f;
        }
        else
        {
            xForce = Random.Range(-maxAcceleration, maxAcceleration);
            yForce = Random.Range(-maxAcceleration, maxAcceleration);
            zForce = Random.Range(-maxAcceleration, maxAcceleration);
        }

        // Flap the wings if an upwards force is being applied.
        if(yForce > 0)
        {
            wingCycle += Time.fixedDeltaTime * (yForce / maxAcceleration) * maxWingRate;
        }

        Vector3 motion = new Vector3(xForce, yForce, zForce);
        body.AddForce(motion * Time.fixedDeltaTime, ForceMode.Impulse);
    }
    #endregion
}
