using System;

namespace RimWorld
{
	// Token: 0x020008B2 RID: 2226
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x060032E2 RID: 13026 RVA: 0x001B6578 File Offset: 0x001B4978
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
