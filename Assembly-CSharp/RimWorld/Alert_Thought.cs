using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AD RID: 1965
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002B69 RID: 11113
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x06002B6A RID: 11114 RVA: 0x0016EAD4 File Offset: 0x0016CED4
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

		// Token: 0x06002B6B RID: 11115 RVA: 0x0016EB00 File Offset: 0x0016CF00
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns());
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x0016EB20 File Offset: 0x0016CF20
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

		// Token: 0x0400173F RID: 5951
		protected string explanationKey;

		// Token: 0x04001740 RID: 5952
		private static List<Thought> tmpThoughts = new List<Thought>();
	}
}
