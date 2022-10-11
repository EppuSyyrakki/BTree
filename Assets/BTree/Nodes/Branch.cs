namespace BTree
{
	/// <summary>
	/// The abstract base type for behaviour tree composite and decorator nodes. Has a reference to character
	/// To have access to Memory.
	/// </summary>
	[NodeTint(0.15f, 0.15f, 0.175f)]
	public abstract class Branch : TreeNode
	{
		[Input(dynamicPortList: true, connectionType = ConnectionType.Override)]
		public TreeResult childPort;
	}
}