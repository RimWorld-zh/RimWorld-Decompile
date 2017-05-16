using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		public Map map;

		public Faction faction;

		public Pawn kidnapped;

		public int fee;

		protected override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_RansomDemand.<>c__Iterator19F <>c__Iterator19F = new ChoiceLetter_RansomDemand.<>c__Iterator19F();
				<>c__Iterator19F.<>f__this = this;
				ChoiceLetter_RansomDemand.<>c__Iterator19F expr_0E = <>c__Iterator19F;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public override bool StillValid
		{
			get
			{
				return base.StillValid && Find.Maps.Contains(this.map) && this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}
	}
}
