using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behaviour/Avoid Other Flocks")]
public class AvoidOtherFlocks : FilteredFlockBehaviour
{         
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0 || agent.flockLeader != null) return Vector2.zero;
        
        
            
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
            
        foreach (Transform item in context)
        {
            FlockAgent targetAgent = item.gameObject.GetComponent<FlockAgent>();
            if (targetAgent == null || targetAgent.flockId == agent.flockId || targetAgent.flockId == 0)
            {
                continue;
            }
                
            nAvoid++;
            avoidanceMove += (Vector2)(agent.transform.position - item.position);
                

        }
        //if (nAvoid > 0) avoidanceMove /= nAvoid;

        return avoidanceMove;            
        

    }
}
