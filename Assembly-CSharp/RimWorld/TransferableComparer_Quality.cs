using System;

namespace RimWorld
{
	public class TransferableComparer_Quality : TransferableComparer
	{
		public TransferableComparer_Quality()
		{
		}

		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

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
