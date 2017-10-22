using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	internal class DebugTile
	{
		public int tile;

		public string displayString;

		public float colorPct;

		public int ticksLeft;

		public Material customMat;

		private Mesh mesh;

		private static List<Vector3> tmpVerts = new List<Vector3>();

		private static List<int> tmpIndices = new List<int>();

		private Vector2 ScreenPos
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return GenWorldUI.WorldToUIPosition(tileCenter);
			}
		}

		private bool VisibleForCamera
		{
			get
			{
				return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(this.ScreenPos);
			}
		}

		public float DistanceToCamera
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return Vector3.Distance(Find.WorldCamera.transform.position, tileCenter);
			}
		}

		public void Draw()
		{
			if (this.VisibleForCamera)
			{
				if ((Object)this.mesh == (Object)null)
				{
					Find.WorldGrid.GetTileVertices(this.tile, DebugTile.tmpVerts);
					for (int i = 0; i < DebugTile.tmpVerts.Count; i++)
					{
						Vector3 a = DebugTile.tmpVerts[i];
						DebugTile.tmpVerts[i] = a + a.normalized * 0.012f;
					}
					this.mesh = new Mesh();
					this.mesh.name = "DebugTile";
					this.mesh.SetVertices(DebugTile.tmpVerts);
					DebugTile.tmpIndices.Clear();
					for (int j = 0; j < DebugTile.tmpVerts.Count - 2; j++)
					{
						DebugTile.tmpIndices.Add(j + 2);
						DebugTile.tmpIndices.Add(j + 1);
						DebugTile.tmpIndices.Add(0);
					}
					this.mesh.SetTriangles(DebugTile.tmpIndices, 0);
				}
				Material material;
				if ((Object)this.customMat != (Object)null)
				{
					material = this.customMat;
				}
				else
				{
					int num = Mathf.RoundToInt((float)(this.colorPct * 100.0));
					num %= 100;
					material = WorldDebugMatsSpectrum.Mat(num);
				}
				Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, material, WorldCameraManager.WorldLayer);
			}
		}

		public void OnGUI()
		{
			if (this.VisibleForCamera)
			{
				Vector2 screenPos = this.ScreenPos;
				Rect rect = new Rect((float)(screenPos.x - 20.0), (float)(screenPos.y - 20.0), 40f, 40f);
				if (this.displayString != null)
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}
	}
}
