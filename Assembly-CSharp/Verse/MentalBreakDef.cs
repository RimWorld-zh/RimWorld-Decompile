using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000B50 RID: 2896
	public class MentalBreakDef : Def
	{
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x00216A70 File Offset: 0x00214E70
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

		// Token: 0x06003F69 RID: 16233 RVA: 0x00216AC8 File Offset: 0x00214EC8
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
	}
}
