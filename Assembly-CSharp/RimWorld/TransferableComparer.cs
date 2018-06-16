using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020002EB RID: 747
	public abstract class TransferableComparer : IComparer<Transferable>
	{
		// Token: 0x06000C56 RID: 3158
		public abstract int Compare(Transferable lhs, Transferable rhs);
	}
}
