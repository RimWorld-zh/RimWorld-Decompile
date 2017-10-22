using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public abstract class SectionLayer
	{
		protected Section section;

		public MapMeshFlag relevantChangeTypes;

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

		public virtual SectionLayerPhaseDef Phase
		{
			get
			{
				return SectionLayerPhaseDefOf.Main;
			}
		}

		public SectionLayer(Section section)
		{
			this.section = section;
		}

		public LayerSubMesh GetSubMesh(Material material)
		{
			if ((UnityEngine.Object)material == (UnityEngine.Object)null)
			{
				return null;
			}
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if ((UnityEngine.Object)this.subMeshes[i].material == (UnityEngine.Object)material)
				{
					return this.subMeshes[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "SectionLayerSubMesh_" + base.GetType().Name + "_" + this.Map.Tile;
			}
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material);
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		protected void FinalizeMesh(MeshParts tags)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags, false);
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
			List<LayerSubMesh>.Enumerator enumerator = this.subMeshes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LayerSubMesh current = enumerator.Current;
					current.Clear(parts);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
