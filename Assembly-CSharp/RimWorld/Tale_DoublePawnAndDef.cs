using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000669 RID: 1641
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x04001387 RID: 4999
		public TaleData_Def defData;

		// Token: 0x0600225F RID: 8799 RVA: 0x001243A4 File Offset: 0x001227A4
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x001243AD File Offset: 0x001227AD
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x001243C4 File Offset: 0x001227C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x001243E4 File Offset: 0x001227E4
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

		// Token: 0x06002263 RID: 8803 RVA: 0x0012440E File Offset: 0x0012280E
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}
	}
}
