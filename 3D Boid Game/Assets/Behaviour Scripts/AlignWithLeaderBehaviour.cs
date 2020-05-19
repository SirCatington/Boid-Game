using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Align With Leader")]
public class AlignWithLeaderBehaviour : FilteredFlockBehaviour
{
    public float radius = 5;
    
   
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        if (agent.flockLeader != null)
        {
            Vector2 centerOffset = (Vector2)agent.flockLeader.transform.position - (Vector2)agent.transform.position;
            float t = centerOffset.magnitude / radius;

            if (t < 1f)
            {
                return agent.flockLeader.transform.up;
            }

        }

        return Vector2.zero;
    }
}
