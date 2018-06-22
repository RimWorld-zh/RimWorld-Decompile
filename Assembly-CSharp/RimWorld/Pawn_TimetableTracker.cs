using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000518 RID: 1304
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x060017B3 RID: 6067 RVA: 0x000CF17C File Offset: 0x000CD57C
		public Pawn_TimetableTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.times = new List<TimeAssignmentDef>(24);
			for (int i = 0; i < 24; i++)
			{
				TimeAssignmentDef item;
				if (i <= 5 || i > 21)
				{
					item = TimeAssignmentDefOf.Sleep;
				}
				else
				{
					item = TimeAssignmentDefOf.Anything;
				}
				this.times.Add(item);
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x000CF1E8 File Offset: 0x000CD5E8
		public TimeAssignmentDef CurrentAssignment
		{
			get
			{
				TimeAssignmentDef result;
				if (!this.pawn.IsColonist)
				{
					result = TimeAssignmentDefOf.Anything;
				}
				else
				{
					result = this.times[GenLocalDate.HourOfDay(this.pawn)];
				}
				return result;
			}
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000CF22E File Offset: 0x000CD62E
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, new object[0]);
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x000CF248 File Offset: 0x000CD648
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x000CF269 File Offset: 0x000CD669
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}

		// Token: 0x04000DF9 RID: 3577
		private Pawn pawn;

		// Token: 0x04000DFA RID: 3578
		public List<TimeAssignmentDef> times;
	}
}
