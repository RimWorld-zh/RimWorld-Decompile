using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D79 RID: 3449
	public class GraphicMeshSet
	{
		// Token: 0x06004D33 RID: 19763 RVA: 0x00282FFC File Offset: 0x002813FC
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

		// Token: 0x06004D34 RID: 19764 RVA: 0x00283044 File Offset: 0x00281444
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0028309C File Offset: 0x0028149C
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x00283100 File Offset: 0x00281500
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}

		// Token: 0x0400337A RID: 13178
		private Mesh[] meshes = new Mesh[4];
	}
}
