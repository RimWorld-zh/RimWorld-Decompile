using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldPath : IDisposable
	{
		private List<int> nodes = new List<int>(128);

		private float totalCostInt = 0f;

		private int curNodeIndex;

		public bool inUse = false;

		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0.0;
			}
		}

		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		public List<int> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		public int FirstNode
		{
			get
			{
				return this.nodes[this.nodes.Count - 1];
			}
		}

		public int LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		public static WorldPath NotFound
		{
			get
			{
				return WorldPathPool.NotFoundPath;
			}
		}

		public void AddNode(int tile)
		{
			this.nodes.Add(tile);
		}

		public void SetupFound(float totalCost)
		{
			if (this == WorldPath.NotFound)
			{
				Log.Warning("Calling SetupFound with totalCost=" + totalCost + " on WorldPath.NotFound");
			}
			else
			{
				this.totalCostInt = totalCost;
				this.curNodeIndex = this.nodes.Count - 1;
			}
		}

		public void Dispose()
		{
			this.ReleaseToPool();
		}

		public void ReleaseToPool()
		{
			if (this != WorldPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
			}
		}

		public static WorldPath NewNotFound()
		{
			WorldPath worldPath = new WorldPath();
			worldPath.totalCostInt = -1f;
			return worldPath;
		}

		public int ConsumeNextNode()
		{
			int result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		public int Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		public override string ToString()
		{
			return this.Found ? (this.inUse ? ("WorldPath(nodeCount= " + this.nodes.Count + ((this.nodes.Count <= 0) ? "" : (" first=" + this.FirstNode + " last=" + this.LastNode)) + " cost=" + this.totalCostInt + " )") : "WorldPath(not in use)") : "WorldPath(not found)";
		}

		public void DrawPath(Caravan pathingCaravan)
		{
			if (this.Found && this.NodesLeftCount > 0)
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
					if ((a2 - vector2).sqrMagnitude > 0.004999999888241291)
					{
						GenDraw.DrawWorldLineBetween(a2, vector2);
					}
				}
			}
		}
	}
}
