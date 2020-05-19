using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behaviour/LeaderBehaviour")]
public class LeaderBehaviour : FilteredFlockBehaviour
{
    public float radius = 7;
    public float leaderRadius = 3;
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

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseOffset = mousePos - (Vector2)agent.transform.position;
            float t = mouseOffset.magnitude / radius;

            if (t < 1)
            {
                return Vector2.zero;
            }

            return mouseOffset * t * t;
        }
        return Vector2.zero;
    }
}
