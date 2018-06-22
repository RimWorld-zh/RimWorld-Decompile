using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000685 RID: 1669
	public abstract class Building_Turret : Building, IAttackTarget, IAttackTargetSearcher, ILoadReferenceable
	{
		// Token: 0x06002334 RID: 9012 RVA: 0x0012EDEF File Offset: 0x0012D1EF
		public Building_Turret()
		{
			this.stunner = new StunHandler(this);
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002335 RID: 9013
		public abstract LocalTargetInfo CurrentTarget { get; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06002336 RID: 9014
		public abstract Verb AttackVerb { get; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06002337 RID: 9015 RVA: 0x0012EE10 File Offset: 0x0012D210
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x0012EE28 File Offset: 0x0012D228
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x0012EE44 File Offset: 0x0012D244
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x0012EE5C File Offset: 0x0012D25C
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x0012EE78 File Offset: 0x0012D278
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600233C RID: 9020 RVA: 0x0012EE94 File Offset: 0x0012D294
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x0012EEB0 File Offset: 0x0012D2B0
		public override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
			this.stunner.StunHandlerTick();
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0012EF28 File Offset: 0x0012D328
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_TargetInfo.Look(ref this.forcedTarget, "forcedTarget");
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0012EF88 File Offset: 0x0012D388
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed)
			{
				this.stunner.Notify_DamageApplied(dinfo, true);
				absorbed = false;
			}
		}

		// Token: 0x06002340 RID: 9024
		public abstract void OrderAttack(LocalTargetInfo targ);

		// Token: 0x06002341 RID: 9025 RVA: 0x0012EFB4 File Offset: 0x0012D3B4
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			CompPowerTrader comp = base.GetComp<CompPowerTrader>();
			bool result;
			if (comp != null && !comp.PowerOn)
			{
				result = true;
			}
			else
			{
				CompMannable comp2 = base.GetComp<CompMannable>();
				result = (comp2 != null && !comp2.MannedNow);
			}
			return result;
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0012F008 File Offset: 0x0012D408
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x040013C8 RID: 5064
		protected StunHandler stunner;

		// Token: 0x040013C9 RID: 5065
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		// Token: 0x040013CA RID: 5066
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x040013CB RID: 5067
		private int lastAttackTargetTick;

		// Token: 0x040013CC RID: 5068
		private const float SightRadiusTurret = 13.4f;
	}
}
