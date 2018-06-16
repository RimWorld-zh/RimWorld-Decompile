using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058D RID: 1421
	[StaticConstructorOnStartup]
	public class WorldLayer
	{
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x000E87A4 File Offset: 0x000E6BA4
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x000E87C0 File Offset: 0x000E6BC0
		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x000E87DC File Offset: 0x000E6BDC
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x000E87F8 File Offset: 0x000E6BF8
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x000E8814 File Offset: 0x000E6C14
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x000E8830 File Offset: 0x000E6C30
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000E8850 File Offset: 0x000E6C50
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

		// Token: 0x06001B1F RID: 6943 RVA: 0x000E8928 File Offset: 0x000E6D28
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

		// Token: 0x06001B20 RID: 6944 RVA: 0x000E8982 File Offset: 0x000E6D82
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x000E8998 File Offset: 0x000E6D98
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

		// Token: 0x06001B22 RID: 6946 RVA: 0x000E8AC4 File Offset: 0x000E6EC4
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000E8AEE File Offset: 0x000E6EEE
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x000E8AF8 File Offset: 0x000E6EF8
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}

		// Token: 0x04000FF9 RID: 4089
		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x04000FFA RID: 4090
		private bool dirty = true;

		// Token: 0x04000FFB RID: 4091
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04000FFC RID: 4092
		private const int MaxVerticesPerMesh = 40000;
	}
}
