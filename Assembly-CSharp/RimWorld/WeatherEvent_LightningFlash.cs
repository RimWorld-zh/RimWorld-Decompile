using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200044A RID: 1098
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x04000B9B RID: 2971
		private int duration;

		// Token: 0x04000B9C RID: 2972
		private Vector2 shadowVector;

		// Token: 0x04000B9D RID: 2973
		private int age = 0;

		// Token: 0x04000B9E RID: 2974
		private const int FlashFadeInTicks = 3;

		// Token: 0x04000B9F RID: 2975
		private const int MinFlashDuration = 15;

		// Token: 0x04000BA0 RID: 2976
		private const int MaxFlashDuration = 60;

		// Token: 0x04000BA1 RID: 2977
		private const float FlashShadowDistance = 5f;

		// Token: 0x04000BA2 RID: 2978
		private static readonly SkyColorSet LightningFlashColors = new SkyColorSet(new Color(0.9f, 0.95f, 1f), new Color(0.784313738f, 0.8235294f, 0.847058833f), new Color(0.9f, 0.95f, 1f), 1.15f);

		// Token: 0x06001312 RID: 4882 RVA: 0x000A4AE4 File Offset: 0x000A2EE4
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06001313 RID: 4883 RVA: 0x000A4B38 File Offset: 0x000A2F38
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x000A4B5C File Offset: 0x000A2F5C
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x000A4B8C File Offset: 0x000A2F8C
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x000A4BAC File Offset: 0x000A2FAC
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x000A4BC8 File Offset: 0x000A2FC8
		protected float LightningBrightness
		{
			get
			{
				float result;
				if (this.age <= 3)
				{
					result = (float)this.age / 3f;
				}
				else
				{
					result = 1f - (float)this.age / (float)this.duration;
				}
				return result;
			}
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x000A4C13 File Offset: 0x000A3013
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x000A4C26 File Offset: 0x000A3026
		public override void WeatherEventTick()
		{
			this.age++;
		}
	}
}
