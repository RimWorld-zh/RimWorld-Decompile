using System;

namespace Verse
{
	// Token: 0x02000C19 RID: 3097
	public static class PsychGlowUtility
	{
		// Token: 0x060043A7 RID: 17319 RVA: 0x0023C528 File Offset: 0x0023A928
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
