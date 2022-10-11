using System.Collections;
using UnityEngine;

namespace BTree
{
    [RequireComponent(typeof(Tree))]
    public class TreeAgent : MonoBehaviour
    {
        [SerializeField] 
        private bool debugMessages = false;
        [SerializeField]
        private Blackboard blackboard;

        private Tree tree;
        private AgentAction current;
  
        protected virtual void Awake()
        {
            blackboard = new Blackboard();
            tree = GetComponent<Tree>();
            tree.PathChanged += SetCurrent;
            tree.Blackboard = blackboard;
        }

        protected virtual void Update()
        {
            if (current == null) 
            {
                ActionNode node = tree.GetActionNode();
                SetCurrent(node);
            }

            Result result = current.Execute();
            tree.SetActionNodeResult(result);
        }

        private void SetCurrent(ActionNode node)
        {
            if (current != null)
            { 
                current.OnExit();
            }

            current = node.GetAction();
            current.Initialize(this, node.MaxDuration);

            if (debugMessages) { Debug.Log($"{name} TreeAgent switched to {current}"); }
        }

        private void OnValidate()
        {
            blackboard.OnAgentValidate();
        }        
    }
}