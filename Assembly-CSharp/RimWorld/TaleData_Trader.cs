using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065D RID: 1629
	public class TaleData_Trader : TaleData
	{
		// Token: 0x0400135F RID: 4959
		public string name;

		// Token: 0x04001360 RID: 4960
		public int pawnID = -1;

		// Token: 0x04001361 RID: 4961
		public Gender gender = Gender.Male;

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x001211EC File Offset: 0x0011F5EC
		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x0012120D File Offset: 0x0011F60D
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x00121248 File Offset: 0x0011F648
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			string nameFull;
			if (this.IsPawn)
			{
				nameFull = this.name;
			}
			else
			{
				nameFull = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name);
			}
			yield return new Rule_String(prefix + "_nameFull", nameFull);
			string nameShortIndefinite;
			if (this.IsPawn)
			{
				nameShortIndefinite = this.name;
			}
			else
			{
				nameShortIndefinite = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name);
			}
			yield return new Rule_String(prefix + "_indefinite", nameShortIndefinite);
			yield return new Rule_String(prefix + "_nameIndef", nameShortIndefinite);
			string nameShortDefinite;
			if (this.IsPawn)
			{
				nameShortDefinite = this.name;
			}
			else
			{
				nameShortDefinite = Find.ActiveLanguageWorker.WithDefiniteArticle(this.name);
			}
			yield return new Rule_String(prefix + "_definite", nameShortDefinite);
			yield return new Rule_String(prefix + "_nameDef", nameShortDefinite);
			yield return new Rule_String(prefix + "_pronoun", this.gender.GetPronoun());
			yield return new Rule_String(prefix + "_possessive", this.gender.GetPossessive());
			yield break;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0012127C File Offset: 0x0011F67C
		public static TaleData_Trader GenerateFrom(ITrader trader)
		{
			TaleData_Trader taleData_Trader = new TaleData_Trader();
			taleData_Trader.name = trader.TraderName;
			Pawn pawn = trader as Pawn;
			if (pawn != null)
			{
				taleData_Trader.pawnID = pawn.thingIDNumber;
				taleData_Trader.gender = pawn.gender;
			}
			return taleData_Trader;
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x001212CC File Offset: 0x0011F6CC
		public static TaleData_Trader GenerateRandom()
		{
			PawnKindDef pawnKindDef = (from d in DefDatabase<PawnKindDef>.AllDefs
			where d.trader
			select d).RandomElement<PawnKindDef>();
			Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType));
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			return TaleData_Trader.GenerateFrom(pawn);
		}
	}
}
