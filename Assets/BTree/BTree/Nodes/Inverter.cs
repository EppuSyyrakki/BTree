using UnityEngine;
using XNode;

namespace BTree
{
    public class Inverter : TreeNode
    {
        [SerializeField, Input(dynamicPortList: false, connectionType: ConnectionType.Override)]
        protected TreeResponse input;

        public override object GetValue(NodePort port)
        {
            var response = GetChildResponse();

            if (response == null) { return null; }

            if (response.Result == Result.Failure)
            {
                response.Result = Result.Success;
            }
            else if (response.Result == Result.Success)
            {
                response.Result = Result.Failure;
            }

            return response;
        }

        internal override void ResetNode()
        {
        }
    }
}