using BTree;
using UnityEngine;

/// <summary>
/// Example of using the In Context to fetch a generic context from the tree through only the MonoBehaviour 
/// interface, handling a null context, and triggering an interrupt in another TreeAgent.
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
        bool isGoal = Context is Goal;
        var rb = player.Ball.GetComponent<Rigidbody>();

        if (rb.velocity.sqrMagnitude > 10f || (rb.position - Agent.transform.position).sqrMagnitude > 3f)
        {
            Response.Result = Result.Failure;
            return;
        }

        float power = isGoal ? shotPower : passPower;
        Vector3 direction = Context.Position - rb.position;
        Vector3 dir = direction.normalized * Mathf.Min(direction.magnitude, power);
        Vector3 lead = isGoal ? Random.insideUnitSphere * 2f : Context.gameObject.transform.forward * 2f;
        Debug.DrawLine(Agent.transform.position, Agent.transform.position + dir, Color.blue, 2f);
        rb.AddForce(dir + Vector3.up + lead, ForceMode.Impulse);
        Response.Result = Result.Success;
    }

    protected override void OnExecute() { }

    protected override void OnExit()
    {
        if (Response.Result != Result.Failure && Context is Player target)
        {
            target.TriggerInterrupt("receivePass", player.Ball);
        }
    }

    protected override void OnFail() { }

    protected override void OnReset() { }

    private void Lob()
    {
        var player = Agent as Player;
        Vector3 away = (player.Ball.Position - player.transform.position).normalized;
        Vector3 toGoal = (player.OpponentGoal.Position - player.Ball.Position).normalized;
        var rb = player.Ball.GetComponent<Rigidbody>();
        Vector3 force = (away + toGoal) * passPower * 0.5f + Vector3.up;
        rb.AddForce(force, ForceMode.Impulse);
        Response.Result = Result.Success;
    }
}
