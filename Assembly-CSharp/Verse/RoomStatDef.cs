using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B6B RID: 2923
	public class RoomStatDef : Def
	{
		// Token: 0x04002AC5 RID: 10949
		public Type workerClass;

		// Token: 0x04002AC6 RID: 10950
		public float updatePriority = 0f;

		// Token: 0x04002AC7 RID: 10951
		public bool displayRounded = false;

		// Token: 0x04002AC8 RID: 10952
		public bool isHidden = false;

		// Token: 0x04002AC9 RID: 10953
		public float roomlessScore = 0f;

		// Token: 0x04002ACA RID: 10954
		public List<RoomStatScoreStage> scoreStages = null;

		// Token: 0x04002ACB RID: 10955
		public RoomStatDef inputStat;

		// Token: 0x04002ACC RID: 10956
		public SimpleCurve curve = null;

		// Token: 0x04002ACD RID: 10957
		[Unsaved]
		private RoomStatWorker workerInt = null;

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x0021AE54 File Offset: 0x00219254
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

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0021AEA0 File Offset: 0x002192A0
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

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0021AEE0 File Offset: 0x002192E0
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

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0021AF54 File Offset: 0x00219354
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
