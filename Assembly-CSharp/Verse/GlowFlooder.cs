using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class GlowFlooder
	{
		private struct GlowFloodCell
		{
			public int intDist;

			public uint status;
		}

		private class CompareGlowFlooderLightSquares : IComparer<int>
		{
			private GlowFloodCell[] grid;

			public CompareGlowFlooderLightSquares(GlowFloodCell[] grid)
			{
				this.grid = grid;
			}

			public int Compare(int a, int b)
			{
				return this.grid[a].intDist.CompareTo(this.grid[b].intDist);
			}
		}

		private Map map;

		private GlowFloodCell[] calcGrid;

		private FastPriorityQueue<int> openSet;

		private uint statusUnseenValue = 0u;

		private uint statusOpenValue = 1u;

		private uint statusFinalizedValue = 2u;

		private int mapSizeX;

		private int mapSizeZ;

		private CompGlower glower;

		private CellIndices cellIndices;

		private Color32[] glowGrid;

		private float attenLinearSlope;

		private Thing[] blockers = new Thing[8];

		private static readonly sbyte[,] Directions = new sbyte[8, 2]
		{
			{
				(sbyte)0,
				(sbyte)(-1)
			},
			{
				(sbyte)1,
				(sbyte)0
			},
			{
				(sbyte)0,
				(sbyte)1
			},
			{
				(sbyte)(-1),
				(sbyte)0
			},
			{
				(sbyte)1,
				(sbyte)(-1)
			},
			{
				(sbyte)1,
				(sbyte)1
			},
			{
				(sbyte)(-1),
				(sbyte)1
			},
			{
				(sbyte)(-1),
				(sbyte)(-1)
			}
		};

		public GlowFlooder(Map map)
		{
			this.map = map;
			IntVec3 size = map.Size;
			this.mapSizeX = size.x;
			IntVec3 size2 = map.Size;
			this.mapSizeZ = size2.z;
			this.calcGrid = new GlowFloodCell[this.mapSizeX * this.mapSizeZ];
			this.openSet = new FastPriorityQueue<int>(new CompareGlowFlooderLightSquares(this.calcGrid));
		}

		public void AddFloodGlowFor(CompGlower theGlower, Color32[] glowGrid)
		{
			this.cellIndices = this.map.cellIndices;
			this.glowGrid = glowGrid;
			this.glower = theGlower;
			this.attenLinearSlope = (float)(-1.0 / theGlower.Props.glowRadius);
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			IntVec3 position = theGlower.parent.Position;
			int num = Mathf.RoundToInt((float)(this.glower.Props.glowRadius * 100.0));
			int num2 = this.cellIndices.CellToIndex(position);
			int num3 = 0;
			bool flag = UnityData.isDebugBuild && DebugViewSettings.drawGlow;
			this.InitStatusesAndPushStartNode(ref num2, position);
			while (this.openSet.Count != 0)
			{
				num2 = this.openSet.Pop();
				IntVec3 c = this.cellIndices.IndexToCell(num2);
				this.calcGrid[num2].status = this.statusFinalizedValue;
				this.SetGlowGridFromDist(num2);
				if (flag)
				{
					this.map.debugDrawer.FlashCell(c, (float)((float)this.calcGrid[num2].intDist / 10.0), this.calcGrid[num2].intDist.ToString("F2"), 50);
					num3++;
				}
				for (int i = 0; i < 8; i++)
				{
					uint num4 = (uint)(c.x + GlowFlooder.Directions[i, 0]);
					uint num5 = (uint)(c.z + GlowFlooder.Directions[i, 1]);
					if (num4 < this.mapSizeX && num5 < this.mapSizeZ)
					{
						int x = (int)num4;
						int z = (int)num5;
						int num6 = this.cellIndices.CellToIndex(x, z);
						if (this.calcGrid[num6].status != this.statusFinalizedValue)
						{
							this.blockers[i] = innerArray[num6];
							if (this.blockers[i] != null)
							{
								if (this.blockers[i].def.blockLight)
								{
									continue;
								}
								this.blockers[i] = null;
							}
							int num7 = (i >= 4) ? 141 : 100;
							int num8 = this.calcGrid[num2].intDist + num7;
							if (num8 <= num)
							{
								switch (i)
								{
								case 4:
								{
									if (this.blockers[0] != null && this.blockers[1] != null)
									{
										continue;
									}
									break;
								}
								case 5:
								{
									if (this.blockers[1] != null && this.blockers[2] != null)
									{
										continue;
									}
									break;
								}
								case 6:
								{
									if (this.blockers[2] != null && this.blockers[3] != null)
									{
										continue;
									}
									break;
								}
								case 7:
								{
									if (this.blockers[0] != null && this.blockers[3] != null)
									{
										continue;
									}
									break;
								}
								}
								if (this.calcGrid[num6].status <= this.statusUnseenValue)
								{
									this.calcGrid[num6].intDist = 999999;
									this.calcGrid[num6].status = this.statusOpenValue;
								}
								if (num8 < this.calcGrid[num6].intDist)
								{
									this.calcGrid[num6].intDist = num8;
									this.calcGrid[num6].status = this.statusOpenValue;
									this.openSet.Push(num6);
								}
							}
						}
					}
				}
			}
		}

		private void InitStatusesAndPushStartNode(ref int curIndex, IntVec3 start)
		{
			this.statusUnseenValue += 3u;
			this.statusOpenValue += 3u;
			this.statusFinalizedValue += 3u;
			curIndex = this.cellIndices.CellToIndex(start);
			this.openSet.Clear();
			this.calcGrid[curIndex].intDist = 100;
			this.openSet.Clear();
			this.openSet.Push(curIndex);
		}

		private void SetGlowGridFromDist(int index)
		{
			float num = (float)((float)this.calcGrid[index].intDist / 100.0);
			ColorInt colB = default(ColorInt);
			if (num <= this.glower.Props.glowRadius)
			{
				float b = (float)(1.0 / (num * num));
				float a = (float)(1.0 + this.attenLinearSlope * num);
				float b2 = Mathf.Lerp(a, b, 0.4f);
				colB = this.glower.Props.glowColor * b2;
			}
			if (colB.r <= 0 && colB.g <= 0 && colB.b <= 0)
				return;
			colB.ClampToNonNegative();
			ColorInt colA = this.glowGrid[index].AsColorInt();
			colA += colB;
			if (num < this.glower.Props.overlightRadius)
			{
				colA.a = 1;
			}
			Color32 color = this.glowGrid[index] = colA.ToColor32;
		}
	}
}
