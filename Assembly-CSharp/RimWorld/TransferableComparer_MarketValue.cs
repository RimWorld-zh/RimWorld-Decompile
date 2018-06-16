using System;

namespace RimWorld
{
	// Token: 0x020008B0 RID: 2224
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x060032DC RID: 13020 RVA: 0x001B642C File Offset: 0x001B482C
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
