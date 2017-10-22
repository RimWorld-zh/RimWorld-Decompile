using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class OverlayDrawHandler
	{
		private static int lastPowerGridDrawFrame;

		private static int lastZoneDrawFrame;

		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		public static bool ShouldDrawZones
		{
			get
			{
				return (byte)(Find.PlaySettings.showZones ? 1 : ((Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1) ? 1 : 0)) != 0;
			}
		}

		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}
	}
}
