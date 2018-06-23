using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200044A RID: 1098
	public sealed class WeatherManager : IExposable
	{
		// Token: 0x04000BA3 RID: 2979
		public Map map;

		// Token: 0x04000BA4 RID: 2980
		public WeatherEventHandler eventHandler = new WeatherEventHandler();

		// Token: 0x04000BA5 RID: 2981
		public WeatherDef curWeather = WeatherDefOf.Clear;

		// Token: 0x04000BA6 RID: 2982
		public WeatherDef lastWeather = WeatherDefOf.Clear;

		// Token: 0x04000BA7 RID: 2983
		public int curWeatherAge = 0;

		// Token: 0x04000BA8 RID: 2984
		private List<Sustainer> ambienceSustainers = new List<Sustainer>();

		// Token: 0x04000BA9 RID: 2985
		public TemperatureMemory growthSeasonMemory;

		// Token: 0x04000BAA RID: 2986
		public const float TransitionTicks = 4000f;

		// Token: 0x0600131E RID: 4894 RVA: 0x000A4B14 File Offset: 0x000A2F14
		public WeatherManager(Map map)
		{
			this.map = map;
			this.growthSeasonMemory = new TemperatureMemory(map);
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x000A4B70 File Offset: 0x000A2F70
		public float TransitionLerpFactor
		{
			get
			{
				float num = (float)this.curWeatherAge / 4000f;
				if (num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x000A4BA8 File Offset: 0x000A2FA8
		public float RainRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.rainRate, this.curWeather.rainRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x000A4BE0 File Offset: 0x000A2FE0
		public float SnowRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.snowRate, this.curWeather.snowRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x000A4C18 File Offset: 0x000A3018
		public float CurWindSpeedFactor
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedFactor, this.curWeather.windSpeedFactor, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06001323 RID: 4899 RVA: 0x000A4C50 File Offset: 0x000A3050
		public float CurMoveSpeedMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.moveSpeedMultiplier, this.curWeather.moveSpeedMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x000A4C88 File Offset: 0x000A3088
		public float CurWeatherAccuracyMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.accuracyMultiplier, this.curWeather.accuracyMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x000A4CC0 File Offset: 0x000A30C0
		public WeatherDef CurPerceivedWeather
		{
			get
			{
				WeatherDef result;
				if (this.curWeather == null)
				{
					result = this.lastWeather;
				}
				else if (this.lastWeather == null)
				{
					result = this.curWeather;
				}
				else
				{
					float num;
					if (this.curWeather.perceivePriority > this.lastWeather.perceivePriority)
					{
						num = 0.18f;
					}
					else if (this.lastWeather.perceivePriority > this.curWeather.perceivePriority)
					{
						num = 0.82f;
					}
					else
					{
						num = 0.5f;
					}
					if (this.TransitionLerpFactor < num)
					{
						result = this.lastWeather;
					}
					else
					{
						result = this.curWeather;
					}
				}
				return result;
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000A4D7C File Offset: 0x000A317C
		public void ExposeData()
		{
			Scribe_Defs.Look<WeatherDef>(ref this.curWeather, "curWeather");
			Scribe_Defs.Look<WeatherDef>(ref this.lastWeather, "lastWeather");
			Scribe_Values.Look<int>(ref this.curWeatherAge, "curWeatherAge", 0, true);
			Scribe_Deep.Look<TemperatureMemory>(ref this.growthSeasonMemory, "growthSeasonMemory", new object[]
			{
				this.map
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ambienceSustainers.Clear();
			}
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x000A4DF1 File Offset: 0x000A31F1
		public void TransitionTo(WeatherDef newWeather)
		{
			this.lastWeather = this.curWeather;
			this.curWeather = newWeather;
			this.curWeatherAge = 0;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000A4E10 File Offset: 0x000A3210
		public void DoWeatherGUI(Rect rect)
		{
			WeatherDef curPerceivedWeather = this.CurPerceivedWeather;
			Text.Anchor = TextAnchor.MiddleRight;
			Rect rect2 = new Rect(rect);
			rect2.width -= 15f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, curPerceivedWeather.LabelCap);
			if (!curPerceivedWeather.description.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, curPerceivedWeather.description);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x000A4E80 File Offset: 0x000A3280
		public void WeatherManagerTick()
		{
			this.eventHandler.WeatherEventHandlerTick();
			this.curWeatherAge++;
			this.curWeather.Worker.WeatherTick(this.map, this.TransitionLerpFactor);
			this.lastWeather.Worker.WeatherTick(this.map, 1f - this.TransitionLerpFactor);
			this.growthSeasonMemory.GrowthSeasonMemoryTick();
			for (int i = 0; i < this.curWeather.ambientSounds.Count; i++)
			{
				bool flag = false;
				for (int j = this.ambienceSustainers.Count - 1; j >= 0; j--)
				{
					if (this.ambienceSustainers[j].def == this.curWeather.ambientSounds[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag && this.VolumeOfAmbientSound(this.curWeather.ambientSounds[i]) > 0.0001f)
				{
					SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
					Sustainer sustainer = this.curWeather.ambientSounds[i].TrySpawnSustainer(info);
					if (sustainer != null)
					{
						this.ambienceSustainers.Add(sustainer);
					}
				}
			}
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x000A4FC1 File Offset: 0x000A33C1
		public void WeatherManagerUpdate()
		{
			this.SetAmbienceSustainersVolume();
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x000A4FCC File Offset: 0x000A33CC
		public void EndAllSustainers()
		{
			for (int i = 0; i < this.ambienceSustainers.Count; i++)
			{
				this.ambienceSustainers[i].End();
			}
			this.ambienceSustainers.Clear();
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x000A5014 File Offset: 0x000A3414
		private void SetAmbienceSustainersVolume()
		{
			for (int i = this.ambienceSustainers.Count - 1; i >= 0; i--)
			{
				float num = this.VolumeOfAmbientSound(this.ambienceSustainers[i].def);
				if (num > 0.0001f)
				{
					this.ambienceSustainers[i].externalParams["LerpFactor"] = num;
				}
				else
				{
					this.ambienceSustainers[i].End();
					this.ambienceSustainers.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x000A50A8 File Offset: 0x000A34A8
		private float VolumeOfAmbientSound(SoundDef soundDef)
		{
			float result;
			if (this.map != Find.CurrentMap)
			{
				result = 0f;
			}
			else
			{
				for (int i = 0; i < Find.WindowStack.Count; i++)
				{
					if (Find.WindowStack[i].silenceAmbientSound)
					{
						return 0f;
					}
				}
				float num = 0f;
				for (int j = 0; j < this.lastWeather.ambientSounds.Count; j++)
				{
					if (this.lastWeather.ambientSounds[j] == soundDef)
					{
						num += 1f - this.TransitionLerpFactor;
					}
				}
				for (int k = 0; k < this.curWeather.ambientSounds.Count; k++)
				{
					if (this.curWeather.ambientSounds[k] == soundDef)
					{
						num += this.TransitionLerpFactor;
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000A51AD File Offset: 0x000A35AD
		public void DrawAllWeather()
		{
			this.eventHandler.WeatherEventsDraw();
			this.lastWeather.Worker.DrawWeather(this.map);
			this.curWeather.Worker.DrawWeather(this.map);
		}
	}
}
