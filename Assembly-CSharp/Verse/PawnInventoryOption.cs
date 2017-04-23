using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public class PawnInventoryOption
	{
		public ThingDef thingDef;

		public IntRange countRange = IntRange.one;

		public float choiceChance = 1f;

		public float skipChance;

		public List<PawnInventoryOption> subOptionsTakeAll;

		public List<PawnInventoryOption> subOptionsChooseOne;

		[DebuggerHidden]
		public IEnumerable<Thing> GenerateThings()
		{
			PawnInventoryOption.<GenerateThings>c__Iterator1CC <GenerateThings>c__Iterator1CC = new PawnInventoryOption.<GenerateThings>c__Iterator1CC();
			<GenerateThings>c__Iterator1CC.<>f__this = this;
			PawnInventoryOption.<GenerateThings>c__Iterator1CC expr_0E = <GenerateThings>c__Iterator1CC;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
