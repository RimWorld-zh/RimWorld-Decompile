using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_UngeneratedPlanetParts : WorldLayer
	{
		private const int SubdivisionsCount = 4;

		private const float ViewAngleOffset = 10f;

		public override IEnumerable Regenerate()
		{
			foreach (object item in base.Regenerate())
			{
				yield return item;
			}
			Vector3 planetViewCenter = Find.WorldGrid.viewCenter;
			float planetViewAngle = Find.WorldGrid.viewAngle;
			if (planetViewAngle < 180.0)
			{
				List<Vector3> tmpVerts;
				List<int> tmpIndices;
				SphereGenerator.Generate(4, 99.85f, -planetViewCenter, (float)(180.0 - Mathf.Min(planetViewAngle, 180f) + 10.0), out tmpVerts, out tmpIndices);
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.UngeneratedPlanetParts);
				subMesh.verts.AddRange(tmpVerts);
				subMesh.tris.AddRange(tmpIndices);
			}
			base.FinalizeMesh(MeshParts.All, true);
		}
	}
}
