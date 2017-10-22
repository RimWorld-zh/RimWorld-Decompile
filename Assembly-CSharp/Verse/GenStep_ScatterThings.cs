using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class GenStep_ScatterThings : GenStep_Scatterer
	{
		public ThingDef thingDef;

		public ThingDef stuff;

		public int clearSpaceSize;

		public int clusterSize = 1;

		public float terrainValidationRadius = 0f;

		[NoTranslate]
		private List<string> terrainValidationDisallowed;

		[Unsaved]
		private IntVec3 clusterCenter;

		[Unsaved]
		private int leftInCluster = 0;

		private const int ClusterRadius = 4;

		private List<Rot4> possibleRotationsInt;

		private static List<Rot4> tmpRotations = new List<Rot4>();

		private List<Rot4> PossibleRotations
		{
			get
			{
				if (this.possibleRotationsInt == null)
				{
					this.possibleRotationsInt = new List<Rot4>();
					if (this.thingDef.rotatable)
					{
						this.possibleRotationsInt.Add(Rot4.North);
						this.possibleRotationsInt.Add(Rot4.East);
						this.possibleRotationsInt.Add(Rot4.South);
						this.possibleRotationsInt.Add(Rot4.West);
					}
					else
					{
						this.possibleRotationsInt.Add(Rot4.North);
					}
				}
				return this.possibleRotationsInt;
			}
		}

		public override void Generate(Map map)
		{
			if (!base.allowOnWater && map.TileInfo.WaterCovered)
				return;
			int count = base.CalculateFinalCount(map);
			IntRange stackSizeRange = (this.thingDef.ingestible == null || !this.thingDef.ingestible.IsMeal || this.thingDef.stackLimit > 10) ? ((this.thingDef.stackLimit <= 5) ? new IntRange(this.thingDef.stackLimit, this.thingDef.stackLimit) : new IntRange(Mathf.RoundToInt((float)((float)this.thingDef.stackLimit * 0.5)), this.thingDef.stackLimit)) : IntRange.one;
			List<int> list = GenStep_ScatterThings.CountDividedIntoStacks(count, stackSizeRange);
			int num = 0;
			while (num < list.Count)
			{
				IntVec3 intVec = default(IntVec3);
				if (((GenStep_Scatterer)this).TryFindScatterCell(map, out intVec))
				{
					this.ScatterAt(intVec, map, list[num]);
					base.usedSpots.Add(intVec);
					num++;
					continue;
				}
				return;
			}
			base.usedSpots.Clear();
			this.clusterCenter = IntVec3.Invalid;
			this.leftInCluster = 0;
		}

		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			bool result2;
			if (this.clusterSize > 1)
			{
				if (this.leftInCluster <= 0)
				{
					if (!base.TryFindScatterCell(map, out this.clusterCenter))
					{
						Log.Error("Could not find cluster center to scatter " + this.thingDef);
					}
					this.leftInCluster = this.clusterSize;
				}
				this.leftInCluster--;
				Rot4 rot = default(Rot4);
				result = CellFinder.RandomClosewalkCellNear(this.clusterCenter, map, 4, (Predicate<IntVec3>)((IntVec3 x) => this.TryGetRandomValidRotation(x, map, out rot)));
				result2 = result.IsValid;
			}
			else
			{
				result2 = base.TryFindScatterCell(map, out result);
			}
			return result2;
		}

		protected override void ScatterAt(IntVec3 loc, Map map, int stackCount = 1)
		{
			Rot4 rot = default(Rot4);
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				Log.Warning("Could not find any valid rotation for " + this.thingDef);
			}
			else
			{
				if (this.clearSpaceSize > 0)
				{
					foreach (IntVec3 item in GridShapeMaker.IrregularLump(loc, map, this.clearSpaceSize))
					{
						Building edifice = item.GetEdifice(map);
						if (edifice != null)
						{
							edifice.Destroy(DestroyMode.Vanish);
						}
					}
				}
				Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
				if (this.thingDef.Minifiable)
				{
					thing = thing.MakeMinified();
				}
				if (thing.def.category == ThingCategory.Item)
				{
					thing.stackCount = stackCount;
					thing.SetForbidden(true, false);
					Thing thing2 = default(Thing);
					GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Near, out thing2, (Action<Thing, int>)null);
					if (base.nearPlayerStart && thing2 != null && thing2.def.category == ThingCategory.Item && TutorSystem.TutorialMode)
					{
						Find.TutorialState.AddStartingItem(thing2);
					}
				}
				else
				{
					GenSpawn.Spawn(thing, loc, map, rot, false);
				}
			}
		}

		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			bool result;
			Rot4 rot = default(Rot4);
			if (!base.CanScatterAt(loc, map))
			{
				result = false;
			}
			else if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				result = false;
			}
			else
			{
				if (this.terrainValidationRadius > 0.0)
				{
					foreach (IntVec3 item in GenRadial.RadialCellsAround(loc, this.terrainValidationRadius, true))
					{
						if (item.InBounds(map))
						{
							TerrainDef terrain = item.GetTerrain(map);
							for (int i = 0; i < this.terrainValidationDisallowed.Count; i++)
							{
								if (terrain.HasTag(this.terrainValidationDisallowed[i]))
								{
									return false;
								}
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		private bool TryGetRandomValidRotation(IntVec3 loc, Map map, out Rot4 rot)
		{
			List<Rot4> possibleRotations = this.PossibleRotations;
			for (int i = 0; i < possibleRotations.Count; i++)
			{
				if (this.IsRotationValid(loc, possibleRotations[i], map))
				{
					GenStep_ScatterThings.tmpRotations.Add(possibleRotations[i]);
				}
			}
			bool result;
			if (((IEnumerable<Rot4>)GenStep_ScatterThings.tmpRotations).TryRandomElement<Rot4>(out rot))
			{
				GenStep_ScatterThings.tmpRotations.Clear();
				result = true;
			}
			else
			{
				rot = Rot4.Invalid;
				result = false;
			}
			return result;
		}

		private bool IsRotationValid(IntVec3 loc, Rot4 rot, Map map)
		{
			return (byte)(GenAdj.OccupiedRect(loc, rot, this.thingDef.size).InBounds(map) ? ((!GenSpawn.WouldWipeAnythingWith(loc, rot, this.thingDef, map, (Predicate<Thing>)((Thing x) => x.def == this.thingDef || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth)))) ? 1 : 0) : 0) != 0;
		}

		public static List<int> CountDividedIntoStacks(int count, IntRange stackSizeRange)
		{
			List<int> list = new List<int>();
			while (count > 0)
			{
				int num = Mathf.Min(count, stackSizeRange.RandomInRange);
				count -= num;
				list.Add(num);
			}
			if (stackSizeRange.max > 2)
			{
				for (int i = 0; i < list.Count * 4; i++)
				{
					int num2 = Rand.RangeInclusive(0, list.Count - 1);
					int num3 = Rand.RangeInclusive(0, list.Count - 1);
					if (num2 != num3 && list[num2] > list[num3])
					{
						int num4 = (int)((float)(list[num2] - list[num3]) * Rand.Value);
						int index;
						List<int> list2;
						(list2 = list)[index = num2] = list2[index] - num4;
						int index2;
						(list2 = list)[index2 = num3] = list2[index2] + num4;
					}
				}
			}
			return list;
		}
	}
}
