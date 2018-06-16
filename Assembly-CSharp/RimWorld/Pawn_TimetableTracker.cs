using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051C RID: 1308
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x060017BB RID: 6075 RVA: 0x000CF130 File Offset: 0x000CD530
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
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x000CF19C File Offset: 0x000CD59C
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

		// Token: 0x060017BD RID: 6077 RVA: 0x000CF1E2 File Offset: 0x000CD5E2
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, new object[0]);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x000CF1FC File Offset: 0x000CD5FC
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000CF21D File Offset: 0x000CD61D
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}

		// Token: 0x04000DFC RID: 3580
		private Pawn pawn;

		// Token: 0x04000DFD RID: 3581
		public List<TimeAssignmentDef> times;
	}
}
