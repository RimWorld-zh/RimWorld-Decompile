using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B2 RID: 946
	public class SymbolResolver_ExtraDoor : SymbolResolver
	{
		// Token: 0x06001069 RID: 4201 RVA: 0x0008AAA4 File Offset: 0x00088EA4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec3 intVec = IntVec3.Invalid;
			int num = -1;
			for (int i = 0; i < 4; i++)
			{
				if (!this.WallHasDoor(rp.rect, new Rot4(i)))
				{
					for (int j = 0; j < 2; j++)
					{
						IntVec3 intVec2;
						if (this.TryFindRandomDoorSpawnCell(rp.rect, new Rot4(i), out intVec2))
						{
							int distanceToExistingDoors = this.GetDistanceToExistingDoors(intVec2, rp.rect);
							if (!intVec.IsValid || distanceToExistingDoors > num)
							{
								intVec = intVec2;
								num = distanceToExistingDoors;
								if (num == 2147483647)
								{
									break;
								}
							}
						}
					}
				}
			}
			if (intVec.IsValid)
			{
				ThingDef thingDef;
				if ((thingDef = rp.wallStuff) == null)
				{
					thingDef = (BaseGenUtility.WallStuffAt(intVec, map) ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff = thingDef;
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, stuff);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, intVec, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0008ABD4 File Offset: 0x00088FD4
		private bool WallHasDoor(CellRect rect, Rot4 dir)
		{
			Map map = BaseGen.globalSettings.map;
			foreach (IntVec3 c in rect.GetEdgeCells(dir))
			{
				if (c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0008AC54 File Offset: 0x00089054
		private bool TryFindRandomDoorSpawnCell(CellRect rect, Rot4 dir, out IntVec3 found)
		{
			Map map = BaseGen.globalSettings.map;
			bool result;
			int newZ2;
			if (dir == Rot4.North)
			{
				int newX;
				if (rect.Width <= 2)
				{
					found = IntVec3.Invalid;
					result = false;
				}
				else if (!Rand.TryRangeInclusiveWhere(rect.minX + 1, rect.maxX - 1, delegate(int x)
				{
					IntVec3 c = new IntVec3(x, 0, rect.maxZ + 1);
					IntVec3 c2 = new IntVec3(x, 0, rect.maxZ - 1);
					return c.InBounds(map) && c.Standable(map) && c2.InBounds(map) && c2.Standable(map);
				}, out newX))
				{
					found = IntVec3.Invalid;
					result = false;
				}
				else
				{
					found = new IntVec3(newX, 0, rect.maxZ);
					result = true;
				}
			}
			else if (dir == Rot4.South)
			{
				int newX2;
				if (rect.Width <= 2)
				{
					found = IntVec3.Invalid;
					result = false;
				}
				else if (!Rand.TryRangeInclusiveWhere(rect.minX + 1, rect.maxX - 1, delegate(int x)
				{
					IntVec3 c = new IntVec3(x, 0, rect.minZ - 1);
					IntVec3 c2 = new IntVec3(x, 0, rect.minZ + 1);
					return c.InBounds(map) && c.Standable(map) && c2.InBounds(map) && c2.Standable(map);
				}, out newX2))
				{
					found = IntVec3.Invalid;
					result = false;
				}
				else
				{
					found = new IntVec3(newX2, 0, rect.minZ);
					result = true;
				}
			}
			else if (dir == Rot4.West)
			{
				int newZ;
				if (rect.Height <= 2)
				{
					found = IntVec3.Invalid;
					result = false;
				}
				else if (!Rand.TryRangeInclusiveWhere(rect.minZ + 1, rect.maxZ - 1, delegate(int z)
				{
					IntVec3 c = new IntVec3(rect.minX - 1, 0, z);
					IntVec3 c2 = new IntVec3(rect.minX + 1, 0, z);
					return c.InBounds(map) && c.Standable(map) && c2.InBounds(map) && c2.Standable(map);
				}, out newZ))
				{
					found = IntVec3.Invalid;
					result = false;
				}
				else
				{
					found = new IntVec3(rect.minX, 0, newZ);
					result = true;
				}
			}
			else if (rect.Height <= 2)
			{
				found = IntVec3.Invalid;
				result = false;
			}
			else if (!Rand.TryRangeInclusiveWhere(rect.minZ + 1, rect.maxZ - 1, delegate(int z)
			{
				IntVec3 c = new IntVec3(rect.maxX + 1, 0, z);
				IntVec3 c2 = new IntVec3(rect.maxX - 1, 0, z);
				return c.InBounds(map) && c.Standable(map) && c2.InBounds(map) && c2.Standable(map);
			}, out newZ2))
			{
				found = IntVec3.Invalid;
				result = false;
			}
			else
			{
				found = new IntVec3(rect.maxX, 0, newZ2);
				result = true;
			}
			return result;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0008AEC4 File Offset: 0x000892C4
		private int GetDistanceToExistingDoors(IntVec3 cell, CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			int num = int.MaxValue;
			foreach (IntVec3 c in rect.EdgeCells)
			{
				if (c.GetDoor(map) != null)
				{
					num = Mathf.Min(num, Mathf.Abs(cell.x - c.x) + Mathf.Abs(cell.z - c.z));
				}
			}
			return num;
		}
	}
}
