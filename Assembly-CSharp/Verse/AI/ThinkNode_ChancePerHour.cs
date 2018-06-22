using System;

namespace Verse.AI
{
	// Token: 0x02000AA9 RID: 2729
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
		// Token: 0x06003D04 RID: 15620 RVA: 0x002049D8 File Offset: 0x00202DD8
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

		// Token: 0x06003D05 RID: 15621
		protected abstract float MtbHours(Pawn pawn);

		// Token: 0x06003D06 RID: 15622 RVA: 0x00204A94 File Offset: 0x00202E94
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

		// Token: 0x06003D07 RID: 15623 RVA: 0x00204AD2 File Offset: 0x00202ED2
		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
