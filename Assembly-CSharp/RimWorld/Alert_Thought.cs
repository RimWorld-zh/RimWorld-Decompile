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
				else if (!ThingOwnerUtility.ContentsFrozen(allMaps_FreeColonist.ParentHolder))
				{
					allMaps_FreeColonist.needs.mood.thoughts.GetAllMoodThoughts(Alert_Thought.tmpThoughts);
					try
					{
						ThoughtDef requiredDef = this.Thought;
						for (int i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
						{
							if (Alert_Thought.tmpThoughts[i].def == requiredDef)
							{
								yield return allMaps_FreeColonist;
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
					}
					finally
					{
						((_003CAffectedPawns_003Ec__Iterator0)/*Error near IL_0163: stateMachine*/)._003C_003E__Finally0();
					}
				}
			}
			yield break;
			IL_01a4:
			/*Error near IL_01a5: Unexpected return in MoveNext()*/;
		}

		public override AlertReport GetReport()
		{
			Pawn pawn = this.AffectedPawns().FirstOrDefault();
			return (pawn == null) ? AlertReport.Inactive : AlertReport.CulpritIs((Thing)pawn);
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
