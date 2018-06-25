using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BB2 RID: 2994
	public class WeatherDef : Def
	{
		// Token: 0x04002C23 RID: 11299
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x04002C24 RID: 11300
		public bool repeatable = false;

		// Token: 0x04002C25 RID: 11301
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x04002C26 RID: 11302
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x04002C27 RID: 11303
		public SimpleCurve commonalityRainfallFactor = null;

		// Token: 0x04002C28 RID: 11304
		public float rainRate = 0f;

		// Token: 0x04002C29 RID: 11305
		public float snowRate = 0f;

		// Token: 0x04002C2A RID: 11306
		public float windSpeedFactor = 1f;

		// Token: 0x04002C2B RID: 11307
		public float moveSpeedMultiplier = 1f;

		// Token: 0x04002C2C RID: 11308
		public float accuracyMultiplier = 1f;

		// Token: 0x04002C2D RID: 11309
		public float perceivePriority;

		// Token: 0x04002C2E RID: 11310
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x04002C2F RID: 11311
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x04002C30 RID: 11312
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x04002C31 RID: 11313
		public SkyColorSet skyColorsNightMid;

		// Token: 0x04002C32 RID: 11314
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x04002C33 RID: 11315
		public SkyColorSet skyColorsDay;

		// Token: 0x04002C34 RID: 11316
		public SkyColorSet skyColorsDusk;

		// Token: 0x04002C35 RID: 11317
		[Unsaved]
		private WeatherWorker workerInt;

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060040EF RID: 16623 RVA: 0x00224ADC File Offset: 0x00222EDC
		public WeatherWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = new WeatherWorker(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x00224B0E File Offset: 0x00222F0E
		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00224B24 File Offset: 0x00222F24
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x00224B50 File Offset: 0x00222F50
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}
	}
}
