using BTree;
using UnityEngine;

/// <summary>
/// Example of using the In Context to fetch a generic context from the tree through only the ITreeContext 
/// interface, and handling a null context.
/// </summary>
public class ShootOrPass : Leaf<ITreeContext>
{
    [SerializeField]
    private float shotPower = 10f;

    [SerializeField]
    private float passPower = 5f;

    private Player player;

    protected override void OnSetup()
    {
        player = Agent as Player;
    }

    protected override void OnEnter()
    {
        if (Context == null)
        {
            Lob();
            return;
        }

        bool isGoal = Context is Goal;        
        var rb = player.Ball.GetComponent<Rigidbody>();

        if (rb.velocity.sqrMagnitude > 10f || (rb.position - Agent.transform.position).sqrMagnitude > 2.5f)
        {
            Response.Result = Result.Failure;
            return;
        }

        float power = isGoal ? shotPower : passPower;
        Vector3 force = (Context.transform.position - rb.position).normalized * power;
        Vector3 lead = isGoal ? Vector3.zero : Context.transform.forward;
        Debug.DrawLine(Agent.transform.position, Agent.transform.position + force, Color.blue, 2f);
        rb.AddForce(force + Vector3.up + lead, ForceMode.Impulse);
        Response.Result = Result.Success;
    }

    protected override void OnExit() { }

    protected override void OnFail() { }

    protected override void OnReset() { }

    private void Lob()
    {
        var player = Agent as Player;
        Vector3 away = (player.Ball.transform.position - player.transform.position).normalized;
        Vector3 toGoal = (player.OpponentGoal.transform.position - player.Ball.transform.position).normalized;
        var rb = player.Ball.GetComponent<Rigidbody>();
        Vector3 force = (away + toGoal) * passPower * 0.5f + Vector3.up * 2f;
        rb.AddForce(force, ForceMode.Impulse);
        Response.Result = Result.Success;
    }
}
