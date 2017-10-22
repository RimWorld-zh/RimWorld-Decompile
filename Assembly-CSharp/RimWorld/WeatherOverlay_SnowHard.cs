using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowHard : SkyOverlay
	{
		private static readonly Material SnowOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);

		public WeatherOverlay_SnowHard()
		{
			base.worldOverlayMat = WeatherOverlay_SnowHard.SnowOverlayWorld;
			base.worldOverlayPanSpeed1 = 0.008f;
			base.worldPanDir1 = new Vector2(-0.5f, -1f);
			base.worldPanDir1.Normalize();
			base.worldOverlayPanSpeed2 = 0.009f;
			base.worldPanDir2 = new Vector2(-0.48f, -1f);
			base.worldPanDir2.Normalize();
		}
	}
}
