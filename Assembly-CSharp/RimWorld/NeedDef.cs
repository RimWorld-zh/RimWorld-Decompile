using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class NeedDef : Def
	{
		public Type needClass;

		public Intelligence minIntelligence = Intelligence.Animal;

		public bool colonistAndPrisonersOnly = false;

		public bool colonistsOnly = false;

		public bool onlyIfCausedByHediff = false;

		public bool neverOnPrisoner = false;

		public bool showOnNeedList = true;

		public float baseLevel = 0.5f;

		public bool major = false;

		public int listPriority = 0;

		public string tutorHighlightTag = (string)null;

		public bool showForCaravanMembers = false;

		public bool scaleBar = false;

		public float fallPerDay = 0.5f;

		public float seekerRisePerHour = 0f;

		public float seekerFallPerHour = 0f;

		public bool freezeWhileSleeping = false;

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
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
			IL_0191:
			/*Error near IL_0192: Unexpected return in MoveNext()*/;
		}
	}
}
