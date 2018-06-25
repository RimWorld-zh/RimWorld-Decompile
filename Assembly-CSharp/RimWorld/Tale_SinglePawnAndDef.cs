using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066D RID: 1645
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x0400138A RID: 5002
		public TaleData_Def defData;

		// Token: 0x06002276 RID: 8822 RVA: 0x00124F10 File Offset: 0x00123310
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x00124F19 File Offset: 0x00123319
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00124F2F File Offset: 0x0012332F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00124F50 File Offset: 0x00123350
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

		// Token: 0x0600227A RID: 8826 RVA: 0x00124F7A File Offset: 0x0012337A
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}
	}
}
