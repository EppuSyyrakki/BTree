using BTree;
using UnityEngine;

public class Idle : Leaf<ITreeContext>
{
    private float idleTime;

    protected override void OnSetup() { }

    protected override void OnEnter() { }

    protected override void OnExecute()
    {
        idleTime += Time.deltaTime;

        if (idleTime > maxDuration) { Response.Result = Result.Success; }
    }

    protected override void OnExit() { }

    protected override void OnReset()
    {
        idleTime = 0;
    }

    protected override void OnFail() { }
}
