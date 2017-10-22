using RimWorld.Planet;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_SinglePawn : Tale
	{
		public TaleData_Pawn pawnData;

		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		public override string ShortSummary
		{
			get
			{
				return base.def.LabelCap + ": " + this.pawnData.name;
			}
		}

		public Tale_SinglePawn()
		{
		}

		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				base.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		public override void PostRemove()
		{
			base.PostRemove();
			WorldPawns worldPawns = Find.WorldPawns;
			if (worldPawns.Contains(this.pawnData.pawn))
			{
				worldPawns.DiscardIfUnimportant(this.pawnData.pawn);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", new object[0]);
		}

		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in this.pawnData.GetRules("anyPawn"))
			{
				yield return rule;
			}
			foreach (Rule rule2 in this.pawnData.GetRules("pawn"))
			{
				yield return rule2;
			}
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}
	}
}
