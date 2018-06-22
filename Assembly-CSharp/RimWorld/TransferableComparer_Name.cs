using System;

namespace RimWorld
{
	// Token: 0x020008AE RID: 2222
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x060032DB RID: 13019 RVA: 0x001B6760 File Offset: 0x001B4B60
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
