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
			foreach (object item in base.Regenerate())
			{
				yield return item;
			}
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			LayerSubMesh sunSubMesh = base.GetSubMesh(WorldMaterials.Sun);
			WorldRendererUtility.PrintQuadTangentialToPlanet(Vector3.forward * 10f, 15f, 0f, sunSubMesh, true, false, true);
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All, true);
		}
	}
}
