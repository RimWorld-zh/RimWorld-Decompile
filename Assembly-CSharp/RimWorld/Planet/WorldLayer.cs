using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058B RID: 1419
	[StaticConstructorOnStartup]
	public class WorldLayer
	{
		// Token: 0x04000FF6 RID: 4086
		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x04000FF7 RID: 4087
		private bool dirty = true;

		// Token: 0x04000FF8 RID: 4088
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04000FF9 RID: 4089
		private const int MaxVerticesPerMesh = 40000;

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x000E89B4 File Offset: 0x000E6DB4
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x000E89D0 File Offset: 0x000E6DD0
		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x000E89EC File Offset: 0x000E6DEC
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x000E8A08 File Offset: 0x000E6E08
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x000E8A24 File Offset: 0x000E6E24
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000E8A40 File Offset: 0x000E6E40
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x000E8A60 File Offset: 0x000E6E60
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

		// Token: 0x06001B1B RID: 6939 RVA: 0x000E8B38 File Offset: 0x000E6F38
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

		// Token: 0x06001B1C RID: 6940 RVA: 0x000E8B92 File Offset: 0x000E6F92
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x000E8BA8 File Offset: 0x000E6FA8
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

		// Token: 0x06001B1E RID: 6942 RVA: 0x000E8CD4 File Offset: 0x000E70D4
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000E8CFE File Offset: 0x000E70FE
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x000E8D08 File Offset: 0x000E7108
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}
	}
}
