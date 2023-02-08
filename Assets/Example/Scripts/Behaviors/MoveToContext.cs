using BTree;
using UnityEngine;

/// <summary>
/// Example of using the Interrupt node with a context sent from the caller. In this case another agent calls
/// the interrupt on the Agent of this node and sends the Ball as context.
/// </summary>
public class MoveToContext : Leaf<Ball>
{
    private Player player;
    Vector3 target = default;

    protected override void OnSetup() 
    {
        player = Agent as Player;
    }

    protected override void OnEnter() 
    {
        if (Agent.debugTree) { Debug.LogWarning(Agent + " started MoveToContext action!"); }

        target = player.MoveTo(GetTarget());
    }

    public override void Execute()
    {
        if (Context == null || Context.gameObject == null)
        {
            Response.Result = Result.Failure;
            return;
        }

        var newTarget = GetTarget();

        if ((newTarget - target).sqrMagnitude > 25f)
        {
            Debug.DrawLine(player.transform.position, newTarget, Color.yellow, 2f);
            target = player.MoveTo(newTarget);
        }

        if ((Context.transform.position - player.transform.position).sqrMagnitude < 2f)
        {
            Response.Result = Result.Success;
        }

        base.Execute();
    }

    protected override void OnExit() { }

    protected override void OnReset()
    {
        target = default;
    }

    protected override void OnFail() { }

    private Vector3 GetTarget()
    {
        var rb = Context.gameObject.GetComponent<Rigidbody>();
        Vector3 estimate = Context.transform.position + Time.deltaTime * rb.velocity * 20f;
        return new Vector3(estimate.x, 0, estimate.z);
    }
}
