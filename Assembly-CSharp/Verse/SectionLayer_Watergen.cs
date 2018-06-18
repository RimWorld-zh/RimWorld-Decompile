using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C56 RID: 3158
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x06004571 RID: 17777 RVA: 0x0024AC30 File Offset: 0x00249030
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x0024AC44 File Offset: 0x00249044
		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x0024AC60 File Offset: 0x00249060
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
