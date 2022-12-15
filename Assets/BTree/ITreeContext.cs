using System.Collections;
using UnityEngine;

namespace BTree
{
    public interface ITreeContext
    {
        Transform transform { get; }
        GameObject gameObject { get; }
    }
}