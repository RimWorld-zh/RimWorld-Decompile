namespace RimWorld
{
	public static class StoryDangerUtility
	{
		public static float Scale(this StoryDanger d)
		{
			float result;
			switch (d)
			{
			case StoryDanger.None:
			{
				result = 0f;
				break;
			}
			case StoryDanger.Low:
			{
				result = 1f;
				break;
			}
			case StoryDanger.High:
			{
				result = 2f;
				break;
			}
			default:
			{
				result = 0f;
				break;
			}
			}
			return result;
		}
	}
}
