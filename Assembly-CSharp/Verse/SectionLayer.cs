using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C42 RID: 3138
	public abstract class SectionLayer
	{
		// Token: 0x0600451B RID: 17691 RVA: 0x00084A78 File Offset: 0x00082E78
		public SectionLayer(Section section)
		{
			this.section = section;
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x0600451C RID: 17692 RVA: 0x00084A9C File Offset: 0x00082E9C
		protected Map Map
		{
			get
			{
				return this.section.map;
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x0600451D RID: 17693 RVA: 0x00084ABC File Offset: 0x00082EBC
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00084AD4 File Offset: 0x00082ED4
		public LayerSubMesh GetSubMesh(Material material)
		{
			LayerSubMesh result;
			if (material == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < this.subMeshes.Count; i++)
				{
					if (this.subMeshes[i].material == material)
					{
						return this.subMeshes[i];
					}
				}
				Mesh mesh = new Mesh();
				if (UnityData.isEditor)
				{
					mesh.name = string.Concat(new object[]
					{
						"SectionLayerSubMesh_",
						base.GetType().Name,
						"_",
						this.Map.Tile
					});
				}
				LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material);
				this.subMeshes.Add(layerSubMesh);
				result = layerSubMesh;
			}
			return result;
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x00084BB0 File Offset: 0x00082FB0
		protected void FinalizeMesh(MeshParts tags)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags);
				}
			}
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x00084C0C File Offset: 0x0008300C
		public virtual void DrawLayer()
		{
			if (this.Visible)
			{
				int count = this.subMeshes.Count;
				for (int i = 0; i < count; i++)
				{
					LayerSubMesh layerSubMesh = this.subMeshes[i];
					if (layerSubMesh.finalized && !layerSubMesh.disabled)
					{
						Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0);
					}
				}
			}
		}

		// Token: 0x06004521 RID: 17697
		public abstract void Regenerate();

		// Token: 0x06004522 RID: 17698 RVA: 0x00084C8C File Offset: 0x0008308C
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}

		// Token: 0x04002F3F RID: 12095
		protected Section section;

		// Token: 0x04002F40 RID: 12096
		public MapMeshFlag relevantChangeTypes = MapMeshFlag.None;

		// Token: 0x04002F41 RID: 12097
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();
	}
}
