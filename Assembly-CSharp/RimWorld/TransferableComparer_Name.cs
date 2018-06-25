using System;

namespace RimWorld
{
	// Token: 0x020008B0 RID: 2224
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x060032DF RID: 13023 RVA: 0x001B68A0 File Offset: 0x001B4CA0
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
