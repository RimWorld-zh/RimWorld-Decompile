using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenLeaving
	{
		private const float LeaveFraction_Kill = 0.5f;

		private const float LeaveFraction_Cancel = 1f;

		public const float LeaveFraction_DeconstructDefault = 0.75f;

		private const float LeaveFraction_FailConstruction = 0.5f;

		private static List<IntVec3> tmpCellsCandidates = new List<IntVec3>();

		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode)
		{
			GenLeaving.DoLeavingsFor(diedThing, map, mode, diedThing.OccupiedRect());
		}

		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode, CellRect leavingsRect)
		{
			if (Current.ProgramState != ProgramState.Playing && mode != DestroyMode.Refund)
				return;
			switch (mode)
			{
			case DestroyMode.Vanish:
				return;
			case DestroyMode.KillFinalize:
				if (diedThing.def.filthLeaving != null)
				{
					for (int i = leavingsRect.minZ; i <= leavingsRect.maxZ; i++)
					{
						for (int j = leavingsRect.minX; j <= leavingsRect.maxX; j++)
						{
							IntVec3 c = new IntVec3(j, 0, i);
							FilthMaker.MakeFilth(c, map, diedThing.def.filthLeaving, Rand.RangeInclusive(1, 3));
						}
					}
				}
				break;
			}
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			if (mode == DestroyMode.KillFinalize && diedThing.def.killedLeavings != null)
			{
				for (int k = 0; k < diedThing.def.killedLeavings.Count; k++)
				{
					Thing thing = ThingMaker.MakeThing(diedThing.def.killedLeavings[k].thingDef, null);
					thing.stackCount = diedThing.def.killedLeavings[k].count;
					thingOwner.TryAdd(thing, true);
				}
			}
			if (GenLeaving.CanBuildingLeaveResources(diedThing, mode))
			{
				Frame frame = diedThing as Frame;
				if (frame != null)
				{
					for (int num = frame.resourceContainer.Count - 1; num >= 0; num--)
					{
						int num2 = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(frame.resourceContainer[num].stackCount);
						if (num2 > 0)
						{
							frame.resourceContainer.TryTransferToContainer(frame.resourceContainer[num], thingOwner, num2, true);
						}
					}
					frame.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
				else
				{
					List<ThingCountClass> list = diedThing.CostListAdjusted();
					for (int l = 0; l < list.Count; l++)
					{
						ThingCountClass thingCountClass = list[l];
						int num3 = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(thingCountClass.count);
						if (num3 > 0 && mode == DestroyMode.KillFinalize && thingCountClass.thingDef.slagDef != null)
						{
							int count = thingCountClass.thingDef.slagDef.smeltProducts.First((ThingCountClass pro) => pro.thingDef == ThingDefOf.Steel).count;
							int num4 = num3 / 2 / 8;
							for (int m = 0; m < num4; m++)
							{
								thingOwner.TryAdd(ThingMaker.MakeThing(thingCountClass.thingDef.slagDef, null), true);
							}
							num3 -= num4 * count;
						}
						if (num3 > 0)
						{
							Thing thing2 = ThingMaker.MakeThing(thingCountClass.thingDef, null);
							thing2.stackCount = num3;
							thingOwner.TryAdd(thing2, true);
						}
					}
				}
			}
			List<IntVec3> list2 = leavingsRect.Cells.InRandomOrder(null).ToList();
			int num5 = 0;
			while (true)
			{
				if (thingOwner.Count > 0)
				{
					if (mode == DestroyMode.KillFinalize && !((Area)map.areaManager.Home)[list2[num5]])
					{
						thingOwner[0].SetForbidden(true, false);
					}
					Thing thing3 = default(Thing);
					if (thingOwner.TryDrop(thingOwner[0], list2[num5], map, ThingPlaceMode.Near, out thing3, (Action<Thing, int>)null))
					{
						num5++;
						if (num5 >= list2.Count)
						{
							num5 = 0;
						}
						continue;
					}
					break;
				}
				return;
			}
			Log.Warning("Failed to place all leavings for destroyed thing " + diedThing + " at " + leavingsRect.CenterCell);
		}

		public static void DoLeavingsFor(TerrainDef terrain, IntVec3 cell, Map map)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
				List<ThingCountClass> list = terrain.CostListAdjusted(null, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingCountClass thingCountClass = list[i];
					int num = GenMath.RoundRandom((float)thingCountClass.count * terrain.resourcesFractionWhenDeconstructed);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(thingCountClass.thingDef, null);
						thing.stackCount = num;
						thingOwner.TryAdd(thing, true);
					}
				}
				while (true)
				{
					if (thingOwner.Count > 0)
					{
						Thing thing2 = default(Thing);
						if (!thingOwner.TryDrop(thingOwner[0], cell, map, ThingPlaceMode.Near, out thing2, (Action<Thing, int>)null))
							break;
						continue;
					}
					return;
				}
				Log.Warning("Failed to place all leavings for removed terrain " + terrain + " at " + cell);
			}
		}

		public static bool CanBuildingLeaveResources(Thing diedThing, DestroyMode mode)
		{
			if (!(diedThing is Building))
			{
				return false;
			}
			if (mode == DestroyMode.KillFinalize && !diedThing.def.leaveResourcesWhenKilled)
			{
				return false;
			}
			switch (mode)
			{
			case DestroyMode.Vanish:
				return false;
			case DestroyMode.KillFinalize:
				return true;
			case DestroyMode.Deconstruct:
				return diedThing.def.resourcesFractionWhenDeconstructed != 0.0;
			case DestroyMode.Cancel:
				return true;
			case DestroyMode.FailConstruction:
				return true;
			case DestroyMode.Refund:
				return true;
			default:
				throw new ArgumentException("Unknown destroy mode " + mode);
			}
		}

		public static Func<int, int> GetBuildingResourcesLeaveCalculator(Thing diedThing, DestroyMode mode)
		{
			if (!GenLeaving.CanBuildingLeaveResources(diedThing, mode))
			{
				return (int count) => 0;
			}
			switch (mode)
			{
			case DestroyMode.Vanish:
				return (int count) => 0;
			case DestroyMode.KillFinalize:
				return (int count) => GenMath.RoundRandom((float)((float)count * 0.5));
			case DestroyMode.Deconstruct:
				return (int count) => GenMath.RoundRandom(Mathf.Min((float)count * diedThing.def.resourcesFractionWhenDeconstructed, (float)(count - 1)));
			case DestroyMode.Cancel:
				return (int count) => GenMath.RoundRandom((float)((float)count * 1.0));
			case DestroyMode.FailConstruction:
				return (int count) => GenMath.RoundRandom((float)((float)count * 0.5));
			case DestroyMode.Refund:
				return (int count) => count;
			default:
				throw new ArgumentException("Unknown destroy mode " + mode);
			}
		}

		public static void DropFilthDueToDamage(Thing t, float damageDealt)
		{
			if (t.def.useHitPoints && t.Spawned && t.def.filthLeaving != null)
			{
				CellRect cellRect = t.OccupiedRect().ExpandedBy(1);
				GenLeaving.tmpCellsCandidates.Clear();
				foreach (IntVec3 item in cellRect)
				{
					if (item.InBounds(t.Map) && item.Walkable(t.Map))
					{
						GenLeaving.tmpCellsCandidates.Add(item);
					}
				}
				if (GenLeaving.tmpCellsCandidates.Any())
				{
					int num = GenMath.RoundRandom(damageDealt * Mathf.Min(0.0166666675f, (float)(1.0 / ((float)t.MaxHitPoints / 10.0))));
					for (int i = 0; i < num; i++)
					{
						FilthMaker.MakeFilth(GenLeaving.tmpCellsCandidates.RandomElement(), t.Map, t.def.filthLeaving, 1);
					}
				}
			}
		}
	}
}
