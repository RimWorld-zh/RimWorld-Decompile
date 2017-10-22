using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompExplosive : ThingComp
	{
		public bool wickStarted;

		protected int wickTicksLeft;

		private Thing instigator;

		public bool detonated;

		protected Sustainer wickSoundSustainer;

		public CompProperties_Explosive Props
		{
			get
			{
				return (CompProperties_Explosive)base.props;
			}
		}

		protected int StartWickThreshold
		{
			get
			{
				return Mathf.RoundToInt(this.Props.startWickHitPointsPercent * (float)base.parent.MaxHitPoints);
			}
		}

		private bool CanEverExplodeFromDamage
		{
			get
			{
				if (this.Props.chanceNeverExplodeFromDamage < 9.9999997473787516E-06)
				{
					return true;
				}
				Rand.PushState();
				Rand.Seed = base.parent.thingIDNumber.GetHashCode();
				bool result = Rand.Value < this.Props.chanceNeverExplodeFromDamage;
				Rand.PopState();
				return result;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.detonated, "detonated", false, false);
		}

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
					this.Detonate(base.parent.MapHeld);
				}
			}
		}

		private void StartWickSustainer()
		{
			SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(base.parent.Position, base.parent.Map, false));
			SoundInfo info = SoundInfo.InMap((Thing)base.parent, MaintenanceType.PerTick);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		public override void PostDraw()
		{
			if (this.wickStarted)
			{
				base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.BurningWick);
			}
		}

		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.CanEverExplodeFromDamage)
			{
				if (dinfo.Def.externalViolence && dinfo.Amount >= base.parent.HitPoints)
				{
					if (base.parent.MapHeld != null)
					{
						this.Detonate(base.parent.MapHeld);
						absorbed = true;
					}
				}
				else if (!this.wickStarted && this.Props.startWickOnDamageTaken != null && dinfo.Def == this.Props.startWickOnDamageTaken)
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.CanEverExplodeFromDamage && !base.parent.Destroyed)
			{
				if (this.wickStarted && dinfo.Def == DamageDefOf.Stun)
				{
					this.StopWick();
				}
				else if (!this.wickStarted && base.parent.HitPoints <= this.StartWickThreshold && dinfo.Def.externalViolence)
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		public void StartWick(Thing instigator = null)
		{
			if (!this.wickStarted)
			{
				this.instigator = instigator;
				this.wickStarted = true;
				this.wickTicksLeft = this.Props.wickTicks.RandomInRange;
				this.StartWickSustainer();
				GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(base.parent, this.Props.explosiveDamageType, null);
			}
		}

		public void StopWick()
		{
			this.wickStarted = false;
			this.instigator = null;
		}

		protected void Detonate(Map map)
		{
			if (!this.detonated)
			{
				this.detonated = true;
				if (base.parent.SpawnedOrAnyParentSpawned)
				{
					if (!base.parent.Destroyed)
					{
						base.parent.Kill(default(DamageInfo?));
					}
					if (map == null)
					{
						Log.Warning("Tried to detonate CompExplosive in a null map.");
					}
					else
					{
						CompProperties_Explosive props = this.Props;
						float num = props.explosiveRadius;
						if (base.parent.stackCount > 1 && props.explosiveExpandPerStackcount > 0.0)
						{
							num += Mathf.Sqrt((float)(base.parent.stackCount - 1) * props.explosiveExpandPerStackcount);
						}
						if (props.explosionEffect != null)
						{
							Effecter effecter = props.explosionEffect.Spawn();
							effecter.Trigger(new TargetInfo(base.parent.PositionHeld, map, false), new TargetInfo(base.parent.PositionHeld, map, false));
							effecter.Cleanup();
						}
						ThingDef postExplosionSpawnThingDef = props.postExplosionSpawnThingDef;
						float postExplosionSpawnChance = props.postExplosionSpawnChance;
						int postExplosionSpawnThingCount = props.postExplosionSpawnThingCount;
						GenExplosion.DoExplosion(base.parent.PositionHeld, map, num, props.explosiveDamageType, this.instigator ?? base.parent, null, null, null, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, props.applyDamageToExplosionCellsNeighbors, props.preExplosionSpawnThingDef, props.preExplosionSpawnChance, props.preExplosionSpawnThingCount);
					}
				}
			}
		}
	}
}
