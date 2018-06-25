using System;

namespace RimWorld
{
	// Token: 0x02000384 RID: 900
	public static class StoryDangerUtility
	{
		// Token: 0x06000F93 RID: 3987 RVA: 0x00083A08 File Offset: 0x00081E08
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
