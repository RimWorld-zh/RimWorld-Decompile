using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A95 RID: 2709
	public class PawnPath : IDisposable
	{
		// Token: 0x04002605 RID: 9733
		private List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x04002606 RID: 9734
		private float totalCostInt = 0f;

		// Token: 0x04002607 RID: 9735
		private int curNodeIndex;

		// Token: 0x04002608 RID: 9736
		public bool inUse = false;

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x001FD0C0 File Offset: 0x001FB4C0
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x001FD0E8 File Offset: 0x001FB4E8
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x001FD104 File Offset: 0x001FB504
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003C2E RID: 15406 RVA: 0x001FD124 File Offset: 0x001FB524
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x001FD140 File Offset: 0x001FB540
		public IntVec3 FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003C30 RID: 15408 RVA: 0x001FD170 File Offset: 0x001FB570
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x001FD194 File Offset: 0x001FB594
		public static PawnPath NotFound
		{
			get
			{
				return PawnPathPool.NotFoundPath;
			}
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x001FD1AE File Offset: 0x001FB5AE
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x001FD1C0 File Offset: 0x001FB5C0
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

		// Token: 0x06003C34 RID: 15412 RVA: 0x001FD214 File Offset: 0x001FB614
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x001FD21D File Offset: 0x001FB61D
		public void ReleaseToPool()
		{
			if (this != PawnPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x001FD24C File Offset: 0x001FB64C
		public static PawnPath NewNotFound()
		{
			return new PawnPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x001FD274 File Offset: 0x001FB674
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x001FD2A0 File Offset: 0x001FB6A0
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x001FD2C8 File Offset: 0x001FB6C8
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

		// Token: 0x06003C3A RID: 15418 RVA: 0x001FD3A4 File Offset: 0x001FB7A4
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
