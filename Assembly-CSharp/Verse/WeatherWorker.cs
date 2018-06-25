using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CB6 RID: 3254
	public class WeatherWorker
	{
		// Token: 0x040030A9 RID: 12457
		private WeatherDef def;

		// Token: 0x040030AA RID: 12458
		public List<SkyOverlay> overlays = new List<SkyOverlay>();

		// Token: 0x040030AB RID: 12459
		private WeatherWorker.SkyThreshold[] skyTargets = new WeatherWorker.SkyThreshold[4];

		// Token: 0x060047BC RID: 18364 RVA: 0x0025CD48 File Offset: 0x0025B148
		public WeatherWorker(WeatherDef def)
		{
			this.def = def;
			foreach (Type genericParam in def.overlayClasses)
			{
				SkyOverlay item = (SkyOverlay)GenGeneric.InvokeStaticGenericMethod(typeof(WeatherPartPool), genericParam, "GetInstanceOf");
				this.overlays.Add(item);
			}
			this.skyTargets[0] = new WeatherWorker.SkyThreshold(def.skyColorsNightMid, 0f);
			this.skyTargets[1] = new WeatherWorker.SkyThreshold(def.skyColorsNightEdge, 0.1f);
			this.skyTargets[2] = new WeatherWorker.SkyThreshold(def.skyColorsDusk, 0.6f);
			this.skyTargets[3] = new WeatherWorker.SkyThreshold(def.skyColorsDay, 1f);
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0025CE70 File Offset: 0x0025B270
		public void DrawWeather(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x0025CEB0 File Offset: 0x0025B2B0
		public void WeatherTick(Map map, float lerpFactor)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].TickOverlay(map);
			}
			for (int j = 0; j < this.def.eventMakers.Count; j++)
			{
				this.def.eventMakers[j].WeatherEventMakerTick(map, lerpFactor);
			}
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x0025CF2C File Offset: 0x0025B32C
		public SkyTarget CurSkyTarget(Map map)
		{
			float num = GenCelestial.CurCelestialSunGlow(map);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < this.skyTargets.Length; i++)
			{
				num3 = i;
				if (num + 0.001f < this.skyTargets[i].celGlowThreshold)
				{
					break;
				}
				num2 = i;
			}
			WeatherWorker.SkyThreshold skyThreshold = this.skyTargets[num2];
			WeatherWorker.SkyThreshold skyThreshold2 = this.skyTargets[num3];
			float num4 = skyThreshold2.celGlowThreshold - skyThreshold.celGlowThreshold;
			float t;
			if (num4 == 0f)
			{
				t = 1f;
			}
			else
			{
				t = (num - skyThreshold.celGlowThreshold) / num4;
			}
			SkyTarget result = default(SkyTarget);
			result.glow = num;
			result.colors = SkyColorSet.Lerp(skyThreshold.colors, skyThreshold2.colors, t);
			if (GenCelestial.IsDaytime(num))
			{
				result.lightsourceShineIntensity = 1f;
				result.lightsourceShineSize = 1f;
			}
			else
			{
				result.lightsourceShineIntensity = 0.7f;
				result.lightsourceShineSize = 0.5f;
			}
			return result;
		}

		// Token: 0x02000CB7 RID: 3255
		private struct SkyThreshold
		{
			// Token: 0x040030AC RID: 12460
			public SkyColorSet colors;

			// Token: 0x040030AD RID: 12461
			public float celGlowThreshold;

			// Token: 0x060047C0 RID: 18368 RVA: 0x0025D05E File Offset: 0x0025B45E
			public SkyThreshold(SkyColorSet colors, float celGlowThreshold)
			{
				this.colors = colors;
				this.celGlowThreshold = celGlowThreshold;
			}
		}
	}
}
