using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000C1 RID: 193
	internal static class TrashUtility
	{
		// Token: 0x04000298 RID: 664
		private const float ChanceHateInertBuilding = 0.008f;

		// Token: 0x04000299 RID: 665
		private static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(450, 500);

		// Token: 0x06000481 RID: 1153 RVA: 0x00033738 File Offset: 0x00031B38
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
					IntVec3 c = iterator.Current;
					if (c.InBounds(p.Map) && c.ContainsStaticFire(p.Map))
					{
						return false;
					}
					iterator.MoveNext();
				}
				result = (p.Position.Roofed(p.Map) || p.Map.weatherManager.RainRate <= 0.25f);
			}
			return result;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00033834 File Offset: 0x00031C34
		public static bool ShouldTrashBuilding(Pawn pawn, Building b, bool attackAllInert = false)
		{
			bool result;
			if (!b.def.useHitPoints)
			{
				result = false;
			}
			else
			{
				if ((b.def.building.isInert && !attackAllInert) || b.def.building.isTrap)
				{
					int num = GenLocalDate.HourOfDay(pawn) / 3;
					int specialSeed = b.GetHashCode() * 612361 ^ pawn.GetHashCode() * 391 ^ num * 73427324;
					if (!Rand.ChanceSeeded(0.008f, specialSeed))
					{
						return false;
					}
				}
				result = ((!b.def.building.isTrap || !((Building_Trap)b).Armed) && TrashUtility.CanTrash(pawn, b) && pawn.HostileTo(b));
			}
			return result;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00033920 File Offset: 0x00031D20
		private static bool CanTrash(Pawn pawn, Thing t)
		{
			return pawn.CanReach(t, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn) && !t.IsBurning();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00033960 File Offset: 0x00031D60
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
				if (pawn.equipment != null && Rand.Value < 0.7f)
				{
					foreach (Verb verb in pawn.equipment.AllEquipmentVerbs)
					{
						if (verb.verbProps.ai_IsBuildingDestroyer)
						{
							Job job2 = new Job(JobDefOf.UseVerbOnThing, t);
							job2.verbToUse = verb;
							TrashUtility.FinalizeTrashJob(job2);
							return job2;
						}
					}
				}
				float value = Rand.Value;
				Job job3;
				if (value < 0.35f && pawn.natives.IgniteVerb != null && t.FlammableNow && !t.IsBurning() && !(t is Building_Door))
				{
					job3 = new Job(JobDefOf.Ignite, t);
				}
				else
				{
					job3 = new Job(JobDefOf.AttackMelee, t);
				}
				TrashUtility.FinalizeTrashJob(job3);
				result = job3;
			}
			return result;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00033AC4 File Offset: 0x00031EC4
		private static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = TrashUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}
	}
}
