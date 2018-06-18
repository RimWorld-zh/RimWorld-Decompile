using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008DB RID: 2267
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x001BB6CC File Offset: 0x001B9ACC
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

		// Token: 0x060033DE RID: 13278 RVA: 0x001BB72C File Offset: 0x001B9B2C
		public static void CompassOnGUI(ref float curBaseY)
		{
			Vector2 center = new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f);
			CompassWidget.CompassOnGUI(center);
			curBaseY -= 84f;
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x001BB774 File Offset: 0x001B9B74
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

		// Token: 0x04001BDD RID: 7133
		private const float Padding = 10f;

		// Token: 0x04001BDE RID: 7134
		private const float Size = 64f;

		// Token: 0x04001BDF RID: 7135
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);
	}
}
