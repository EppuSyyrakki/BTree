using System.Collections;
using UnityEngine;

namespace BTree
{
    [RequireComponent(typeof(Tree))]
    public class TreeAgent : MonoBehaviour, ITreeContext
    {
        private Tree tree;
        private ILeaf current;
 
        protected virtual void Awake()
        {
            tree = GetComponent<Tree>();
        }

        protected virtual void Update()
        {
            if (current == null) 
            {
                if (tree.Evaluate(out current))
                {
                    current.Enter(this);
                }
                else
                {
                    return;
                }
            }

            current.Execute();

            if (current.Result == Result.Running) { return; }
            
            current.Exit();

            if (tree.Evaluate(out ILeaf next))
            {                   
                next.Enter(this);
                current = next;
            }
            else
            {
                tree.Restart();
                current = null;
            }            
        }

        /// <summary>
        /// This can be used to force removal of a context from the Tree. Useful when a context object is destroyed 
        /// for example. In this case it could be compared with the Context property to see 
        /// </summary>
        public void RemoveContext(ITreeContext remove)
        {
            tree.RemoveContext(remove);
        }
    }
}