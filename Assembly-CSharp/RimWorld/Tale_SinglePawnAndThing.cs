using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066C RID: 1644
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x06002279 RID: 8825 RVA: 0x00124F0C File Offset: 0x0012330C
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00124F15 File Offset: 0x00123315
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00124F2C File Offset: 0x0012332C
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x00124F63 File Offset: 0x00123363
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", new object[0]);
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x00124F84 File Offset: 0x00123384
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule r in this.<SpecialTextGenerationRules>__BaseCallProxy0())
			{
				yield return r;
			}
			foreach (Rule r2 in this.thingData.GetRules("THING"))
			{
				yield return r2;
			}
			yield break;
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00124FAE File Offset: 0x001233AE
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x04001387 RID: 4999
		public TaleData_Thing thingData;
	}
}
