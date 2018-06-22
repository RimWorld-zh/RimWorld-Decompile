using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066B RID: 1643
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x06002273 RID: 8819 RVA: 0x00124B58 File Offset: 0x00122F58
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00124B61 File Offset: 0x00122F61
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00124B77 File Offset: 0x00122F77
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x00124B98 File Offset: 0x00122F98
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

		// Token: 0x06002277 RID: 8823 RVA: 0x00124BC2 File Offset: 0x00122FC2
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04001386 RID: 4998
		public TaleData_Def defData;
	}
}
