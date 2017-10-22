using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Fire : AttachableThing, ISizeReporter
	{
		public const float MinFireSize = 0.1f;

		private const float MinSizeForSpark = 1f;

		private const float TicksBetweenSparksBase = 150f;

		private const float TicksBetweenSparksReductionPerFireSize = 40f;

		private const float MinTicksBetweenSparks = 75f;

		private const float MinFireSizeToEmitSpark = 1f;

		private const float MaxFireSize = 1.75f;

		private const int TicksToBurnFloor = 7500;

		private const int ComplexCalcsInterval = 150;

		private const float CellIgniteChancePerTickPerSize = 0.01f;

		private const float MinSizeForIgniteMovables = 0.4f;

		private const float FireBaseGrowthPerTick = 0.00055f;

		private const int SmokeIntervalRandomAddon = 10;

		private const float BaseSkyExtinguishChance = 0.04f;

		private const int BaseSkyExtinguishDamage = 10;

		private const float HeatPerFireSizePerInterval = 160f;

		private const float HeatFactorWhenDoorPresent = 0.15f;

		private const float SnowClearRadiusPerFireSize = 3f;

		private const float SnowClearDepthFactor = 0.1f;

		private const int FireCountParticlesOff = 15;

		private int ticksSinceSpawn;

		public float fireSize = 0.1f;

		private int ticksSinceSpread;

		private float flammabilityMax = 0.5f;

		private int ticksUntilSmoke;

		private Sustainer sustainer;

		private static List<Thing> flammableList = new List<Thing>();

		private static int fireCount;

		private static int lastFireCountUpdateTick;

		private static readonly IntRange SmokeIntervalRange = new IntRange(130, 200);

		public override string Label
		{
			get
			{
				if (base.parent != null)
				{
					return "FireOn".Translate(base.parent.LabelCap);
				}
				return "Fire".Translate();
			}
		}

		public override string InspectStringAddon
		{
			get
			{
				return "Burning".Translate() + " (" + "FireSizeLower".Translate(((float)(this.fireSize * 100.0)).ToString("F0")) + ")";
			}
		}

		private float SpreadInterval
		{
			get
			{
				float num = (float)(150.0 - (this.fireSize - 1.0) * 40.0);
				if (num < 75.0)
				{
					num = 75f;
				}
				return num;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.fireSize, "fireSize", 0f, false);
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.RecalcPathsOnAndAroundMe(map);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.HomeArea, this, OpportunityType.Important);
			this.ticksSinceSpread = (int)(this.SpreadInterval * Rand.Value);
			LongEventHandler.ExecuteWhenFinished((Action)delegate()
			{
				SoundDef def = SoundDef.Named("FireBurning");
				SoundInfo info = SoundInfo.InMap(new TargetInfo(base.Position, map, false), MaintenanceType.PerTick);
				this.sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, def, info);
			});
		}

		public float CurrentSize()
		{
			return this.fireSize;
		}

		public override void DeSpawn()
		{
			if (this.sustainer.externalParams.sizeAggregator == null)
			{
				this.sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
			}
			this.sustainer.externalParams.sizeAggregator.RemoveReporter(this);
			Map map = base.Map;
			base.DeSpawn();
			this.RecalcPathsOnAndAroundMe(map);
		}

		private void RecalcPathsOnAndAroundMe(Map map)
		{
			IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
			for (int i = 0; i < adjacentCellsAndInside.Length; i++)
			{
				IntVec3 c = base.Position + adjacentCellsAndInside[i];
				if (c.InBounds(map))
				{
					map.pathGrid.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		public override void AttachTo(Thing parent)
		{
			base.AttachTo(parent);
			Pawn pawn = parent as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.WasOnFire, pawn);
			}
		}

		public override void Tick()
		{
			this.ticksSinceSpawn++;
			if (Fire.lastFireCountUpdateTick != Find.TickManager.TicksGame)
			{
				Fire.fireCount = base.Map.listerThings.ThingsOfDef(base.def).Count;
				Fire.lastFireCountUpdateTick = Find.TickManager.TicksGame;
			}
			if (this.sustainer != null)
			{
				this.sustainer.Maintain();
			}
			else
			{
				Log.ErrorOnce("Fire sustainer was null at " + base.Position, 917321);
			}
			this.ticksUntilSmoke--;
			if (this.ticksUntilSmoke <= 0)
			{
				this.SpawnSmokeParticles();
			}
			if (Fire.fireCount < 15 && this.fireSize > 0.699999988079071 && Rand.Value < this.fireSize * 0.0099999997764825821)
			{
				MoteMaker.ThrowMicroSparks(this.DrawPos, base.Map);
			}
			if (this.fireSize > 1.0)
			{
				this.ticksSinceSpread++;
				if ((float)this.ticksSinceSpread >= this.SpreadInterval)
				{
					this.TrySpread();
					this.ticksSinceSpread = 0;
				}
			}
			if (this.IsHashIntervalTick(150))
			{
				this.DoComplexCalcs();
			}
			if (this.ticksSinceSpawn >= 7500)
			{
				this.TryMakeFloorBurned();
			}
		}

		private void SpawnSmokeParticles()
		{
			if (Fire.fireCount < 15)
			{
				MoteMaker.ThrowSmoke(this.DrawPos, base.Map, this.fireSize);
			}
			if (this.fireSize > 0.5 && base.parent == null)
			{
				MoteMaker.ThrowFireGlow(base.Position, base.Map, this.fireSize);
			}
			float num = (float)(this.fireSize / 2.0);
			if (num > 1.0)
			{
				num = 1f;
			}
			num = (float)(1.0 - num);
			this.ticksUntilSmoke = Fire.SmokeIntervalRange.Lerped(num) + (int)(10.0 * Rand.Value);
		}

		private void DoComplexCalcs()
		{
			bool flag = false;
			Fire.flammableList.Clear();
			this.flammabilityMax = 0f;
			if (!base.Position.GetTerrain(base.Map).HasTag("Water"))
			{
				if (base.parent == null)
				{
					if (base.Position.TerrainFlammableNow(base.Map))
					{
						this.flammabilityMax = base.Position.GetTerrain(base.Map).GetStatValueAbstract(StatDefOf.Flammability, null);
					}
					List<Thing> list = base.Map.thingGrid.ThingsListAt(base.Position);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (thing is Building_Door)
						{
							flag = true;
						}
						float statValue = thing.GetStatValue(StatDefOf.Flammability, true);
						if (!(statValue < 0.0099999997764825821))
						{
							Fire.flammableList.Add(list[i]);
							if (statValue > this.flammabilityMax)
							{
								this.flammabilityMax = statValue;
							}
							if (base.parent == null && this.fireSize > 0.40000000596046448 && list[i].def.category == ThingCategory.Pawn)
							{
								list[i].TryAttachFire((float)(this.fireSize * 0.20000000298023224));
							}
						}
					}
				}
				else
				{
					Fire.flammableList.Add(base.parent);
					this.flammabilityMax = base.parent.GetStatValue(StatDefOf.Flammability, true);
				}
			}
			if (this.flammabilityMax < 0.0099999997764825821)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				Thing thing2 = (base.parent == null) ? ((Fire.flammableList.Count <= 0) ? null : Fire.flammableList.RandomElement()) : base.parent;
				if (thing2 != null && (!(this.fireSize < 0.40000000596046448) || thing2 == base.parent || thing2.def.category != ThingCategory.Pawn))
				{
					this.DoFireDamage(thing2);
				}
				if (base.Spawned)
				{
					float num = (float)(this.fireSize * 160.0);
					if (flag)
					{
						num = (float)(num * 0.15000000596046448);
					}
					GenTemperature.PushHeat(base.Position, base.Map, num);
					if (Rand.Value < 0.40000000596046448)
					{
						float radius = (float)(this.fireSize * 3.0);
						SnowUtility.AddSnowRadial(base.Position, base.Map, radius, (float)(0.0 - this.fireSize * 0.10000000149011612));
					}
					this.fireSize += (float)(0.00054999999701976776 * this.flammabilityMax * 150.0);
					if (this.fireSize > 1.75)
					{
						this.fireSize = 1.75f;
					}
					if (base.Map.weatherManager.RainRate > 0.0099999997764825821 && this.VulnerableToRain() && Rand.Value < 6.0)
					{
						base.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 10, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
					}
				}
			}
		}

		private void TryMakeFloorBurned()
		{
			if (base.parent == null && base.Spawned)
			{
				Map map = base.Map;
				TerrainGrid terrainGrid = map.terrainGrid;
				TerrainDef terrain = base.Position.GetTerrain(map);
				if (terrain.burnedDef != null && base.Position.TerrainFlammableNow(map))
				{
					terrainGrid.RemoveTopLayer(base.Position, false);
					terrainGrid.SetTerrain(base.Position, terrain.burnedDef);
				}
			}
		}

		private bool VulnerableToRain()
		{
			if (!base.Spawned)
			{
				return false;
			}
			RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
			if (roofDef == null)
			{
				return true;
			}
			if (roofDef.isThickRoof)
			{
				return false;
			}
			Thing edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.holdsRoof;
		}

		private void DoFireDamage(Thing targ)
		{
			float value = (float)(0.012500000186264515 + 0.003599999938160181 * this.fireSize);
			value = Mathf.Clamp(value, 0.0125f, 0.05f);
			int num = GenMath.RoundRandom((float)(value * 150.0));
			if (num < 1)
			{
				num = 1;
			}
			Pawn pawn = targ as Pawn;
			if (pawn != null)
			{
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Flame, num, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
				targ.TakeDamage(dinfo);
				Apparel apparel = default(Apparel);
				if (pawn.apparel != null && ((IEnumerable<Apparel>)pawn.apparel.WornApparel).TryRandomElement<Apparel>(out apparel))
				{
					apparel.TakeDamage(new DamageInfo(DamageDefOf.Flame, num, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			}
			else
			{
				targ.TakeDamage(new DamageInfo(DamageDefOf.Flame, num, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
		}

		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!base.Destroyed && dinfo.Def == DamageDefOf.Extinguish)
			{
				this.fireSize -= (float)((float)dinfo.Amount / 100.0);
				if (this.fireSize <= 0.10000000149011612)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		protected void TrySpread()
		{
			IntVec3 position = base.Position;
			bool flag;
			if (Rand.Chance(0.8f))
			{
				position = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(1, 8)];
				flag = true;
			}
			else
			{
				position = base.Position + GenRadial.ManualRadialPattern[Rand.RangeInclusive(10, 20)];
				flag = false;
			}
			if (position.InBounds(base.Map) && Rand.Chance(FireUtility.ChanceToStartFireIn(position, base.Map)))
			{
				if (!flag)
				{
					CellRect startRect = CellRect.SingleCell(base.Position);
					CellRect endRect = CellRect.SingleCell(position);
					if (GenSight.LineOfSight(base.Position, position, base.Map, startRect, endRect))
					{
						Spark spark = (Spark)GenSpawn.Spawn(ThingDefOf.Spark, base.Position, base.Map);
						spark.Launch(this, position, null);
					}
				}
				else
				{
					FireUtility.TryStartFireIn(position, base.Map, 0.1f);
				}
			}
		}
	}
}
