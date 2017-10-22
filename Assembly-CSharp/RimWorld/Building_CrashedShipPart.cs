using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Noise;
using Verse.Sound;

namespace RimWorld
{
	public class Building_CrashedShipPart : Building
	{
		private const float MechanoidsDefendRadius = 21f;

		private const int SnowExpandInterval = 500;

		private const float SnowAddAmount = 0.12f;

		private const float SnowMaxRadius = 55f;

		public float pointsLeft;

		protected int age;

		private Lord lord;

		private float snowRadius;

		private int ticksToPlantHarm;

		private ModuleBase snowNoise;

		public static readonly string MemoDamaged = "ShipPartDamaged";

		private static HashSet<IntVec3> reachableCells = new HashSet<IntVec3>();

		protected virtual float PlantHarmRange
		{
			get
			{
				return 0f;
			}
		}

		protected virtual int PlantHarmInterval
		{
			get
			{
				return 4;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Lord>(ref this.lord, "defenseLord", false);
			Scribe_Values.Look<float>(ref this.pointsLeft, "mechanoidPointsLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<float>(ref this.snowRadius, "snowRadius", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("AwokeDaysAgo".Translate(this.age.TicksToDays().ToString("F1")));
			return stringBuilder.ToString();
		}

		public override void Tick()
		{
			base.Tick();
			this.age++;
			this.ticksToPlantHarm--;
			if (this.ticksToPlantHarm <= 0)
			{
				this.HarmPlant();
			}
			if ((Find.TickManager.TicksGame + this.HashOffset()) % 500 == 0)
			{
				this.ExpandSnow();
			}
		}

		public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(dinfo, out absorbed);
			if (!absorbed)
			{
				if (dinfo.Def.harmsHealth)
				{
					if (this.lord != null)
					{
						this.lord.ReceiveMemo(Building_CrashedShipPart.MemoDamaged);
					}
					float num = (float)(this.HitPoints - dinfo.Amount);
					if (num < (float)base.MaxHitPoints * 0.98000001907348633 && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
					{
						goto IL_008f;
					}
					if (num < (float)base.MaxHitPoints * 0.89999997615814209)
						goto IL_008f;
				}
				goto IL_0095;
			}
			return;
			IL_0095:
			absorbed = false;
			return;
			IL_008f:
			this.TrySpawnMechanoids();
			goto IL_0095;
		}

		public void Notify_AdjacentBlueprintReplacedWithSolidThing(Pawn by)
		{
			if (by.Faction != Faction.OfMechanoids)
			{
				this.TrySpawnMechanoids();
			}
		}

		private void TrySpawnMechanoids()
		{
			if (!(this.pointsLeft <= 0.0))
			{
				if (this.lord == null)
				{
					IntVec3 invalid = default(IntVec3);
					if (!CellFinder.TryFindRandomCellNear(base.Position, base.Map, 5, (Predicate<IntVec3>)((IntVec3 c) => c.Standable(base.Map) && base.Map.reachability.CanReach(c, (Thing)this, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false))), out invalid))
					{
						Log.Error("Found no place for mechanoids to defend " + this);
						invalid = IntVec3.Invalid;
					}
					LordJob_MechanoidsDefendShip lordJob = new LordJob_MechanoidsDefendShip(this, base.Faction, 21f, invalid);
					this.lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, base.Map, null);
				}
				PawnKindDef kindDef = default(PawnKindDef);
				IntVec3 center = default(IntVec3);
				while ((from def in DefDatabase<PawnKindDef>.AllDefs
				where def.RaceProps.IsMechanoid && def.isFighter && def.combatPower <= this.pointsLeft
				select def).TryRandomElement<PawnKindDef>(out kindDef) && (from cell in GenAdj.CellsAdjacent8Way(this)
				where this.CanSpawnMechanoidAt(cell)
				select cell).TryRandomElement<IntVec3>(out center))
				{
					Pawn pawn = PawnGenerator.GeneratePawn(kindDef, Faction.OfMechanoids);
					if (!GenPlace.TryPlaceThing(pawn, center, base.Map, ThingPlaceMode.Near, null))
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
						break;
					}
					this.lord.AddPawn(pawn);
					this.pointsLeft -= pawn.kindDef.combatPower;
				}
				this.pointsLeft = 0f;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(base.Map);
			}
		}

		private bool CanSpawnMechanoidAt(IntVec3 c)
		{
			if (!c.Walkable(base.Map))
			{
				return false;
			}
			return true;
		}

		private void ExpandSnow()
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.054999999701976776, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			if (this.snowRadius < 8.0)
			{
				this.snowRadius += 1.3f;
			}
			else if (this.snowRadius < 17.0)
			{
				this.snowRadius += 0.7f;
			}
			else if (this.snowRadius < 30.0)
			{
				this.snowRadius += 0.4f;
			}
			else
			{
				this.snowRadius += 0.1f;
			}
			if (this.snowRadius > 55.0)
			{
				this.snowRadius = 55f;
			}
			CellRect occupiedRect = this.OccupiedRect();
			Building_CrashedShipPart.reachableCells.Clear();
			base.Map.floodFiller.FloodFill(base.Position, (Predicate<IntVec3>)delegate(IntVec3 x)
			{
				if ((float)x.DistanceToSquared(base.Position) > this.snowRadius * this.snowRadius)
				{
					return false;
				}
				return occupiedRect.Contains(x) || !x.Filled(base.Map);
			}, (Action<IntVec3>)delegate(IntVec3 x)
			{
				Building_CrashedShipPart.reachableCells.Add(x);
			}, false);
			int num = GenRadial.NumCellsInRadius(this.snowRadius);
			for (int num2 = 0; num2 < num; num2++)
			{
				IntVec3 intVec = base.Position + GenRadial.RadialPattern[num2];
				if (intVec.InBounds(base.Map) && Building_CrashedShipPart.reachableCells.Contains(intVec))
				{
					float value = this.snowNoise.GetValue(intVec);
					value = (float)(value + 1.0);
					value = (float)(value * 0.5);
					if (value < 0.10000000149011612)
					{
						value = 0.1f;
					}
					if (!(base.Map.snowGrid.GetDepth(intVec) > value))
					{
						float lengthHorizontal = (intVec - base.Position).LengthHorizontal;
						float num3 = (float)(1.0 - lengthHorizontal / this.snowRadius);
						base.Map.snowGrid.AddDepth(intVec, (float)(num3 * 0.11999999731779099 * value));
					}
				}
			}
		}

		private void HarmPlant()
		{
			if (!(this.PlantHarmRange < 9.9999997473787516E-05))
			{
				float angle = Rand.Range(0f, 360f);
				float num = Rand.Range(0f, this.PlantHarmRange);
				num = Mathf.Sqrt(num / this.PlantHarmRange) * this.PlantHarmRange;
				Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
				Vector3 point = Vector3.forward * num;
				Vector3 v = rotation * point;
				IntVec3 b = IntVec3.FromVector3(v);
				IntVec3 c = base.Position + b;
				if (c.InBounds(base.Map))
				{
					Plant plant = c.GetPlant(base.Map);
					if (plant != null)
					{
						if (Rand.Value < 0.20000000298023224)
						{
							plant.Kill(default(DamageInfo?));
						}
						else
						{
							plant.MakeLeafless(false);
						}
					}
				}
				this.ticksToPlantHarm = this.PlantHarmInterval;
			}
		}
	}
}
