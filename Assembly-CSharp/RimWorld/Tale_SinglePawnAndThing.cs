using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066E RID: 1646
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x0400138B RID: 5003
		public TaleData_Thing thingData;

		// Token: 0x0600227C RID: 8828 RVA: 0x001252C4 File Offset: 0x001236C4
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x001252CD File Offset: 0x001236CD
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x001252E4 File Offset: 0x001236E4
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x0012531B File Offset: 0x0012371B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", new object[0]);
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x0012533C File Offset: 0x0012373C
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

		// Token: 0x06002281 RID: 8833 RVA: 0x00125366 File Offset: 0x00123766
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}
	}
}
