using System;

namespace Verse
{
	// Token: 0x02000C1A RID: 3098
	public static class PsychGlowUtility
	{
		// Token: 0x0600439D RID: 17309 RVA: 0x0023ADCC File Offset: 0x002391CC
		public static string GetLabel(this PsychGlow gl)
		{
			string result;
			if (gl != PsychGlow.Dark)
			{
				if (gl != PsychGlow.Lit)
				{
					if (gl != PsychGlow.Overlit)
					{
						throw new ArgumentException();
					}
					result = "LitBrightly".Translate();
				}
				else
				{
					result = "Lit".Translate();
				}
			}
			else
			{
				result = "Dark".Translate();
			}
			return result;
		}
	}
}
