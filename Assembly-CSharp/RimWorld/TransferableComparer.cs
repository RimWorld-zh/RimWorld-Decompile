using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020002ED RID: 749
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		// Token: 0x06000C59 RID: 3161
		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
