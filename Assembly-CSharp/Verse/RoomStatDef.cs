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

		public float roomlessScore = 0f;

		public List<RoomStatScoreStage> scoreStages = null;

		public RoomStatDef inputStat;

		public SimpleCurve curve = null;

		[Unsaved]
		private RoomStatWorker workerInt = null;

		public RoomStatDef()
		{
		}

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
