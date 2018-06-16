using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200044C RID: 1100
	public class WeatherEvent_LightningFlash : WeatherEvent
	{
		// Token: 0x06001318 RID: 4888 RVA: 0x000A4778 File Offset: 0x000A2B78
		public WeatherEvent_LightningFlash(Map map) : base(map)
		{
			this.duration = Rand.Range(15, 60);
			this.shadowVector = new Vector2(Rand.Range(-5f, 5f), Rand.Range(-5f, 0f));
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x000A47CC File Offset: 0x000A2BCC
		public override bool Expired
		{
			get
			{
				return this.age > this.duration;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x000A47F0 File Offset: 0x000A2BF0
		public override SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(1f, WeatherEvent_LightningFlash.LightningFlashColors, 1f, 1f);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x000A4820 File Offset: 0x000A2C20
		public override Vector2? OverrideShadowVector
		{
			get
			{
				return new Vector2?(this.shadowVector);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x000A4840 File Offset: 0x000A2C40
		public override float SkyTargetLerpFactor
		{
			get
			{
				return this.LightningBrightness;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x000A485C File Offset: 0x000A2C5C
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

		// Token: 0x0600131E RID: 4894 RVA: 0x000A48A7 File Offset: 0x000A2CA7
		public override void FireEvent()
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(this.map);
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x000A48BA File Offset: 0x000A2CBA
		public override void WeatherEventTick()
		{
			this.age++;
		}

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
	}
}
