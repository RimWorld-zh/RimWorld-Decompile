using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fallout : SkyOverlay
	{
		private static readonly Material FalloutOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);

		public WeatherOverlay_Fallout()
		{
			base.worldOverlayMat = WeatherOverlay_Fallout.FalloutOverlayWorld;
			base.worldOverlayPanSpeed1 = 0.0008f;
			base.worldPanDir1 = new Vector2(-0.25f, -1f);
			base.worldPanDir1.Normalize();
			base.worldOverlayPanSpeed2 = 0.0012f;
			base.worldPanDir2 = new Vector2(-0.24f, -1f);
			base.worldPanDir2.Normalize();
		}
	}
}
