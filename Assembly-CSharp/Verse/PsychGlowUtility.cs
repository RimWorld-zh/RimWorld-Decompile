using System;

namespace Verse
{
	public static class PsychGlowUtility
	{
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
