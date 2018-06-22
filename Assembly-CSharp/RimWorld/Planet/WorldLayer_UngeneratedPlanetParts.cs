using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000598 RID: 1432
	public class WorldLayer_UngeneratedPlanetParts : WorldLayer
	{
		// Token: 0x06001B5A RID: 7002 RVA: 0x000EC2A0 File Offset: 0x000EA6A0
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

		// Token: 0x04001022 RID: 4130
		private const int SubdivisionsCount = 4;

		// Token: 0x04001023 RID: 4131
		private const float ViewAngleOffset = 10f;
	}
}
