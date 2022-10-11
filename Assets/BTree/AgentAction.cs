using System.Collections;
using UnityEngine;

namespace BTree
{
    /// <summary>
    /// Base class for the actions. BTreeAgent will run these according to the logic in the agent's tree asset.
    /// </summary>
    public abstract class AgentAction
    {
        protected float maxDuration = 0;
        protected bool log = false;
        protected float elapsed = 0f;

        /// <summary> Reference to the agent running this action </summary>
        public TreeAgent Agent { get; private set; }

        public virtual void Initialize(TreeAgent agent, float maxDuration, bool log = false)
        {
            Agent = agent;
            this.maxDuration = maxDuration > 0 ? maxDuration : float.MaxValue;
            this.log = log;
        }

        public virtual Result Execute()
        {
            elapsed += Time.deltaTime;

            if (elapsed > maxDuration)
            {
                return Result.Success;
            }

            return Result.Running;
        }

        public virtual void OnExit()
        {
            Agent = null;
        }
    }
}