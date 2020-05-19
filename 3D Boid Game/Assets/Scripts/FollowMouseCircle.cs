using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseCircle : MonoBehaviour
{
    Transform transform;
    SpriteRenderer spriteRenderer;

    public Flock flock;
    public Grenade grenadePrefab;

    public float h = 25;
    public float gravity = -18;
    float strengthMultiplier = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;
        gameObject.transform.position = pz;

        transform.localScale = Vector2.one * 2 * strengthMultiplier;

        List<FlockAgent> agents = flock.GetAgentsOfType("Leader", 1);
        foreach (FlockAgent agent in agents)
        {
            DrawPath(agent.transform.position + Vector3.forward, pz);
        }

        
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && strengthMultiplier <= 5)
        {
            strengthMultiplier += 2f * Time.deltaTime;

            if (Input.GetMouseButton(0)){
                spriteRenderer.color = new Color(0 / 255f, 125 / 255f, 255 / 255f, 72f / 255f);
            }
            else if (Input.GetMouseButton(1))
            {
                spriteRenderer.color = new Color(255 / 255f, 125 / 255f, 0 / 255f, 72f / 255f);
            }
        }

        if (strengthMultiplier > 1)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
        

        if (Input.GetMouseButtonUp(0))
        {
            agents = flock.GetAgentsOfType("Leader", 1);
            foreach (FlockAgent agent in agents)
            {
                Grenade grenade = Instantiate(grenadePrefab, agent.transform.position + Vector3.forward, Quaternion.identity);
                grenade.strength = 2f * strengthMultiplier;
                Physics.gravity = Vector3.forward * gravity;
                Rigidbody rigidbody = grenade.GetComponent<Rigidbody>();
                rigidbody.velocity = CalculateLaunchData(rigidbody.position, pz).initialVelocity;
            }
            strengthMultiplier = 1;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            agents = flock.GetAgentsOfType("Leader", 1);
            foreach (FlockAgent agent in agents)
            {
                Grenade grenade = Instantiate(grenadePrefab, agent.transform.position + Vector3.forward, Quaternion.identity);
                grenade.strength = -2f * strengthMultiplier;
                Physics.gravity = Vector3.forward * gravity;
                Rigidbody rigidbody = grenade.GetComponent<Rigidbody>();
                rigidbody.velocity = CalculateLaunchData(rigidbody.position, pz).initialVelocity;
            }
            strengthMultiplier = 1;
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
