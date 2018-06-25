using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D78 RID: 3448
	public class GraphicMeshSet
	{
		// Token: 0x04003385 RID: 13189
		private Mesh[] meshes = new Mesh[4];

		// Token: 0x06004D4C RID: 19788 RVA: 0x002846D8 File Offset: 0x00282AD8
		public GraphicMeshSet(Mesh normalMesh, Mesh leftMesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			this.meshes[2] = normalMesh;
			array[num] = (array2[num2] = normalMesh);
			this.meshes[3] = leftMesh;
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x00284720 File Offset: 0x00282B20
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x00284778 File Offset: 0x00282B78
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x002847DC File Offset: 0x00282BDC
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}
	}
}
