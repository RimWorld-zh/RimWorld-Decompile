using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051C RID: 1308
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x060017BC RID: 6076 RVA: 0x000CF184 File Offset: 0x000CD584
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
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x000CF1F0 File Offset: 0x000CD5F0
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

		// Token: 0x060017BE RID: 6078 RVA: 0x000CF236 File Offset: 0x000CD636
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, new object[0]);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000CF250 File Offset: 0x000CD650
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000CF271 File Offset: 0x000CD671
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
