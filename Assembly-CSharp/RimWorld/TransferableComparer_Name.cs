using System;

namespace RimWorld
{
	// Token: 0x020008B2 RID: 2226
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x060032E0 RID: 13024 RVA: 0x001B64B0 File Offset: 0x001B48B0
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
