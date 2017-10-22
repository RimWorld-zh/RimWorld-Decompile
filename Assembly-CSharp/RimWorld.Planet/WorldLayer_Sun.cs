using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Sun : WorldLayer
	{
		private const float SunDrawSize = 15f;

		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

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
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			LayerSubMesh sunSubMesh = base.GetSubMesh(WorldMaterials.Sun);
			WorldRendererUtility.PrintQuadTangentialToPlanet(Vector3.forward * 10f, 15f, 0f, sunSubMesh, true, false, true);
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			IL_0133:
			/*Error near IL_0134: Unexpected return in MoveNext()*/;
		}
	}
}
