using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020002C0 RID: 704
	public class RecordWorker
	{
		// Token: 0x040006E4 RID: 1764
		public RecordDef def;

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000693D4 File Offset: 0x000677D4
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
