using BTree;
using UnityEngine;
using XNode;

/// <summary>
/// Example of using the GetValue method to check something in the game world while the Evaluate function is being
/// run inside the Tree.
/// </summary>
public class IsFriendMovingToBall : Leaf<NoContext>
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
        if (tree == null) { return null; }

        var player = tree.agent as Player;

        foreach (var friend in player.TeamMates)
        {
            if (friend.MovingToBall)
            {
                return new TreeResult(this, Result.Success);
            }
        }

        return new TreeResult(this, Result.Failure);
    }
}
