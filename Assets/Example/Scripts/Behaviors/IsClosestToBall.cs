using BTree;
using UnityEngine;
using XNode;

/// <summary>
/// Example of using the GetValue method to check something in the game world while the Evaluate function is being
/// run inside the Tree.
/// </summary>
public class IsClosestToBall : Leaf<NoContext>
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

        if (IsClosest(player))
        {
            result.Value = Result.Success;
        }

        return result;
    }

    private static bool IsClosest(Player player)
    {
        if (player.Ball == null) { return false; }

        Vector3 ball = player.Ball.transform.position;
        float self = (player.transform.position - ball).sqrMagnitude;
        float closest = float.MaxValue;

        foreach (var p in player.TeamMates)
        {
            float friend = (p.transform.position - ball).sqrMagnitude;

            if (friend < closest)
            {
                closest = friend;
            }
        }
        
        if (self < closest)
        {
            return true;
        }

        return false;
    }
}
