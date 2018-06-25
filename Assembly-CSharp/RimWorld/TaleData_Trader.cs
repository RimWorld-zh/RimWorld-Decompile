using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065F RID: 1631
	public class TaleData_Trader : TaleData
	{
		// Token: 0x04001363 RID: 4963
		public string name;

		// Token: 0x04001364 RID: 4964
		public int pawnID = -1;

		// Token: 0x04001365 RID: 4965
		public Gender gender = Gender.Male;

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x001215A4 File Offset: 0x0011F9A4
		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x001215C5 File Offset: 0x0011F9C5
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00121600 File Offset: 0x0011FA00
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

		// Token: 0x06002213 RID: 8723 RVA: 0x00121634 File Offset: 0x0011FA34
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

		// Token: 0x06002214 RID: 8724 RVA: 0x00121684 File Offset: 0x0011FA84
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
