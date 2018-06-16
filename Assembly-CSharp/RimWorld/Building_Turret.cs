using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000689 RID: 1673
	public abstract class Building_Turret : Building, IAttackTarget, IAttackTargetSearcher, ILoadReferenceable
	{
		// Token: 0x0600233A RID: 9018 RVA: 0x0012EC2F File Offset: 0x0012D02F
		public Building_Turret()
		{
			this.stunner = new StunHandler(this);
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600233B RID: 9019
		public abstract LocalTargetInfo CurrentTarget { get; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x0600233C RID: 9020
		public abstract Verb AttackVerb { get; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600233D RID: 9021 RVA: 0x0012EC50 File Offset: 0x0012D050
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x0012EC68 File Offset: 0x0012D068
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x0012EC84 File Offset: 0x0012D084
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x0012EC9C File Offset: 0x0012D09C
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x0012ECB8 File Offset: 0x0012D0B8
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x0012ECD4 File Offset: 0x0012D0D4
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0012ECF0 File Offset: 0x0012D0F0
		public override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
			this.stunner.StunHandlerTick();
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0012ED68 File Offset: 0x0012D168
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

		// Token: 0x06002345 RID: 9029 RVA: 0x0012EDC8 File Offset: 0x0012D1C8
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed)
			{
				this.stunner.Notify_DamageApplied(dinfo, true);
				absorbed = false;
			}
		}

		// Token: 0x06002346 RID: 9030
		public abstract void OrderAttack(LocalTargetInfo targ);

		// Token: 0x06002347 RID: 9031 RVA: 0x0012EDF4 File Offset: 0x0012D1F4
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

		// Token: 0x06002348 RID: 9032 RVA: 0x0012EE48 File Offset: 0x0012D248
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x040013CA RID: 5066
		protected StunHandler stunner;

		// Token: 0x040013CB RID: 5067
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		// Token: 0x040013CC RID: 5068
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x040013CD RID: 5069
		private int lastAttackTargetTick;

		// Token: 0x040013CE RID: 5070
		private const float SightRadiusTurret = 13.4f;
	}
}
