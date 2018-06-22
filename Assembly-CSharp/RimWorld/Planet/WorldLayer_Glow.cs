using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058B RID: 1419
	public class WorldLayer_Glow : WorldLayer
	{
		// Token: 0x06001B22 RID: 6946 RVA: 0x000E9124 File Offset: 0x000E7524
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
			List<Vector3> tmpVerts;
			List<int> tmpIndices;
			SphereGenerator.Generate(4, 108.1f, Vector3.forward, 360f, out tmpVerts, out tmpIndices);
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.PlanetGlow);
			subMesh.verts.AddRange(tmpVerts);
			subMesh.tris.AddRange(tmpIndices);
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}

		// Token: 0x04000FFA RID: 4090
		private const int SubdivisionsCount = 4;

		// Token: 0x04000FFB RID: 4091
		public const float GlowRadius = 8f;
	}
}
