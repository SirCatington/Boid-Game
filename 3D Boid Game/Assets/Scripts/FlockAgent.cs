﻿using System.Collections;
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
    public bool isPlayer;
    public int flockId;
    public string type;

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Initialize(Flock flock, bool isPlayerInput, int id, string agentType)
    {
        agentFlock = flock;
        isPlayer = isPlayerInput;
        flockId = id;
        type = agentType;
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

    public void AttractTo(Vector2 attractionPoint, float attractionForce, float radius)
    {        
        Vector2 attractionOffset = attractionPoint - (Vector2)transform.position;
        float t = attractionOffset.magnitude / radius;
        Vector2 attractionMove = attractionOffset / (t * t);
        attractionMove.Normalize();
        attractionMove *= attractionForce;
        transform.position += (Vector3)attractionMove * Time.deltaTime;

    }

    public void KillSelf()
    {
        if(flockLeader == this)
        {
            Debug.Log("Chosing Successor");
            agentFlock.ChooseSuccessor(this);
        }
        agentFlock.RemoveBoid(this);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
