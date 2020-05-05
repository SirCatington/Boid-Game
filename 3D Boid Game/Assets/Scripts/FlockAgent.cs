using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock {get {return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    public FlockAgent flockLeader;


    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity, int nearbyBoids)
    {
        if (nearbyBoids != 0)
        {
            transform.up = velocity;
            transform.position += (Vector3)velocity * Time.deltaTime;
        }
        
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    FlockAgent collider = collision.gameObject.GetComponent<FlockAgent>();
    //    if (collider != null)
    //    {
    //        collider.KillSelf();
    //    }
    //    KillSelf();
    //}

    public void KillSelf()
    {
        if(flockLeader == this)
        {
            agentFlock.ChooseSuccessor(this);
        }
        agentFlock.RemoveBoid(this);
        Destroy(gameObject);
    }
}
