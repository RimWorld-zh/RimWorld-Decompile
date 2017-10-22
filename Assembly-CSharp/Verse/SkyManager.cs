using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class SkyManager
	{
		private Map map;

		private float curSkyGlowInt;

		private List<Pair<SkyOverlay, float>> tempOverlays = new List<Pair<SkyOverlay, float>>();

		private static readonly Color FogOfWarBaseColor = new Color32((byte)77, (byte)69, (byte)66, (byte)255);

		public const float NightMaxCelGlow = 0.1f;

		public const float DuskMaxCelGlow = 0.6f;

		public float CurSkyGlow
		{
			get
			{
				return this.curSkyGlowInt;
			}
		}

		public SkyManager(Map map)
		{
			this.map = map;
		}

		public void SkyManagerUpdate()
		{
			SkyTarget curSky = this.CurrentSkyTarget();
			this.curSkyGlowInt = curSky.glow;
			if (this.map == Find.VisibleMap)
			{
				MatBases.LightOverlay.color = curSky.colors.sky;
				Find.CameraColor.saturation = curSky.colors.saturation;
				Color color = curSky.colors.sky;
				color.a = 1f;
				color *= SkyManager.FogOfWarBaseColor;
				MatBases.FogOfWar.color = color;
				Color color2 = curSky.colors.shadow;
				Vector3? overridenShadowVector = this.GetOverridenShadowVector();
				if (overridenShadowVector.HasValue)
				{
					this.SetSunShadowVector(overridenShadowVector.Value);
				}
				else
				{
					GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow);
					this.SetSunShadowVector(lightSourceInfo.vector);
					color2 = Color.Lerp(Color.white, color2, GenCelestial.CurShadowStrength(this.map));
				}
				GenCelestial.LightInfo lightSourceInfo2 = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingSun);
				GenCelestial.LightInfo lightSourceInfo3 = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.LightingMoon);
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectSun, new Vector4(lightSourceInfo2.vector.x, 0f, lightSourceInfo2.vector.y, lightSourceInfo2.intensity));
				Shader.SetGlobalVector(ShaderPropertyIDs.WaterCastVectMoon, new Vector4(lightSourceInfo3.vector.x, 0f, lightSourceInfo3.vector.y, lightSourceInfo3.intensity));
				Shader.SetGlobalFloat("_LightsourceShineSizeReduction", (float)(20.0 * (1.0 / curSky.lightsourceShineSize)));
				Shader.SetGlobalFloat("_LightsourceShineIntensity", curSky.lightsourceShineIntensity);
				MatBases.SunShadow.color = color2;
				this.UpdateOverlays(curSky);
			}
		}

		public void ForceSetCurSkyGlow(float curSkyGlow)
		{
			this.curSkyGlowInt = curSkyGlow;
		}

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
				this.AddTempOverlay(new Pair<SkyOverlay, float>(overlays2[j], (float)(1.0 - this.map.weatherManager.TransitionLerpFactor)));
			}
			for (int k = 0; k < this.map.gameConditionManager.ActiveConditions.Count; k++)
			{
				GameCondition gameCondition = this.map.gameConditionManager.ActiveConditions[k];
				List<SkyOverlay> list = gameCondition.SkyOverlays();
				if (list != null)
				{
					for (int l = 0; l < list.Count; l++)
					{
						this.AddTempOverlay(new Pair<SkyOverlay, float>(list[l], gameCondition.SkyTargetLerpFactor()));
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

		private void SetSunShadowVector(Vector2 vec)
		{
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSunLightDirection, new Vector4(vec.x, 0f, vec.y, GenCelestial.CurShadowStrength(this.map)));
		}

		private SkyTarget CurrentSkyTarget()
		{
			SkyTarget b = this.map.weatherManager.curWeather.Worker.CurSkyTarget(this.map);
			SkyTarget a = this.map.weatherManager.lastWeather.Worker.CurSkyTarget(this.map);
			SkyTarget skyTarget = SkyTarget.Lerp(a, b, this.map.weatherManager.TransitionLerpFactor);
			float num = this.map.gameConditionManager.AggregateSkyTargetLerpFactor();
			if (num > 9.9999997473787516E-05)
			{
				SkyTarget value = this.map.gameConditionManager.AggregateSkyTarget().Value;
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
				if (compAffectsSky.LerpFactor > 0.0)
				{
					skyTarget = ((!compAffectsSky.Props.lerpDarken) ? SkyTarget.Lerp(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor) : SkyTarget.LerpDarken(skyTarget, compAffectsSky.SkyTarget, compAffectsSky.LerpFactor));
				}
			}
			return skyTarget;
		}

		private Vector3? GetOverridenShadowVector()
		{
			List<WeatherEvent> liveEventsListForReading = this.map.weatherManager.eventHandler.LiveEventsListForReading;
			int num = 0;
			Vector3? result;
			while (true)
			{
				if (num < liveEventsListForReading.Count)
				{
					Vector2? overrideShadowVector = liveEventsListForReading[num].OverrideShadowVector;
					if (overrideShadowVector.HasValue)
					{
						result = overrideShadowVector;
						break;
					}
					num++;
					continue;
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.AffectsSky);
				Vector2? overrideShadowVector2;
				for (int i = 0; i < list.Count; i++)
				{
					overrideShadowVector2 = list[i].TryGetComp<CompAffectsSky>().OverrideShadowVector;
					if (overrideShadowVector2.HasValue)
						goto IL_00b9;
				}
				result = default(Vector3?);
				break;
				IL_00b9:
				result = overrideShadowVector2;
				break;
			}
			return result;
		}

		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SkyManager: ");
			stringBuilder.AppendLine("CurCelestialSunGlow: " + GenCelestial.CurCelestialSunGlow(Find.VisibleMap));
			stringBuilder.AppendLine("CurSkyGlow: " + this.CurSkyGlow.ToStringPercent());
			stringBuilder.AppendLine("CurrentSkyTarget: " + this.CurrentSkyTarget().ToString());
			return stringBuilder.ToString();
		}
	}
}
