using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020002C0 RID: 704
	public class RecordWorker
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x0006936C File Offset: 0x0006776C
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

		// Token: 0x040006E5 RID: 1765
		public RecordDef def;
	}
}
