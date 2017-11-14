using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EdgeThing : SymbolResolver
	{
		private List<int> randomRotations = new List<int>
		{
			0,
			1,
			2,
			3
		};

		private int MaxTriesToAvoidOtherEdgeThings = 4;

		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingDef != null)
			{
				bool? edgeThingAvoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings;
				bool avoidOtherEdgeThings = edgeThingAvoidOtherEdgeThings.HasValue && edgeThingAvoidOtherEdgeThings.Value;
				IntVec3 intVec = default(IntVec3);
				if (rp.thingRot.HasValue)
				{
					if (!this.TryFindSpawnCell(rp.rect, rp.singleThingDef, rp.thingRot.Value, avoidOtherEdgeThings, out intVec))
					{
						return false;
					}
				}
				else if (!rp.singleThingDef.rotatable)
				{
					if (!this.TryFindSpawnCell(rp.rect, rp.singleThingDef, Rot4.North, avoidOtherEdgeThings, out intVec))
					{
						return false;
					}
				}
				else
				{
					bool flag = false;
					int num = 0;
					while (num < 4)
					{
						if (!this.TryFindSpawnCell(rp.rect, rp.singleThingDef, new Rot4(num), avoidOtherEdgeThings, out intVec))
						{
							num++;
							continue;
						}
						flag = true;
						break;
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.singleThingDef ?? DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
			{
				int result;
				if ((x.IsWeapon || x.IsMedicine || x.IsDrug) && x.graphicData != null && !x.destroyOnDrop && x.size.x <= rp.rect.Width && x.size.z <= rp.rect.Width && x.size.x <= rp.rect.Height)
				{
					result = ((x.size.z <= rp.rect.Height) ? 1 : 0);
					goto IL_00ba;
				}
				result = 0;
				goto IL_00ba;
				IL_00ba:
				return (byte)result != 0;
			}).RandomElement();
			IntVec3 invalid = IntVec3.Invalid;
			Rot4 value = Rot4.North;
			bool? edgeThingAvoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings;
			bool avoidOtherEdgeThings = edgeThingAvoidOtherEdgeThings.HasValue && edgeThingAvoidOtherEdgeThings.Value;
			if (rp.thingRot.HasValue)
			{
				if (this.TryFindSpawnCell(rp.rect, thingDef, rp.thingRot.Value, avoidOtherEdgeThings, out invalid))
				{
					value = rp.thingRot.Value;
					goto IL_017c;
				}
				return;
			}
			if (!thingDef.rotatable)
			{
				if (this.TryFindSpawnCell(rp.rect, thingDef, Rot4.North, avoidOtherEdgeThings, out invalid))
				{
					value = Rot4.North;
					goto IL_017c;
				}
				return;
			}
			this.randomRotations.Shuffle();
			bool flag = false;
			int num = 0;
			while (num < this.randomRotations.Count)
			{
				if (!this.TryFindSpawnCell(rp.rect, thingDef, new Rot4(this.randomRotations[num]), avoidOtherEdgeThings, out invalid))
				{
					num++;
					continue;
				}
				value = new Rot4(this.randomRotations[num]);
				flag = true;
				break;
			}
			if (!flag)
				return;
			goto IL_017c;
			IL_017c:
			ResolveParams resolveParams = rp;
			resolveParams.rect = CellRect.SingleCell(invalid);
			resolveParams.thingRot = value;
			resolveParams.singleThingDef = thingDef;
			BaseGen.symbolStack.Push("thing", resolveParams);
		}

		private bool TryFindSpawnCell(CellRect rect, ThingDef thingDef, Rot4 rot, bool avoidOtherEdgeThings, out IntVec3 spawnCell)
		{
			if (avoidOtherEdgeThings)
			{
				spawnCell = IntVec3.Invalid;
				int num = -1;
				for (int i = 0; i < this.MaxTriesToAvoidOtherEdgeThings; i++)
				{
					IntVec3 intVec = default(IntVec3);
					if (this.TryFindSpawnCell(rect, thingDef, rot, out intVec))
					{
						int distanceSquaredToExistingEdgeThing = this.GetDistanceSquaredToExistingEdgeThing(intVec, rect, thingDef);
						if (!spawnCell.IsValid || distanceSquaredToExistingEdgeThing > num)
						{
							spawnCell = intVec;
							num = distanceSquaredToExistingEdgeThing;
							if (num == 2147483647)
								break;
						}
					}
				}
				return spawnCell.IsValid;
			}
			return this.TryFindSpawnCell(rect, thingDef, rot, out spawnCell);
		}

		private bool TryFindSpawnCell(CellRect rect, ThingDef thingDef, Rot4 rot, out IntVec3 spawnCell)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec3 zero = IntVec3.Zero;
			IntVec2 size = thingDef.size;
			GenAdj.AdjustForRotation(ref zero, ref size, rot);
			CellRect empty = CellRect.Empty;
			Predicate<CellRect> basePredicate = (CellRect x) => x.Cells.All((IntVec3 y) => y.Standable(map)) && !GenSpawn.WouldWipeAnythingWith(x, thingDef, map, (Thing z) => z.def.category == ThingCategory.Building) && (thingDef.category != ThingCategory.Item || x.CenterCell.GetFirstItem(map) == null);
			bool flag = false;
			if (thingDef.category == ThingCategory.Building)
			{
				flag = rect.TryFindRandomInnerRectTouchingEdge(size, out empty, (Predicate<CellRect>)((CellRect x) => basePredicate(x) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(x, map) && GenConstruct.TerrainCanSupport(x, map, thingDef)));
				if (!flag)
				{
					flag = rect.TryFindRandomInnerRectTouchingEdge(size, out empty, (Predicate<CellRect>)((CellRect x) => basePredicate(x) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(x, map)));
				}
			}
			if (!flag && !rect.TryFindRandomInnerRectTouchingEdge(size, out empty, basePredicate))
			{
				spawnCell = IntVec3.Invalid;
				return false;
			}
			CellRect.CellRectIterator iterator = empty.GetIterator();
			while (!iterator.Done())
			{
				if (GenAdj.OccupiedRect(iterator.Current, rot, thingDef.size) == empty)
				{
					spawnCell = iterator.Current;
					return true;
				}
				iterator.MoveNext();
			}
			Log.Error("We found a valid rect but we couldn't find the root position. This should never happen.");
			spawnCell = IntVec3.Invalid;
			return false;
		}

		private int GetDistanceSquaredToExistingEdgeThing(IntVec3 cell, CellRect rect, ThingDef thingDef)
		{
			Map map = BaseGen.globalSettings.map;
			int num = 2147483647;
			foreach (IntVec3 edgeCell in rect.EdgeCells)
			{
				List<Thing> thingList = edgeCell.GetThingList(map);
				bool flag = false;
				int num2 = 0;
				while (num2 < thingList.Count)
				{
					if (thingList[num2].def != thingDef)
					{
						num2++;
						continue;
					}
					flag = true;
					break;
				}
				if (flag)
				{
					num = Mathf.Min(num, cell.DistanceToSquared(edgeCell));
				}
			}
			return num;
		}
	}
}
