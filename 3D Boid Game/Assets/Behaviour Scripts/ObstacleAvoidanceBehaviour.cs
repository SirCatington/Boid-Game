using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Obstacle Avoidance")]
public class ObstacleAvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        
            Vector2 avoidanceMove = Vector2.zero;
            RaycastHit2D hit = Physics2D.Raycast(agent.transform.position, agent.transform.up, flock.neighborRadius, LayerMask.GetMask("Obstacles"));
            //Debug.DrawRay(agent.transform.position, agent.transform.up, Color.green, 0.01f);
            if (hit.collider != null)
            {

                for (int angle = 20; angle < 180;)
                {
                    Vector2 rotation = agent.transform.up;
                    rotation = Quaternion.Euler(0, 0, angle) * rotation;
                    RaycastHit2D potentialHit = Physics2D.Raycast(agent.transform.position, rotation, flock.neighborRadius, LayerMask.GetMask("Obstacles"));
                    //Debug.DrawRay(agent.transform.position, rotation * flock.neighborRadius, Color.red, 0.1f);
                    if (potentialHit.collider != null)
                    {
                        return rotation;
                    }

                    potentialHit = Physics2D.Raycast(agent.transform.position, -rotation, flock.neighborRadius, LayerMask.GetMask("Obstacles"));
                    //Debug.DrawRay(agent.transform.position, rotation * flock.neighborRadius, Color.red, 0.1f);
                    if (potentialHit.collider != null)
                    {
                        return rotation;
                    }

                    angle += 20;
                }
            }
        
        return Vector2.zero;
    }
}

