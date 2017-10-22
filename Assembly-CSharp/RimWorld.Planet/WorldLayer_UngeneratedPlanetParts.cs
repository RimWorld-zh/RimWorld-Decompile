using System;
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
			IEnumerator enumerator = this._003CRegenerate_003E__BaseCallProxy0().GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			finally
			{
				IDisposable disposable;
				IDisposable disposable2 = disposable = (enumerator as IDisposable);
				if (disposable != null)
				{
					disposable2.Dispose();
				}
			}
			Vector3 planetViewCenter = Find.WorldGrid.viewCenter;
			float planetViewAngle = Find.WorldGrid.viewAngle;
			if (planetViewAngle < 180.0)
			{
				List<Vector3> collection = default(List<Vector3>);
				List<int> collection2 = default(List<int>);
				SphereGenerator.Generate(4, 99.85f, -planetViewCenter, (float)(180.0 - Mathf.Min(planetViewAngle, 180f) + 10.0), out collection, out collection2);
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.UngeneratedPlanetParts);
				subMesh.verts.AddRange(collection);
				subMesh.tris.AddRange(collection2);
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			IL_016d:
			/*Error near IL_016e: Unexpected return in MoveNext()*/;
		}
	}
}
