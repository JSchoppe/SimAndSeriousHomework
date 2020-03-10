using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Exercise4 : MonoBehaviour
{
    [SerializeField] private GameObject paintSpot;
    [SerializeField] private float maxSpread;
    [SerializeField] private int minSpotCount;
    [SerializeField] private int maxSpotCount;
    [SerializeField] private float minSpotSize;
    [SerializeField] private float maxSpotSize;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GeneratePaint());
    }

    private IEnumerator GeneratePaint()
    {
        while(true)
        {
            int totalSpots = Random.Range(minSpotCount, maxSpotCount);

            for(int i = 0; i < totalSpots; i++)
            {
                float spreadX = Probability.NextGaussian(-8, 8, 0.5f, 0);
                float spreadY = Probability.NextGaussian(-8, 8, 0.5f, 0);
                float scale = Probability.NextGaussian(0.5f, 1.5f, 1f, 0);

                GameObject newSpot = Instantiate(paintSpot);
                newSpot.transform.position = new Vector2(spreadX, spreadY);
                newSpot.transform.localScale = scale * Vector2.one;
            }

            yield return new WaitForSeconds(2);
        }
    }
}
