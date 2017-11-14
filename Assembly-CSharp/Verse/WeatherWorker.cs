using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class WeatherWorker
	{
		private struct SkyThreshold
		{
			public SkyColorSet colors;

			public float celGlowThreshold;

			public SkyThreshold(SkyColorSet colors, float celGlowThreshold)
			{
				this.colors = colors;
				this.celGlowThreshold = celGlowThreshold;
			}
		}

		private WeatherDef def;

		public List<SkyOverlay> overlays = new List<SkyOverlay>();

		private SkyThreshold[] skyTargets = new SkyThreshold[4];

		public WeatherWorker(WeatherDef def)
		{
			this.def = def;
			foreach (Type overlayClass in def.overlayClasses)
			{
				SkyOverlay item = (SkyOverlay)GenGeneric.InvokeStaticGenericMethod(typeof(WeatherPartPool), overlayClass, "GetInstanceOf");
				this.overlays.Add(item);
			}
			this.skyTargets[0] = new SkyThreshold(def.skyColorsNightMid, 0f);
			this.skyTargets[1] = new SkyThreshold(def.skyColorsNightEdge, 0.1f);
			this.skyTargets[2] = new SkyThreshold(def.skyColorsDusk, 0.6f);
			this.skyTargets[3] = new SkyThreshold(def.skyColorsDay, 1f);
		}

		public void DrawWeather(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

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

		public SkyTarget CurSkyTarget(Map map)
		{
			float num = GenCelestial.CurCelestialSunGlow(map);
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while (num4 < this.skyTargets.Length)
			{
				num3 = num4;
				if (!(num + 0.0010000000474974513 < this.skyTargets[num4].celGlowThreshold))
				{
					num2 = num4;
					num4++;
					continue;
				}
				break;
			}
			SkyThreshold skyThreshold = this.skyTargets[num2];
			SkyThreshold skyThreshold2 = this.skyTargets[num3];
			float num5 = skyThreshold2.celGlowThreshold - skyThreshold.celGlowThreshold;
			float t = (float)((num5 != 0.0) ? ((num - skyThreshold.celGlowThreshold) / num5) : 1.0);
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
	}
}
