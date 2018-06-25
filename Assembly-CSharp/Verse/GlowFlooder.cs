using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C15 RID: 3093
	public class GlowFlooder
	{
		// Token: 0x04002E31 RID: 11825
		private Map map;

		// Token: 0x04002E32 RID: 11826
		private GlowFlooder.GlowFloodCell[] calcGrid;

		// Token: 0x04002E33 RID: 11827
		private FastPriorityQueue<int> openSet;

		// Token: 0x04002E34 RID: 11828
		private uint statusUnseenValue = 0u;

		// Token: 0x04002E35 RID: 11829
		private uint statusOpenValue = 1u;

		// Token: 0x04002E36 RID: 11830
		private uint statusFinalizedValue = 2u;

		// Token: 0x04002E37 RID: 11831
		private int mapSizeX;

		// Token: 0x04002E38 RID: 11832
		private int mapSizeZ;

		// Token: 0x04002E39 RID: 11833
		private CompGlower glower;

		// Token: 0x04002E3A RID: 11834
		private CellIndices cellIndices;

		// Token: 0x04002E3B RID: 11835
		private Color32[] glowGrid;

		// Token: 0x04002E3C RID: 11836
		private float attenLinearSlope;

		// Token: 0x04002E3D RID: 11837
		private Thing[] blockers = new Thing[8];

		// Token: 0x04002E3E RID: 11838
		private static readonly sbyte[,] Directions = new sbyte[,]
		{
			{
				0,
				-1
			},
			{
				1,
				0
			},
			{
				0,
				1
			},
			{
				-1,
				0
			},
			{
				1,
				-1
			},
			{
				1,
				1
			},
			{
				-1,
				1
			},
			{
				-1,
				-1
			}
		};

		// Token: 0x060043A0 RID: 17312 RVA: 0x0023BEC4 File Offset: 0x0023A2C4
		public GlowFlooder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new GlowFlooder.GlowFloodCell[this.mapSizeX * this.mapSizeZ];
			this.openSet = new FastPriorityQueue<int>(new GlowFlooder.CompareGlowFlooderLightSquares(this.calcGrid));
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0023BF58 File Offset: 0x0023A358
		public void AddFloodGlowFor(CompGlower theGlower, Color32[] glowGrid)
		{
			this.cellIndices = this.map.cellIndices;
			this.glowGrid = glowGrid;
			this.glower = theGlower;
			this.attenLinearSlope = -1f / theGlower.Props.glowRadius;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			IntVec3 position = theGlower.parent.Position;
			int num = Mathf.RoundToInt(this.glower.Props.glowRadius * 100f);
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
					this.map.debugDrawer.FlashCell(c, (float)this.calcGrid[num2].intDist / 10f, this.calcGrid[num2].intDist.ToString("F2"), 50);
					num3++;
				}
				for (int i = 0; i < 8; i++)
				{
					uint num4 = (uint)(c.x + (int)GlowFlooder.Directions[i, 0]);
					uint num5 = (uint)(c.z + (int)GlowFlooder.Directions[i, 1]);
					if ((ulong)num4 < (ulong)((long)this.mapSizeX) && (ulong)num5 < (ulong)((long)this.mapSizeZ))
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
									goto IL_3B3;
								}
								this.blockers[i] = null;
							}
							int num7;
							if (i < 4)
							{
								num7 = 100;
							}
							else
							{
								num7 = 141;
							}
							int num8 = this.calcGrid[num2].intDist + num7;
							if (num8 <= num)
							{
								if (i >= 4)
								{
									switch (i)
									{
									case 4:
										if (this.blockers[0] != null && this.blockers[1] != null)
										{
											goto IL_3B3;
										}
										break;
									case 5:
										if (this.blockers[1] != null && this.blockers[2] != null)
										{
											goto IL_3B3;
										}
										break;
									case 6:
										if (this.blockers[2] != null && this.blockers[3] != null)
										{
											goto IL_3B3;
										}
										break;
									case 7:
										if (this.blockers[0] != null && this.blockers[3] != null)
										{
											goto IL_3B3;
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
					IL_3B3:;
				}
			}
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x0023C32C File Offset: 0x0023A72C
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

		// Token: 0x060043A3 RID: 17315 RVA: 0x0023C3AC File Offset: 0x0023A7AC
		private void SetGlowGridFromDist(int index)
		{
			float num = (float)this.calcGrid[index].intDist / 100f;
			ColorInt colB = default(ColorInt);
			if (num <= this.glower.Props.glowRadius)
			{
				float b = 1f / (num * num);
				float a = 1f + this.attenLinearSlope * num;
				float b2 = Mathf.Lerp(a, b, 0.4f);
				colB = this.glower.Props.glowColor * b2;
			}
			if (colB.r > 0 || colB.g > 0 || colB.b > 0)
			{
				colB.ClampToNonNegative();
				ColorInt colA = this.glowGrid[index].AsColorInt();
				colA += colB;
				if (num < this.glower.Props.overlightRadius)
				{
					colA.a = 1;
				}
				Color32 toColor = colA.ToColor32;
				this.glowGrid[index] = toColor;
			}
		}

		// Token: 0x02000C16 RID: 3094
		private struct GlowFloodCell
		{
			// Token: 0x04002E3F RID: 11839
			public int intDist;

			// Token: 0x04002E40 RID: 11840
			public uint status;
		}

		// Token: 0x02000C17 RID: 3095
		private class CompareGlowFlooderLightSquares : IComparer<int>
		{
			// Token: 0x04002E41 RID: 11841
			private GlowFlooder.GlowFloodCell[] grid;

			// Token: 0x060043A5 RID: 17317 RVA: 0x0023C4D9 File Offset: 0x0023A8D9
			public CompareGlowFlooderLightSquares(GlowFlooder.GlowFloodCell[] grid)
			{
				this.grid = grid;
			}

			// Token: 0x060043A6 RID: 17318 RVA: 0x0023C4EC File Offset: 0x0023A8EC
			public int Compare(int a, int b)
			{
				return this.grid[a].intDist.CompareTo(this.grid[b].intDist);
			}
		}
	}
}
