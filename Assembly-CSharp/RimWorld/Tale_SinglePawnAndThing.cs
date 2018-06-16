using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000670 RID: 1648
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x0600227F RID: 8831 RVA: 0x00124D5C File Offset: 0x0012315C
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x00124D65 File Offset: 0x00123165
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00124D7C File Offset: 0x0012317C
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00124DB3 File Offset: 0x001231B3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", new object[0]);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00124DD4 File Offset: 0x001231D4
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

		// Token: 0x06002284 RID: 8836 RVA: 0x00124DFE File Offset: 0x001231FE
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x04001389 RID: 5001
		public TaleData_Thing thingData;
	}
}
