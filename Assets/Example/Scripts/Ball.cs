using BTree;
using UnityEngine;

public class Ball : MonoBehaviour, ITreeContext
{
    public bool Scored = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 Position
    {
        get
        {
            var p = transform.position;
            return new Vector3(p.x, p.y > 1f ? 0.5f : 0, p.z);
        }
    }

    public Vector3 Estimate
    {
        get
        {
            return Position + rb.velocity * rb.mass / rb.drag;
        }
    }
}
