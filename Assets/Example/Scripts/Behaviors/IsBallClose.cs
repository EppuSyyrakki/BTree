using BTree;
using UnityEngine;
using XNode;

/// <summary>
/// Example of using the GetValue method to check something in the game world while the Evaluate function is being
/// run inside the Tree.
/// </summary>
public class IsBallClose : Leaf<NoContext>
{
    [SerializeField]
    private float distance = 3f;

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override void ResetLeaf()
    {
    }

    public override object GetValue(NodePort port)
    {
        if (tree == null) { return null; }

        var player = tree.agent as Player;
        var result = new TreeResult(this, Result.Failure);

        if (Vector3.Distance(player.Ball.transform.position, player.transform.position) < distance)
        {
            // Debug.Log(player.gameObject.name + " found a teammate near the ball");
            result.Value = Result.Success;
        }

        return result;    
    }
}
