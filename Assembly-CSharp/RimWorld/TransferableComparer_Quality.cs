namespace RimWorld
{
	public class TransferableComparer_Quality : TransferableComparer
	{
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return this.GetValueFor(lhs).CompareTo(this.GetValueFor(rhs));
		}

		private int GetValueFor(Transferable t)
		{
			QualityCategory qualityCategory = default(QualityCategory);
			return t.AnyThing.TryGetQuality(out qualityCategory) ? ((int)qualityCategory) : (-1);
		}
	}
}
