using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB1 RID: 3249
	public class SkyManager
	{
		// Token: 0x06004785 RID: 18309 RVA: 0x0025AA04 File Offset: 0x00258E04
		public SkyManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004786 RID: 18310 RVA: 0x0025AA20 File Offset: 0x00258E20
		public float CurSkyGlow
		{
			get
			{
				return this.curSkyGlowInt;
			}
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0025AA3C File Offset: 0x00258E3C
		public void SkyManagerUpdate()
		{
			SkyTarget curSky = this.CurrentSkyTarget();
			this.curSkyGlowInt = curSky.glow;
			if (this.map == Find.CurrentMap)
			{
				MatBases.LightOverlay.color = curSky.colors.sky;
				Find.CameraColor.saturation = curSky.colors.saturation;
				Color color = curSky.colors.sky;
				color.a = 1f;
				color *= SkyManager.FogOfWarBaseColor;
				MatBases.FogOfWar.color = color;
				Color color2 = curSky.colors.shadow;
				Vector3? overridenShadowVector = this.GetOverridenShadowVector();
				if (overridenShadowVector != null)
				{
					this.SetSunShadowVector(overridenShadowVector.Value);
				}
				else
				{
					this.SetSunShadowVector(GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow).vector);
					color2 = Color.Lerp(Color.white, color2, GenCelestial.CurShadowStrength(this.map));
				}
				GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingSun);
				GenCelestial.LightInfo lightSourceInfo2 = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingMoon);
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectSun, new Vector4(lightSourceInfo.vector.x, 0f, lightSourceInfo.vector.y, lightSourceInfo.intensity));
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectMoon, new Vector4(lightSourceInfo2.vector.x, 0f, lightSourceInfo2.vector.y, lightSourceInfo2.intensity));
				Shader.SetGlobalFloat("_LightsourceShineSizeReduction", 20f * (1f / curSky.lightsourceShineSize));
				Shader.SetGlobalFloat("_LightsourceShineIntensity", curSky.lightsourceShineIntensity);
				MatBases.SunShadow.color = color2;
				this.UpdateOverlays(curSky);
			}
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0025ABFB File Offset: 0x00258FFB
		public void ForceSetCurSkyGlow(float curSkyGlow)
		{
			this.curSkyGlowInt = curSkyGlow;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0025AC08 File Offset: 0x00259008
		private void UpdateOverlays(SkyTarget curSky)
		{
			this.tempOverlays.Clear();
			List<SkyOverlay> overlays = this.map.weatherManager.curWeather.Worker.overlays;
			for (int i = 0; i < overlays.Count; i++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays[i], this.map.weatherManager.TransitionLerpFactor));
			}
			List<SkyOverlay> overlays2 = this.map.weatherManager.lastWeather.Worker.overlays;
			for (int j = 0; j < overlays2.Count; j++)
			{
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays2[j], 1f - this.map.weatherManager.TransitionLerpFactor));
			}
			for (int k = 0; k < this.map.gameConditionManager.ActiveConditions.Count; k++)
			{
				GameCondition gameCondition = this.map.gameConditionManager.ActiveConditions[k];
				List<SkyOverlay> list = gameCondition.SkyOverlays(this.map);
				if (list != null)
				{
					for (int l = 0; l < list.Count; l++)
					{
						this.AddTempOverlay(new Pair<SkyOverlay, float>(list[l], gameCondition.SkyTargetLerpFactor(this.map)));
					}
				}
			}
			for (int m = 0; m < this.tempOverlays.Count; m++)
			{
				Color overlay = curSky.colors.overlay;
				overlay.a = this.tempOverlays[m].Second;
				this.tempOverlays[m].First.OverlayColor = overlay;
			}
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0025ADDC File Offset: 0x002591DC
		private void AddTempOverlay(Pair<SkyOverlay, float> pair)
		{
			for (int i = 0; i < this.tempOverlays.Count; i++)
			{
				if (this.tempOverlays[i].First == pair.First)
				{
					this.tempOverlays[i] = new Pair<SkyOverlay, float>(this.tempOverlays[i].First, Mathf.Clamp01(this.tempOverlays[i].Second + pair.Second));
					return;
				}
			}
			this.tempOverlays.Add(pair);
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0025AE80 File Offset: 0x00259280
		private void SetSunShadowVector(Vector2 vec)
		{
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSunLightDirection, new Vector4(vec.x, 0f, vec.y, GenCelestial.CurShadowStrength(this.map)));
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0025AEB0 File Offset: 0x002592B0
		private SkyTarget CurrentSkyTarget()
		{
			SkyTarget b = this.map.weatherManager.curWeather.Worker.CurSkyTarget(this.map);
			SkyTarget a = this.map.weatherManager.lastWeather.Worker.CurSkyTarget(this.map);
			SkyTarget skyTarget = SkyTarget.Lerp(a, b, this.map.weatherManager.TransitionLerpFactor);
			float num = this.map.gameConditionManager.AggregateSkyTargetLerpFactor(this.map);
			if (num > 0.0001f)
			{
				SkyTarget value = this.map.gameConditionManager.AggregateSkyTarget(this.map).Value;
				skyTarget = SkyTarget.LerpDarken(skyTarget, value, num);
			}
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			for (int i = 0; i < liveEventsListForReading.Count; i++)
			{
				if (liveEventsListForReading[i].CurrentlyAffectsSky)
				{
					skyTarget = SkyTarget.Lerp(skyTarget, liveEventsListForReading[i].SkyTarget, liveEventsListForReading[i].SkyTargetLerpFactor);
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			for (int j = 0; j < list.Count; j++)
			{
				CompAffectsSky compAffectsSky = list[j].TryGetComp<CompAffectsSky>();
				if (compAffectsSky.LerpFactor > 0f)
				{
					if (compAffectsSky.Props.lerpDarken)
					{
						skyTarget = SkyTarget.LerpDarken(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
					else
					{
						skyTarget = SkyTarget.Lerp(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor);
					}
				}
			}
			return skyTarget;
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x0025B074 File Offset: 0x00259474
		private Vector3? GetOverridenShadowVector()
		{
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			for (int i = 0; i < liveEventsListForReading.Count; i++)
			{
				Vector2? overrideShadowVector = liveEventsListForReading[i].OverrideShadowVector;
				if (overrideShadowVector != null)
				{
					return (overrideShadowVector == null) ? null : new Vector3?(overrideShadowVector.GetValueOrDefault());
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
			for (int j = 0; j < list.Count; j++)
			{
				Vector2? overrideShadowVector2 = list[j].TryGetComp<CompAffectsSky>().OverrideShadowVector;
				if (overrideShadowVector2 != null)
				{
					return (overrideShadowVector2 == null) ? null : new Vector3?(overrideShadowVector2.GetValueOrDefault());
				}
			}
			return null;
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x0025B194 File Offset: 0x00259594
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SkyManager: ");
			stringBuilder.AppendLine("CurCelestialSunGlow: " + GenCelestial.CurCelestialSunGlow(Find.CurrentMap));
			stringBuilder.AppendLine("CurSkyGlow: " + this.CurSkyGlow.ToStringPercent());
			stringBuilder.AppendLine("CurrentSkyTarget: " + this.CurrentSkyTarget().ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x04003087 RID: 12423
		private Map map;

		// Token: 0x04003088 RID: 12424
		private float curSkyGlowInt;

		// Token: 0x04003089 RID: 12425
		private List<Pair<SkyOverlay, float>> tempOverlays = new List<Pair<SkyOverlay, float>>();

		// Token: 0x0400308A RID: 12426
		private static readonly Color FogOfWarBaseColor = new Color32(77, 69, 66, byte.MaxValue);

		// Token: 0x0400308B RID: 12427
		public const float NightMaxCelGlow = 0.1f;

		// Token: 0x0400308C RID: 12428
		public const float DuskMaxCelGlow = 0.6f;
	}
}
