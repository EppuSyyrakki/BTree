﻿using UnityEngine;
using XNode;

namespace BTree.Nodes
{
	/// <summary>
	/// Chooses a child that is able to run and returns its result. If all children fail, returns failure.
	/// </summary>
	public class Selector : Branch
	{
		private int _index;

		private bool HasNextChild
		{
			get
			{
				if (children == null) { return false; }
				return _index < children.Length;
			}
		}

		internal override void ResetNode()
		{
			_index = 0;
		}

		public override object GetValue(NodePort port)
		{
			while (HasNextChild)
			{
				var result = GetChildResultAtIndex(_index);

				if (result.Value == Result.Running || result.Value == Result.Success)
				{
					return result;
				}

				_index++;
			}

			return new TreeResult(this, Result.Failure);
		}
	}
}