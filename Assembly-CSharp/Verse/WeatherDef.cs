using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BB4 RID: 2996
	public class WeatherDef : Def
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060040EA RID: 16618 RVA: 0x0022432C File Offset: 0x0022272C
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

		// Token: 0x060040EB RID: 16619 RVA: 0x0022435E File Offset: 0x0022275E
		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00224374 File Offset: 0x00222774
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x002243A0 File Offset: 0x002227A0
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		// Token: 0x04002C1E RID: 11294
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x04002C1F RID: 11295
		public bool repeatable = false;

		// Token: 0x04002C20 RID: 11296
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x04002C21 RID: 11297
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x04002C22 RID: 11298
		public SimpleCurve commonalityRainfallFactor = null;

		// Token: 0x04002C23 RID: 11299
		public float rainRate = 0f;

		// Token: 0x04002C24 RID: 11300
		public float snowRate = 0f;

		// Token: 0x04002C25 RID: 11301
		public float windSpeedFactor = 1f;

		// Token: 0x04002C26 RID: 11302
		public float moveSpeedMultiplier = 1f;

		// Token: 0x04002C27 RID: 11303
		public float accuracyMultiplier = 1f;

		// Token: 0x04002C28 RID: 11304
		public float perceivePriority;

		// Token: 0x04002C29 RID: 11305
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x04002C2A RID: 11306
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x04002C2B RID: 11307
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x04002C2C RID: 11308
		public SkyColorSet skyColorsNightMid;

		// Token: 0x04002C2D RID: 11309
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x04002C2E RID: 11310
		public SkyColorSet skyColorsDay;

		// Token: 0x04002C2F RID: 11311
		public SkyColorSet skyColorsDusk;

		// Token: 0x04002C30 RID: 11312
		[Unsaved]
		private WeatherWorker workerInt;
	}
}
