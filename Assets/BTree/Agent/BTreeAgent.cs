using System.Collections;
using UnityEngine;

namespace BTree.Agent
{
    [RequireComponent(typeof(SceneTree))]
    public class BTreeAgent : MonoBehaviour
    {             
        [SerializeField]
        private Blackboard blackboard;

        public Blackboard Blackboard => blackboard;

        private void Awake()
        {
            blackboard = new Blackboard();
        }

        private void OnValidate()
        {
            blackboard.OnAgentValidate();
        }
    }
}