using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066B RID: 1643
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x06002262 RID: 8802 RVA: 0x00123E3C File Offset: 0x0012223C
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x00123E45 File Offset: 0x00122245
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x00123E5C File Offset: 0x0012225C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00123E7C File Offset: 0x0012227C
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

		// Token: 0x06002266 RID: 8806 RVA: 0x00123EA6 File Offset: 0x001222A6
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04001385 RID: 4997
		public TaleData_Def defData;
	}
}
