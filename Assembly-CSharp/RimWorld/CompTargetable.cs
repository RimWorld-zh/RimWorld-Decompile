using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075C RID: 1884
	public abstract class CompTargetable : CompUseEffect
	{
		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002999 RID: 10649 RVA: 0x001615C4 File Offset: 0x0015F9C4
		private CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x0600299A RID: 10650
		protected abstract bool PlayerChoosesTarget { get; }

		// Token: 0x0600299B RID: 10651 RVA: 0x001615E4 File Offset: 0x0015F9E4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x00161600 File Offset: 0x0015FA00
		public override bool SelectedUseOption(Pawn p)
		{
			bool result;
			if (this.PlayerChoosesTarget)
			{
				Find.Targeter.BeginTargeting(this.GetTargetingParameters(), delegate(LocalTargetInfo t)
				{
					this.target = t.Thing;
					this.parent.GetComp<CompUsable>().TryStartUseJob(p);
				}, p, null, null);
				result = true;
			}
			else
			{
				this.target = null;
				result = false;
			}
			return result;
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0016166C File Offset: 0x0015FA6C
		public override void DoEffect(Pawn usedBy)
		{
			if (!this.PlayerChoosesTarget || this.target != null)
			{
				if (this.target == null || this.GetTargetingParameters().CanTarget(this.target))
				{
					base.DoEffect(usedBy);
					foreach (Thing thing in this.GetTargets(this.target))
					{
						foreach (CompTargetEffect compTargetEffect in this.parent.GetComps<CompTargetEffect>())
						{
							compTargetEffect.DoEffectOn(usedBy, thing);
						}
					}
					this.target = null;
				}
			}
		}

		// Token: 0x0600299E RID: 10654
		protected abstract TargetingParameters GetTargetingParameters();

		// Token: 0x0600299F RID: 10655
		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		// Token: 0x060029A0 RID: 10656 RVA: 0x00161770 File Offset: 0x0015FB70
		public bool BaseTargetValidator(Thing t)
		{
			if (this.Props.psychicSensitiveTargetsOnly)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) <= 0f)
				{
					return false;
				}
			}
			if (this.Props.fleshCorpsesOnly)
			{
				Corpse corpse = t as Corpse;
				if (corpse != null && !corpse.InnerPawn.RaceProps.IsFlesh)
				{
					return false;
				}
			}
			if (this.Props.nonDessicatedCorpsesOnly)
			{
				Corpse corpse2 = t as Corpse;
				if (corpse2 != null && corpse2.GetRotStage() == RotStage.Dessicated)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400169B RID: 5787
		private Thing target;
	}
}
