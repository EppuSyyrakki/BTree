using UnityEngine;
using XNode;

namespace BTree
{
    /// <summary>
    /// Chooses a child that is able to run and returns its result. If all children fail, returns failure.
    /// </summary>
    public class Selector : Branch
    {
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

            if (response.Result == Result.Success)
            {
                // Only allow a single success to return.
                storedResponse = response;
                return storedResponse;
            }

            var resolved = Resolve(response);
            return ResolveConditions(resolved);
        }

        private TreeResponse Resolve(TreeResponse response)
        {
            if (response.Result == Result.Running) // || response.Result == Result.Waiting)
            {
                // If child is running or waiting, send it on as is.
                return response;
            }

            if (response.Result == Result.Failure)
            {
                if (HasNextChild)
                {
                    // Child failed but there is more children to go through so check them.
                    index++;
                    response = GetChildResponseAtIndex(index);
                    return Resolve(response);
                }
            }

            return response;
        }
    }
}