using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200059A RID: 1434
	public class WorldLayer_Sun : WorldLayer
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001B58 RID: 7000 RVA: 0x000EB1C4 File Offset: 0x000E95C4
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001B59 RID: 7001 RVA: 0x000EB1E0 File Offset: 0x000E95E0
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000EB200 File Offset: 0x000E9600
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
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			LayerSubMesh sunSubMesh = base.GetSubMesh(WorldMaterials.Sun);
			WorldRendererUtility.PrintQuadTangentialToPlanet(Vector3.forward * 10f, 15f, 0f, sunSubMesh, true, false, true);
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}

		// Token: 0x04001021 RID: 4129
		private const float SunDrawSize = 15f;
	}
}
