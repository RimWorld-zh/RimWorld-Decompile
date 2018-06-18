using System;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D61 RID: 3425
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x06004CAD RID: 19629 RVA: 0x0027EABD File Offset: 0x0027CEBD
		public Stance_Warmup()
		{
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x0027EAC8 File Offset: 0x0027CEC8
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

		// Token: 0x06004CAF RID: 19631 RVA: 0x0027EBD3 File Offset: 0x0027CFD3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x0027EBF0 File Offset: 0x0027CFF0
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x0027EC44 File Offset: 0x0027D044
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

		// Token: 0x06004CB2 RID: 19634 RVA: 0x0027ED73 File Offset: 0x0027D173
		protected override void Expire()
		{
			this.verb.WarmupComplete();
			base.Expire();
		}

		// Token: 0x04003325 RID: 13093
		private Sustainer sustainer;

		// Token: 0x04003326 RID: 13094
		private bool targetStartedDowned;
	}
}
