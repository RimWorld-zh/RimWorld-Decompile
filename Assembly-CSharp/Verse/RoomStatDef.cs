using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class RoomStatDef : Def
	{
		public Type workerClass;

		public float updatePriority = 0f;

		public bool displayRounded = false;

		public bool isHidden = false;

		public float defaultScore = 0f;

		public List<RoomStatScoreStage> scoreStages = null;

		[Unsaved]
		private RoomStatWorker workerInt = null;

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

		public RoomStatScoreStage GetScoreStage(float score)
		{
			return (!this.scoreStages.NullOrEmpty()) ? this.scoreStages[this.GetScoreStageIndex(score)] : null;
		}

		public int GetScoreStageIndex(float score)
		{
			if (this.scoreStages.NullOrEmpty())
			{
				throw new InvalidOperationException("No score stages available.");
			}
			int result = 0;
			int num = 0;
			while (num < this.scoreStages.Count && score >= this.scoreStages[num].minScore)
			{
				result = num;
				num++;
			}
			return result;
		}

		public string ScoreToString(float score)
		{
			return (!this.displayRounded) ? score.ToString("F2") : Mathf.RoundToInt(score).ToString();
		}
	}
}
