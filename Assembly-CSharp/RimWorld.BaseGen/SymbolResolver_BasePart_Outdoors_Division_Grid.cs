using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Outdoors_Division_Grid : SymbolResolver
	{
		private class Child
		{
			public CellRect rect;

			public int gridX;

			public int gridY;

			public bool merged;
		}

		private const int MinWidthOrHeight = 13;

		private const int MinRoomsPerRow = 2;

		private const int MaxRoomsPerRow = 4;

		private const int MinPathwayWidth = 1;

		private const int MaxPathwayWidth = 5;

		private const int MinRoomSize = 6;

		private const float AllowNonSquareRoomsInTheFirstStepChance = 0.2f;

		private List<Pair<int, int>> optionsX = new List<Pair<int, int>>();

		private List<Pair<int, int>> optionsZ = new List<Pair<int, int>>();

		private List<Child> children = new List<Child>();

		private static List<Pair<Pair<int, int>, Pair<int, int>>> options = new List<Pair<Pair<int, int>, Pair<int, int>>>();

		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.rect.Width < 13 && rp.rect.Height < 13)
			{
				return false;
			}
			this.FillOptions(rp.rect);
			return this.optionsX.Any() && this.optionsZ.Any();
		}

		public override void Resolve(ResolveParams rp)
		{
			this.FillOptions(rp.rect);
			if ((Rand.Chance(0.2f) || (!this.TryResolveRandomOption(0, 0, rp) && !this.TryResolveRandomOption(0, 1, rp))) && !this.TryResolveRandomOption(1, 0, rp) && !this.TryResolveRandomOption(2, 0, rp) && !this.TryResolveRandomOption(2, 1, rp) && !this.TryResolveRandomOption(999999, 999999, rp))
			{
				Log.Warning("Grid resolver could not resolve any grid size. params=" + rp);
			}
		}

		private void FillOptions(CellRect rect)
		{
			this.FillOptions(this.optionsX, rect.Width);
			this.FillOptions(this.optionsZ, rect.Height);
			if (this.optionsZ.Any((Predicate<Pair<int, int>>)((Pair<int, int> x) => x.First > 1)))
			{
				this.optionsX.RemoveAll((Predicate<Pair<int, int>>)((Pair<int, int> x) => x.First >= 3 && this.GetRoomSize(x.First, x.Second, rect.Width) <= 7));
			}
			if (this.optionsX.Any((Predicate<Pair<int, int>>)((Pair<int, int> x) => x.First > 1)))
			{
				this.optionsZ.RemoveAll((Predicate<Pair<int, int>>)((Pair<int, int> x) => x.First >= 3 && this.GetRoomSize(x.First, x.Second, rect.Height) <= 7));
			}
		}

		private void FillOptions(List<Pair<int, int>> outOptions, int length)
		{
			outOptions.Clear();
			for (int i = 2; i <= 4; i++)
			{
				for (int j = 1; j <= 5; j++)
				{
					int roomSize = this.GetRoomSize(i, j, length);
					if (roomSize != -1 && roomSize >= 6 && roomSize >= 2 * j - 1)
					{
						outOptions.Add(new Pair<int, int>(i, j));
					}
				}
			}
		}

		private int GetRoomSize(int roomsPerRow, int pathwayWidth, int totalLength)
		{
			int num = totalLength - (roomsPerRow - 1) * pathwayWidth;
			if (num % roomsPerRow != 0)
			{
				return -1;
			}
			return num / roomsPerRow;
		}

		private bool TryResolveRandomOption(int maxWidthHeightDiff, int maxPathwayWidthDiff, ResolveParams rp)
		{
			SymbolResolver_BasePart_Outdoors_Division_Grid.options.Clear();
			for (int i = 0; i < this.optionsX.Count; i++)
			{
				int first = this.optionsX[i].First;
				int second = this.optionsX[i].Second;
				int roomSize = this.GetRoomSize(first, second, rp.rect.Width);
				for (int j = 0; j < this.optionsZ.Count; j++)
				{
					int first2 = this.optionsZ[j].First;
					int second2 = this.optionsZ[j].Second;
					int roomSize2 = this.GetRoomSize(first2, second2, rp.rect.Height);
					if (Mathf.Abs(roomSize - roomSize2) <= maxWidthHeightDiff && Mathf.Abs(second - second2) <= maxPathwayWidthDiff)
					{
						SymbolResolver_BasePart_Outdoors_Division_Grid.options.Add(new Pair<Pair<int, int>, Pair<int, int>>(this.optionsX[i], this.optionsZ[j]));
					}
				}
			}
			if (SymbolResolver_BasePart_Outdoors_Division_Grid.options.Any())
			{
				Pair<Pair<int, int>, Pair<int, int>> pair = SymbolResolver_BasePart_Outdoors_Division_Grid.options.RandomElement();
				this.ResolveOption(pair.First.First, pair.First.Second, pair.Second.First, pair.Second.Second, rp);
				return true;
			}
			return false;
		}

		private void ResolveOption(int roomsPerRowX, int pathwayWidthX, int roomsPerRowZ, int pathwayWidthZ, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			int roomSize = this.GetRoomSize(roomsPerRowX, pathwayWidthX, rp.rect.Width);
			int roomSize2 = this.GetRoomSize(roomsPerRowZ, pathwayWidthZ, rp.rect.Height);
			ThingDef thingDef = null;
			if (pathwayWidthX >= 3)
			{
				thingDef = ((rp.faction != null && (int)rp.faction.def.techLevel < 4) ? ThingDefOf.TorchLamp : ThingDefOf.StandingLamp);
			}
			TerrainDef floorDef = rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			int num = roomSize;
			for (int i = 0; i < roomsPerRowX - 1; i++)
			{
				CellRect rect = new CellRect(rp.rect.minX + num, rp.rect.minZ, pathwayWidthX, rp.rect.Height);
				ResolveParams resolveParams = rp;
				resolveParams.rect = rect;
				resolveParams.floorDef = floorDef;
				resolveParams.streetHorizontal = new bool?(false);
				BaseGen.symbolStack.Push("street", resolveParams);
				num += roomSize + pathwayWidthX;
			}
			int num2 = roomSize2;
			for (int j = 0; j < roomsPerRowZ - 1; j++)
			{
				CellRect rect2 = new CellRect(rp.rect.minX, rp.rect.minZ + num2, rp.rect.Width, pathwayWidthZ);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect2;
				resolveParams2.floorDef = floorDef;
				resolveParams2.streetHorizontal = new bool?(true);
				BaseGen.symbolStack.Push("street", resolveParams2);
				num2 += roomSize2 + pathwayWidthZ;
			}
			num = 0;
			num2 = 0;
			this.children.Clear();
			for (int num3 = 0; num3 < roomsPerRowX; num3++)
			{
				for (int num4 = 0; num4 < roomsPerRowZ; num4++)
				{
					Child child = new Child();
					child.rect = new CellRect(rp.rect.minX + num, rp.rect.minZ + num2, roomSize, roomSize2);
					child.gridX = num3;
					child.gridY = num4;
					this.children.Add(child);
					num2 += roomSize2 + pathwayWidthZ;
				}
				num += roomSize + pathwayWidthX;
				num2 = 0;
			}
			this.MergeRandomChildren();
			this.children.Shuffle();
			for (int k = 0; k < this.children.Count; k++)
			{
				if (thingDef != null)
				{
					IntVec3 c = new IntVec3(this.children[k].rect.maxX + 1, 0, this.children[k].rect.maxZ);
					if (rp.rect.Contains(c) && c.Standable(map))
					{
						ResolveParams resolveParams3 = rp;
						resolveParams3.rect = CellRect.SingleCell(c);
						resolveParams3.singleThingDef = thingDef;
						BaseGen.symbolStack.Push("thing", resolveParams3);
					}
				}
				ResolveParams resolveParams4 = rp;
				resolveParams4.rect = this.children[k].rect;
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams4);
			}
		}

		private void MergeRandomChildren()
		{
			if (this.children.Count >= 4)
			{
				int num = GenMath.RoundRandom((float)((float)this.children.Count / 6.0));
				int num2 = 0;
				while (num2 < num)
				{
					Child child = this.children.Find((Predicate<Child>)((Child x) => !x.merged));
					if (child != null)
					{
						Child child2 = this.children.Find((Predicate<Child>)((Child x) => x != child && ((Mathf.Abs(x.gridX - child.gridX) == 1 && x.gridY == child.gridY) || (Mathf.Abs(x.gridY - child.gridY) == 1 && x.gridX == child.gridX))));
						if (child2 != null)
						{
							this.children.Remove(child);
							this.children.Remove(child2);
							Child child3 = new Child();
							child3.gridX = Mathf.Min(child.gridX, child2.gridX);
							child3.gridY = Mathf.Min(child.gridY, child2.gridY);
							child3.merged = true;
							child3.rect = CellRect.FromLimits(Mathf.Min(child.rect.minX, child2.rect.minX), Mathf.Min(child.rect.minZ, child2.rect.minZ), Mathf.Max(child.rect.maxX, child2.rect.maxX), Mathf.Max(child.rect.maxZ, child2.rect.maxZ));
							this.children.Add(child3);
						}
						num2++;
						continue;
					}
					break;
				}
			}
		}
	}
}
