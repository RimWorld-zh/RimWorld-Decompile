using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C3A RID: 3130
	public class LayerSubMesh
	{
		// Token: 0x04002F2B RID: 12075
		public bool finalized = false;

		// Token: 0x04002F2C RID: 12076
		public bool disabled = false;

		// Token: 0x04002F2D RID: 12077
		public Material material;

		// Token: 0x04002F2E RID: 12078
		public Mesh mesh;

		// Token: 0x04002F2F RID: 12079
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04002F30 RID: 12080
		public List<int> tris = new List<int>();

		// Token: 0x04002F31 RID: 12081
		public List<Color32> colors = new List<Color32>();

		// Token: 0x04002F32 RID: 12082
		public List<Vector3> uvs = new List<Vector3>();

		// Token: 0x0600450C RID: 17676 RVA: 0x00245A18 File Offset: 0x00243E18
		public LayerSubMesh(Mesh mesh, Material material)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00245A74 File Offset: 0x00243E74
		public void Clear(MeshParts parts)
		{
			if ((byte)(parts & MeshParts.Verts) != 0)
			{
				this.verts.Clear();
			}
			if ((byte)(parts & MeshParts.Tris) != 0)
			{
				this.tris.Clear();
			}
			if ((byte)(parts & MeshParts.Colors) != 0)
			{
				this.colors.Clear();
			}
			if ((byte)(parts & MeshParts.UVs) != 0)
			{
				this.uvs.Clear();
			}
			this.finalized = false;
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x00245ADC File Offset: 0x00243EDC
		public void FinalizeMesh(MeshParts parts)
		{
			if (this.finalized)
			{
				Log.Warning("Finalizing mesh which is already finalized. Did you forget to call Clear()?", false);
			}
			if ((byte)(parts & MeshParts.Verts) != 0 || (byte)(parts & MeshParts.Tris) != 0)
			{
				this.mesh.Clear();
			}
			if ((byte)(parts & MeshParts.Verts) != 0)
			{
				if (this.verts.Count > 0)
				{
					this.mesh.SetVertices(this.verts);
				}
				else
				{
					Log.Error("Cannot cook Verts for " + this.material.ToString() + ": no ingredients data. If you want to not render this submesh, disable it.", false);
				}
			}
			if ((byte)(parts & MeshParts.Tris) != 0)
			{
				if (this.tris.Count > 0)
				{
					this.mesh.SetTriangles(this.tris, 0);
				}
				else
				{
					Log.Error("Cannot cook Tris for " + this.material.ToString() + ": no ingredients data.", false);
				}
			}
			if ((byte)(parts & MeshParts.Colors) != 0)
			{
				if (this.colors.Count > 0)
				{
					this.mesh.SetColors(this.colors);
				}
			}
			if ((byte)(parts & MeshParts.UVs) != 0)
			{
				if (this.uvs.Count > 0)
				{
					this.mesh.SetUVs(0, this.uvs);
				}
			}
			this.finalized = true;
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00245C24 File Offset: 0x00244024
		public override string ToString()
		{
			return "LayerSubMesh(" + this.material.ToString() + ")";
		}
	}
}
