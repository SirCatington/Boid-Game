﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Follow Leader")]
public class FollowLeaderBehaviour : FilteredFlockBehaviour
{
    public float radius = 5; 
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (agent.flockLeader != null)
        {
            Vector2 centerOffset = (Vector2)agent.flockLeader.transform.position - (Vector2)agent.transform.position;
            float t = centerOffset.magnitude / radius;
            if (t > 2f)
            {
                agent.flockLeader = null;
                return Vector2.zero;
            }
            else if (t < 1f)
            {                
                t = 0.1f;
            }
            return centerOffset * Mathf.Sin(Mathf.PI * t);
        }

        return Vector2.zero;
       
    }
}