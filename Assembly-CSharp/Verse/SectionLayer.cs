using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C3F RID: 3135
	public abstract class SectionLayer
	{
		// Token: 0x04002F49 RID: 12105
		protected Section section;

		// Token: 0x04002F4A RID: 12106
		public MapMeshFlag relevantChangeTypes = MapMeshFlag.None;

		// Token: 0x04002F4B RID: 12107
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x06004524 RID: 17700 RVA: 0x00084C64 File Offset: 0x00083064
		public SectionLayer(Section section)
		{
			this.section = section;
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06004525 RID: 17701 RVA: 0x00084C88 File Offset: 0x00083088
		protected Map Map
		{
			get
			{
				return this.section.map;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06004526 RID: 17702 RVA: 0x00084CA8 File Offset: 0x000830A8
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00084CC0 File Offset: 0x000830C0
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

		// Token: 0x06004528 RID: 17704 RVA: 0x00084D9C File Offset: 0x0008319C
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

		// Token: 0x06004529 RID: 17705 RVA: 0x00084DF8 File Offset: 0x000831F8
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

		// Token: 0x0600452A RID: 17706
		public abstract void Regenerate();

		// Token: 0x0600452B RID: 17707 RVA: 0x00084E78 File Offset: 0x00083278
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}
	}
}
