using System;

namespace Verse
{
	// Token: 0x02000C16 RID: 3094
	public static class PsychGlowUtility
	{
		// Token: 0x060043A4 RID: 17316 RVA: 0x0023C16C File Offset: 0x0023A56C
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
