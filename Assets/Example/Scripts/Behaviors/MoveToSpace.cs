using BTree;
using UnityEngine;
using UnityEngine.AI;

public class MoveToSpace : Leaf<ITreeContext>
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
        Debug.DrawLine(target, player.Position, Color.white, 2f);
    }

    protected override void OnExecute()
    {
        if ((Agent.transform.position - target).sqrMagnitude < 1f)
        {
            Response.Result = Result.Success;
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
        if ((player.OwnGoal.Position - player.Ball.Position).sqrMagnitude >
            (player.OpponentGoal.Position - player.Ball.Position).sqrMagnitude
            && Random.Range(0, 2.5f) > player.AttackWeight)
        {
            return player.OwnGoal.transform;
        }

        return player.OpponentGoal.transform;
    }
}
