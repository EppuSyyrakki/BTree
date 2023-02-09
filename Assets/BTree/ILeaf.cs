using System;

namespace BTree
{
    /// <summary>
    /// A wrapper enabling access to any class inheriting Leaf without specifying its T. In the basic implementation
    /// is used through Update of the TreeAgent class.
    /// </summary>
    public interface ILeaf
    {
        /// <summary>
        /// Called on the same frame 
        /// </summary>
        void Enter();
        void Execute();
        void Exit();
        TreeResponse Response { get; }
        void Fail();
        event Func<bool> OnExceptionFail;
    }
}