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

		public bool scaleBar;

		public float fallPerDay = 0.5f;

		public float seekerRisePerHour;

		public float seekerFallPerHour;

		public bool freezeWhileSleeping;

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (base.description.NullOrEmpty())
			{
				yield return "no description";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.needClass == null)
			{
				yield return "needClass is null";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.needClass != typeof(Need_Seeker))
				yield break;
			if (this.seekerRisePerHour != 0.0 && this.seekerFallPerHour != 0.0)
				yield break;
			yield return "seeker rise/fall rates not set";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_018b:
			/*Error near IL_018c: Unexpected return in MoveNext()*/;
		}
	}
}
