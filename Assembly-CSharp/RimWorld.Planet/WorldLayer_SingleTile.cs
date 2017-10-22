using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public abstract class WorldLayer_SingleTile : WorldLayer
	{
		private int lastDrawnTile = -1;

		private List<Vector3> verts = new List<Vector3>();

		protected abstract int Tile
		{
			get;
		}

		protected abstract Material Material
		{
			get;
		}

		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnTile;
			}
		}

		public override IEnumerable Regenerate()
		{
			IEnumerator enumerator = this._003CRegenerate_003E__BaseCallProxy0().GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			finally
			{
				IDisposable disposable;
				IDisposable disposable2 = disposable = (enumerator as IDisposable);
				if (disposable != null)
				{
					disposable2.Dispose();
				}
			}
			int tile = this.Tile;
			if (tile >= 0)
			{
				LayerSubMesh subMesh = base.GetSubMesh(this.Material);
				Find.WorldGrid.GetTileVertices(tile, this.verts);
				int count = subMesh.verts.Count;
				int num = 0;
				int count2 = this.verts.Count;
				while (num < count2)
				{
					subMesh.verts.Add(this.verts[num] + this.verts[num].normalized * 0.012f);
					subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, num) + Vector2.one) / 2f);
					if (num < count2 - 2)
					{
						subMesh.tris.Add(count + num + 2);
						subMesh.tris.Add(count + num + 1);
						subMesh.tris.Add(count);
					}
					num++;
				}
				base.FinalizeMesh(MeshParts.All);
			}
			this.lastDrawnTile = tile;
			yield break;
			IL_0224:
			/*Error near IL_0225: Unexpected return in MoveNext()*/;
		}
	}
}
