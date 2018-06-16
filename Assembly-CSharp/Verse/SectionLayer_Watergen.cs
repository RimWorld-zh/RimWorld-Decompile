using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C57 RID: 3159
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x06004573 RID: 17779 RVA: 0x0024AC58 File Offset: 0x00249058
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x0024AC6C File Offset: 0x0024906C
		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x0024AC88 File Offset: 0x00249088
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
