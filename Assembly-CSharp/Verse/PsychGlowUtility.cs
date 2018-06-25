using System;

namespace Verse
{
	// Token: 0x02000C18 RID: 3096
	public static class PsychGlowUtility
	{
		// Token: 0x060043A7 RID: 17319 RVA: 0x0023C248 File Offset: 0x0023A648
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
