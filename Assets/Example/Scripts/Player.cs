using BTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Player : TreeAgent
{
    public enum Team { None = 0, Red, Blue }

    [SerializeField]
    private Team team;

    [SerializeField, Range(2, 8)]
    private float defensiveRange = 5f;

    [SerializeField, Range(0.1f, 2)]
    private float attackWeight = 1.25f;

    private NavMeshAgent navMeshAgent;

    public bool IsDefending { get; set; }
    public bool MovingToBall { get; set; }
    public Ball Ball { get; set; }
    public Goal OwnGoal { get; set; }
    public Goal OpponentGoal { get; set; }
    public List<Player> TeamMates { get; private set; }
    public Vector3 StartPos { get; private set; }
    public Team Side => team;
    public float DefensiveRange => defensiveRange;
    public float AttackWeight => attackWeight;

    protected override void Awake()
    {
        base.Awake();
        StartPos = transform.position;
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
        //Debug.DrawLine(transform.position, pos, Color.red, 2f);
        return pos;
    }
}
