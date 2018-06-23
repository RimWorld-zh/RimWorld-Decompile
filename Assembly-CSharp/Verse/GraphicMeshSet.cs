using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D76 RID: 3446
	public class GraphicMeshSet
	{
		// Token: 0x04003385 RID: 13189
		private Mesh[] meshes = new Mesh[4];

		// Token: 0x06004D48 RID: 19784 RVA: 0x002845AC File Offset: 0x002829AC
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

		// Token: 0x06004D49 RID: 19785 RVA: 0x002845F4 File Offset: 0x002829F4
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0028464C File Offset: 0x00282A4C
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x002846B0 File Offset: 0x00282AB0
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}
	}
}
