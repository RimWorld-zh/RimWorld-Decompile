using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066E RID: 1646
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x04001387 RID: 4999
		public TaleData_Thing thingData;

		// Token: 0x0600227D RID: 8829 RVA: 0x0012505C File Offset: 0x0012345C
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00125065 File Offset: 0x00123465
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x0012507C File Offset: 0x0012347C
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x001250B3 File Offset: 0x001234B3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", new object[0]);
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x001250D4 File Offset: 0x001234D4
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

		// Token: 0x06002282 RID: 8834 RVA: 0x001250FE File Offset: 0x001234FE
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}
	}
}
