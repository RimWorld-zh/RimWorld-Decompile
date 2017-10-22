using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WeatherEvent_LightningStrike : WeatherEvent_LightningFlash
	{
		private IntVec3 strikeLoc = IntVec3.Invalid;

		private Mesh boltMesh;

		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);

		public WeatherEvent_LightningStrike(Map map) : base(map)
		{
		}

		public WeatherEvent_LightningStrike(Map map, IntVec3 forcedStrikeLoc) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
		}

		public override void FireEvent()
		{
			base.FireEvent();
			if (!this.strikeLoc.IsValid)
			{
				this.strikeLoc = CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 sq) => sq.Standable(base.map) && !base.map.roofGrid.Roofed(sq)), base.map, 1000);
			}
			this.boltMesh = LightningBoltMeshPool.RandomBoltMesh;
			GenExplosion.DoExplosion(this.strikeLoc, base.map, 1.9f, DamageDefOf.Flame, null, null, null, null, null, 0f, 1, false, null, 0f, 1);
			Vector3 loc = this.strikeLoc.ToVector3Shifted();
			for (int i = 0; i < 4; i++)
			{
				MoteMaker.ThrowSmoke(loc, base.map, 1.5f);
				MoteMaker.ThrowMicroSparks(loc, base.map);
				MoteMaker.ThrowLightningGlow(loc, base.map, 1.5f);
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(this.strikeLoc, base.map, false), MaintenanceType.None);
			SoundDefOf.Thunder_OnMap.PlayOneShot(info);
		}

		public override void WeatherEventDraw()
		{
			Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_LightningStrike.LightningMat, base.LightningBrightness), 0);
		}
	}
}
