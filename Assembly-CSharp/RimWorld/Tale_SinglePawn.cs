using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return this.def.LabelCap + ": " + this.pawnData.name;
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
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
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

		[DebuggerHidden]
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			Tale_SinglePawn.<SpecialTextGenerationRules>c__Iterator135 <SpecialTextGenerationRules>c__Iterator = new Tale_SinglePawn.<SpecialTextGenerationRules>c__Iterator135();
			<SpecialTextGenerationRules>c__Iterator.<>f__this = this;
			Tale_SinglePawn.<SpecialTextGenerationRules>c__Iterator135 expr_0E = <SpecialTextGenerationRules>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom();
		}
	}
}
