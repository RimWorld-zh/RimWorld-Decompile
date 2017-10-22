using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Street : SymbolResolver
	{
		private static List<bool> street = new List<bool>();

		public override void Resolve(ResolveParams rp)
		{
			bool? streetHorizontal = rp.streetHorizontal;
			bool flag = (!streetHorizontal.HasValue) ? (rp.rect.Width >= rp.rect.Height) : streetHorizontal.Value;
			int width = (!flag) ? rp.rect.Width : rp.rect.Height;
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			this.CalculateStreet(rp.rect, flag, floorDef);
			this.FillStreetGaps(flag, width);
			this.RemoveShortStreetParts(flag, width);
			this.SpawnFloor(rp.rect, flag, floorDef);
		}

		private void CalculateStreet(CellRect rect, bool horizontal, TerrainDef floorDef)
		{
			SymbolResolver_Street.street.Clear();
			int num = (!horizontal) ? rect.Height : rect.Width;
			for (int num2 = 0; num2 < num; num2++)
			{
				if (horizontal)
				{
					SymbolResolver_Street.street.Add(this.CausesStreet(new IntVec3(rect.minX + num2, 0, rect.minZ - 1), floorDef) && this.CausesStreet(new IntVec3(rect.minX + num2, 0, rect.maxZ + 1), floorDef));
				}
				else
				{
					SymbolResolver_Street.street.Add(this.CausesStreet(new IntVec3(rect.minX - 1, 0, rect.minZ + num2), floorDef) && this.CausesStreet(new IntVec3(rect.maxX + 1, 0, rect.minZ + num2), floorDef));
				}
			}
		}

		private void FillStreetGaps(bool horizontal, int width)
		{
			int num = -1;
			for (int i = 0; i < SymbolResolver_Street.street.Count; i++)
			{
				if (SymbolResolver_Street.street[i])
				{
					num = i;
				}
				else if (num != -1 && i - num <= width)
				{
					int num2 = i + 1;
					while (num2 < i + width + 1 && num2 < SymbolResolver_Street.street.Count)
					{
						if (!SymbolResolver_Street.street[num2])
						{
							num2++;
							continue;
						}
						SymbolResolver_Street.street[i] = true;
						break;
					}
				}
			}
		}

		private void RemoveShortStreetParts(bool horizontal, int width)
		{
			for (int i = 0; i < SymbolResolver_Street.street.Count; i++)
			{
				if (SymbolResolver_Street.street[i])
				{
					int num = 0;
					int num2 = i;
					while (num2 < SymbolResolver_Street.street.Count && SymbolResolver_Street.street[num2])
					{
						num++;
						num2++;
					}
					int num3 = 0;
					int num4 = i;
					while (num4 >= 0 && SymbolResolver_Street.street[num4])
					{
						num3++;
						num4--;
					}
					int num5 = num3 + num - 1;
					if (num5 < width)
					{
						SymbolResolver_Street.street[i] = false;
					}
				}
			}
		}

		private void SpawnFloor(CellRect rect, bool horizontal, TerrainDef floorDef)
		{
			Map map = BaseGen.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			CellRect.CellRectIterator iterator = rect.GetIterator();
			for (; !iterator.Done(); iterator.MoveNext())
			{
				IntVec3 current = iterator.Current;
				if (horizontal && SymbolResolver_Street.street[current.x - rect.minX])
				{
					goto IL_0071;
				}
				if (!horizontal && SymbolResolver_Street.street[current.z - rect.minZ])
					goto IL_0071;
				continue;
				IL_0071:
				terrainGrid.SetTerrain(current, floorDef);
			}
		}

		private bool CausesStreet(IntVec3 c, TerrainDef floorDef)
		{
			Map map = BaseGen.globalSettings.map;
			bool result;
			if (!c.InBounds(map))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				result = ((byte)((edifice != null && edifice.def == ThingDefOf.Wall) ? 1 : ((c.GetDoor(map) != null) ? 1 : ((c.GetTerrain(map) == floorDef) ? 1 : 0))) != 0);
			}
			return result;
		}
	}
}
