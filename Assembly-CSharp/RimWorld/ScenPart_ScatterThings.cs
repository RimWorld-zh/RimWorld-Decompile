using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064E RID: 1614
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06002181 RID: 8577
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x06002182 RID: 8578 RVA: 0x0011C438 File Offset: 0x0011A838
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData != null)
			{
				new GenStep_ScatterThings
				{
					nearPlayerStart = this.NearPlayerStart,
					thingDef = this.thingDef,
					stuff = this.stuff,
					count = this.count,
					spotMustBeStandable = true,
					minSpacing = 5f,
					clusterSize = ((this.thingDef.category != ThingCategory.Building) ? 4 : 1)
				}.Generate(map);
			}
		}
	}
}
