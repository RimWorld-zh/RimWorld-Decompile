using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C8 RID: 1480
	public class WorldInfo : IExposable
	{
		// Token: 0x04001149 RID: 4425
		public string name = "DefaultWorldName";

		// Token: 0x0400114A RID: 4426
		public float planetCoverage;

		// Token: 0x0400114B RID: 4427
		public string seedString = "SeedError";

		// Token: 0x0400114C RID: 4428
		public int randomValue = Rand.Int;

		// Token: 0x0400114D RID: 4429
		public OverallRainfall overallRainfall = OverallRainfall.Normal;

		// Token: 0x0400114E RID: 4430
		public OverallTemperature overallTemperature = OverallTemperature.Normal;

		// Token: 0x0400114F RID: 4431
		public IntVec3 initialMapSize = new IntVec3(250, 1, 250);

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001CBA RID: 7354 RVA: 0x000F6D00 File Offset: 0x000F5100
		public string FileNameNoExtension
		{
			get
			{
				return GenText.CapitalizedNoSpaces(this.name);
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x000F6D20 File Offset: 0x000F5120
		public int Seed
		{
			get
			{
				return GenText.StableStringHash(this.seedString);
			}
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000F6D40 File Offset: 0x000F5140
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
	}
}
