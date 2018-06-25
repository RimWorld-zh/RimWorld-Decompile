using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200053E RID: 1342
	public class WorldPath : IDisposable
	{
		// Token: 0x04000EB9 RID: 3769
		private List<int> nodes = new List<int>(128);

		// Token: 0x04000EBA RID: 3770
		private float totalCostInt = 0f;

		// Token: 0x04000EBB RID: 3771
		private int curNodeIndex;

		// Token: 0x04000EBC RID: 3772
		public bool inUse = false;

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x000DA4F4 File Offset: 0x000D88F4
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600190E RID: 6414 RVA: 0x000DA51C File Offset: 0x000D891C
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600190F RID: 6415 RVA: 0x000DA538 File Offset: 0x000D8938
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001910 RID: 6416 RVA: 0x000DA558 File Offset: 0x000D8958
		public List<int> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001911 RID: 6417 RVA: 0x000DA574 File Offset: 0x000D8974
		public int FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001912 RID: 6418 RVA: 0x000DA5A4 File Offset: 0x000D89A4
		public int LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001913 RID: 6419 RVA: 0x000DA5C8 File Offset: 0x000D89C8
		public static WorldPath NotFound
		{
			get
			{
				return WorldPathPool.NotFoundPath;
			}
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x000DA5E2 File Offset: 0x000D89E2
		public void AddNodeAtStart(int tile)
		{
			this.nodes.Add(tile);
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x000DA5F4 File Offset: 0x000D89F4
		public void SetupFound(float totalCost)
		{
			if (this == WorldPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on WorldPath.NotFound", false);
			}
			else
			{
				this.totalCostInt = totalCost;
				this.curNodeIndex = this.nodes.Count - 1;
			}
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x000DA648 File Offset: 0x000D8A48
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x000DA651 File Offset: 0x000D8A51
		public void ReleaseToPool()
		{
			if (this != WorldPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x000DA680 File Offset: 0x000D8A80
		public static WorldPath NewNotFound()
		{
			return new WorldPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x000DA6A8 File Offset: 0x000D8AA8
		public int ConsumeNextNode()
		{
			int result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x000DA6D4 File Offset: 0x000D8AD4
		public int Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x000DA6FC File Offset: 0x000D8AFC
		public override string ToString()
		{
			string result;
			if (!this.Found)
			{
				result = "WorldPath(not found)";
			}
			else if (!this.inUse)
			{
				result = "WorldPath(not in use)";
			}
			else
			{
				result = string.Concat(new object[]
				{
					"WorldPath(nodeCount= ",
					this.nodes.Count,
					(this.nodes.Count <= 0) ? "" : string.Concat(new object[]
					{
						" first=",
						this.FirstNode,
						" last=",
						this.LastNode
					}),
					" cost=",
					this.totalCostInt,
					" )"
				});
			}
			return result;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x000DA7D8 File Offset: 0x000D8BD8
		public void DrawPath(Caravan pathingCaravan)
		{
			if (this.Found)
			{
				if (this.NodesLeftCount > 0)
				{
					WorldGrid worldGrid = Find.WorldGrid;
					float d = 0.05f;
					for (int i = 0; i < this.NodesLeftCount - 1; i++)
					{
						Vector3 a = worldGrid.GetTileCenter(this.Peek(i));
						Vector3 vector = worldGrid.GetTileCenter(this.Peek(i + 1));
						a += a.normalized * d;
						vector += vector.normalized * d;
						GenDraw.DrawWorldLineBetween(a, vector);
					}
					if (pathingCaravan != null)
					{
						Vector3 a2 = pathingCaravan.DrawPos;
						Vector3 vector2 = worldGrid.GetTileCenter(this.Peek(0));
						a2 += a2.normalized * d;
						vector2 += vector2.normalized * d;
						if ((a2 - vector2).sqrMagnitude > 0.005f)
						{
							GenDraw.DrawWorldLineBetween(a2, vector2);
						}
					}
				}
			}
		}
	}
}
