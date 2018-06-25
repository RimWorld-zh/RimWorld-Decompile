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
		// Token: 0x04000FFA RID: 4090
		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x04000FFB RID: 4091
		private bool dirty = true;

		// Token: 0x04000FFC RID: 4092
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04000FFD RID: 4093
		private const int MaxVerticesPerMesh = 40000;

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x000E8C1C File Offset: 0x000E701C
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x000E8C38 File Offset: 0x000E7038
		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x000E8C54 File Offset: 0x000E7054
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x000E8C70 File Offset: 0x000E7070
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x000E8C8C File Offset: 0x000E708C
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000E8CA8 File Offset: 0x000E70A8
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000E8CC8 File Offset: 0x000E70C8
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

		// Token: 0x06001B1A RID: 6938 RVA: 0x000E8DA0 File Offset: 0x000E71A0
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

		// Token: 0x06001B1B RID: 6939 RVA: 0x000E8DFA File Offset: 0x000E71FA
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x000E8E10 File Offset: 0x000E7210
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

		// Token: 0x06001B1D RID: 6941 RVA: 0x000E8F3C File Offset: 0x000E733C
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000E8F66 File Offset: 0x000E7366
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000E8F70 File Offset: 0x000E7370
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}
	}
}
