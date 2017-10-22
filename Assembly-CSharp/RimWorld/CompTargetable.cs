using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public abstract class CompTargetable : CompUseEffect
	{
		private Thing target;

		private CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)base.props;
			}
		}

		protected abstract bool PlayerChoosesTarget
		{
			get;
		}

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
				Find.Targeter.BeginTargeting(this.GetTargetingParameters(), (Action<LocalTargetInfo>)delegate(LocalTargetInfo t)
				{
					this.target = t.Thing;
					base.parent.GetComp<CompUsable>().TryStartUseJob(p);
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
			if (this.PlayerChoosesTarget && this.target == null)
				return;
			if (this.target != null && !this.GetTargetingParameters().CanTarget(this.target))
				return;
			base.DoEffect(usedBy);
			foreach (Thing target2 in this.GetTargets(this.target))
			{
				foreach (CompTargetEffect comp in base.parent.GetComps<CompTargetEffect>())
				{
					comp.DoEffectOn(usedBy, target2);
				}
			}
			this.target = null;
		}

		protected abstract TargetingParameters GetTargetingParameters();

		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		public bool BaseTargetValidator(Thing t)
		{
			bool result;
			if (this.Props.psychicSensitiveTargetsOnly)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) <= 0.0)
				{
					result = false;
					goto IL_007f;
				}
			}
			if (this.Props.fleshCorpsesOnly)
			{
				Corpse corpse = t as Corpse;
				if (corpse != null && !corpse.InnerPawn.RaceProps.IsFlesh)
				{
					result = false;
					goto IL_007f;
				}
			}
			result = true;
			goto IL_007f;
			IL_007f:
			return result;
		}
	}
}
