using UnityEngine;
using XNode;

namespace BTree
{
    public class Interrupt : TreeNode
    {
        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        protected TreeResponse input;

        [SerializeField, Tooltip("Identifier through which this interruption is called from the TreeAgent.")]
        private string id;

        [SerializeField, Tooltip("Reset the whole tree after this interruption is called.")]
        private bool forceReset = false;

        [SerializeField, Tooltip("Adds a context from the interruption call with this name.")]
        private string outContext;

        [SerializeField, Tooltip("")]
        private bool overwriteOut;

        internal string Id => id;
        internal bool ForceReset => forceReset;
        internal string OutContext => outContext;
        internal bool OverwriteOut => overwriteOut;

        internal TreeResponse Response => GetChildResponse();

        public override object GetValue(NodePort port)
        {
            return GetChildResponse();
        }
    }
}
