namespace BTree
{
    /// <summary>
    /// A wrapper enabling access to any class inheriting Leaf without specifying its T.
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