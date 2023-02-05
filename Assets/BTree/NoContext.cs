using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    /// <summary>
    /// A helper context type that can be used as a "null" generic parameter for Leaf classes that don't require
    /// a context.
    /// </summary>
    public class NoContext : ITreeContext
    {
        public Transform transform => throw new System.NotImplementedException();

        public GameObject gameObject => throw new System.NotImplementedException();

        public List<TreeAgent> References => throw new System.NotImplementedException();
    }
}