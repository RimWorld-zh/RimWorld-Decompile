using System;
using System.Collections.Generic;
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

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (base.description.NullOrEmpty())
			{
				yield return "no description";
			}
			if (this.needClass == null)
			{
				yield return "needClass is null";
			}
			if (this.needClass == typeof(Need_Seeker))
			{
				if (this.seekerRisePerHour != 0.0 && this.seekerFallPerHour != 0.0)
					yield break;
				yield return "seeker rise/fall rates not set";
			}
		}
	}
}
