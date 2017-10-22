using Verse;
using Verse.AI;

namespace RimWorld
{
	internal static class TrashUtility
	{
		private const float ChanceHateInertBuilding = 0.008f;

		private static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(450, 500);

		public static bool ShouldTrashPlant(Pawn pawn, Plant p)
		{
			bool result;
			if (!p.sown || p.def.plant.IsTree || !p.FlammableNow || !TrashUtility.CanTrash(pawn, p))
			{
				result = false;
			}
			else
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(p.Position, 2).ClipInsideMap(p.Map).GetIterator();
				while (!iterator.Done())
				{
					IntVec3 current = iterator.Current;
					if (current.InBounds(p.Map) && current.ContainsStaticFire(p.Map))
						goto IL_0095;
					iterator.MoveNext();
				}
				result = ((byte)((p.Position.Roofed(p.Map) || !(p.Map.weatherManager.RainRate > 0.25)) ? 1 : 0) != 0);
			}
			goto IL_00ee;
			IL_00ee:
			return result;
			IL_0095:
			result = false;
			goto IL_00ee;
		}

		public static bool ShouldTrashBuilding(Pawn pawn, Building b)
		{
			bool result;
			if (!b.def.useHitPoints)
			{
				result = false;
			}
			else
			{
				if (b.def.building.isInert || b.def.building.isTrap)
				{
					int num = GenLocalDate.HourOfDay(pawn) / 3;
					int specialSeed = b.GetHashCode() * 612361 ^ pawn.GetHashCode() * 391 ^ num * 734273247;
					if (!Rand.ChanceSeeded(0.008f, specialSeed))
					{
						result = false;
						goto IL_00d8;
					}
				}
				result = ((byte)((!b.def.building.isTrap || !((Building_Trap)b).Armed) ? ((TrashUtility.CanTrash(pawn, b) && pawn.HostileTo(b)) ? 1 : 0) : 0) != 0);
			}
			goto IL_00d8;
			IL_00d8:
			return result;
		}

		private static bool CanTrash(Pawn pawn, Thing t)
		{
			return (byte)((pawn.CanReach(t, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn) && !t.IsBurning()) ? 1 : 0) != 0;
		}

		public static Job TrashJob(Pawn pawn, Thing t)
		{
			Plant plant = t as Plant;
			Job result;
			if (plant != null)
			{
				Job job = new Job(JobDefOf.Ignite, t);
				TrashUtility.FinalizeTrashJob(job);
				result = job;
			}
			else
			{
				if (pawn.equipment != null && Rand.Value < 0.699999988079071)
				{
					foreach (Verb allEquipmentVerb in pawn.equipment.AllEquipmentVerbs)
					{
						if (allEquipmentVerb.verbProps.ai_IsBuildingDestroyer)
						{
							Job job2 = new Job(JobDefOf.UseVerbOnThing, t);
							job2.verbToUse = allEquipmentVerb;
							TrashUtility.FinalizeTrashJob(job2);
							return job2;
						}
					}
				}
				Job job3 = null;
				float value = Rand.Value;
				job3 = ((!(value < 0.34999999403953552) || pawn.natives.IgniteVerb == null || !t.FlammableNow || t.IsBurning() || t is Building_Door) ? new Job(JobDefOf.AttackMelee, t) : new Job(JobDefOf.Ignite, t));
				TrashUtility.FinalizeTrashJob(job3);
				result = job3;
			}
			return result;
		}

		private static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = TrashUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}
	}
}
