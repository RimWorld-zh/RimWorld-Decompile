using System;

namespace RimWorld
{
	// Token: 0x020008AE RID: 2222
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x060032DB RID: 13019 RVA: 0x001B6AF0 File Offset: 0x001B4EF0
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
