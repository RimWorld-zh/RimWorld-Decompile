using System;
using System.Collections.Generic;

namespace RimWorld
{
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		protected TransferableComparer()
		{
		}

		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
