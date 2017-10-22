using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public abstract class Alert_Thought : Alert
	{
		protected string explanationKey;

		private static List<Thought> tmpThoughts = new List<Thought>();

		protected abstract ThoughtDef Thought
		{
			get;
		}

		private IEnumerable<Pawn> AffectedPawns()
		{
			foreach (Pawn allMaps_FreeColonist in PawnsFinder.AllMaps_FreeColonists)
			{
				if (allMaps_FreeColonist.Dead)
				{
					Log.Error("Dead pawn in PawnsFinder.AllMaps_FreeColonists:" + allMaps_FreeColonist);
				}
				else
				{
					allMaps_FreeColonist.needs.mood.thoughts.GetAllMoodThoughts(Alert_Thought.tmpThoughts);
					try
					{
						ThoughtDef requiredDef = this.Thought;
						int i = 0;
						while (i < Alert_Thought.tmpThoughts.Count)
						{
							if (Alert_Thought.tmpThoughts[i].def != requiredDef)
							{
								i++;
								continue;
							}
							yield return allMaps_FreeColonist;
							break;
						}
					}
					finally
					{
						((_003CAffectedPawns_003Ec__Iterator18B)/*Error near IL_0138: stateMachine*/)._003C_003E__Finally0();
					}
				}
			}
		}

		public override AlertReport GetReport()
		{
			Pawn pawn = this.AffectedPawns().FirstOrDefault();
			if (pawn != null)
			{
				return AlertReport.CulpritIs((Thing)pawn);
			}
			return AlertReport.Inactive;
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn item in this.AffectedPawns())
			{
				stringBuilder.AppendLine("    " + item.NameStringShort);
			}
			return this.explanationKey.Translate(stringBuilder.ToString());
		}
	}
}
