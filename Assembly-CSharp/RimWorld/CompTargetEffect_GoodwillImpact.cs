using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000755 RID: 1877
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600298D RID: 10637 RVA: 0x00161670 File Offset: 0x0015FA70
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00161690 File Offset: 0x0015FA90
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
