using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		private const float Padding = 10f;

		private const float Size = 64f;

		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);

		private static float Angle
		{
			get
			{
				Vector2 b = GenWorldUI.WorldToUIPosition(Find.WorldGrid.NorthPolePos);
				Vector2 a = new Vector2((float)((float)UI.screenWidth / 2.0), (float)((float)UI.screenHeight / 2.0));
				b.y = (float)UI.screenHeight - b.y;
				return a.AngleTo(b);
			}
		}

		public static void CompassOnGUI(ref float curBaseY)
		{
			Vector2 center = new Vector2((float)((float)UI.screenWidth - 10.0 - 32.0), (float)(curBaseY - 10.0 - 32.0));
			CompassWidget.CompassOnGUI(center);
			curBaseY -= 84f;
		}

		private static void CompassOnGUI(Vector2 center)
		{
			Widgets.DrawTextureRotated(center, CompassWidget.CompassTex, CompassWidget.Angle, 1f);
			Rect rect = new Rect((float)(center.x - 32.0), (float)(center.y - 32.0), 64f, 64f);
			TooltipHandler.TipRegion(rect, "CompassTip".Translate());
			if (Widgets.ButtonInvisible(rect, false))
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
		}
	}
}
