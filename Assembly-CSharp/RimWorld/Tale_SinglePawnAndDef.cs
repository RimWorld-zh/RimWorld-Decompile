using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066F RID: 1647
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x06002279 RID: 8825 RVA: 0x001249A8 File Offset: 0x00122DA8
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x001249B1 File Offset: 0x00122DB1
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x001249C7 File Offset: 0x00122DC7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x001249E8 File Offset: 0x00122DE8
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

		// Token: 0x0600227D RID: 8829 RVA: 0x00124A12 File Offset: 0x00122E12
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04001388 RID: 5000
		public TaleData_Def defData;
	}
}
