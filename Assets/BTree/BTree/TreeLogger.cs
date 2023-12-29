using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BTree
{
    [Serializable]
    public class TreeLogger
    {
        [SerializeField]
        private bool enable = false;

        [SerializeField]
        private Logging logging;

        public void Log<T>(T source, string msg)
        {
            if (!enable) { return; }
        }

        [Flags]
        private enum Logging
        {
            LeafOperations,
            BranchOperations,

        }
    }
}
