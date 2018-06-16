using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003AB RID: 939
	public class SymbolResolver_EdgeThing : SymbolResolver
	{
		// Token: 0x0600104B RID: 4171 RVA: 0x00089280 File Offset: 0x00087680
		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
			}
			else
			{
				if (rp.singleThingDef != null)
				{
					bool? edgeThingAvoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings;
					bool avoidOtherEdgeThings = edgeThingAvoidOtherEdgeThings != null && edgeThingAvoidOtherEdgeThings.Value;
					if (rp.thingRot != null)
					{
						IntVec3 intVec;
						if (!this.TryFindSpawnCell(rp.rect, rp.singleThingDef, rp.thingRot.Value, avoidOtherEdgeThings, out intVec))
						{
							return false;
						}
					}
					else if (!rp.singleThingDef.rotatable)
					{
						IntVec3 intVec;
						if (!this.TryFindSpawnCell(rp.rect, rp.singleThingDef, Rot4.North, avoidOtherEdgeThings, out intVec))
						{
							return false;
						}
					}
					else
					{
						bool flag = false;
						for (int i = 0; i < 4; i++)
						{
							IntVec3 intVec;
							if (this.TryFindSpawnCell(rp.rect, rp.singleThingDef, new Rot4(i), avoidOtherEdgeThings, out intVec))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000893B8 File Offset: 0x000877B8
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.singleThingDef ?? (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where (x.IsWeapon || x.IsMedicine || x.IsDrug) && x.graphicData != null && !x.destroyOnDrop && x.size.x <= rp.rect.Width && x.size.z <= rp.rect.Width && x.size.x <= rp.rect.Height && x.size.z <= rp.rect.Height
			select x).RandomElement<ThingDef>();
			IntVec3 invalid = IntVec3.Invalid;
			Rot4 value = Rot4.North;
			bool? edgeThingAvoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings;
			bool avoidOtherEdgeThings = edgeThingAvoidOtherEdgeThings != null && edgeThingAvoidOtherEdgeThings.Value;
			if (rp.thingRot != null)
			{
				if (!this.TryFindSpawnCell(rp.rect, thingDef, rp.thingRot.Value, avoidOtherEdgeThings, out invalid))
				{
					return;
				}
				value = rp.thingRot.Value;
			}
			else if (!thingDef.rotatable)
			{
				if (!this.TryFindSpawnCell(rp.rect, thingDef, Rot4.North, avoidOtherEdgeThings, out invalid))
				{
					return;
				}
				value = Rot4.North;
			}
			else
			{
				this.randomRotations.Shuffle<int>();
				bool flag = false;
				for (int i = 0; i < this.randomRotations.Count; i++)
				{
					if (this.TryFindSpawnCell(rp.rect, thingDef, new Rot4(this.randomRotations[i]), avoidOtherEdgeThings, out invalid))
					{
						value = new Rot4(this.randomRotations[i]);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			ResolveParams rp2 = rp;
			rp2.rect = CellRect.SingleCell(invalid);
			rp2.thingRot = new Rot4?(value);
			rp2.singleThingDef = thingDef;
			BaseGen.symbolStack.Push("thing", rp2);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00089594 File Offset: 0x00087994
		private bool TryFindSpawnCell(CellRect rect, ThingDef thingDef, Rot4 rot, bool avoidOtherEdgeThings, out IntVec3 spawnCell)
		{
			bool result;
			if (avoidOtherEdgeThings)
			{
				spawnCell = IntVec3.Invalid;
				int num = -1;
				for (int i = 0; i < this.MaxTriesToAvoidOtherEdgeThings; i++)
				{
					IntVec3 intVec;
					if (this.TryFindSpawnCell(rect, thingDef, rot, out intVec))
					{
						int distanceSquaredToExistingEdgeThing = this.GetDistanceSquaredToExistingEdgeThing(intVec, rect, thingDef);
						if (!spawnCell.IsValid || distanceSquaredToExistingEdgeThing > num)
						{
							spawnCell = intVec;
							num = distanceSquaredToExistingEdgeThing;
							if (num == 2147483647)
							{
								break;
							}
						}
					}
				}
				result = spawnCell.IsValid;
			}
			else
			{
				result = this.TryFindSpawnCell(rect, thingDef, rot, out spawnCell);
			}
			return result;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00089640 File Offset: 0x00087A40
		private bool TryFindSpawnCell(CellRect rect, ThingDef thingDef, Rot4 rot, out IntVec3 spawnCell)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec3 zero = IntVec3.Zero;
			IntVec2 size = thingDef.size;
			GenAdj.AdjustForRotation(ref zero, ref size, rot);
			CellRect empty = CellRect.Empty;
			Predicate<CellRect> basePredicate = delegate(CellRect x)
			{
				if (x.Cells.All((IntVec3 y) => y.Standable(map)))
				{
					if (!GenSpawn.WouldWipeAnythingWith(x, thingDef, map, (Thing z) => z.def.category == ThingCategory.Building))
					{
						return thingDef.category != ThingCategory.Item || x.CenterCell.GetFirstItem(map) == null;
					}
				}
				return false;
			};
			bool flag = false;
			if (thingDef.category == ThingCategory.Building)
			{
				flag = rect.TryFindRandomInnerRectTouchingEdge(size, out empty, (CellRect x) => basePredicate(x) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(x, map) && GenConstruct.TerrainCanSupport(x, map, thingDef));
				if (!flag)
				{
					flag = rect.TryFindRandomInnerRectTouchingEdge(size, out empty, (CellRect x) => basePredicate(x) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(x, map));
				}
			}
			bool result;
			if (!flag && !rect.TryFindRandomInnerRectTouchingEdge(size, out empty, basePredicate))
			{
				spawnCell = IntVec3.Invalid;
				result = false;
			}
			else
			{
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
				Log.Error("We found a valid rect but we couldn't find the root position. This should never happen.", false);
				spawnCell = IntVec3.Invalid;
				result = false;
			}
			return result;
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0008979C File Offset: 0x00087B9C
		private int GetDistanceSquaredToExistingEdgeThing(IntVec3 cell, CellRect rect, ThingDef thingDef)
		{
			Map map = BaseGen.globalSettings.map;
			int num = int.MaxValue;
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				List<Thing> thingList = intVec.GetThingList(map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def == thingDef)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					num = Mathf.Min(num, cell.DistanceToSquared(intVec));
				}
			}
			return num;
		}

		// Token: 0x04000A15 RID: 2581
		private List<int> randomRotations = new List<int>
		{
			0,
			1,
			2,
			3
		};

		// Token: 0x04000A16 RID: 2582
		private int MaxTriesToAvoidOtherEdgeThings = 4;
	}
}
