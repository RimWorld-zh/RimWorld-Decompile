using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200070E RID: 1806
	public class CompExplosive : ThingComp
	{
		// Token: 0x040015D6 RID: 5590
		public bool wickStarted = false;

		// Token: 0x040015D7 RID: 5591
		protected int wickTicksLeft = 0;

		// Token: 0x040015D8 RID: 5592
		private Thing instigator;

		// Token: 0x040015D9 RID: 5593
		public bool destroyedThroughDetonation;

		// Token: 0x040015DA RID: 5594
		protected Sustainer wickSoundSustainer = null;

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x001539A4 File Offset: 0x00151DA4
		public CompProperties_Explosive Props
		{
			get
			{
				return (CompProperties_Explosive)this.props;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x001539C4 File Offset: 0x00151DC4
		protected int StartWickThreshold
		{
			get
			{
				return Mathf.RoundToInt(this.Props.startWickHitPointsPercent * (float)this.parent.MaxHitPoints);
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002794 RID: 10132 RVA: 0x001539F8 File Offset: 0x00151DF8
		private bool CanEverExplodeFromDamage
		{
			get
			{
				bool result;
				if (this.Props.chanceNeverExplodeFromDamage < 1E-05f)
				{
					result = true;
				}
				else
				{
					Rand.PushState();
					Rand.Seed = this.parent.thingIDNumber.GetHashCode();
					bool flag = Rand.Value < this.Props.chanceNeverExplodeFromDamage;
					Rand.PopState();
					result = flag;
				}
				return result;
			}
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x00153A64 File Offset: 0x00151E64
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyedThroughDetonation, "destroyedThroughDetonation", false, false);
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x00153AC0 File Offset: 0x00151EC0
		public override void CompTick()
		{
			if (this.wickStarted)
			{
				if (this.wickSoundSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSoundSustainer.Maintain();
				}
				this.wickTicksLeft--;
				if (this.wickTicksLeft <= 0)
				{
					this.Detonate(this.parent.MapHeld);
				}
			}
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x00153B28 File Offset: 0x00151F28
		private void StartWickSustainer()
		{
			SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x00153B84 File Offset: 0x00151F84
		private void EndWickSustainer()
		{
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x00153BA6 File Offset: 0x00151FA6
		public override void PostDraw()
		{
			if (this.wickStarted)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BurningWick);
			}
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x00153BD0 File Offset: 0x00151FD0
		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.CanEverExplodeFromDamage)
			{
				if (dinfo.Def.externalViolence && dinfo.Amount >= (float)this.parent.HitPoints && this.CanExplodeFromDamageType(dinfo.Def))
				{
					if (this.parent.MapHeld != null)
					{
						this.Detonate(this.parent.MapHeld);
						if (this.parent.Destroyed)
						{
							absorbed = true;
						}
					}
				}
				else if (!this.wickStarted && this.Props.startWickOnDamageTaken != null && dinfo.Def == this.Props.startWickOnDamageTaken)
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x00153CA8 File Offset: 0x001520A8
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.CanEverExplodeFromDamage)
			{
				if (this.CanExplodeFromDamageType(dinfo.Def))
				{
					if (!this.parent.Destroyed)
					{
						if (this.wickStarted && dinfo.Def == DamageDefOf.Stun)
						{
							this.StopWick();
						}
						else if (!this.wickStarted && this.parent.HitPoints <= this.StartWickThreshold)
						{
							if (dinfo.Def.externalViolence)
							{
								this.StartWick(dinfo.Instigator);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x00153D58 File Offset: 0x00152158
		public void StartWick(Thing instigator = null)
		{
			if (!this.wickStarted)
			{
				if (this.ExplosiveRadius() > 0f)
				{
					this.instigator = instigator;
					this.wickStarted = true;
					this.wickTicksLeft = this.Props.wickTicks.RandomInRange;
					this.StartWickSustainer();
					GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this.parent, this.Props.explosiveDamageType, null);
				}
			}
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00153DCC File Offset: 0x001521CC
		public void StopWick()
		{
			this.wickStarted = false;
			this.instigator = null;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x00153DE0 File Offset: 0x001521E0
		public float ExplosiveRadius()
		{
			CompProperties_Explosive props = this.Props;
			float num = props.explosiveRadius;
			if (this.parent.stackCount > 1 && props.explosiveExpandPerStackcount > 0f)
			{
				num += Mathf.Sqrt((float)(this.parent.stackCount - 1) * props.explosiveExpandPerStackcount);
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				num += Mathf.Sqrt(this.parent.GetComp<CompRefuelable>().Fuel * props.explosiveExpandPerFuel);
			}
			return num;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x00153E84 File Offset: 0x00152284
		protected void Detonate(Map map)
		{
			if (this.parent.SpawnedOrAnyParentSpawned)
			{
				CompProperties_Explosive props = this.Props;
				float num = this.ExplosiveRadius();
				if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
				{
					this.parent.GetComp<CompRefuelable>().ConsumeFuel(this.parent.GetComp<CompRefuelable>().Fuel);
				}
				if (props.destroyThingOnExplosionSize <= num && !this.parent.Destroyed)
				{
					this.destroyedThroughDetonation = true;
					this.parent.Kill(null, null);
				}
				this.EndWickSustainer();
				this.wickStarted = false;
				if (map == null)
				{
					Log.Warning("Tried to detonate CompExplosive in a null map.", false);
				}
				else
				{
					if (props.explosionEffect != null)
					{
						Effecter effecter = props.explosionEffect.Spawn();
						effecter.Trigger(new TargetInfo(this.parent.PositionHeld, map, false), new TargetInfo(this.parent.PositionHeld, map, false));
						effecter.Cleanup();
					}
					IntVec3 positionHeld = this.parent.PositionHeld;
					float radius = num;
					DamageDef explosiveDamageType = props.explosiveDamageType;
					Thing thing = this.instigator ?? this.parent;
					int damageAmountBase = props.damageAmountBase;
					SoundDef explosionSound = props.explosionSound;
					ThingDef postExplosionSpawnThingDef = props.postExplosionSpawnThingDef;
					float postExplosionSpawnChance = props.postExplosionSpawnChance;
					int postExplosionSpawnThingCount = props.postExplosionSpawnThingCount;
					GenExplosion.DoExplosion(positionHeld, map, radius, explosiveDamageType, thing, damageAmountBase, explosionSound, null, null, null, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, props.applyDamageToExplosionCellsNeighbors, props.preExplosionSpawnThingDef, props.preExplosionSpawnChance, props.preExplosionSpawnThingCount, props.chanceToStartFire, props.damageFalloff);
				}
			}
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x00154034 File Offset: 0x00152434
		private bool CanExplodeFromDamageType(DamageDef damage)
		{
			return this.Props.requiredDamageTypeToExplode == null || this.Props.requiredDamageTypeToExplode == damage;
		}
	}
}
