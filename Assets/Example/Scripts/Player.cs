using BTree;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Player : TreeAgent
{
    public enum Team { None = 0, Red, Blue }

    [SerializeField]
    private Team team;

    [SerializeField]
    private float shotPower = 500f;

    private NavMeshAgent navMeshAgent;

    public Ball Ball { get; set; }
    
    public Vector3 MoveTo 
    { set
        {
            navMeshAgent.SetDestination(value);
            Debug.DrawLine(transform.position, value, Color.red, 1f);
        } 
    }
    public Team Side => team;
    public float ShotPower => shotPower;

    protected override void Awake()
    {
        base.Awake();      
        navMeshAgent = GetComponent<NavMeshAgent>();
        var players = FindObjectsOfType<Player>().ToList();
        players.Remove(this);
    }

}
