using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Exercise2 : MonoBehaviour
{
    [SerializeField] private int runsPerFrame = 100;
    [SerializeField] private Text runsCounterText;
    [SerializeField] private Text oddsOutput;

    // Boolean is whether the cards are aces or not.
    private ChanceTable<bool> cardDeck;

    private int totalOccurences;
    private int totalRuns;

    private void Start()
    {
        totalOccurences = 0;
        totalRuns = 0;

        StartCoroutine(DeckPullRun());
    }

    private IEnumerator DeckPullRun()
    {
        while(true)
        {
            for(int i = 0; i < runsPerFrame; i++)
            {
                RegenerateDeck();
                if (cardDeck.Draw() == true)
                {
                    if (cardDeck.Draw() == true)
                    {
                        totalOccurences++;
                    }
                }
                totalRuns++;
            }

            runsCounterText.text = "Run Iteration: " + totalRuns;
            oddsOutput.text = "Odds(Experiment): " + (double)totalOccurences / totalRuns;

            yield return null;
        }
    }

    private void RegenerateDeck()
    {
        cardDeck = new ChanceTable<bool>();
        for (int i = 0; i < 48; i++)
        {
            cardDeck.AddOutcome(false, 1);
        }
        for (int i = 0; i < 4; i++)
        {
            cardDeck.AddOutcome(true, 1);
        }
    }
}
