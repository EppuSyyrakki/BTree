using UnityEngine;

namespace BTree
{
    public interface ITreeContext
    {
        GameObject gameObject { get; }
        Vector3 Position { get; }
    }
}
