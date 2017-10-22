namespace Verse.AI
{
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
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
				if (num <= 0.0)
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
					result = ((!flag) ? ThinkResult.NoJob : base.TryIssueJobPackage(pawn, jobParams));
				}
			}
			return result;
		}

		protected abstract float MtbHours(Pawn pawn);

		private int GetLastTryTick(Pawn pawn)
		{
			int num = default(int);
			return (!pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out num)) ? (-99999) : num;
		}

		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
