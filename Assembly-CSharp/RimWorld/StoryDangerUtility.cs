using System;

namespace RimWorld
{
	// Token: 0x02000382 RID: 898
	public static class StoryDangerUtility
	{
		// Token: 0x06000F90 RID: 3984 RVA: 0x000836BC File Offset: 0x00081ABC
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
