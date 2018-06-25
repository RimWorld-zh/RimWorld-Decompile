using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000598 RID: 1432
	public class WorldLayer_Sun : WorldLayer
	{
		// Token: 0x0400101E RID: 4126
		private const float SunDrawSize = 15f;

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001B53 RID: 6995 RVA: 0x000EB368 File Offset: 0x000E9768
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001B54 RID: 6996 RVA: 0x000EB384 File Offset: 0x000E9784
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000EB3A4 File Offset: 0x000E97A4
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
	}
}
