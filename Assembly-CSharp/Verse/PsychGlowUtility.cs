using System;

namespace Verse
{
	public static class PsychGlowUtility
	{
		public static string GetLabel(this PsychGlow gl)
		{
			string result;
			switch (gl)
			{
			case PsychGlow.Dark:
			{
				result = "Dark".Translate();
				break;
			}
			case PsychGlow.Lit:
			{
				result = "Lit".Translate();
				break;
			}
			case PsychGlow.Overlit:
			{
				result = "LitBrightly".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}
	}
}
