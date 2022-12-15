namespace BTree
{
    /// <summary>
    /// A wrapper interface to allow AgentTree to access Leaf inheritor without specifying its T.
    /// </summary>
    public interface ILeaf
    {
        void Enter(TreeAgent agent);
        void Execute();
        void Exit();
        Result Result { get; }
        void Fail();
    }
}