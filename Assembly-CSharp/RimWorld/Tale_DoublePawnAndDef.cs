using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000667 RID: 1639
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x04001383 RID: 4995
		public TaleData_Def defData;

		// Token: 0x0600225C RID: 8796 RVA: 0x00123FEC File Offset: 0x001223EC
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00123FF5 File Offset: 0x001223F5
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x0012400C File Offset: 0x0012240C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x0012402C File Offset: 0x0012242C
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

		// Token: 0x06002260 RID: 8800 RVA: 0x00124056 File Offset: 0x00122456
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}
	}
}
