using System;

namespace RimWorld
{
	public static class StoryDangerUtility
	{
		public static float Scale(this StoryDanger d)
		{
			float result;
			if (d != StoryDanger.None)
			{
				if (d != StoryDanger.Low)
				{
					if (d != StoryDanger.High)
					{
						result = 0f;
					}
					else
					{
						result = 2f;
					}
				}
				else
				{
					result = 1f;
				}
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
