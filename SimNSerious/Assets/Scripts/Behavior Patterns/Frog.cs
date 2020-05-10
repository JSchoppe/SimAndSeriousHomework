using System.Collections;
using UnityEngine;

public sealed class Frog : Behaver
{
    // Simulate a distribution where the extremes are more likely.
    // This would match a lifecycle with very short growth phases.
    private static readonly Distribution FrogSize = new Distribution(
        (float x) => { return 9*(x - 0.3f)*(x - 0.3f) + 0.1f; },
        0.2f, 0.35f
    );
    // Linear Distribution. Frequent jumps unlikely.
    private static readonly Distribution JumpInterval = new Distribution(
        (float x) => { return 2 - x; },
        1, 2
    );
    // Square Distribution. Small jumps very unlikely.
    private static readonly Distribution JumpForce = new Distribution(
        (float x) => { return x * x; },
        1, 2
    );
    // Sin Distribution. 45degree jumps most likely.
    private static readonly Distribution JumpAngle = new Distribution(
        (float x) => { return Mathf.Sin(2 * x); },
        0, Mathf.PI / 2
    );
    // Turn direction is completely random.
    private static readonly Distribution TurnAngle = new Distribution(-90, 90);


    private void Start()
    {
        Initialize();
        StartCoroutine(ConsiderJump());
    }


    private IEnumerator ConsiderJump()
    {
        while(true)
        {
            transform.eulerAngles += Vector3.up * TurnAngle.Next();

            float jumpForce = JumpForce.Next();
            float jumpAngle = JumpAngle.Next();
            body.AddForce(
                Vector3.up * jumpForce * Mathf.Sin(jumpAngle) +
                transform.forward * jumpForce * Mathf.Cos(jumpAngle),
                ForceMode.Impulse
            );
            yield return new WaitForSeconds(JumpInterval.Next());
        }
    }

    private void FixedUpdate()
    {
        foreach(Liquid liquid in Liquid.SceneLiquids)
            if(liquid.IsObjectInside(transform))
            {
                body.AddForce(
                    10 * Vector3.up * Time.fixedDeltaTime,
                    ForceMode.Impulse
                );
            }
    }

    public override void RandomizeTraits()
    {
        transform.localScale = FrogSize.Next() * Vector3.one;
    }
}
