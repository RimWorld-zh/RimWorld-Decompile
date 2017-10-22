using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	internal abstract class IncidentWorker_ShipPartCrash : IncidentWorker
	{
		private const float ShipPointsFactor = 0.9f;

		private const int IncidentMinimumPoints = 300;

		private const float ShrapnelDistanceFront = 6f;

		private const float ShrapnelDistanceSide = 4f;

		private const float ShrapnelDistanceBack = 30f;

		private static readonly SimpleCurve ShrapnelDistanceFromAngle = new SimpleCurve
		{
			{
				new CurvePoint(0f, 6f),
				true
			},
			{
				new CurvePoint(90f, 4f),
				true
			},
			{
				new CurvePoint(135f, 4f),
				true
			},
			{
				new CurvePoint(180f, 30f),
				true
			},
			{
				new CurvePoint(225f, 4f),
				true
			},
			{
				new CurvePoint(270f, 4f),
				true
			},
			{
				new CurvePoint(360f, 6f),
				true
			}
		};

		private static readonly SimpleCurve ShrapnelAngleDistribution = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 90f),
				true
			},
			{
				new CurvePoint(0.25f, 135f),
				true
			},
			{
				new CurvePoint(0.5f, 180f),
				true
			},
			{
				new CurvePoint(0.75f, 225f),
				true
			},
			{
				new CurvePoint(0.9f, 270f),
				true
			},
			{
				new CurvePoint(1f, 360f),
				true
			}
		};

		private static readonly IntRange ShrapnelMetal = new IntRange(6, 10);

		private static readonly IntRange ShrapnelRubble = new IntRange(300, 400);

		protected virtual int CountToSpawn
		{
			get
			{
				return 1;
			}
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			if (map.listerThings.ThingsOfDef(base.def.shipPart).Count > 0)
			{
				return false;
			}
			return true;
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = 0;
			int countToSpawn = this.CountToSpawn;
			IntVec3 cell = IntVec3.Invalid;
			float angle = Rand.Range(0f, 360f);
			int num2 = 0;
			while (num2 < countToSpawn)
			{
				Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 c)
				{
					if (c.Fogged(map))
					{
						return false;
					}
					foreach (IntVec3 item in GenAdj.CellsOccupiedBy(c, Rot4.North, base.def.shipPart.size))
					{
						if (!item.Standable(map))
						{
							return false;
						}
						if (map.roofGrid.Roofed(item))
						{
							return false;
						}
					}
					if (!map.reachability.CanReachColony(c))
					{
						return false;
					}
					return true;
				};
				IntVec3 intVec = default(IntVec3);
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(14, validator, map, out intVec))
				{
					GenExplosion.DoExplosion(intVec, map, 3f, DamageDefOf.Flame, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
					Building_CrashedShipPart building_CrashedShipPart = (Building_CrashedShipPart)GenSpawn.Spawn(base.def.shipPart, intVec, map);
					building_CrashedShipPart.SetFaction(Faction.OfMechanoids, null);
					building_CrashedShipPart.pointsLeft = (float)(parms.points * 0.89999997615814209);
					if (building_CrashedShipPart.pointsLeft < 300.0)
					{
						building_CrashedShipPart.pointsLeft = 300f;
					}
					IncidentWorker_ShipPartCrash.SpawnShrapnel(ThingDefOf.ChunkSlagSteel, IncidentWorker_ShipPartCrash.ShrapnelMetal.RandomInRange, intVec, map, angle);
					IncidentWorker_ShipPartCrash.SpawnShrapnel(ThingDefOf.SlagRubble, IncidentWorker_ShipPartCrash.ShrapnelRubble.RandomInRange, intVec, map, angle);
					num++;
					cell = intVec;
					num2++;
					continue;
				}
				break;
			}
			if (num > 0)
			{
				if (map == Find.VisibleMap)
				{
					Find.CameraDriver.shaker.DoShake(1f);
				}
				base.SendStandardLetter(new TargetInfo(cell, map, false));
			}
			return num > 0;
		}

		public static void SpawnShrapnel(ThingDef def, int quantity, IntVec3 center, Map map, float angle)
		{
			for (int num = 0; num < quantity; num++)
			{
				IntVec3 intVec = IncidentWorker_ShipPartCrash.GenerateShrapnelLocation(center, angle);
				if (intVec.InBounds(map) && !intVec.Impassable(map) && !intVec.Filled(map))
				{
					RoofDef roofDef = map.roofGrid.RoofAt(intVec);
					if (roofDef == null && (from thing in intVec.GetThingList(map)
					where thing.def == def
					select thing).Count() <= 0)
					{
						GenSpawn.Spawn(def, intVec, map);
					}
				}
			}
		}

		public static IntVec3 GenerateShrapnelLocation(IntVec3 center, float angleOffset)
		{
			float num = IncidentWorker_ShipPartCrash.ShrapnelAngleDistribution.Evaluate(Rand.Value);
			float d = IncidentWorker_ShipPartCrash.ShrapnelDistanceFromAngle.Evaluate(num) * Rand.Value;
			return (Vector3Utility.HorizontalVectorFromAngle(num + angleOffset) * d).ToIntVec3() + center;
		}
	}
}
