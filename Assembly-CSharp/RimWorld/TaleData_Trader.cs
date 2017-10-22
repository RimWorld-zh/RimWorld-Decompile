using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleData_Trader : TaleData
	{
		public string name;

		public int pawnID = -1;

		public Gender gender = Gender.Male;

		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", (string)null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		public override IEnumerable<Rule> GetRules(string prefix)
		{
			string nameFull = (!this.IsPawn) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name) : this.name;
			yield return (Rule)new Rule_String(prefix + "_nameFull", nameFull);
			string nameShortIndefinite = (!this.IsPawn) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name) : this.name;
			yield return (Rule)new Rule_String(prefix + "_nameShortIndefinite", nameShortIndefinite);
			string nameShortDefinite = (!this.IsPawn) ? Find.ActiveLanguageWorker.WithDefiniteArticle(this.name) : this.name;
			yield return (Rule)new Rule_String(prefix + "_nameShortDefinite", nameShortDefinite);
			yield return (Rule)new Rule_String(prefix + "_pronoun", this.gender.GetPronoun());
			yield return (Rule)new Rule_String(prefix + "_possessive", this.gender.GetPossessive());
		}

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

		public static TaleData_Trader GenerateRandom()
		{
			PawnKindDef pawnKindDef = (from d in DefDatabase<PawnKindDef>.AllDefs
			where d.trader
			select d).RandomElement();
			Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType));
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			return TaleData_Trader.GenerateFrom(pawn);
		}
	}
}
