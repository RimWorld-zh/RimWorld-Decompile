using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000B52 RID: 2898
	public class MentalBreakDef : Def
	{
		// Token: 0x040029F5 RID: 10741
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x040029F6 RID: 10742
		public MentalStateDef mentalState;

		// Token: 0x040029F7 RID: 10743
		public float baseCommonality;

		// Token: 0x040029F8 RID: 10744
		public SimpleCurve commonalityFactorPerPopulationCurve = null;

		// Token: 0x040029F9 RID: 10745
		public MentalBreakIntensity intensity = MentalBreakIntensity.None;

		// Token: 0x040029FA RID: 10746
		public TraitDef requiredTrait;

		// Token: 0x040029FB RID: 10747
		private MentalBreakWorker workerInt = null;

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x00216B4C File Offset: 0x00214F4C
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

		// Token: 0x06003F6C RID: 16236 RVA: 0x00216BA4 File Offset: 0x00214FA4
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
