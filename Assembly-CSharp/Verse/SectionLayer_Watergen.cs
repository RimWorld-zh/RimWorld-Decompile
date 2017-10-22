using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		private List<LayerSubMesh> renderOrder;

		public override SectionLayerPhaseDef Phase
		{
			get
			{
				return SectionLayerPhaseDefOf.WaterGeneration;
			}
		}

		public SectionLayer_Watergen(Section section) : base(section)
		{
			base.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		public override void Regenerate()
		{
			base.Regenerate();
			this.renderOrder = (from sm in base.subMeshes
			orderby sm.material.renderQueue
			select sm).ToList();
		}

		public override void DrawLayer()
		{
			if (this.Visible)
			{
				Matrix4x4 matrix = Find.Camera.cameraToWorldMatrix * Find.Camera.projectionMatrix * Find.Camera.worldToCameraMatrix;
				int count = this.renderOrder.Count;
				for (int num = 0; num < count; num++)
				{
					LayerSubMesh layerSubMesh = this.renderOrder[num];
					if (layerSubMesh.finalized && !layerSubMesh.disabled)
					{
						layerSubMesh.material.SetPass(0);
						Graphics.DrawMeshNow(layerSubMesh.mesh, matrix);
					}
				}
			}
		}
	}
}
