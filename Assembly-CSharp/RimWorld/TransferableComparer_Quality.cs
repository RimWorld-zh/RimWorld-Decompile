using System;

namespace RimWorld
{
	// Token: 0x020008B2 RID: 2226
	public class TransferableComparer_Quality : TransferableComparer
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x001B6BC4 File Offset: 0x001B4FC4
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x001B6BF0 File Offset: 0x001B4FF0
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
