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
	}
}

