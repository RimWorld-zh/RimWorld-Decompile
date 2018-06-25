using System;

namespace Verse.AI
{
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
		protected ThinkNode_ChancePerHour()
		{
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			if (Find.TickManager.TicksGame < this.GetLastTryTick(pawn) + 2500)
			{
				result = ThinkResult.NoJob;
			}
			else
			{
				this.SetLastTryTick(pawn, Find.TickManager.TicksGame);
				float num = this.MtbHours(pawn);
				if (num <= 0f)
				{
					result = ThinkResult.NoJob;
				}
				else
				{
					Rand.PushState();
					int salt = Gen.HashCombineInt(base.UniqueSaveKey, 26504059);
					Rand.Seed = pawn.RandSeedForHour(salt);
					bool flag = Rand.MTBEventOccurs(num, 2500f, 2500f);
					Rand.PopState();
					if (flag)
					{
						result = base.TryIssueJobPackage(pawn, jobParams);
					}
					else
					{
						result = ThinkResult.NoJob;
					}
				}
			}
			return result;
		}

		protected abstract float MtbHours(Pawn pawn);

		private int GetLastTryTick(Pawn pawn)
		{
			int num;
			int result;
			if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out num))
			{
				result = num;
			}
			else
			{
				result = -99999;
			}
			return result;
		}

		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
