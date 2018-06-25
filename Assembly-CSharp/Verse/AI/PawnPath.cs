using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A94 RID: 2708
	public class PawnPath : IDisposable
	{
		// Token: 0x040025F5 RID: 9717
		private List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x040025F6 RID: 9718
		private float totalCostInt = 0f;

		// Token: 0x040025F7 RID: 9719
		private int curNodeIndex;

		// Token: 0x040025F8 RID: 9720
		public bool inUse = false;

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x001FCD94 File Offset: 0x001FB194
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x001FCDBC File Offset: 0x001FB1BC
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x001FCDD8 File Offset: 0x001FB1D8
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x001FCDF8 File Offset: 0x001FB1F8
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003C2E RID: 15406 RVA: 0x001FCE14 File Offset: 0x001FB214
		public IntVec3 FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x001FCE44 File Offset: 0x001FB244
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003C30 RID: 15408 RVA: 0x001FCE68 File Offset: 0x001FB268
		public static PawnPath NotFound
		{
			get
			{
				return PawnPathPool.NotFoundPath;
			}
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x001FCE82 File Offset: 0x001FB282
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x001FCE94 File Offset: 0x001FB294
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

		// Token: 0x06003C33 RID: 15411 RVA: 0x001FCEE8 File Offset: 0x001FB2E8
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x001FCEF1 File Offset: 0x001FB2F1
		public void ReleaseToPool()
		{
			if (this != PawnPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x001FCF20 File Offset: 0x001FB320
		public static PawnPath NewNotFound()
		{
			return new PawnPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x001FCF48 File Offset: 0x001FB348
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x001FCF74 File Offset: 0x001FB374
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x001FCF9C File Offset: 0x001FB39C
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

		// Token: 0x06003C39 RID: 15417 RVA: 0x001FD078 File Offset: 0x001FB478
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
