using System;

namespace RimWorld
{
	// Token: 0x020008AF RID: 2223
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x060032DD RID: 13021 RVA: 0x001B6858 File Offset: 0x001B4C58
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true));
		}
	}
}
