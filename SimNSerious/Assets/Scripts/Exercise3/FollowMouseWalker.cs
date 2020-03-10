using UnityEngine;

public sealed class FollowMouseWalker : Walker
{
    [SerializeField] Camera targetCamera;
    [SerializeField][Range(0, 1)] private float chanceToFollowMouse = 0.5f;

    public override void Step()
    {
        if(Probability.Chance(chanceToFollowMouse))
        {
            // Step towards the mouse location instead of randomly.
            Vector2 mouseLocation = targetCamera.ScreenToWorldPoint(Input.mousePosition);

            // Move towards the cursor.
            if(mouseLocation.x < locationX) { locationX--; }
            else { locationX++; }
            if (mouseLocation.y < locationY) { locationY--; }
            else { locationY++; }
            transform.position = new Vector2(locationX, locationY);
        }
        else
        {
            base.Step();
        }
    }
}
