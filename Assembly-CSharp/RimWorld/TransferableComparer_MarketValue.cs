using System;

namespace RimWorld
{
	// Token: 0x020008AC RID: 2220
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x060032D7 RID: 13015 RVA: 0x001B66DC File Offset: 0x001B4ADC
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
