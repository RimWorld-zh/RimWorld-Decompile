using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A92 RID: 2706
	public class PawnPath : IDisposable
	{
		// Token: 0x040025F4 RID: 9716
		private List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x040025F5 RID: 9717
		private float totalCostInt = 0f;

		// Token: 0x040025F6 RID: 9718
		private int curNodeIndex;

		// Token: 0x040025F7 RID: 9719
		public bool inUse = false;

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003C26 RID: 15398 RVA: 0x001FCC68 File Offset: 0x001FB068
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x001FCC90 File Offset: 0x001FB090
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003C28 RID: 15400 RVA: 0x001FCCAC File Offset: 0x001FB0AC
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x001FCCCC File Offset: 0x001FB0CC
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x001FCCE8 File Offset: 0x001FB0E8
		public IntVec3 FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x001FCD18 File Offset: 0x001FB118
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x001FCD3C File Offset: 0x001FB13C
		public static PawnPath NotFound
		{
			get
			{
				return PawnPathPool.NotFoundPath;
			}
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x001FCD56 File Offset: 0x001FB156
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x001FCD68 File Offset: 0x001FB168
		public void SetupFound(float totalCost)
		{
			if (this == PawnPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on PawnPath.NotFound", false);
			}
			else
			{
				this.totalCostInt = totalCost;
				this.curNodeIndex = this.nodes.Count - 1;
			}
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x001FCDBC File Offset: 0x001FB1BC
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x001FCDC5 File Offset: 0x001FB1C5
		public void ReleaseToPool()
		{
			if (this != PawnPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x001FCDF4 File Offset: 0x001FB1F4
		public static PawnPath NewNotFound()
		{
			return new PawnPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x001FCE1C File Offset: 0x001FB21C
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x001FCE48 File Offset: 0x001FB248
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x001FCE70 File Offset: 0x001FB270
		public override string ToString()
		{
			string result;
			if (!this.Found)
			{
				result = "PawnPath(not found)";
			}
			else if (!this.inUse)
			{
				result = "PawnPath(not in use)";
			}
			else
			{
				result = string.Concat(new object[]
				{
					"PawnPath(nodeCount= ",
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

		// Token: 0x06003C35 RID: 15413 RVA: 0x001FCF4C File Offset: 0x001FB34C
		public void DrawPath(Pawn pathingPawn)
		{
			if (this.Found)
			{
				float y = AltitudeLayer.Item.AltitudeFor();
				if (this.NodesLeftCount > 0)
				{
					for (int i = 0; i < this.NodesLeftCount - 1; i++)
					{
						Vector3 a = this.Peek(i).ToVector3Shifted();
						a.y = y;
						Vector3 b = this.Peek(i + 1).ToVector3Shifted();
						b.y = y;
						GenDraw.DrawLineBetween(a, b);
					}
					if (pathingPawn != null)
					{
						Vector3 drawPos = pathingPawn.DrawPos;
						drawPos.y = y;
						Vector3 b2 = this.Peek(0).ToVector3Shifted();
						b2.y = y;
						if ((drawPos - b2).sqrMagnitude > 0.01f)
						{
							GenDraw.DrawLineBetween(drawPos, b2);
						}
					}
				}
			}
		}
	}
}
