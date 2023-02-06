using UnityEngine;
using XNode;

namespace BTree
{
	/// <summary>
	/// Used in the editor to predefine the behaviour tree. Is copied as a scene asset to agents on Awake()
	/// </summary>
	[CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "BTree/New Behaviour Tree")]
	[RequireNode(typeof(Root))]
	public class TreeAsset : NodeGraph
	{
        public Root Root { get; private set; }

		internal void Initialize(TreeAgent agent)
		{
            Root = nodes.Find(x => x is Root) as Root;
            Root.RecursiveSetup(agent);
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

