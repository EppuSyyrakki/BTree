using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace BTree
{
    /// <summary>
    /// Loops through child nodes. If any child fails, returns failure. Returns success after all nodes
    /// succeeded and has repeated given times.
    /// </summary>
    public class Sequence : Branch
    {
        [SerializeField]
        private bool haltOnFailure = true;

        [SerializeField, Input(dynamicPortList: true, connectionType: ConnectionType.Override)]
        protected TreeResponse input;

        private int index;
        private bool HasNextChild => index + 1 < children.Length;

        internal override void ResetNode()
        {
            index = 0;
            storedResponse = null;
        }

        public override object GetValue(NodePort port)
        {
            if (storedResponse != null) { return storedResponse; }

            index = 0;
            var response = GetChildResponseAtIndex(index);
            var resolved = Resolve(response);
            return ResolveConditions(resolved);
        }

        private TreeResponse Resolve(TreeResponse response)
        {
            if (response.Result == Result.Running) // || response.Result == Result.Waiting)
            {
                // If child is running, send it on as is.
                return response;
            }

            if (response.Result == Result.Success
                || (!haltOnFailure && response.Result == Result.Failure))
            {
                if (HasNextChild)
                {
                    // Child succeeded but there is more children to go through so check them.
                    index++;
                    response = GetChildResponseAtIndex(index);
                    return Resolve(response);
                }

                // This is the last child, set success.
                response.Result = Result.Success;
            }

            return response;
        }
    }
}