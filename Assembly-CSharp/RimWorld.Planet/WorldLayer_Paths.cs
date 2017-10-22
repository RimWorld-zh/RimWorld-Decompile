using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public abstract class WorldLayer_Paths : WorldLayer
	{
		public struct OutputDirection
		{
			public int neighbor;

			public float width;

			public float distortionFrequency;

			public float distortionIntensity;
		}

		protected bool pointyEnds = false;

		private List<Vector3> tmpVerts = new List<Vector3>();

		private List<Vector3> tmpHexVerts = new List<Vector3>();

		private List<int> tmpNeighbors = new List<int>();

		public void GeneratePaths(LayerSubMesh subMesh, int tileID, List<OutputDirection> nodes, Color32 color, bool allowSmoothTransition)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			worldGrid.GetTileVertices(tileID, this.tmpVerts);
			worldGrid.GetTileNeighbors(tileID, this.tmpNeighbors);
			if (nodes.Count == 1 && this.pointyEnds)
			{
				int count = subMesh.verts.Count;
				List<Vector3> verts = this.tmpVerts;
				List<int> obj = this.tmpNeighbors;
				OutputDirection outputDirection = nodes[0];
				this.AddPathEndpoint(subMesh, verts, obj.IndexOf(outputDirection.neighbor), color, nodes[0]);
				List<Vector3> verts2 = subMesh.verts;
				Vector3 tileCenter = worldGrid.GetTileCenter(tileID);
				OutputDirection outputDirection2 = nodes[0];
				float distortionFrequency = outputDirection2.distortionFrequency;
				OutputDirection outputDirection3 = nodes[0];
				verts2.Add(this.FinalizePoint(tileCenter, distortionFrequency, outputDirection3.distortionIntensity));
				subMesh.colors.Add(color.MutateAlpha((byte)0));
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
			}
			else
			{
				if (nodes.Count == 2)
				{
					int count2 = subMesh.verts.Count;
					List<int> obj2 = this.tmpNeighbors;
					OutputDirection outputDirection4 = nodes[0];
					int num = obj2.IndexOf(outputDirection4.neighbor);
					List<int> obj3 = this.tmpNeighbors;
					OutputDirection outputDirection5 = nodes[1];
					int num2 = obj3.IndexOf(outputDirection5.neighbor);
					if (allowSmoothTransition && Mathf.Abs(num - num2) > 1 && Mathf.Abs((num - num2 + this.tmpVerts.Count) % this.tmpVerts.Count) > 1)
					{
						this.AddPathEndpoint(subMesh, this.tmpVerts, num, color, nodes[0]);
						this.AddPathEndpoint(subMesh, this.tmpVerts, num2, color, nodes[1]);
						subMesh.tris.Add(count2);
						subMesh.tris.Add(count2 + 5);
						subMesh.tris.Add(count2 + 1);
						subMesh.tris.Add(count2 + 5);
						subMesh.tris.Add(count2 + 4);
						subMesh.tris.Add(count2 + 1);
						subMesh.tris.Add(count2 + 1);
						subMesh.tris.Add(count2 + 4);
						subMesh.tris.Add(count2 + 2);
						subMesh.tris.Add(count2 + 4);
						subMesh.tris.Add(count2 + 3);
						subMesh.tris.Add(count2 + 2);
						return;
					}
				}
				float num3 = 0f;
				for (int i = 0; i < nodes.Count; i++)
				{
					float a = num3;
					OutputDirection outputDirection6 = nodes[i];
					num3 = Mathf.Max(a, outputDirection6.width);
				}
				Vector3 tileCenter2 = worldGrid.GetTileCenter(tileID);
				this.tmpHexVerts.Clear();
				for (int j = 0; j < this.tmpVerts.Count; j++)
				{
					this.tmpHexVerts.Add(this.FinalizePoint(Vector3.LerpUnclamped(tileCenter2, this.tmpVerts[j], (float)(num3 * 0.5 * 2.0)), 0f, 0f));
				}
				tileCenter2 = this.FinalizePoint(tileCenter2, 0f, 0f);
				int count3 = subMesh.verts.Count;
				subMesh.verts.Add(tileCenter2);
				subMesh.colors.Add(color);
				int count4 = subMesh.verts.Count;
				for (int k = 0; k < this.tmpHexVerts.Count; k++)
				{
					subMesh.verts.Add(this.tmpHexVerts[k]);
					subMesh.colors.Add(color.MutateAlpha((byte)0));
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count4 + (k + 1) % this.tmpHexVerts.Count);
					subMesh.tris.Add(count4 + k);
				}
				for (int l = 0; l < nodes.Count; l++)
				{
					OutputDirection outputDirection7 = nodes[l];
					if (outputDirection7.width != 0.0)
					{
						int count5 = subMesh.verts.Count;
						List<int> obj4 = this.tmpNeighbors;
						OutputDirection outputDirection8 = nodes[l];
						int num4 = obj4.IndexOf(outputDirection8.neighbor);
						this.AddPathEndpoint(subMesh, this.tmpVerts, num4, color, nodes[l]);
						subMesh.tris.Add(count5);
						subMesh.tris.Add(count4 + (num4 + this.tmpHexVerts.Count - 1) % this.tmpHexVerts.Count);
						subMesh.tris.Add(count3);
						subMesh.tris.Add(count5);
						subMesh.tris.Add(count3);
						subMesh.tris.Add(count5 + 1);
						subMesh.tris.Add(count5 + 1);
						subMesh.tris.Add(count3);
						subMesh.tris.Add(count5 + 2);
						subMesh.tris.Add(count3);
						subMesh.tris.Add(count4 + (num4 + 2) % this.tmpHexVerts.Count);
						subMesh.tris.Add(count5 + 2);
					}
				}
			}
		}

		private void AddPathEndpoint(LayerSubMesh subMesh, List<Vector3> verts, int index, Color32 color, OutputDirection data)
		{
			int index2 = (index + 1) % verts.Count;
			Vector3 a = this.FinalizePoint(verts[index], data.distortionFrequency, data.distortionIntensity);
			Vector3 b = this.FinalizePoint(verts[index2], data.distortionFrequency, data.distortionIntensity);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, (float)(0.5 - data.width)));
			subMesh.colors.Add(color.MutateAlpha((byte)0));
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f));
			subMesh.colors.Add(color);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, (float)(0.5 + data.width)));
			subMesh.colors.Add(color.MutateAlpha((byte)0));
		}

		public abstract Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity);
	}
}
