using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000B53 RID: 2899
	public class MentalBreakDef : Def
	{
		// Token: 0x040029FC RID: 10748
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x040029FD RID: 10749
		public MentalStateDef mentalState;

		// Token: 0x040029FE RID: 10750
		public float baseCommonality;

		// Token: 0x040029FF RID: 10751
		public SimpleCurve commonalityFactorPerPopulationCurve = null;

		// Token: 0x04002A00 RID: 10752
		public MentalBreakIntensity intensity = MentalBreakIntensity.None;

		// Token: 0x04002A01 RID: 10753
		public TraitDef requiredTrait;

		// Token: 0x04002A02 RID: 10754
		private MentalBreakWorker workerInt = null;

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x00216E2C File Offset: 0x0021522C
		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.workerClass != null)
					{
						this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
						this.workerInt.def = this;
					}
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x00216E84 File Offset: 0x00215284
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.intensity == MentalBreakIntensity.None)
			{
				yield return "intensity not set";
			}
			yield break;
		}
	}
}
