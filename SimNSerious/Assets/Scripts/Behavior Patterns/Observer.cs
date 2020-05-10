using System.Collections;
using UnityEngine;

public sealed class Observer : Behaver
{
    public Light cameraFlash;

    #region Behaver Traits
    // Define random generation for the observers.

    // Approximate normal distribution using sin wave.
    private static readonly Distribution Height = new Distribution(
        (float x) => { return Mathf.Sin(4 * Mathf.PI * x) * 0.5f + 0.5f; },
        0.9f, 1.3f
    );
    // All shirt hues are equally likely.
    private static readonly Distribution ShirtHue = new Distribution(0, 1);
    // Color saturation is generally lower.
    private static readonly Distribution ShirtSat = new Distribution(
        (float x) => { return (x - 1) * (x - 1); },
        0, 1
    );
    // Lightness is generally lower.
    private static readonly Distribution ShirtVal = new Distribution(
        (float x) => { return (x - 1) * (x - 1); },
        0, 1
    );

    public override void RandomizeTraits()
    {
        // Give this observer a custom height.
        transform.localScale = Vector3.one * Height.Next();

        // Customize the shirt color of this observer.
        Material ShirtMaterial = new Material(Shader.Find("Diffuse"));
        ShirtMaterial.color = Color.HSVToRGB(
            ShirtHue.Next(),
            ShirtSat.Next(),
            ShirtVal.Next()
        );
        gameObject.GetComponent<Renderer>().material = ShirtMaterial;
    }
    #endregion

    private static readonly Distribution ThoughtFrequency = new Distribution(1, 4);
    private static Distribution WalkDistance = new Distribution(1, 3);

    private Transform focalTarget;
    private Vector3 walkTarget;

    // Define the moment where the observer considers changing its current pattern.
    private IEnumerator ThoughtPattern()
    {
        while(true)
        {
            Behaver random = GetRandomInScene(this);
            if(random != null)
            {
                focalTarget = GetRandomInScene(this).transform;
            }
            if(Random.Range(0, 1f) < 0.7f)
            {
                ChangePosition();
            }
            if (Random.Range(0, 1f) < 0.05f)
            {
                StartCoroutine(TakePicture());
            }
            yield return new WaitForSeconds(ThoughtFrequency.Next());
        }
    }

    private const float pictureTime = 0.4f;
    private const float pictureLightIntensity = 60;
    private IEnumerator TakePicture()
    {
        cameraFlash.enabled = true;
        float timeElapsed = 0;
        while(timeElapsed < pictureTime)
        {
            timeElapsed += Time.deltaTime;
            cameraFlash.intensity = pictureLightIntensity * (1 - (timeElapsed / pictureTime));

            yield return new WaitForEndOfFrame();
        }
        cameraFlash.enabled = false;
    }

    private void ChangePosition()
    {
        // Randomly choose whether to go left/right and back/forward.
        Vector3 xComponent = (Random.Range(0, 2) == 1) ? Vector3.left : Vector3.right;
        Vector3 zComponent = (Random.Range(0, 2) == 1) ? Vector3.back : Vector3.forward;
        // Set the target position to walk to.
        walkTarget = body.position + xComponent * WalkDistance.Next() + zComponent * WalkDistance.Next();
    }

    #region MonoBehaviour
    private void Start()
    {
        Initialize();
        StartCoroutine(ThoughtPattern());
    }
    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, walkTarget) > 0.1f)
        {
            body.AddForce((walkTarget - transform.position) * Time.fixedDeltaTime * 3, ForceMode.Impulse);
        }
    }
    private void Update()
    {
        // Rotate to look towards the target behaver.
        if(focalTarget != null)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.LookRotation(
                    focalTarget.position - transform.position
                ),
                60f * Time.deltaTime
            );
        }
    }
    #endregion
}
