namespace RimWorld
{
	public struct ThoughtState
	{
		private int stageIndex;

		private string reason;

		private const int InactiveIndex = -99999;

		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		public static ThoughtState Inactive
		{
			get
			{
				ThoughtState result = default(ThoughtState);
				result.stageIndex = -99999;
				return result;
			}
		}

		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			ThoughtState result = default(ThoughtState);
			result.stageIndex = stageIndex;
			return result;
		}

		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			ThoughtState result = default(ThoughtState);
			result.stageIndex = stageIndex;
			result.reason = reason;
			return result;
		}

		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		public static implicit operator ThoughtState(bool value)
		{
			if (value)
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}
	}
}
