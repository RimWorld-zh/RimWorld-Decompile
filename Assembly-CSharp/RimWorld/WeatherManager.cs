using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200044E RID: 1102
	public sealed class WeatherManager : IExposable
	{
		// Token: 0x06001327 RID: 4903 RVA: 0x000A4B04 File Offset: 0x000A2F04
		public WeatherManager(Map map)
		{
			this.map = map;
			this.growthSeasonMemory = new TemperatureMemory(map);
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x000A4B60 File Offset: 0x000A2F60
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
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x000A4B98 File Offset: 0x000A2F98
		public float RainRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.rainRate, this.curWeather.rainRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x000A4BD0 File Offset: 0x000A2FD0
		public float SnowRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.snowRate, this.curWeather.snowRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x000A4C08 File Offset: 0x000A3008
		public float CurWindSpeedFactor
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedFactor, this.curWeather.windSpeedFactor, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x000A4C40 File Offset: 0x000A3040
		public float CurMoveSpeedMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.moveSpeedMultiplier, this.curWeather.moveSpeedMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x0600132D RID: 4909 RVA: 0x000A4C78 File Offset: 0x000A3078
		public float CurWeatherAccuracyMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.accuracyMultiplier, this.curWeather.accuracyMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x000A4CB0 File Offset: 0x000A30B0
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

		// Token: 0x0600132F RID: 4911 RVA: 0x000A4D6C File Offset: 0x000A316C
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

		// Token: 0x06001330 RID: 4912 RVA: 0x000A4DE1 File Offset: 0x000A31E1
		public void TransitionTo(WeatherDef newWeather)
		{
			this.lastWeather = this.curWeather;
			this.curWeather = newWeather;
			this.curWeatherAge = 0;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000A4E00 File Offset: 0x000A3200
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

		// Token: 0x06001332 RID: 4914 RVA: 0x000A4E70 File Offset: 0x000A3270
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

		// Token: 0x06001333 RID: 4915 RVA: 0x000A4FB1 File Offset: 0x000A33B1
		public void WeatherManagerUpdate()
		{
			this.SetAmbienceSustainersVolume();
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x000A4FBC File Offset: 0x000A33BC
		public void EndAllSustainers()
		{
			for (int i = 0; i < this.ambienceSustainers.Count; i++)
			{
				this.ambienceSustainers[i].End();
			}
			this.ambienceSustainers.Clear();
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x000A5004 File Offset: 0x000A3404
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

		// Token: 0x06001336 RID: 4918 RVA: 0x000A5098 File Offset: 0x000A3498
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

		// Token: 0x06001337 RID: 4919 RVA: 0x000A519D File Offset: 0x000A359D
		public void DrawAllWeather()
		{
			this.eventHandler.WeatherEventsDraw();
			this.lastWeather.Worker.DrawWeather(this.map);
			this.curWeather.Worker.DrawWeather(this.map);
		}

		// Token: 0x04000BA6 RID: 2982
		public Map map;

		// Token: 0x04000BA7 RID: 2983
		public WeatherEventHandler eventHandler = new WeatherEventHandler();

		// Token: 0x04000BA8 RID: 2984
		public WeatherDef curWeather = WeatherDefOf.Clear;

		// Token: 0x04000BA9 RID: 2985
		public WeatherDef lastWeather = WeatherDefOf.Clear;

		// Token: 0x04000BAA RID: 2986
		public int curWeatherAge = 0;

		// Token: 0x04000BAB RID: 2987
		private List<Sustainer> ambienceSustainers = new List<Sustainer>();

		// Token: 0x04000BAC RID: 2988
		public TemperatureMemory growthSeasonMemory;

		// Token: 0x04000BAD RID: 2989
		public const float TransitionTicks = 4000f;
	}
}
