using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ChemicalDef : Def
	{
		public HediffDef addictionHediff;

		public HediffDef toleranceHediff;

		public bool canBinge = true;

		public float onGeneratedAddictedToleranceChance = 0f;

		public List<HediffGiver_Event> onGeneratedAddictedEvents = null;
	}
}
