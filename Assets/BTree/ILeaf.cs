namespace BTree
{
    /// <summary>
    /// A wrapper interface to allow AgentTree to access Leaf inheritor without specifying its T.
    /// </summary>
    public interface ILeaf
    {
        void Enter();
        void Execute();
        void Exit();
        TreeResponse Response { get; }
        void Fail();
    }
}