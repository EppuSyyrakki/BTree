using BTree;
using UnityEngine;

public class MoveToSpace : Leaf<NoContext>
{
    [SerializeField]
    private float multiplier = 4f;

    private Vector3 target;

    protected override void OnEnter()
    {
        var player = Agent as Player;
        Vector3 random = Random.insideUnitSphere * multiplier;
        Transform goal = GetGoalTransform(player);
        Vector3 towards = (goal.position - player.transform.position).normalized * multiplier * 0.5f;
        random.y = 0;
        target = player.MoveTo(player.transform.position + random + towards);
    }

    public override void Execute()
    {
        if ((Agent.transform.position - target).sqrMagnitude < 1f)
        {
            Result = Result.Success;
        }

        //base.Execute();
    }

    protected override void OnExit()
    {
    }

    internal override void ResetNode()
    {
        base.ResetNode();
        target = default;
    }

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
