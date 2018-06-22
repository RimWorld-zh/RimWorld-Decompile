using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006DD RID: 1757
	[StaticConstructorOnStartup]
	public class ShieldBelt : Apparel
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x00147E24 File Offset: 0x00146224
		private float EnergyMax
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true);
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x00147E48 File Offset: 0x00146248
		private float EnergyGainPerTick
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true) / 60f;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x00147E70 File Offset: 0x00146270
		public float Energy
		{
			get
			{
				return this.energy;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x00147E8C File Offset: 0x0014628C
		public ShieldState ShieldState
		{
			get
			{
				ShieldState result;
				if (this.ticksToReset > 0)
				{
					result = ShieldState.Resetting;
				}
				else
				{
					result = ShieldState.Active;
				}
				return result;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x00147EB8 File Offset: 0x001462B8
		private bool ShouldDisplay
		{
			get
			{
				Pawn wearer = base.Wearer;
				return wearer.Spawned && !wearer.Dead && !wearer.Downed && (wearer.InAggroMentalState || wearer.Drafted || (wearer.Faction.HostileTo(Faction.OfPlayer) && !wearer.IsPrisoner) || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks);
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x00147F6C File Offset: 0x0014636C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.energy, "energy", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1, false);
			Scribe_Values.Look<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x00147FBC File Offset: 0x001463BC
		public override IEnumerable<Gizmo> GetWornGizmos()
		{
			if (Find.Selector.SingleSelectedThing == base.Wearer)
			{
				yield return new Gizmo_EnergyShieldStatus
				{
					shield = this
				};
			}
			yield break;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x00147FE8 File Offset: 0x001463E8
		public override float GetSpecialApparelScoreOffset()
		{
			return this.EnergyMax * this.ApparelScorePerEnergyMax;
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0014800C File Offset: 0x0014640C
		public override void Tick()
		{
			base.Tick();
			if (base.Wearer == null)
			{
				this.energy = 0f;
			}
			else if (this.ShieldState == ShieldState.Resetting)
			{
				this.ticksToReset--;
				if (this.ticksToReset <= 0)
				{
					this.Reset();
				}
			}
			else if (this.ShieldState == ShieldState.Active)
			{
				this.energy += this.EnergyGainPerTick;
				if (this.energy > this.EnergyMax)
				{
					this.energy = this.EnergyMax;
				}
			}
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x001480AC File Offset: 0x001464AC
		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (this.ShieldState == ShieldState.Active)
			{
				if ((dinfo.Instigator != null && !dinfo.Instigator.Position.AdjacentTo8WayOrInside(base.Wearer.Position)) || dinfo.Def.isExplosive)
				{
					if (dinfo.Instigator != null)
					{
						AttachableThing attachableThing = dinfo.Instigator as AttachableThing;
						if (attachableThing != null && attachableThing.parent == base.Wearer)
						{
							return false;
						}
					}
					this.energy -= dinfo.Amount * this.EnergyLossPerDamage;
					if (dinfo.Def == DamageDefOf.EMP)
					{
						this.energy = -1f;
					}
					if (this.energy < 0f)
					{
						this.Break();
					}
					else
					{
						this.AbsorbedDamage(dinfo);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x001481A7 File Offset: 0x001465A7
		public void KeepDisplaying()
		{
			this.lastKeepDisplayTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x001481BC File Offset: 0x001465BC
		private void AbsorbedDamage(DamageInfo dinfo)
		{
			SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
			Vector3 loc = base.Wearer.TrueCenter() + this.impactAngleVect.RotatedBy(180f) * 0.5f;
			float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
			MoteMaker.MakeStaticMote(loc, base.Wearer.Map, ThingDefOf.Mote_ExplosionFlash, num);
			int num2 = (int)num;
			for (int i = 0; i < num2; i++)
			{
				MoteMaker.ThrowDustPuff(loc, base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
			this.KeepDisplaying();
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x001482B8 File Offset: 0x001466B8
		private void Break()
		{
			SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			MoteMaker.MakeStaticMote(base.Wearer.TrueCenter(), base.Wearer.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
			for (int i = 0; i < 6; i++)
			{
				Vector3 loc = base.Wearer.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f);
				MoteMaker.ThrowDustPuff(loc, base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.energy = 0f;
			this.ticksToReset = this.StartingTicksToReset;
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x00148398 File Offset: 0x00146798
		private void Reset()
		{
			if (base.Wearer.Spawned)
			{
				SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
				MoteMaker.ThrowLightningGlow(base.Wearer.TrueCenter(), base.Wearer.Map, 3f);
			}
			this.ticksToReset = -1;
			this.energy = this.EnergyOnReset;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00148418 File Offset: 0x00146818
		public override void DrawWornExtras()
		{
			if (this.ShieldState == ShieldState.Active && this.ShouldDisplay)
			{
				float num = Mathf.Lerp(1.2f, 1.55f, this.energy);
				Vector3 vector = base.Wearer.Drawer.DrawPos;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				int num2 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
				if (num2 < 8)
				{
					float num3 = (float)(8 - num2) / 8f * 0.05f;
					vector += this.impactAngleVect * num3;
					num -= num3;
				}
				float angle = (float)Rand.Range(0, 360);
				Vector3 s = new Vector3(num, 1f, num);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, ShieldBelt.BubbleMat, 0);
			}
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x00148504 File Offset: 0x00146904
		public override bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return !(verb is Verb_LaunchProjectile) || ReachabilityImmediate.CanReachImmediate(root, targ, map, PathEndMode.Touch, null);
		}

		// Token: 0x04001549 RID: 5449
		private float energy = 0f;

		// Token: 0x0400154A RID: 5450
		private int ticksToReset = -1;

		// Token: 0x0400154B RID: 5451
		private int lastKeepDisplayTick = -9999;

		// Token: 0x0400154C RID: 5452
		private Vector3 impactAngleVect;

		// Token: 0x0400154D RID: 5453
		private int lastAbsorbDamageTick = -9999;

		// Token: 0x0400154E RID: 5454
		private const float MinDrawSize = 1.2f;

		// Token: 0x0400154F RID: 5455
		private const float MaxDrawSize = 1.55f;

		// Token: 0x04001550 RID: 5456
		private const float MaxDamagedJitterDist = 0.05f;

		// Token: 0x04001551 RID: 5457
		private const int JitterDurationTicks = 8;

		// Token: 0x04001552 RID: 5458
		private int StartingTicksToReset = 3200;

		// Token: 0x04001553 RID: 5459
		private float EnergyOnReset = 0.2f;

		// Token: 0x04001554 RID: 5460
		private float EnergyLossPerDamage = 0.033f;

		// Token: 0x04001555 RID: 5461
		private int KeepDisplayingTicks = 1000;

		// Token: 0x04001556 RID: 5462
		private float ApparelScorePerEnergyMax = 0.25f;

		// Token: 0x04001557 RID: 5463
		private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
	}
}
