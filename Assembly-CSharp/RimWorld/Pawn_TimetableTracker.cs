using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051A RID: 1306
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x04000DFD RID: 3581
		private Pawn pawn;

		// Token: 0x04000DFE RID: 3582
		public List<TimeAssignmentDef> times;

		// Token: 0x060017B6 RID: 6070 RVA: 0x000CF534 File Offset: 0x000CD934
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
		// (get) Token: 0x060017B7 RID: 6071 RVA: 0x000CF5A0 File Offset: 0x000CD9A0
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

		// Token: 0x060017B8 RID: 6072 RVA: 0x000CF5E6 File Offset: 0x000CD9E6
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, new object[0]);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x000CF600 File Offset: 0x000CDA00
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000CF621 File Offset: 0x000CDA21
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}
	}
}
