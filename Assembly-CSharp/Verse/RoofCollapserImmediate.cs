using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public static class RoofCollapserImmediate
	{
		private static readonly IntRange ThinRoofCrushDamageRange = new IntRange(15, 30);

		public static void DropRoofInCells(IntVec3 c, Map map)
		{
			RoofCollapserImmediate.DropRoofInCellPhaseOne(c, map);
			RoofCollapserImmediate.DropRoofInCellPhaseTwo(c, map);
			SoundDefOf.RoofCollapse.PlayOneShot(new TargetInfo(c, map, false));
		}

		public static void DropRoofInCells(IEnumerable<IntVec3> cells, Map map)
		{
			IntVec3 cell = IntVec3.Invalid;
			foreach (IntVec3 item in cells)
			{
				RoofCollapserImmediate.DropRoofInCellPhaseOne(item, map);
			}
			foreach (IntVec3 item2 in cells)
			{
				RoofCollapserImmediate.DropRoofInCellPhaseTwo(item2, map);
				cell = item2;
			}
			if (cell.IsValid)
			{
				SoundDefOf.RoofCollapse.PlayOneShot(new TargetInfo(cell, map, false));
			}
		}

		public static void DropRoofInCells(List<IntVec3> cells, Map map)
		{
			if (!cells.NullOrEmpty())
			{
				for (int i = 0; i < cells.Count; i++)
				{
					RoofCollapserImmediate.DropRoofInCellPhaseOne(cells[i], map);
				}
				for (int j = 0; j < cells.Count; j++)
				{
					RoofCollapserImmediate.DropRoofInCellPhaseTwo(cells[j], map);
				}
				SoundDefOf.RoofCollapse.PlayOneShot(new TargetInfo(cells[0], map, false));
			}
		}

		private static void DropRoofInCellPhaseOne(IntVec3 c, Map map)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(c);
			if (roofDef != null)
			{
				if (roofDef.collapseLeavingThingDef != null && roofDef.collapseLeavingThingDef.passability == Traversability.Impassable)
				{
					for (int i = 0; i < 2; i++)
					{
						List<Thing> thingList = c.GetThingList(map);
						for (int num = thingList.Count - 1; num >= 0; num--)
						{
							Thing thing = thingList[num];
							map.roofCollapseBuffer.Notify_Crushed(thing);
							Pawn pawn = thing as Pawn;
							DamageInfo dinfo = default(DamageInfo);
							if (pawn != null)
							{
								BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
								dinfo = new DamageInfo(DamageDefOf.Crush, 99999, -1f, null, brain, null, DamageInfo.SourceCategory.Collapse);
							}
							else
							{
								dinfo = new DamageInfo(DamageDefOf.Crush, 99999, -1f, null, null, null, DamageInfo.SourceCategory.Collapse);
								dinfo.SetBodyRegion(BodyPartHeight.Top, BodyPartDepth.Outside);
							}
							thing.TakeDamage(dinfo);
							if (!thing.Destroyed && thing.def.destroyable)
							{
								thing.Destroy(DestroyMode.Vanish);
							}
						}
					}
				}
				else
				{
					List<Thing> thingList2 = c.GetThingList(map);
					for (int num2 = thingList2.Count - 1; num2 >= 0; num2--)
					{
						Thing thing2 = thingList2[num2];
						if (thing2.def.category == ThingCategory.Item || thing2.def.category == ThingCategory.Plant || thing2.def.category == ThingCategory.Building || thing2.def.category == ThingCategory.Pawn)
						{
							map.roofCollapseBuffer.Notify_Crushed(thing2);
							float num3 = (float)RoofCollapserImmediate.ThinRoofCrushDamageRange.RandomInRange;
							if (thing2.def.building != null)
							{
								num3 *= thing2.def.building.roofCollapseDamageMultiplier;
							}
							DamageInfo dinfo2 = new DamageInfo(DamageDefOf.Crush, GenMath.RoundRandom(num3), -1f, null, null, null, DamageInfo.SourceCategory.Collapse);
							dinfo2.SetBodyRegion(BodyPartHeight.Top, BodyPartDepth.Outside);
							thing2.TakeDamage(dinfo2);
						}
					}
				}
				if (roofDef.collapseLeavingThingDef != null)
				{
					Thing thing3 = GenSpawn.Spawn(roofDef.collapseLeavingThingDef, c, map);
					if (thing3.def.rotatable)
					{
						thing3.Rotation = Rot4.Random;
					}
				}
				for (int j = 0; j < 1; j++)
				{
					Vector3 a = c.ToVector3Shifted();
					a += Gen.RandomHorizontalVector(0.6f);
					MoteMaker.ThrowDustPuff(a, map, 2f);
				}
			}
		}

		private static void DropRoofInCellPhaseTwo(IntVec3 c, Map map)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(c);
			if (roofDef != null)
			{
				if (roofDef.filthLeaving != null)
				{
					FilthMaker.MakeFilth(c, map, roofDef.filthLeaving, 1);
				}
				if (!roofDef.isThickRoof)
				{
					map.roofGrid.SetRoof(c, null);
				}
			}
		}
	}
}
