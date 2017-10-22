using Verse;

namespace RimWorld
{
	public class CompUseEffect_StartWick : CompUseEffect
	{
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			base.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
