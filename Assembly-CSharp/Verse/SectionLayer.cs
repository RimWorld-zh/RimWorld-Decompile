using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public abstract class SectionLayer
	{
		protected Section section;

		public MapMeshFlag relevantChangeTypes = MapMeshFlag.None;

		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		public SectionLayer(Section section)
		{
			this.section = section;
		}

		protected Map Map
		{
			get
			{
				return this.section.map;
			}
		}

		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

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

		public abstract void Regenerate();

		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}
	}
}
