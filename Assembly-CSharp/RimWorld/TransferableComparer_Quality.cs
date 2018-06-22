using System;

namespace RimWorld
{
	// Token: 0x020008B0 RID: 2224
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x060032DF RID: 13023 RVA: 0x001B67B0 File Offset: 0x001B4BB0
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x001B67DC File Offset: 0x001B4BDC
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
