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
			if (p.sown && !p.def.plant.IsTree && p.FlammableNow && TrashUtility.CanTrash(pawn, p))
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(p.Position, 2).ClipInsideMap(p.Map).GetIterator();
				while (!iterator.Done())
				{
					IntVec3 current = iterator.Current;
					if (current.InBounds(p.Map) && current.ContainsStaticFire(p.Map))
					{
						return false;
					}
					iterator.MoveNext();
				}
				if (!p.Position.Roofed(p.Map) && p.Map.weatherManager.RainRate > 0.25)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool ShouldTrashBuilding(Pawn pawn, Building b, bool attackAllInert = false)
		{
			if (!b.def.useHitPoints)
			{
				return false;
			}
			if (b.def.building.isInert && !attackAllInert)
			{
				goto IL_0042;
			}
			if (b.def.building.isTrap)
				goto IL_0042;
			goto IL_007f;
			IL_0042:
			int num = GenLocalDate.HourOfDay(pawn) / 3;
			int specialSeed = b.GetHashCode() * 612361 ^ pawn.GetHashCode() * 391 ^ num * 734273247;
			if (!Rand.ChanceSeeded(0.008f, specialSeed))
			{
				return false;
			}
			goto IL_007f;
			IL_007f:
			if (b.def.building.isTrap && ((Building_Trap)b).Armed)
			{
				return false;
			}
			if (TrashUtility.CanTrash(pawn, b) && pawn.HostileTo(b))
			{
				return true;
			}
			return false;
		}

		private static bool CanTrash(Pawn pawn, Thing t)
		{
			if (pawn.CanReach(t, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn) && !t.IsBurning())
			{
				return true;
			}
			return false;
		}

		public static Job TrashJob(Pawn pawn, Thing t)
		{
			Plant plant = t as Plant;
			if (plant != null)
			{
				Job job = new Job(JobDefOf.Ignite, t);
				TrashUtility.FinalizeTrashJob(job);
				return job;
			}
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
			return job3;
		}

		private static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = TrashUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}
	}
}
