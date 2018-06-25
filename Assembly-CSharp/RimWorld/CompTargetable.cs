using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public abstract class CompTargetable : CompUseEffect
	{
		private Thing target;

		protected CompTargetable()
		{
		}

		private CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		protected abstract bool PlayerChoosesTarget { get; }

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

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

		protected abstract TargetingParameters GetTargetingParameters();

		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

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

		[CompilerGenerated]
		private sealed class <SelectedUseOption>c__AnonStorey0
		{
			internal Pawn p;

			internal CompTargetable $this;

			public <SelectedUseOption>c__AnonStorey0()
			{
			}

			internal void <>m__0(LocalTargetInfo t)
			{
				this.$this.target = t.Thing;
				this.$this.parent.GetComp<CompUsable>().TryStartUseJob(this.p);
			}
		}
	}
}
