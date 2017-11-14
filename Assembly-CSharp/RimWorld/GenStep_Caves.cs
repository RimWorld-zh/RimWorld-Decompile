using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	public class GenStep_Caves : GenStep
	{
		private ModuleBase directionNoise;

		private static HashSet<IntVec3> tmpGroupSet = new HashSet<IntVec3>();

		private const float OpenTunnelsPer10k = 5.8f;

		private const float ClosedTunnelsPer10k = 2.5f;

		private const int MaxOpenTunnelsPerRockGroup = 3;

		private const int MaxClosedTunnelsPerRockGroup = 1;

		private const float DirectionChangeSpeed = 8f;

		private const float DirectionNoiseFrequency = 0.00205f;

		private const int MinRocksToGenerateAnyTunnel = 300;

		private const int AllowBranchingAfterThisManyCells = 15;

		private const float MinTunnelWidth = 1.4f;

		private const float WidthOffsetPerCell = 0.034f;

		private const float BranchChance = 0.1f;

		private static readonly FloatRange BranchedTunnelWidthOffset = new FloatRange(0.2f, 0.4f);

		private static readonly SimpleCurve TunnelsWidthPerRockCount = new SimpleCurve
		{
			{
				new CurvePoint(100f, 2f),
				true
			},
			{
				new CurvePoint(300f, 4f),
				true
			},
			{
				new CurvePoint(3000f, 5.5f),
				true
			}
		};

		private static List<IntVec3> tmpCells = new List<IntVec3>();

		private static HashSet<IntVec3> groupSet = new HashSet<IntVec3>();

		private static HashSet<IntVec3> groupVisited = new HashSet<IntVec3>();

		private static List<IntVec3> subGroup = new List<IntVec3>();

		public override void Generate(Map map)
		{
			if (Find.World.HasCaves(map.Tile))
			{
				this.directionNoise = new Perlin(0.0020500000100582838, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
				MapGenFloatGrid elevation = MapGenerator.Elevation;
				BoolGrid visited = new BoolGrid(map);
				List<IntVec3> group = new List<IntVec3>();
				foreach (IntVec3 allCell in map.AllCells)
				{
					if (!visited[allCell] && this.IsRock(allCell, elevation, map))
					{
						group.Clear();
						map.floodFiller.FloodFill(allCell, (IntVec3 x) => this.IsRock(x, elevation, map), delegate(IntVec3 x)
						{
							visited[x] = true;
							group.Add(x);
						}, 2147483647, false, null);
						this.Trim(group, map);
						this.RemoveSmallDisconnectedSubGroups(group, map);
						if (group.Count >= 300)
						{
							this.DoOpenTunnels(group, map);
							this.DoClosedTunnels(group, map);
						}
					}
				}
			}
		}

		private void Trim(List<IntVec3> group, Map map)
		{
			GenMorphology.Open(group, 6, map);
		}

		private bool IsRock(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return c.InBounds(map) && elevation[c] > 0.699999988079071;
		}

		private void DoOpenTunnels(List<IntVec3> group, Map map)
		{
			int a = GenMath.RoundRandom((float)((float)group.Count * Rand.Range(0.9f, 1.1f) * 5.8000001907348633 / 10000.0));
			a = Mathf.Min(a, 3);
			if (a > 0)
			{
				a = Rand.RangeInclusive(1, a);
			}
			float num = GenStep_Caves.TunnelsWidthPerRockCount.Evaluate((float)group.Count);
			for (int i = 0; i < a; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num2 = -1f;
				float dir = -1f;
				float num3 = -1f;
				for (int j = 0; j < 10; j++)
				{
					IntVec3 intVec = this.FindRandomEdgeCellForTunnel(group, map);
					float distToCave = this.GetDistToCave(intVec, group, map, 40f, false);
					float num4 = default(float);
					float num5 = this.FindBestInitialDir(intVec, group, out num4);
					if (!start.IsValid || distToCave > num2 || (distToCave == num2 && num4 > num3))
					{
						start = intVec;
						num2 = distToCave;
						dir = num5;
						num3 = num4;
					}
				}
				float width = Rand.Range((float)(num * 0.800000011920929), num);
				this.Dig(start, dir, width, group, map, false, null);
			}
		}

		private void DoClosedTunnels(List<IntVec3> group, Map map)
		{
			int a = GenMath.RoundRandom((float)((float)group.Count * Rand.Range(0.9f, 1.1f) * 2.5 / 10000.0));
			a = Mathf.Min(a, 1);
			if (a > 0)
			{
				a = Rand.RangeInclusive(0, a);
			}
			float num = GenStep_Caves.TunnelsWidthPerRockCount.Evaluate((float)group.Count);
			for (int i = 0; i < a; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num2 = -1f;
				for (int j = 0; j < 7; j++)
				{
					IntVec3 intVec = group.RandomElement();
					float distToCave = this.GetDistToCave(intVec, group, map, 30f, true);
					if (!start.IsValid || distToCave > num2)
					{
						start = intVec;
						num2 = distToCave;
					}
				}
				float width = Rand.Range((float)(num * 0.800000011920929), num);
				this.Dig(start, Rand.Range(0f, 360f), width, group, map, true, null);
			}
		}

		private IntVec3 FindRandomEdgeCellForTunnel(List<IntVec3> group, Map map)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			GenStep_Caves.tmpCells.Clear();
			GenStep_Caves.tmpGroupSet.Clear();
			GenStep_Caves.tmpGroupSet.AddRange(group);
			for (int i = 0; i < group.Count; i++)
			{
				if (group[i].DistanceToEdge(map) >= 3 && !(caves[group[i]] > 0.0))
				{
					for (int j = 0; j < 4; j++)
					{
						IntVec3 item = group[i] + cardinalDirections[j];
						if (!GenStep_Caves.tmpGroupSet.Contains(item))
						{
							GenStep_Caves.tmpCells.Add(group[i]);
							break;
						}
					}
				}
			}
			if (!GenStep_Caves.tmpCells.Any())
			{
				Log.Warning("Could not find any valid edge cell.");
				return group.RandomElement();
			}
			return GenStep_Caves.tmpCells.RandomElement();
		}

		private float FindBestInitialDir(IntVec3 start, List<IntVec3> group, out float dist)
		{
			float num = (float)this.GetDistToNonRock(start, group, IntVec3.East, 40);
			float num2 = (float)this.GetDistToNonRock(start, group, IntVec3.West, 40);
			float num3 = (float)this.GetDistToNonRock(start, group, IntVec3.South, 40);
			float num4 = (float)this.GetDistToNonRock(start, group, IntVec3.North, 40);
			float num5 = (float)this.GetDistToNonRock(start, group, IntVec3.NorthWest, 40);
			float num6 = (float)this.GetDistToNonRock(start, group, IntVec3.NorthEast, 40);
			float num7 = (float)this.GetDistToNonRock(start, group, IntVec3.SouthWest, 40);
			float num8 = (float)this.GetDistToNonRock(start, group, IntVec3.SouthEast, 40);
			dist = Mathf.Max(num, num2, num3, num4, num5, num6, num7, num8);
			return GenMath.MaxByRandomIfEqual(0f, (float)(num + num8 / 2.0 + num6 / 2.0), 45f, (float)(num8 + num3 / 2.0 + num / 2.0), 90f, (float)(num3 + num8 / 2.0 + num7 / 2.0), 135f, (float)(num7 + num3 / 2.0 + num2 / 2.0), 180f, (float)(num2 + num7 / 2.0 + num5 / 2.0), 225f, (float)(num5 + num4 / 2.0 + num2 / 2.0), 270f, (float)(num4 + num6 / 2.0 + num5 / 2.0), 315f, (float)(num6 + num4 / 2.0 + num / 2.0));
		}

		private void Dig(IntVec3 start, float dir, float width, List<IntVec3> group, Map map, bool closed, HashSet<IntVec3> visited = null)
		{
			Vector3 vector = start.ToVector3Shifted();
			IntVec3 intVec = start;
			float num = 0f;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			bool flag = false;
			bool flag2 = false;
			if (visited == null)
			{
				visited = new HashSet<IntVec3>();
			}
			GenStep_Caves.tmpGroupSet.Clear();
			GenStep_Caves.tmpGroupSet.AddRange(group);
			int num2 = 0;
			while (true)
			{
				if (closed)
				{
					int num3 = GenRadial.NumCellsInRadius((float)(width / 2.0 + 1.5));
					for (int i = 0; i < num3; i++)
					{
						IntVec3 intVec2 = intVec + GenRadial.RadialPattern[i];
						if (!visited.Contains(intVec2))
						{
							if (!GenStep_Caves.tmpGroupSet.Contains(intVec2))
								return;
							if (caves[intVec2] > 0.0)
								return;
						}
					}
				}
				if (num2 >= 15)
				{
					float num4 = width;
					FloatRange branchedTunnelWidthOffset = GenStep_Caves.BranchedTunnelWidthOffset;
					if (num4 > 1.3999999761581421 + branchedTunnelWidthOffset.max)
					{
						if (!flag && Rand.Chance(0.1f))
						{
							this.DigInBestDirection(intVec, dir, new FloatRange(40f, 90f), width - GenStep_Caves.BranchedTunnelWidthOffset.RandomInRange, group, map, closed, visited);
							flag = true;
						}
						if (!flag2 && Rand.Chance(0.1f))
						{
							this.DigInBestDirection(intVec, dir, new FloatRange(-90f, -40f), width - GenStep_Caves.BranchedTunnelWidthOffset.RandomInRange, group, map, closed, visited);
							flag2 = true;
						}
					}
				}
				bool flag3 = default(bool);
				this.SetCaveAround(intVec, width, map, visited, out flag3);
				if (!flag3)
				{
					while (vector.ToIntVec3() == intVec)
					{
						vector += Vector3Utility.FromAngleFlat(dir) * 0.5f;
						num = (float)(num + 0.5);
					}
					if (GenStep_Caves.tmpGroupSet.Contains(vector.ToIntVec3()))
					{
						int x = intVec.x;
						IntVec3 intVec3 = vector.ToIntVec3();
						IntVec3 intVec4 = new IntVec3(x, 0, intVec3.z);
						if (this.IsRock(intVec4, elevation, map))
						{
							caves[intVec4] = Mathf.Max(caves[intVec4], width);
							visited.Add(intVec4);
						}
						intVec = vector.ToIntVec3();
						dir = (float)(dir + (float)this.directionNoise.GetValue(num * 60.0, (float)start.x * 200.0, (float)start.z * 200.0) * 8.0);
						width = (float)(width - 0.034000001847743988);
						if (!(width < 1.3999999761581421))
						{
							num2++;
							continue;
						}
					}
				}
				break;
			}
		}

		private void DigInBestDirection(IntVec3 curIntVec, float curDir, FloatRange dirOffset, float width, List<IntVec3> group, Map map, bool closed, HashSet<IntVec3> visited = null)
		{
			int num = -1;
			float dir = -1f;
			for (int i = 0; i < 6; i++)
			{
				float num2 = curDir + dirOffset.RandomInRange;
				int distToNonRock = this.GetDistToNonRock(curIntVec, group, num2, 50);
				if (distToNonRock > num)
				{
					num = distToNonRock;
					dir = num2;
				}
			}
			if (num >= 18)
			{
				this.Dig(curIntVec, dir, width, group, map, closed, visited);
			}
		}

		private void SetCaveAround(IntVec3 around, float tunnelWidth, Map map, HashSet<IntVec3> visited, out bool hitAnotherTunnel)
		{
			hitAnotherTunnel = false;
			int num = GenRadial.NumCellsInRadius((float)(tunnelWidth / 2.0));
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = around + GenRadial.RadialPattern[i];
				if (this.IsRock(intVec, elevation, map))
				{
					if (caves[intVec] > 0.0 && !visited.Contains(intVec))
					{
						hitAnotherTunnel = true;
					}
					caves[intVec] = Mathf.Max(caves[intVec], tunnelWidth);
					visited.Add(intVec);
				}
			}
		}

		private int GetDistToNonRock(IntVec3 from, List<IntVec3> group, IntVec3 offset, int maxDist)
		{
			GenStep_Caves.groupSet.Clear();
			GenStep_Caves.groupSet.AddRange(group);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = from + offset * i;
				if (!GenStep_Caves.groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		private int GetDistToNonRock(IntVec3 from, List<IntVec3> group, float dir, int maxDist)
		{
			GenStep_Caves.groupSet.Clear();
			GenStep_Caves.groupSet.AddRange(group);
			Vector3 a = Vector3Utility.FromAngleFlat(dir);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = (from.ToVector3Shifted() + a * (float)i).ToIntVec3();
				if (!GenStep_Caves.groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		private float GetDistToCave(IntVec3 cell, List<IntVec3> group, Map map, float maxDist, bool treatOpenSpaceAsCave)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			GenStep_Caves.tmpGroupSet.Clear();
			GenStep_Caves.tmpGroupSet.AddRange(group);
			int num = GenRadial.NumCellsInRadius(maxDist);
			IntVec3[] radialPattern = GenRadial.RadialPattern;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + radialPattern[i];
				if (treatOpenSpaceAsCave && !GenStep_Caves.tmpGroupSet.Contains(intVec))
				{
					goto IL_007b;
				}
				if (intVec.InBounds(map) && caves[intVec] > 0.0)
					goto IL_007b;
				continue;
				IL_007b:
				return cell.DistanceTo(intVec);
			}
			return maxDist;
		}

		private void RemoveSmallDisconnectedSubGroups(List<IntVec3> group, Map map)
		{
			GenStep_Caves.groupSet.Clear();
			GenStep_Caves.groupSet.AddRange(group);
			GenStep_Caves.groupVisited.Clear();
			for (int i = 0; i < group.Count; i++)
			{
				if (!GenStep_Caves.groupVisited.Contains(group[i]) && GenStep_Caves.groupSet.Contains(group[i]))
				{
					GenStep_Caves.subGroup.Clear();
					map.floodFiller.FloodFill(group[i], (IntVec3 x) => GenStep_Caves.groupSet.Contains(x), delegate(IntVec3 x)
					{
						GenStep_Caves.subGroup.Add(x);
						GenStep_Caves.groupVisited.Add(x);
					}, 2147483647, false, null);
					if (GenStep_Caves.subGroup.Count < 300 || (float)GenStep_Caves.subGroup.Count < 0.05000000074505806 * (float)group.Count)
					{
						for (int j = 0; j < GenStep_Caves.subGroup.Count; j++)
						{
							GenStep_Caves.groupSet.Remove(GenStep_Caves.subGroup[j]);
						}
					}
				}
			}
			group.Clear();
			group.AddRange(GenStep_Caves.groupSet);
		}
	}
}
