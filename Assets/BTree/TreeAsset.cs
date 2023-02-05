using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// Used in the editor to predefine the behaviour tree. Is copied as a scene asset to agents on Awake()
	/// </summary>
	[CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "BTree/New Behaviour Tree")]
	[RequireNode(typeof(RootNode))]
	public class TreeAsset : NodeGraph
	{
        public RootNode Root { get; private set; }

		internal void Initialize(TreeAgent agent)
		{
            foreach (var n in nodes)
            {
                var tn = n as TreeNode;

                if (tn == null)
                {
                    Debug.LogError(agent + " has non-TreeNode in their tree.");
                    return;
                }

                if (tn is RootNode rootNode)
                {
                    Root = rootNode;
                }

                tn.Setup(agent);
            }

            return;
        }

        internal void ResetNodes()
        {
            foreach (var n in nodes)
            {
                var tn = n as TreeNode;
                tn.ResetNode();
            }
        }
    }
}

