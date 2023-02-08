using BTree;
using UnityEngine;
using UnityEngine.AI;

public class MoveToSpace : Leaf<NoContext>
{
    [SerializeField]
    private float multiplier = 4f;

    private Vector3 target;
    private float originalSpeed;
    private Player player;
    private NavMeshAgent navMeshAgent;

    protected override void OnSetup()
    {
        player = Agent as Player;
        navMeshAgent = player.GetComponent<NavMeshAgent>();
        originalSpeed = navMeshAgent.speed;
    }

    protected override void OnEnter()
    {
        navMeshAgent.speed = 0.75f * originalSpeed;
        Vector3 random = Random.insideUnitSphere * multiplier;
        Transform goal = GetGoalTransform(player);
        Vector3 towards = (goal.position - player.transform.position).normalized * multiplier * 0.5f;
        random.y = 0;
        target = player.MoveTo(player.StartPos + random + towards);
    }

    public override void Execute()
    {
        if ((Agent.transform.position - target).sqrMagnitude < 1.5f)
        {
            Response.Result = Result.Success;
        }
        else
        {
            base.Execute();
        }   
    }

    protected override void OnExit()
    {
        navMeshAgent.speed = originalSpeed;
    }

    protected override void OnReset()
    {
        target = default;
    }

    protected override void OnFail() { }

    private Transform GetGoalTransform(Player player)
    {
        if ((player.OwnGoal.transform.position - player.transform.position).sqrMagnitude > 
            (player.OpponentGoal.transform.position - player.transform.position).sqrMagnitude)
        {
            return player.OwnGoal.transform;
        }

        return player.OpponentGoal.transform;
    }
}
