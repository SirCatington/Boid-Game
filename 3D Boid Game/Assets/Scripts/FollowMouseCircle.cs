using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseCircle : MonoBehaviour
{
    Transform transform;
    public Flock flock;
    public Grenade grenadePrefab;

    public float h = 25;
    public float gravity = -18;
    public float strength;
    

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;
        gameObject.transform.position = pz;

        List<FlockAgent> agents = flock.GetAgentsOfType("Leader", 1);
        foreach (FlockAgent agent in agents)
        {
            DrawPath(agent.transform.position + Vector3.forward, pz);
        }

        if (Input.GetMouseButtonDown(0))
        {
            agents = flock.GetAgentsOfType("Leader", 1);
            foreach (FlockAgent agent in agents)
            {
                Grenade grenade = Instantiate(grenadePrefab, agent.transform.position + Vector3.forward, Quaternion.identity);
                grenade.strength = strength;
                Physics.gravity = Vector3.forward * gravity;
                Rigidbody rigidbody = grenade.GetComponent<Rigidbody>();
                rigidbody.velocity = CalculateLaunchData(rigidbody.position, pz).initialVelocity;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            agents = flock.GetAgentsOfType("Leader", 1);
            foreach (FlockAgent agent in agents)
            {
                Grenade grenade = Instantiate(grenadePrefab, agent.transform.position + Vector3.forward, Quaternion.identity);
                grenade.strength = -strength;
                Physics.gravity = Vector3.forward * gravity;
                Rigidbody rigidbody = grenade.GetComponent<Rigidbody>();
                rigidbody.velocity = CalculateLaunchData(rigidbody.position, pz).initialVelocity;
            }
        }
    }

    void DrawPath(Vector3 startingPos, Vector3 target)
    {
        LaunchData launchData = CalculateLaunchData(startingPos, target);
        Vector3 previousDrawPoint = startingPos;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.forward * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = startingPos + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }

    LaunchData CalculateLaunchData(Vector3 startingPos, Vector3 target)
    {
        float displacementZ = target.z - startingPos.z;
        Vector3 displacementXY = new Vector3(target.x - startingPos.x, target.y - startingPos.y, 0);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementZ - h) / gravity);
        Vector3 velocityZ = Vector3.forward * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXY = displacementXY / time;

        return new LaunchData(velocityXY + velocityZ, time);
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData (Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
}
