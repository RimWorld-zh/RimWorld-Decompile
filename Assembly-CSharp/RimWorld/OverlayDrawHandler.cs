using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200038B RID: 907
	public static class OverlayDrawHandler
	{
		// Token: 0x06000FD4 RID: 4052 RVA: 0x000849F0 File Offset: 0x00082DF0
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00084A00 File Offset: 0x00082E00
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00084A26 File Offset: 0x00082E26
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00084A34 File Offset: 0x00082E34
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}

		// Token: 0x040009A1 RID: 2465
		private static int lastPowerGridDrawFrame;

		// Token: 0x040009A2 RID: 2466
		private static int lastZoneDrawFrame;
	}
}
