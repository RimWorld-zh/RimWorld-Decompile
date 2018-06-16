using System;

namespace RimWorld
{
	// Token: 0x020008B1 RID: 2225
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x060032DE RID: 13022 RVA: 0x001B6468 File Offset: 0x001B4868
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true));
		}
	}
}
