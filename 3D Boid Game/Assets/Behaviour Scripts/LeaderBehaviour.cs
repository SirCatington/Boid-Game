using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behaviour/LeaderBehaviour")]
public class LeaderBehaviour : FilteredFlockBehaviour
{
    public float radius = 7;
    public float leaderRadius = 3;
    Vector2 previousMouseOffset;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (agent.isPlayer)
        {
            //Vector2 leaderOffset = (Vector2)agent.flockLeader.transform.position - (Vector2)agent.transform.position;
            //float t = leaderOffset.magnitude / radius;

            //if (t > 1)
            //{
            //    return Vector2.zero;
            //}
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                float b = previousMouseOffset.magnitude / radius;
                return previousMouseOffset * b * b;

            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseOffset = mousePos - (Vector2)agent.transform.position;
            float t = mouseOffset.magnitude / radius;
            previousMouseOffset = mouseOffset;
            if (t < 1)
            {
                return Vector2.zero;
            }

            return mouseOffset * t * t;
        }
        return Vector2.zero;
    }
}
