using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				string text = this.def.LabelCap + ": " + this.firstPawnData.name;
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
				this.surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
			}
		}

		public override bool Concerns(Thing th)
		{
			return (this.secondPawnData != null && this.secondPawnData.pawn == th) || base.Concerns(th) || this.firstPawnData.pawn == th;
		}

		public override void PostRemove()
		{
			base.PostRemove();
			WorldPawns worldPawns = Find.WorldPawns;
			if (worldPawns.Contains(this.firstPawnData.pawn))
			{
				worldPawns.DiscardIfUnimportant(this.firstPawnData.pawn);
			}
			if (this.secondPawnData != null && worldPawns.Contains(this.secondPawnData.pawn))
			{
				worldPawns.DiscardIfUnimportant(this.secondPawnData.pawn);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.firstPawnData, "firstPawnData", new object[0]);
			Scribe_Deep.Look<TaleData_Pawn>(ref this.secondPawnData, "secondPawnData", new object[0]);
		}

		[DebuggerHidden]
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			Tale_DoublePawn.<SpecialTextGenerationRules>c__Iterator130 <SpecialTextGenerationRules>c__Iterator = new Tale_DoublePawn.<SpecialTextGenerationRules>c__Iterator130();
			<SpecialTextGenerationRules>c__Iterator.<>f__this = this;
			Tale_DoublePawn.<SpecialTextGenerationRules>c__Iterator130 expr_0E = <SpecialTextGenerationRules>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.firstPawnData = TaleData_Pawn.GenerateRandom();
			this.secondPawnData = TaleData_Pawn.GenerateRandom();
		}
	}
}
