using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C55 RID: 3157
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x0600457D RID: 17789 RVA: 0x0024C0DC File Offset: 0x0024A4DC
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0024C0F0 File Offset: 0x0024A4F0
		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x0024C10C File Offset: 0x0024A50C
		public override void DrawLayer()
		{
			if (this.Visible)
			{
				int count = this.subMeshes.Count;
				for (int i = 0; i < count; i++)
				{
					LayerSubMesh layerSubMesh = this.subMeshes[i];
					if (layerSubMesh.finalized && !layerSubMesh.disabled)
					{
						Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, SubcameraDefOf.WaterDepth.LayerId);
					}
				}
			}
		}
	}
}
