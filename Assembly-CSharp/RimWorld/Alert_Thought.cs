using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AB RID: 1963
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x0400173D RID: 5949
		protected string explanationKey;

		// Token: 0x0400173E RID: 5950
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002B66 RID: 11110
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x06002B67 RID: 11111 RVA: 0x0016EDFC File Offset: 0x0016D1FC
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

		// Token: 0x06002B68 RID: 11112 RVA: 0x0016EE28 File Offset: 0x0016D228
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns());
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x0016EE48 File Offset: 0x0016D248
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
