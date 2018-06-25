using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class RecordWorker
	{
		public RecordDef def;

		public RecordWorker()
		{
		}

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
