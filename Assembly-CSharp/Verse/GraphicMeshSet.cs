using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D79 RID: 3449
	public class GraphicMeshSet
	{
		// Token: 0x0400338C RID: 13196
		private Mesh[] meshes = new Mesh[4];

		// Token: 0x06004D4C RID: 19788 RVA: 0x002849B8 File Offset: 0x00282DB8
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

		// Token: 0x06004D4D RID: 19789 RVA: 0x00284A00 File Offset: 0x00282E00
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x00284A58 File Offset: 0x00282E58
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x00284ABC File Offset: 0x00282EBC
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}
	}
}
