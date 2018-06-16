using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CA RID: 1482
	public class WorldInfo : IExposable
	{
		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x000F6AE4 File Offset: 0x000F4EE4
		public string FileNameNoExtension
		{
			get
			{
				return GenText.CapitalizedNoSpaces(this.name);
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x000F6B04 File Offset: 0x000F4F04
		public int Seed
		{
			get
			{
				return GenText.StableStringHash(this.seedString);
			}
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000F6B24 File Offset: 0x000F4F24
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<float>(ref this.planetCoverage, "planetCoverage", 0f, false);
			Scribe_Values.Look<string>(ref this.seedString, "seedString", null, false);
			Scribe_Values.Look<int>(ref this.randomValue, "randomValue", 0, false);
			Scribe_Values.Look<OverallRainfall>(ref this.overallRainfall, "overallRainfall", OverallRainfall.AlmostNone, false);
			Scribe_Values.Look<OverallTemperature>(ref this.overallTemperature, "overallTemperature", OverallTemperature.VeryCold, false);
			Scribe_Values.Look<IntVec3>(ref this.initialMapSize, "initialMapSize", default(IntVec3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.WorldInfoPostLoadInit(this);
			}
		}

		// Token: 0x0400114C RID: 4428
		public string name = "DefaultWorldName";

		// Token: 0x0400114D RID: 4429
		public float planetCoverage;

		// Token: 0x0400114E RID: 4430
		public string seedString = "SeedError";

		// Token: 0x0400114F RID: 4431
		public int randomValue = Rand.Int;

		// Token: 0x04001150 RID: 4432
		public OverallRainfall overallRainfall = OverallRainfall.Normal;

		// Token: 0x04001151 RID: 4433
		public OverallTemperature overallTemperature = OverallTemperature.Normal;

		// Token: 0x04001152 RID: 4434
		public IntVec3 initialMapSize = new IntVec3(250, 1, 250);
	}
}
