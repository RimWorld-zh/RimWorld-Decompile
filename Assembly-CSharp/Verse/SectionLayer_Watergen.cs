using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C53 RID: 3155
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x0600457A RID: 17786 RVA: 0x0024C000 File Offset: 0x0024A400
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x0024C014 File Offset: 0x0024A414
		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x0024C030 File Offset: 0x0024A430
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
