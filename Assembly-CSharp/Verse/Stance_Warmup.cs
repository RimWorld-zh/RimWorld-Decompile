using System;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D62 RID: 3426
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x06004CAF RID: 19631 RVA: 0x0027EADD File Offset: 0x0027CEDD
		public Stance_Warmup()
		{
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x0027EAE8 File Offset: 0x0027CEE8
		public Stance_Warmup(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
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
					info.pitchFactor = 1f / verb.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true);
				}
				this.sustainer = verb.verbProps.soundAiming.TrySpawnSustainer(info);
			}
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x0027EBF3 File Offset: 0x0027CFF3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x0027EC10 File Offset: 0x0027D010
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x0027EC64 File Offset: 0x0027D064
		public override void StanceTick()
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
			if (!this.targetStartedDowned)
			{
				if (this.focusTarg.HasThing && this.focusTarg.Thing is Pawn && ((Pawn)this.focusTarg.Thing).Downed)
				{
					this.stanceTracker.SetStance(new Stance_Mobile());
					return;
				}
			}
			if (this.focusTarg.HasThing)
			{
				if (!this.focusTarg.Thing.Spawned || !this.verb.CanHitTargetFrom(base.Pawn.Position, this.focusTarg))
				{
					this.stanceTracker.SetStance(new Stance_Mobile());
					return;
				}
			}
			if (this.focusTarg == base.Pawn.mindState.enemyTarget)
			{
				base.Pawn.mindState.Notify_EngagedTarget();
			}
			base.StanceTick();
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x0027ED93 File Offset: 0x0027D193
		protected override void Expire()
		{
			this.verb.WarmupComplete();
			base.Expire();
		}

		// Token: 0x04003327 RID: 13095
		private Sustainer sustainer;

		// Token: 0x04003328 RID: 13096
		private bool targetStartedDowned;
	}
}
