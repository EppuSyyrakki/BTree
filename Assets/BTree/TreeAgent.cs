using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTree
{
    [RequireComponent(typeof(Tree))]
    public class TreeAgent : MonoBehaviour, ITreeContext
    {
        private Tree tree;
        private TreeResponse current;

        public bool Reset { get; internal set; }

        protected virtual void Awake()
        {
            tree = GetComponent<Tree>();
        }

        protected virtual void Update()
        {
            if (current == null || Reset)
            {
                tree.Evaluate(out current);
                current.Origin.Enter(this);
            }

            if (current.CheckConditions())
            {
                current.Origin.Execute();
            }           

            if (current.Origin.Result == Result.Running) { return; }
            
            current.Origin.Exit();

            if (tree.Evaluate(out TreeResponse next))
            {                   
                next.Origin.Enter(this);
                current = next;
            }
            else
            {
                tree.Restart();
                current = null;
            }            
        }

        /// <summary>
        /// This can be used to force removal of a context from the Tree. Useful when a context object is destroyed.
        /// </summary>
        public void RemoveContext(ITreeContext remove)
        {
            tree.RemoveContext(remove);
        }
    }
}