using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents;
    public CompositeBehaviour behaviour;
    public CameraController cameraController;
    public bool gameEnd;

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.16f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    [Range(1f, 10f)]
    public float leaderRadius = 5;
    public int leaderCount = 5;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    Color BaseBoidColor;



    // Start is called before the first frame update
    void OnEnable()
    {
        gameEnd = false;
        agents = new List<FlockAgent>();
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
            newAgent.Initialize(this, false, 0, "Basic");
            agents.Add(newAgent);
        }
        InstantiateLeader(NewColor(249, 57, 67), true, 1);
        InstantiateLeader(NewColor(252, 176, 179), false, 2);
        InstantiateLeader(NewColor(126, 178, 221), false, 3);
        InstantiateLeader(NewColor(68, 94, 147), false, 4);



    }

    public int BoidNumber()
    {
        return agents.Count;
    }
    
    

    Color NewColor(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    void InstantiateLeader(Color leaderColor, bool isPlayer, int id)
    {
        FlockAgent leaderAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );


        leaderAgent.GetComponentInChildren<Renderer>().material.SetColor("_Color", leaderColor);
        leaderAgent.flockLeader = leaderAgent;

        leaderAgent.name = "Leader Agent " + id;
        leaderAgent.Initialize(this, isPlayer, id, "Leader");
        agents.Add(leaderAgent);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents)
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
                //Vector2 centerOffset = (Vector2)agent.flockLeader.transform.position - (Vector2)agent.transform.position;
                //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(agent.flockLeader.GetComponentInChildren<SpriteRenderer>().color, BaseBoidColor, (centerOffset.magnitude/5) - 1);
                
                //agent.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.Lerp(agent.flockLeader.GetComponentInChildren<Renderer>().sharedMaterial.color, BaseBoidColor, (centerOffset.magnitude / leaderRadius) - 1));
                agent.GetComponentInChildren<Renderer>().material.SetColor("_Color", agent.flockLeader.GetComponentInChildren<Renderer>().sharedMaterial.color);
            }

            if (agent.flockLeader == agent && agent.isPlayer && !gameEnd)
            {
                cameraController.MoveCameraTo(agent.transform.position);
                behaviour.weights[5] = 2;
            }
            else if (gameEnd)
            {
                cameraController.MoveCameraTo(Vector3.zero);
                behaviour.weights[5] = 0;
            }
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        int[] flockIds = new int[5];
        int id = agent.flockId;
        foreach (Collider2D c in contextColliders)
        {
            if (c.tag == "Boid") {
                FlockAgent targetAgent = c.gameObject.GetComponent<FlockAgent>();

                flockIds[targetAgent.flockId] += 1;

                if (agent.flockLeader == null && targetAgent.flockLeader != null)
                {
                    agent.flockLeader = targetAgent.flockLeader;
                    agent.flockId = targetAgent.flockId;

                }
                
            }

            context.Add(c.transform);
        }

        id = System.Array.IndexOf(flockIds, Mathf.Max(flockIds));
        List<FlockAgent> leaderAgent = GetAgentsOfType("Leader", id);
        if (leaderAgent.Count != 0)
        {
            if (agent.type == "Leader" && id != agent.flockId)
            {

                ChooseSuccessor(agent);
                agent.type = "Basic";
                if (agent.isPlayer)
                {
                    agent.isPlayer = false;
                }
            }
            agent.flockId = id;
            agent.flockLeader = leaderAgent[0];
            
        }

        return context;
    }

    public void RemoveBoid(FlockAgent agent)
    {
        agents.Remove(agent);
    }

    public bool playerAlive()
    {
        foreach (FlockAgent agent in agents)
        {
            if (agent.flockId == 1)
            {
                return true;
            }
        }
        return false;
    }

    public bool OnlyFlockAlive(int id)
    {
        foreach (FlockAgent agent in agents)
        {
            if (agent.flockId != id)
            {
                return false;
            }
        }
        return true;
    }

    public void ChooseSuccessor(FlockAgent leaderAgent)
    {
        FlockAgent newLeader = GetAgentOfType("Basic", leaderAgent.flockId);
        if (newLeader != null)
        {
            newLeader.flockLeader = newLeader;            
            newLeader.type = "Leader";

            if (leaderAgent.isPlayer)
            {
                newLeader.isPlayer = true;
            }            
        }
        
            
       
        if (newLeader != null)
        {
            foreach (FlockAgent agent in agents)
            {
                if (agent.flockLeader == leaderAgent)
                {
                    agent.flockLeader = newLeader;                  
                    
                }

            }
        }
    }

    public FlockAgent GetAgentOfType(string type, int id)
    {
       
        foreach (FlockAgent agent in agents)
        {
            if (agent.type == type && agent.flockId == id)
            {
                return agent;
            }
        }

        return null;

    }

    public List<FlockAgent> GetAgentsOfType(string type, int id){
        List<FlockAgent> filteredAgents = new List<FlockAgent>();
        foreach (FlockAgent agent in agents)
        {
            if (agent.type == type && agent.flockId == id)
            {
                filteredAgents.Add(agent);
            }
        }
        
        return filteredAgents;
        
    }
}
