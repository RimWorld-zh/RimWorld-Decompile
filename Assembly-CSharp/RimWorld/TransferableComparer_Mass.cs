using System;

namespace RimWorld
{
	// Token: 0x020008AD RID: 2221
	public class TransferableComparer_Mass : TransferableComparer
	{
		// Token: 0x060032D9 RID: 13017 RVA: 0x001B6718 File Offset: 0x001B4B18
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.GetStatValue(StatDefOf.Mass, true).CompareTo(rhs.AnyThing.GetStatValue(StatDefOf.Mass, true));
		}
	}
}
