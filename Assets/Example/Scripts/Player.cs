using BTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Player : TreeAgent
{
    public enum Team { None = 0, Red, Blue }

    [SerializeField]
    private Team team;

    private NavMeshAgent navMeshAgent;

    public bool MovingToBall { get; set; }
    public Ball Ball { get; set; }
    public Goal OwnGoal { get; set; }
    public Goal OpponentGoal { get; set; }
    public List<Player> TeamMates { get; private set; }

    public Team Side => team;

    protected override void Awake()
    {
        base.Awake();      
        navMeshAgent = GetComponent<NavMeshAgent>();
        var players = FindObjectsOfType<Player>().ToList();
        players.Remove(this);
        TeamMates = new List<Player>(players.Count / 2);

        foreach (var p in players)
        {
            if (p.Side == team)
            {
                TeamMates.Add(p);
            }
        }
    }

    public Vector3 MoveTo(Vector3 position)
    {
        Vector3 pos;
        if (NavMesh.SamplePosition(position, out var hit, 4f, NavMesh.AllAreas))
        {
            pos = hit.position;            
        }
        else
        {
            pos = Vector3.zero;
        }

        navMeshAgent.SetDestination(pos);
        Debug.DrawLine(transform.position, pos, Color.red, 2f);
        return pos;
    }
}
