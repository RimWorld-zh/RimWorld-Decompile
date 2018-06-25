using System;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D60 RID: 3424
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x04003330 RID: 13104
		private Sustainer sustainer;

		// Token: 0x04003331 RID: 13105
		private bool targetStartedDowned;

		// Token: 0x06004CC6 RID: 19654 RVA: 0x00280199 File Offset: 0x0027E599
		public Stance_Warmup()
		{
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x002801A4 File Offset: 0x0027E5A4
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

		// Token: 0x06004CC8 RID: 19656 RVA: 0x002802AF File Offset: 0x0027E6AF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x002802CC File Offset: 0x0027E6CC
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x00280320 File Offset: 0x0027E720
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

		// Token: 0x06004CCB RID: 19659 RVA: 0x0028044F File Offset: 0x0027E84F
		protected override void Expire()
		{
			this.verb.WarmupComplete();
			base.Expire();
		}
	}
}
