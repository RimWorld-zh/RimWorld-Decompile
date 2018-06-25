using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066D RID: 1645
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x04001386 RID: 4998
		public TaleData_Def defData;

		// Token: 0x06002277 RID: 8823 RVA: 0x00124CA8 File Offset: 0x001230A8
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00124CB1 File Offset: 0x001230B1
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00124CC7 File Offset: 0x001230C7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00124CE8 File Offset: 0x001230E8
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			if (this.def.defSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses tale type with def but defSymbol is not set.", false);
			}
			foreach (Rule r in this.<SpecialTextGenerationRules>__BaseCallProxy0())
			{
				yield return r;
			}
			foreach (Rule r2 in this.defData.GetRules(this.def.defSymbol))
			{
				yield return r2;
			}
			yield break;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00124D12 File Offset: 0x00123112
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}
	}
}
