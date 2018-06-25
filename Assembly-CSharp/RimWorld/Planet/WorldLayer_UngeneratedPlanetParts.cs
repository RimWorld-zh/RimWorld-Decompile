using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200059A RID: 1434
	public class WorldLayer_UngeneratedPlanetParts : WorldLayer
	{
		// Token: 0x04001026 RID: 4134
		private const int SubdivisionsCount = 4;

		// Token: 0x04001027 RID: 4135
		private const float ViewAngleOffset = 10f;

		// Token: 0x06001B5D RID: 7005 RVA: 0x000EC658 File Offset: 0x000EAA58
		public override IEnumerable Regenerate()
		{
			IEnumerator enumerator = this.<Regenerate>__BaseCallProxy0().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Vector3 planetViewCenter = Find.WorldGrid.viewCenter;
			float planetViewAngle = Find.WorldGrid.viewAngle;
			if (planetViewAngle < 180f)
			{
				List<Vector3> collection;
				List<int> collection2;
				SphereGenerator.Generate(4, 99.85f, -planetViewCenter, 180f - Mathf.Min(planetViewAngle, 180f) + 10f, out collection, out collection2);
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.UngeneratedPlanetParts);
				subMesh.verts.AddRange(collection);
				subMesh.tris.AddRange(collection2);
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}
	}
}
