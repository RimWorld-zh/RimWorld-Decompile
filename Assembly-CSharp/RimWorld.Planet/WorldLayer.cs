using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class WorldLayer
	{
		private const int MaxVerticesPerMesh = 40000;

		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		private bool dirty = true;

		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		protected LayerSubMesh GetSubMesh(Material material, out int subMeshIndex)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.material == material && layerSubMesh.verts.Count < 40000)
				{
					subMeshIndex = i;
					return layerSubMesh;
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "WorldLayerSubMesh_" + base.GetType().Name + "_" + Find.World.info.seedString;
			}
			LayerSubMesh layerSubMesh2 = new LayerSubMesh(mesh, material);
			subMeshIndex = this.subMeshes.Count;
			this.subMeshes.Add(layerSubMesh2);
			return layerSubMesh2;
		}

		protected void FinalizeMesh(MeshParts tags, bool optimize = false)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags, optimize);
				}
			}
		}

		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		public void Render()
		{
			if (this.ShouldRegenerate)
			{
				this.RegenerateNow();
			}
			int layer = this.Layer;
			Quaternion rotation = this.Rotation;
			float alpha = this.Alpha;
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].finalized)
				{
					if (alpha != 1f)
					{
						Color color = this.subMeshes[i].material.color;
						WorldLayer.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * alpha));
						Graphics.DrawMesh(this.subMeshes[i].mesh, Vector3.zero, rotation, this.subMeshes[i].material, layer, null, 0, WorldLayer.propertyBlock);
					}
					else
					{
						Graphics.DrawMesh(this.subMeshes[i].mesh, Vector3.zero, rotation, this.subMeshes[i].material, layer);
					}
				}
			}
		}

		[DebuggerHidden]
		public virtual IEnumerable Regenerate()
		{
			WorldLayer.<Regenerate>c__IteratorED <Regenerate>c__IteratorED = new WorldLayer.<Regenerate>c__IteratorED();
			<Regenerate>c__IteratorED.<>f__this = this;
			WorldLayer.<Regenerate>c__IteratorED expr_0E = <Regenerate>c__IteratorED;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public void SetDirty()
		{
			this.dirty = true;
		}

		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}
	}
}
