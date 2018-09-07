using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class WatchBuildingUtility
	{
		private static List<int> allowedDirections = new List<int>();

		public static IEnumerable<IntVec3> CalculateWatchCells(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			List<int> allowedDirections = WatchBuildingUtility.CalculateAllowedDirections(def, rot);
			for (int i = 0; i < allowedDirections.Count; i++)
			{
				foreach (IntVec3 c in WatchBuildingUtility.GetWatchCellRect(def, center, rot, allowedDirections[i]))
				{
					if (WatchBuildingUtility.EverPossibleToWatchFrom(c, center, map, true))
					{
						yield return c;
					}
				}
			}
			yield break;
		}

		public static bool TryFindBestWatchCell(Thing toWatch, Pawn pawn, bool desireSit, out IntVec3 result, out Building chair)
		{
			List<int> list = WatchBuildingUtility.CalculateAllowedDirections(toWatch.def, toWatch.Rotation);
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < list.Count; i++)
			{
				CellRect watchCellRect = WatchBuildingUtility.GetWatchCellRect(toWatch.def, toWatch.Position, toWatch.Rotation, list[i]);
				IntVec3 centerCell = watchCellRect.CenterCell;
				int num = watchCellRect.Area * 4;
				for (int j = 0; j < num; j++)
				{
					IntVec3 intVec2 = centerCell + GenRadial.RadialPattern[j];
					if (watchCellRect.Contains(intVec2))
					{
						bool flag = false;
						Building building = null;
						if (WatchBuildingUtility.EverPossibleToWatchFrom(intVec2, toWatch.Position, toWatch.Map, false) && !intVec2.IsForbidden(pawn) && pawn.CanReserve(intVec2, 1, -1, null, false) && pawn.Map.pawnDestinationReservationManager.CanReserve(intVec2, pawn, false))
						{
							if (desireSit)
							{
								building = intVec2.GetEdifice(pawn.Map);
								if (building != null && building.def.building.isSittable && pawn.CanReserve(building, 1, -1, null, false))
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
						if (flag)
						{
							if (desireSit)
							{
								Rot4 rotation = building.Rotation;
								Rot4 rot = new Rot4(list[i]);
								if (rotation != rot.Opposite)
								{
									intVec = intVec2;
									goto IL_17F;
								}
							}
							result = intVec2;
							chair = building;
							return true;
						}
					}
					IL_17F:;
				}
			}
			if (intVec.IsValid)
			{
				result = intVec;
				chair = intVec.GetEdifice(pawn.Map);
				return true;
			}
			result = IntVec3.Invalid;
			chair = null;
			return false;
		}

		public static bool CanWatchFromBed(Pawn pawn, Building_Bed bed, Thing toWatch)
		{
			if (!WatchBuildingUtility.EverPossibleToWatchFrom(pawn.Position, toWatch.Position, pawn.Map, true))
			{
				return false;
			}
			if (toWatch.def.rotatable)
			{
				Rot4 rotation = bed.Rotation;
				CellRect cellRect = toWatch.OccupiedRect();
				if (rotation == Rot4.North && cellRect.maxZ < pawn.Position.z)
				{
					return false;
				}
				if (rotation == Rot4.South && cellRect.minZ > pawn.Position.z)
				{
					return false;
				}
				if (rotation == Rot4.East && cellRect.maxX < pawn.Position.x)
				{
					return false;
				}
				if (rotation == Rot4.West && cellRect.minX > pawn.Position.x)
				{
					return false;
				}
			}
			List<int> list = WatchBuildingUtility.CalculateAllowedDirections(toWatch.def, toWatch.Rotation);
			for (int i = 0; i < list.Count; i++)
			{
				if (WatchBuildingUtility.GetWatchCellRect(toWatch.def, toWatch.Position, toWatch.Rotation, list[i]).Contains(pawn.Position))
				{
					return true;
				}
			}
			return false;
		}

		private static CellRect GetWatchCellRect(ThingDef def, IntVec3 center, Rot4 rot, int watchRot)
		{
			Rot4 a = new Rot4(watchRot);
			if (def.building == null)
			{
				def = (def.entityDefToBuild as ThingDef);
			}
			CellRect result;
			if (a.IsHorizontal)
			{
				int num = center.x + GenAdj.CardinalDirections[watchRot].x * def.building.watchBuildingStandDistanceRange.min;
				int num2 = center.x + GenAdj.CardinalDirections[watchRot].x * def.building.watchBuildingStandDistanceRange.max;
				int num3 = center.z + def.building.watchBuildingStandRectWidth / 2;
				int num4 = center.z - def.building.watchBuildingStandRectWidth / 2;
				if (def.building.watchBuildingStandRectWidth % 2 == 0)
				{
					if (a == Rot4.West)
					{
						num4++;
					}
					else
					{
						num3--;
					}
				}
				result = new CellRect(Mathf.Min(num, num2), num4, Mathf.Abs(num - num2) + 1, num3 - num4 + 1);
			}
			else
			{
				int num5 = center.z + GenAdj.CardinalDirections[watchRot].z * def.building.watchBuildingStandDistanceRange.min;
				int num6 = center.z + GenAdj.CardinalDirections[watchRot].z * def.building.watchBuildingStandDistanceRange.max;
				int num7 = center.x + def.building.watchBuildingStandRectWidth / 2;
				int num8 = center.x - def.building.watchBuildingStandRectWidth / 2;
				if (def.building.watchBuildingStandRectWidth % 2 == 0)
				{
					if (a == Rot4.North)
					{
						num8++;
					}
					else
					{
						num7--;
					}
				}
				result = new CellRect(num8, Mathf.Min(num5, num6), num7 - num8 + 1, Mathf.Abs(num5 - num6) + 1);
			}
			return result;
		}

		private static bool EverPossibleToWatchFrom(IntVec3 watchCell, IntVec3 buildingCenter, Map map, bool bedAllowed)
		{
			return (watchCell.Standable(map) || (bedAllowed && watchCell.GetEdifice(map) is Building_Bed)) && GenSight.LineOfSight(buildingCenter, watchCell, map, true, null, 0, 0);
		}

		private static List<int> CalculateAllowedDirections(ThingDef toWatchDef, Rot4 toWatchRot)
		{
			WatchBuildingUtility.allowedDirections.Clear();
			if (toWatchDef.rotatable)
			{
				WatchBuildingUtility.allowedDirections.Add(toWatchRot.AsInt);
			}
			else
			{
				WatchBuildingUtility.allowedDirections.Add(0);
				WatchBuildingUtility.allowedDirections.Add(1);
				WatchBuildingUtility.allowedDirections.Add(2);
				WatchBuildingUtility.allowedDirections.Add(3);
			}
			return WatchBuildingUtility.allowedDirections;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static WatchBuildingUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <CalculateWatchCells>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal ThingDef def;

			internal Rot4 rot;

			internal List<int> <allowedDirections>__0;

			internal int <i>__1;

			internal IntVec3 center;

			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__2;

			internal Map map;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CalculateWatchCells>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					allowedDirections = WatchBuildingUtility.CalculateAllowedDirections(def, rot);
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						while (enumerator.MoveNext())
						{
							c = enumerator.Current;
							if (WatchBuildingUtility.EverPossibleToWatchFrom(c, center, map, true))
							{
								this.$current = c;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					i++;
					break;
				default:
					return false;
				}
				if (i < allowedDirections.Count)
				{
					enumerator = WatchBuildingUtility.GetWatchCellRect(def, center, rot, allowedDirections[i]).GetEnumerator();
					num = 4294967293u;
					goto Block_2;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WatchBuildingUtility.<CalculateWatchCells>c__Iterator0 <CalculateWatchCells>c__Iterator = new WatchBuildingUtility.<CalculateWatchCells>c__Iterator0();
				<CalculateWatchCells>c__Iterator.def = def;
				<CalculateWatchCells>c__Iterator.rot = rot;
				<CalculateWatchCells>c__Iterator.center = center;
				<CalculateWatchCells>c__Iterator.map = map;
				return <CalculateWatchCells>c__Iterator;
			}
		}
	}
}
