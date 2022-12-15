using UnityEngine;
using BTree;

public class Ball : MonoBehaviour, ITreeContext
{
    public bool Scored = false;

    public bool IsCloseTo(Player.Team team, float distance = 2f, Player except = null)
    {
        var colliders = Physics.OverlapSphere(transform.position, distance);

        foreach(var col in colliders)
        {
            if (col.TryGetComponent<Player>(out var player) && player.Side == team)
            {
                if (except != null && player == except)
                return true;
            }
        }

        return false;
    }
}
