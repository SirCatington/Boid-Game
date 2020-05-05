using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Align With Leader")]
public class AlignWithLeaderBehaviour : FilteredFlockBehaviour
{
   

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        if (agent.flockLeader != null)
        {
             return agent.flockLeader.transform.up;           
            
        }

        return Vector2.zero;
    }
}
