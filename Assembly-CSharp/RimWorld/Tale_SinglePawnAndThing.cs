using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000670 RID: 1648
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x06002281 RID: 8833 RVA: 0x00124DD4 File Offset: 0x001231D4
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00124DDD File Offset: 0x001231DD
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00124DF4 File Offset: 0x001231F4
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00124E2B File Offset: 0x0012322B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", new object[0]);
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00124E4C File Offset: 0x0012324C
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

		// Token: 0x06002286 RID: 8838 RVA: 0x00124E76 File Offset: 0x00123276
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x04001389 RID: 5001
		public TaleData_Thing thingData;
	}
}
