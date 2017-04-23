using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class NeedDef : Def
	{
		public Type needClass;

		public Intelligence minIntelligence;

		public bool colonistAndPrisonersOnly;

		public bool colonistsOnly;

		public bool onlyIfCausedByHediff;

		public bool neverOnPrisoner;

		public bool showOnNeedList = true;

		public float baseLevel = 0.5f;

		public bool major;

		public int listPriority;

		public string tutorHighlightTag;

		public bool showForCaravanMembers;

		public float fallPerDay = 0.5f;

		public float seekerRisePerHour;

		public float seekerFallPerHour;

		public bool freezeWhileSleeping;

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			NeedDef.<ConfigErrors>c__Iterator90 <ConfigErrors>c__Iterator = new NeedDef.<ConfigErrors>c__Iterator90();
			<ConfigErrors>c__Iterator.<>f__this = this;
			NeedDef.<ConfigErrors>c__Iterator90 expr_0E = <ConfigErrors>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
