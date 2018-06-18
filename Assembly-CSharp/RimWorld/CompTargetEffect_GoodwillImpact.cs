using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000757 RID: 1879
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002990 RID: 10640 RVA: 0x00161348 File Offset: 0x0015F748
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x00161368 File Offset: 0x0015F768
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (user.Faction != null && target.Faction != null)
			{
				Faction faction = target.Faction;
				Faction faction2 = user.Faction;
				int goodwillImpact = this.PropsGoodwillImpact.goodwillImpact;
				string reason = "GoodwillChangedReason_UsedItem".Translate(new object[]
				{
					this.parent.LabelShort,
					target.LabelShort
				});
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(target);
				faction.TryAffectGoodwillWith(faction2, goodwillImpact, true, true, reason, lookTarget);
			}
		}
	}
}
