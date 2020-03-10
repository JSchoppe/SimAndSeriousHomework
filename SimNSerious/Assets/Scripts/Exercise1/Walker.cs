using UnityEngine;

public class Walker : MonoBehaviour
{
    // Manipulate the probability table from the inspector.
    [SerializeField] private float noMoveWeight = 1;
    [SerializeField] private float leftWeight = 1;
    [SerializeField] private float rightWeight = 1;
    [SerializeField] private float upWeight = 1;
    [SerializeField] private float downWeight = 1;

    private ChanceTable<int> horizontalChance;
    private ChanceTable<int> verticalChance;
    protected int locationX;
    protected int locationY;

    private void Awake()
    {
        // Construct and populate probability tables.
        horizontalChance = new ChanceTable<int>();
        verticalChance = new ChanceTable<int>();
        horizontalChance.AddOutcome(-1, leftWeight);
        horizontalChance.AddOutcome(0, noMoveWeight);
        horizontalChance.AddOutcome(1, rightWeight);
        verticalChance.AddOutcome(-1, downWeight);
        verticalChance.AddOutcome(0, noMoveWeight);
        verticalChance.AddOutcome(1, upWeight);

        // Output The table for debug purposes.
        Debug.Log("Horizontal Chance Table:\n" + horizontalChance.ToString());
        Debug.Log("Vertical Chance Table:\n" + verticalChance.ToString());

        // Initialize the object's position.
        locationX = 0;
        locationY = 0;
        gameObject.transform.position = new Vector2(locationX, locationY);
    }

    /// <summary>Invokes the walker to make a choice to move somewhere</summary>
    public virtual void Step()
    {
        // Generate random movement values based on the weights.
        locationX += horizontalChance.Roll();
        locationY += verticalChance.Roll();
        
        gameObject.transform.position = new Vector2(locationX, locationY);
    }
}
