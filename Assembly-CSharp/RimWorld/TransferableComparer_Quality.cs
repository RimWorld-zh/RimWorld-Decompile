using System;

namespace RimWorld
{
	// Token: 0x020008B4 RID: 2228
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x060032E6 RID: 13030 RVA: 0x001B65C8 File Offset: 0x001B49C8
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x001B65F4 File Offset: 0x001B49F4
		private int GetValueFor(Transferable t)
		{
			QualityCategory qualityCategory;
			int result;
			if (!t.AnyThing.TryGetQuality(out qualityCategory))
			{
				result = -1;
			}
			else
			{
				result = (int)qualityCategory;
			}
			return result;
		}
	}
}
