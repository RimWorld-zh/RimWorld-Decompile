using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020002C2 RID: 706
	public class RecordWorker
	{
		// Token: 0x040006E6 RID: 1766
		public RecordDef def;

		// Token: 0x06000BCA RID: 3018 RVA: 0x00069520 File Offset: 0x00067920
		public virtual bool ShouldMeasureTimeNow(Pawn pawn)
		{
			bool result;
			if (this.def.measuredTimeJobs == null)
			{
				result = false;
			}
			else
			{
				Job curJob = pawn.CurJob;
				if (curJob == null)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < this.def.measuredTimeJobs.Count; i++)
					{
						if (curJob.def == this.def.measuredTimeJobs[i])
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}
	}
}
