using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008D9 RID: 2265
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x04001BDB RID: 7131
		private const float Padding = 10f;

		// Token: 0x04001BDC RID: 7132
		private const float Size = 64f;

		// Token: 0x04001BDD RID: 7133
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x001BB9F4 File Offset: 0x001B9DF4
		private static float Angle
		{
			get
			{
				Vector2 b = GenWorldUI.WorldToUIPosition(Find.WorldGrid.NorthPolePos);
				Vector2 a = new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
				b.y = (float)UI.screenHeight - b.y;
				return a.AngleTo(b);
			}
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x001BBA54 File Offset: 0x001B9E54
		public static void CompassOnGUI(ref float curBaseY)
		{
			Vector2 center = new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f);
			CompassWidget.CompassOnGUI(center);
			curBaseY -= 84f;
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x001BBA9C File Offset: 0x001B9E9C
		private static void CompassOnGUI(Vector2 center)
		{
			Widgets.DrawTextureRotated(center, CompassWidget.CompassTex, CompassWidget.Angle, 1f);
			Rect rect = new Rect(center.x - 32f, center.y - 32f, 64f, 64f);
			TooltipHandler.TipRegion(rect, "CompassTip".Translate());
			if (Widgets.ButtonInvisible(rect, false))
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
		}
	}
}
