using System;

namespace RimWorld
{
	// Token: 0x020008B1 RID: 2225
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x060032E0 RID: 13024 RVA: 0x001B6530 File Offset: 0x001B4930
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true));
		}
	}
}
