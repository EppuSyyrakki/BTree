using BTree;
using UnityEngine;

public class Idle : Leaf<ITreeContext>
{
    private float idleTime;

    protected override void OnEnter()
    {
    }

    public override void Execute()
    {
        idleTime += Time.deltaTime;

        if (idleTime > maxDuration) { Response.Result = Result.Success; }        
    }

    protected override void OnExit()
    {
    }

    internal override void ResetNode()
    {
        base.ResetNode();
        idleTime = 0;
    }
}
