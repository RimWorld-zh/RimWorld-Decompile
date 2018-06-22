using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000758 RID: 1880
	public abstract class CompTargetable : CompUseEffect
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x00161830 File Offset: 0x0015FC30
		private CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002995 RID: 10645
		protected abstract bool PlayerChoosesTarget { get; }

		// Token: 0x06002996 RID: 10646 RVA: 0x00161850 File Offset: 0x0015FC50
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x0016186C File Offset: 0x0015FC6C
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

		// Token: 0x06002998 RID: 10648 RVA: 0x001618D8 File Offset: 0x0015FCD8
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

		// Token: 0x06002999 RID: 10649
		protected abstract TargetingParameters GetTargetingParameters();

		// Token: 0x0600299A RID: 10650
		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		// Token: 0x0600299B RID: 10651 RVA: 0x001619DC File Offset: 0x0015FDDC
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

		// Token: 0x04001699 RID: 5785
		private Thing target;
	}
}
