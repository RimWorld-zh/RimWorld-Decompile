using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038D RID: 909
	public static class OverlayDrawHandler
	{
		// Token: 0x040009A6 RID: 2470
		private static int lastPowerGridDrawFrame;

		// Token: 0x040009A7 RID: 2471
		private static int lastZoneDrawFrame;

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00084D3C File Offset: 0x0008313C
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x00084D4C File Offset: 0x0008314C
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00084D72 File Offset: 0x00083172
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x00084D80 File Offset: 0x00083180
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}
	}
}
