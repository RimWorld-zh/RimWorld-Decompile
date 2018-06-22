using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000594 RID: 1428
	public abstract class WorldLayer_SingleTile : WorldLayer
	{
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001B41 RID: 6977
		protected abstract int Tile { get; }

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001B42 RID: 6978
		protected abstract Material Material { get; }

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x000E8CEC File Offset: 0x000E70EC
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnTile;
			}
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000E8D20 File Offset: 0x000E7120
		public override IEnumerable Regenerate()
		{
			IEnumerator enumerator = this.<Regenerate>__BaseCallProxy0().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			int tile = this.Tile;
			if (tile >= 0)
			{
				LayerSubMesh subMesh = base.GetSubMesh(this.Material);
				Find.WorldGrid.GetTileVertices(tile, this.verts);
				int count = subMesh.verts.Count;
				int i = 0;
				int count2 = this.verts.Count;
				while (i < count2)
				{
					subMesh.verts.Add(this.verts[i] + this.verts[i].normalized * 0.012f);
					subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, i) + Vector2.one) / 2f);
					if (i < count2 - 2)
					{
						subMesh.tris.Add(count + i + 2);
						subMesh.tris.Add(count + i + 1);
						subMesh.tris.Add(count);
					}
					i++;
				}
				base.FinalizeMesh(MeshParts.All);
			}
			this.lastDrawnTile = tile;
			yield break;
		}

		// Token: 0x04001016 RID: 4118
		private int lastDrawnTile = -1;

		// Token: 0x04001017 RID: 4119
		private List<Vector3> verts = new List<Vector3>();
	}
}
