using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064A RID: 1610
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06002179 RID: 8569
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x0600217A RID: 8570 RVA: 0x0011C538 File Offset: 0x0011A938
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
