using BTree;
using UnityEngine;
using XNode;

/// <summary>
/// Example of using the GetValue method to check something in the game world while the Evaluate function is being
/// run inside the Tree.
/// </summary>
public class IsBallControlled : Leaf<ITreeContext>
{
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
        var player = tree.agent as Player;
        var result = new TreeResult(this, Result.Failure);

        if (IsFriendNearBall(player))
        {
            result.Value = Result.Success;
        }

        return result;
    }

    private static bool IsFriendNearBall(Player player)
    {
        if (player.Ball == null) { return false; }

        var colliders = Physics.OverlapSphere(player.transform.position, 2f);

        foreach (var col in colliders)
        {            
            if (col.TryGetComponent<Player>(out var found)
                && found != player
                && found.Side == player.Side)
            {
                 return true;            
            }
        }

        return false;
    }
}
