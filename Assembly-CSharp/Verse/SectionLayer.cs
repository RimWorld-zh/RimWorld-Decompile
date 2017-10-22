using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public abstract class SectionLayer
	{
		protected Section section;

		public MapMeshFlag relevantChangeTypes = MapMeshFlag.None;

		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

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

		public SectionLayer(Section section)
		{
			this.section = section;
		}

		public LayerSubMesh GetSubMesh(Material material)
		{
			LayerSubMesh result;
			int i;
			if ((Object)material == (Object)null)
			{
				result = null;
			}
			else
			{
				for (i = 0; i < this.subMeshes.Count; i++)
				{
					if ((Object)this.subMeshes[i].material == (Object)material)
						goto IL_0038;
				}
				Mesh mesh = new Mesh();
				if (UnityData.isEditor)
				{
					mesh.name = "SectionLayerSubMesh_" + base.GetType().Name + "_" + this.Map.Tile;
				}
				LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material);
				this.subMeshes.Add(layerSubMesh);
				result = layerSubMesh;
			}
			goto IL_00cd;
			IL_00cd:
			return result;
			IL_0038:
			result = this.subMeshes[i];
			goto IL_00cd;
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
				for (int num = 0; num < count; num++)
				{
					LayerSubMesh layerSubMesh = this.subMeshes[num];
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
			foreach (LayerSubMesh subMesh in this.subMeshes)
			{
				subMesh.Clear(parts);
			}
		}
	}
}
