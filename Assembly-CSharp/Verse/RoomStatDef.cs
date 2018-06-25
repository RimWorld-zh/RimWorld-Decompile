using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B6E RID: 2926
	public class RoomStatDef : Def
	{
		// Token: 0x04002ACC RID: 10956
		public Type workerClass;

		// Token: 0x04002ACD RID: 10957
		public float updatePriority = 0f;

		// Token: 0x04002ACE RID: 10958
		public bool displayRounded = false;

		// Token: 0x04002ACF RID: 10959
		public bool isHidden = false;

		// Token: 0x04002AD0 RID: 10960
		public float roomlessScore = 0f;

		// Token: 0x04002AD1 RID: 10961
		public List<RoomStatScoreStage> scoreStages = null;

		// Token: 0x04002AD2 RID: 10962
		public RoomStatDef inputStat;

		// Token: 0x04002AD3 RID: 10963
		public SimpleCurve curve = null;

		// Token: 0x04002AD4 RID: 10964
		[Unsaved]
		private RoomStatWorker workerInt = null;

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x0021B210 File Offset: 0x00219610
		public RoomStatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomStatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0021B25C File Offset: 0x0021965C
		public RoomStatScoreStage GetScoreStage(float score)
		{
			RoomStatScoreStage result;
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				result = null;
			}
			else
			{
				result = this.scoreStages[this.GetScoreStageIndex(score)];
			}
			return result;
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0021B29C File Offset: 0x0021969C
		public int GetScoreStageIndex(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				throw new InvalidOperationException("No score stages available.");
			}
			int result = 0;
			for (int i = 0; i < this.scoreStages.Count; i++)
			{
				if (score < this.scoreStages[i].minScore)
				{
					break;
				}
				result = i;
			}
			return result;
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0021B310 File Offset: 0x00219710
		public string ScoreToString(float score)
		{
			string result;
			if (this.displayRounded)
			{
				result = Mathf.RoundToInt(score).ToString();
			}
			else
			{
				result = score.ToString("F2");
			}
			return result;
		}
	}
}
