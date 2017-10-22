using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Glow : WorldLayer
	{
		private const int SubdivisionsCount = 4;

		public const float GlowRadius = 8f;

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
			List<Vector3> tmpVerts;
			List<int> tmpIndices;
			SphereGenerator.Generate(4, 108.1f, Vector3.forward, 360f, out tmpVerts, out tmpIndices);
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.PlanetGlow);
			subMesh.verts.AddRange(tmpVerts);
			subMesh.tris.AddRange(tmpIndices);
			base.FinalizeMesh(MeshParts.All);
			yield break;
			IL_013b:
			/*Error near IL_013c: Unexpected return in MoveNext()*/;
		}
	}
}
