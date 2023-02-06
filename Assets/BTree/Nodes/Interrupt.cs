using UnityEngine;
using XNode;

namespace BTree
{
    public class Interrupt : TreeNode
    {
        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        protected TreeResponse input;

        [SerializeField]
        private string id;

        internal string Id => id;

        public override object GetValue(NodePort port)
        {
            return GetChildResponse();
        }
    }
}
