using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A9 RID: 1961
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x0400173D RID: 5949
		protected string explanationKey;

		// Token: 0x0400173E RID: 5950
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002B62 RID: 11106
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x06002B63 RID: 11107 RVA: 0x0016ECAC File Offset: 0x0016D0AC
		private IEnumerable<Pawn> AffectedPawns()
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
			{
				if (p.Dead)
				{
					Log.Error("Dead pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists:" + p, false);
				}
				else
				{
					p.needs.mood.thoughts.GetAllMoodThoughts(Alert_Thought.tmpThoughts);
					try
					{
						ThoughtDef requiredDef = this.Thought;
						for (int i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
						{
							if (Alert_Thought.tmpThoughts[i].def == requiredDef)
							{
								yield return p;
								break;
							}
						}
					}
					finally
					{
						Alert_Thought.tmpThoughts.Clear();
					}
				}
			}
			yield break;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x0016ECD8 File Offset: 0x0016D0D8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns());
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x0016ECF8 File Offset: 0x0016D0F8
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns())
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return this.explanationKey.Translate(new object[]
			{
				stringBuilder.ToString()
			});
		}
	}
}
