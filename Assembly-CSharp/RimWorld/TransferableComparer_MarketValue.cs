using System;

namespace RimWorld
{
	// Token: 0x020008B0 RID: 2224
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x060032DE RID: 13022 RVA: 0x001B64F4 File Offset: 0x001B48F4
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
