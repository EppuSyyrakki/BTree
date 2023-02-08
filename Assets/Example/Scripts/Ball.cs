using BTree;
using UnityEngine;

public class Ball : MonoBehaviour, ITreeContext
{
    public bool Scored = false;

    public Vector3 Position
    {
        get 
        {
            var p = transform.position;
            return new Vector3(p.x, p.y > 1f ? 0.5f : 0, p.z);
        }        
    }

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
