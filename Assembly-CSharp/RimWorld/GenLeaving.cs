using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private static Func<ThingDefCountClass, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<int, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<int, int> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<int, int> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<int, int> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<int, int> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<int, int> <>f__am$cache6;

		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode)
		{
			GenLeaving.DoLeavingsFor(diedThing, map, mode, diedThing.OccupiedRect(), null);
		}

		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode, CellRect leavingsRect, Predicate<IntVec3> nearPlaceValidator = null)
		{
			if ((Current.ProgramState == ProgramState.Playing || mode == DestroyMode.Refund) && mode != DestroyMode.Vanish)
			{
				if (mode == DestroyMode.KillFinalize && diedThing.def.filthLeaving != null)
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
						for (int l = frame.resourceContainer.Count - 1; l >= 0; l--)
						{
							int num = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(frame.resourceContainer[l].stackCount);
							if (num > 0)
							{
								frame.resourceContainer.TryTransferToContainer(frame.resourceContainer[l], thingOwner, num, true);
							}
						}
						frame.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
					}
					else
					{
						List<ThingDefCountClass> list = diedThing.CostListAdjusted();
						for (int m = 0; m < list.Count; m++)
						{
							ThingDefCountClass thingDefCountClass = list[m];
							int num2 = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(thingDefCountClass.count);
							if (num2 > 0)
							{
								if (mode == DestroyMode.KillFinalize && thingDefCountClass.thingDef.slagDef != null)
								{
									int count = thingDefCountClass.thingDef.slagDef.smeltProducts.First((ThingDefCountClass pro) => pro.thingDef == ThingDefOf.Steel).count;
									int num3 = num2 / 2 / 8;
									for (int n = 0; n < num3; n++)
									{
										thingOwner.TryAdd(ThingMaker.MakeThing(thingDefCountClass.thingDef.slagDef, null), true);
									}
									num2 -= num3 * count;
								}
							}
							if (num2 > 0)
							{
								Thing thing2 = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
								thing2.stackCount = num2;
								thingOwner.TryAdd(thing2, true);
							}
						}
					}
				}
				List<IntVec3> list2 = leavingsRect.Cells.InRandomOrder(null).ToList<IntVec3>();
				int num4 = 0;
				while (thingOwner.Count > 0)
				{
					if (mode == DestroyMode.KillFinalize && !map.areaManager.Home[list2[num4]])
					{
						thingOwner[0].SetForbidden(true, false);
					}
					ThingOwner<Thing> thingOwner2 = thingOwner;
					Thing thing3 = thingOwner[0];
					IntVec3 dropLoc = list2[num4];
					ThingPlaceMode mode2 = ThingPlaceMode.Near;
					Thing thing4;
					ref Thing lastResultingThing = ref thing4;
					if (!thingOwner2.TryDrop(thing3, dropLoc, map, mode2, out lastResultingThing, null, nearPlaceValidator))
					{
						Log.Warning(string.Concat(new object[]
						{
							"Failed to place all leavings for destroyed thing ",
							diedThing,
							" at ",
							leavingsRect.CenterCell
						}), false);
						break;
					}
					num4++;
					if (num4 >= list2.Count)
					{
						num4 = 0;
					}
				}
			}
		}

		public static void DoLeavingsFor(TerrainDef terrain, IntVec3 cell, Map map)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
				List<ThingDefCountClass> list = terrain.CostListAdjusted(null, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = list[i];
					int num = GenMath.RoundRandom((float)thingDefCountClass.count * terrain.resourcesFractionWhenDeconstructed);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
						thing.stackCount = num;
						thingOwner.TryAdd(thing, true);
					}
				}
				while (thingOwner.Count > 0)
				{
					Thing thing2;
					if (!thingOwner.TryDrop(thingOwner[0], cell, map, ThingPlaceMode.Near, out thing2, null, null))
					{
						Log.Warning(string.Concat(new object[]
						{
							"Failed to place all leavings for removed terrain ",
							terrain,
							" at ",
							cell
						}), false);
						break;
					}
				}
			}
		}

		public static bool CanBuildingLeaveResources(Thing diedThing, DestroyMode mode)
		{
			bool result;
			if (!(diedThing is Building))
			{
				result = false;
			}
			else if (mode == DestroyMode.KillFinalize && !diedThing.def.leaveResourcesWhenKilled)
			{
				result = false;
			}
			else
			{
				switch (mode)
				{
				case DestroyMode.Vanish:
					result = false;
					break;
				case DestroyMode.WillReplace:
					result = false;
					break;
				case DestroyMode.KillFinalize:
					result = true;
					break;
				case DestroyMode.Deconstruct:
					result = (diedThing.def.resourcesFractionWhenDeconstructed != 0f);
					break;
				case DestroyMode.FailConstruction:
					result = true;
					break;
				case DestroyMode.Cancel:
					result = true;
					break;
				case DestroyMode.Refund:
					result = true;
					break;
				default:
					throw new ArgumentException("Unknown destroy mode " + mode);
				}
			}
			return result;
		}

		public static Func<int, int> GetBuildingResourcesLeaveCalculator(Thing diedThing, DestroyMode mode)
		{
			if (GenLeaving.CanBuildingLeaveResources(diedThing, mode))
			{
				switch (mode)
				{
				case DestroyMode.Vanish:
					return (int count) => 0;
				case DestroyMode.KillFinalize:
					return (int count) => GenMath.RoundRandom((float)count * 0.5f);
				case DestroyMode.Deconstruct:
					return (int count) => GenMath.RoundRandom(Mathf.Min((float)count * diedThing.def.resourcesFractionWhenDeconstructed, (float)(count - 1)));
				case DestroyMode.FailConstruction:
					return (int count) => GenMath.RoundRandom((float)count * 0.5f);
				case DestroyMode.Cancel:
					return (int count) => GenMath.RoundRandom((float)count * 1f);
				case DestroyMode.Refund:
					return (int count) => count;
				}
				throw new ArgumentException("Unknown destroy mode " + mode);
			}
			return (int count) => 0;
		}

		public static void DropFilthDueToDamage(Thing t, float damageDealt)
		{
			if (t.def.useHitPoints && t.Spawned && t.def.filthLeaving != null)
			{
				CellRect cellRect = t.OccupiedRect().ExpandedBy(1);
				GenLeaving.tmpCellsCandidates.Clear();
				foreach (IntVec3 intVec in cellRect)
				{
					if (intVec.InBounds(t.Map) && intVec.Walkable(t.Map))
					{
						GenLeaving.tmpCellsCandidates.Add(intVec);
					}
				}
				if (GenLeaving.tmpCellsCandidates.Any<IntVec3>())
				{
					int num = GenMath.RoundRandom(damageDealt * Mathf.Min(0.0166666675f, 1f / ((float)t.MaxHitPoints / 10f)));
					for (int i = 0; i < num; i++)
					{
						FilthMaker.MakeFilth(GenLeaving.tmpCellsCandidates.RandomElement<IntVec3>(), t.Map, t.def.filthLeaving, 1);
					}
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenLeaving()
		{
		}

		[CompilerGenerated]
		private static bool <DoLeavingsFor>m__0(ThingDefCountClass pro)
		{
			return pro.thingDef == ThingDefOf.Steel;
		}

		[CompilerGenerated]
		private static int <GetBuildingResourcesLeaveCalculator>m__1(int count)
		{
			return 0;
		}

		[CompilerGenerated]
		private static int <GetBuildingResourcesLeaveCalculator>m__2(int count)
		{
			return 0;
		}

		[CompilerGenerated]
		private static int <GetBuildingResourcesLeaveCalculator>m__3(int count)
		{
			return GenMath.RoundRandom((float)count * 0.5f);
		}

		[CompilerGenerated]
		private static int <GetBuildingResourcesLeaveCalculator>m__4(int count)
		{
			return GenMath.RoundRandom((float)count * 1f);
		}

		[CompilerGenerated]
		private static int <GetBuildingResourcesLeaveCalculator>m__5(int count)
		{
			return GenMath.RoundRandom((float)count * 0.5f);
		}

		[CompilerGenerated]
		private static int <GetBuildingResourcesLeaveCalculator>m__6(int count)
		{
			return count;
		}

		[CompilerGenerated]
		private sealed class <GetBuildingResourcesLeaveCalculator>c__AnonStorey0
		{
			internal Thing diedThing;

			public <GetBuildingResourcesLeaveCalculator>c__AnonStorey0()
			{
			}

			internal int <>m__0(int count)
			{
				return GenMath.RoundRandom(Mathf.Min((float)count * this.diedThing.def.resourcesFractionWhenDeconstructed, (float)(count - 1)));
			}
		}
	}
}
