using Verse;

namespace RimWorld
{
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			base.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
