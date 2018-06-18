using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B6F RID: 2927
	public class RoomStatDef : Def
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x0021A7DC File Offset: 0x00218BDC
		public RoomStatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomStatWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0021A818 File Offset: 0x00218C18
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

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0021A858 File Offset: 0x00218C58
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

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0021A8CC File Offset: 0x00218CCC
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

		// Token: 0x04002AC4 RID: 10948
		public Type workerClass;

		// Token: 0x04002AC5 RID: 10949
		public float updatePriority = 0f;

		// Token: 0x04002AC6 RID: 10950
		public bool displayRounded = false;

		// Token: 0x04002AC7 RID: 10951
		public bool isHidden = false;

		// Token: 0x04002AC8 RID: 10952
		public float defaultScore = 0f;

		// Token: 0x04002AC9 RID: 10953
		public List<RoomStatScoreStage> scoreStages = null;

		// Token: 0x04002ACA RID: 10954
		[Unsaved]
		private RoomStatWorker workerInt = null;
	}
}
