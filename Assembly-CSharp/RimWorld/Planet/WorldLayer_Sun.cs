using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000596 RID: 1430
	public class WorldLayer_Sun : WorldLayer
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001B4F RID: 6991 RVA: 0x000EB218 File Offset: 0x000E9618
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001B50 RID: 6992 RVA: 0x000EB234 File Offset: 0x000E9634
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000EB254 File Offset: 0x000E9654
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

		// Token: 0x0400101E RID: 4126
		private const float SunDrawSize = 15f;
	}
}
