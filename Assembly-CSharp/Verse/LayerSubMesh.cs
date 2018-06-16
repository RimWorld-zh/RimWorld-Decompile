using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C3E RID: 3134
	public class LayerSubMesh
	{
		// Token: 0x06004505 RID: 17669 RVA: 0x00244670 File Offset: 0x00242A70
		public LayerSubMesh(Mesh mesh, Material material)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x002446CC File Offset: 0x00242ACC
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

		// Token: 0x06004507 RID: 17671 RVA: 0x00244734 File Offset: 0x00242B34
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

		// Token: 0x06004508 RID: 17672 RVA: 0x0024487C File Offset: 0x00242C7C
		public override string ToString()
		{
			return "LayerSubMesh(" + this.material.ToString() + ")";
		}

		// Token: 0x04002F23 RID: 12067
		public bool finalized = false;

		// Token: 0x04002F24 RID: 12068
		public bool disabled = false;

		// Token: 0x04002F25 RID: 12069
		public Material material;

		// Token: 0x04002F26 RID: 12070
		public Mesh mesh;

		// Token: 0x04002F27 RID: 12071
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04002F28 RID: 12072
		public List<int> tris = new List<int>();

		// Token: 0x04002F29 RID: 12073
		public List<Color32> colors = new List<Color32>();

		// Token: 0x04002F2A RID: 12074
		public List<Vector3> uvs = new List<Vector3>();
	}
}
