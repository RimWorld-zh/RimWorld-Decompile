using RimWorld;
using Verse.Sound;

namespace Verse
{
	public class Stance_Warmup : Stance_Busy
	{
		private Sustainer sustainer;

		private bool targetStartedDowned;

		public Stance_Warmup()
		{
		}

		public Stance_Warmup(int ticks, LocalTargetInfo focusTarg, Verb verb)
			: base(ticks, focusTarg, verb)
		{
			if (focusTarg.HasThing && focusTarg.Thing is Pawn)
			{
				Pawn pawn = (Pawn)focusTarg.Thing;
				this.targetStartedDowned = pawn.Downed;
				if (pawn.apparel != null)
				{
					for (int i = 0; i < pawn.apparel.WornApparelCount; i++)
					{
						Apparel apparel = pawn.apparel.WornApparel[i];
						ShieldBelt shieldBelt = apparel as ShieldBelt;
						if (shieldBelt != null)
						{
							shieldBelt.KeepDisplaying();
						}
					}
				}
			}
			if (verb != null && verb.verbProps.soundAiming != null)
			{
				SoundInfo info = SoundInfo.InMap(verb.caster, MaintenanceType.PerTick);
				if (verb.CasterIsPawn)
				{
					info.pitchFactor = (float)(1.0 / verb.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true));
				}
				this.sustainer = verb.verbProps.soundAiming.TrySpawnSustainer(info);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
		}

		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(base.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(base.stanceTracker.pawn, base.focusTarg, (int)((float)base.ticksLeft * base.pieSizeFactor), 0.2f);
			}
		}

		public override void StanceTick()
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
			if (!this.targetStartedDowned && base.focusTarg.HasThing && base.focusTarg.Thing is Pawn && ((Pawn)base.focusTarg.Thing).Downed)
			{
				base.stanceTracker.SetStance(new Stance_Mobile());
			}
			else if (base.focusTarg.HasThing && (!base.focusTarg.Thing.Spawned || !base.verb.CanHitTargetFrom(base.Pawn.Position, base.focusTarg)))
			{
				base.stanceTracker.SetStance(new Stance_Mobile());
			}
			else
			{
				if (base.focusTarg == (LocalTargetInfo)base.Pawn.mindState.enemyTarget)
				{
					base.Pawn.mindState.Notify_EngagedTarget();
				}
				base.StanceTick();
			}
		}

		protected override void Expire()
		{
			base.verb.WarmupComplete();
			base.Expire();
		}
	}
}
