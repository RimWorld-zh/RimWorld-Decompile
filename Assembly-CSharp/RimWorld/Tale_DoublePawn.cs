using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_DoublePawn : Tale
	{
		public TaleData_Pawn firstPawnData;

		public TaleData_Pawn secondPawnData;

		public override Pawn DominantPawn
		{
			get
			{
				return this.firstPawnData.pawn;
			}
		}

		public override string ShortSummary
		{
			get
			{
				string text = base.def.LabelCap + ": " + this.firstPawnData.name;
				if (this.secondPawnData != null)
				{
					text = text + ", " + this.secondPawnData.name;
				}
				return text;
			}
		}

		public Tale_DoublePawn()
		{
		}

		public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
		{
			this.firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
			if (secondPawn != null)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
			}
			if (firstPawn.SpawnedOrAnyParentSpawned)
			{
				base.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", new object[0]);
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", new object[0]);
		}

		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			using (IEnumerator<Rule> enumerator = this.firstPawnData.GetRules("anyPawn").GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Rule r4 = enumerator.Current;
					yield return r4;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<Rule> enumerator2 = this.firstPawnData.GetRules(base.def.firstPawnSymbol).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Rule r3 = enumerator2.Current;
					yield return r3;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.secondPawnData != null)
			{
				using (IEnumerator<Rule> enumerator3 = this.firstPawnData.GetRules("anyPawn").GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						Rule r2 = enumerator3.Current;
						yield return r2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				using (IEnumerator<Rule> enumerator4 = this.secondPawnData.GetRules(base.def.secondPawnSymbol).GetEnumerator())
				{
					if (enumerator4.MoveNext())
					{
						Rule r = enumerator4.Current;
						yield return r;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_02c9:
			/*Error near IL_02ca: Unexpected return in MoveNext()*/;
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}
	}
}
