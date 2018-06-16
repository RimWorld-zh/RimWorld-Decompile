using System;

namespace RimWorld
{
	// Token: 0x020008B4 RID: 2228
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x060032E4 RID: 13028 RVA: 0x001B6500 File Offset: 0x001B4900
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x001B652C File Offset: 0x001B492C
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
