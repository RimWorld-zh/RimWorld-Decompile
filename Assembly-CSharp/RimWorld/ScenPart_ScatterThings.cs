using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064C RID: 1612
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600217C RID: 8572
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x0600217D RID: 8573 RVA: 0x0011C8F0 File Offset: 0x0011ACF0
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
