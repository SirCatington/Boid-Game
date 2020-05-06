using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;
    public CameraController cameraController;

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    Color BaseBoidColor;


    // Start is called before the first frame update
    void Start()
    {
        BaseBoidColor = agentPrefab.GetComponentInChildren<Renderer>().sharedMaterial.color;

        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount - 1; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }

        FlockAgent leaderAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
        Color leaderColor = new Color(255f/255f, 107/255f, 107/255f); 
        leaderAgent.GetComponentInChildren<Renderer>().material.SetColor("_Color", leaderColor);
        leaderAgent.flockLeader = leaderAgent;

        leaderAgent.name = "Agent " + startingCount;
        leaderAgent.Initialize(this);
        agents.Add(leaderAgent);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count/6f);

            Vector2 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move, context.Count);

            if (agent.flockLeader == null)
            {
                //agent.GetComponentInChildren<SpriteRenderer>().color = BaseBoidColor;
                agent.GetComponentInChildren<Renderer>().material.SetColor("_Color", BaseBoidColor);
            }
            else
            {
                Vector2 centerOffset = (Vector2)agent.flockLeader.transform.position - (Vector2)agent.transform.position;
                //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(agent.flockLeader.GetComponentInChildren<SpriteRenderer>().color, BaseBoidColor, (centerOffset.magnitude/5) - 1);
                agent.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.Lerp(agent.flockLeader.GetComponentInChildren<Renderer>().sharedMaterial.color, BaseBoidColor, (centerOffset.magnitude / 5) - 1));

            }

            if (agent.flockLeader == agent)
            {
                cameraController.MoveCameraTo(agent.transform.position);
            }
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders)
        {
            if (c.tag == "Boid") {
                if (c != agent.AgentCollider)
                {
                    FlockAgent targetAgent = c.gameObject.GetComponent<FlockAgent>();
                    if (agent.flockLeader == null && targetAgent.flockLeader != null)
                    {
                        agent.flockLeader = targetAgent.flockLeader;
                        
                    }
                    
                    
                }
            }
            context.Add(c.transform);
        }
        return context;
    }

    public void RemoveBoid(FlockAgent agent)
    {
        agents.Remove(agent);
    }

    public void ChooseSuccessor(FlockAgent leaderAgent)
    {
        FlockAgent newLeader = null;
        foreach (FlockAgent agent in agents)
        {
            if (agent.flockLeader == leaderAgent)
            {
                agent.flockLeader = agent;
                newLeader = agent;
                break;
            }
        }
        if (newLeader != null)
        {
            foreach (FlockAgent agent in agents)
            {
                if (agent.flockLeader == leaderAgent)
                {
                    agent.flockLeader = newLeader;
                    Vector2 centerOffset = (Vector2)agent.flockLeader.transform.position - (Vector2)agent.transform.position;
                    agent.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.Lerp(agent.flockLeader.GetComponentInChildren<Renderer>().sharedMaterial.color, BaseBoidColor, (centerOffset.magnitude / 5) - 1));
                }
                
            }
        }
    }
}
