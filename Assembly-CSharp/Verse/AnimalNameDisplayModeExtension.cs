using System;

namespace Verse
{
	// Token: 0x02000FA8 RID: 4008
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x060060F9 RID: 24825 RVA: 0x003103EC File Offset: 0x0030E7EC
		public static string ToStringHuman(this AnimalNameDisplayMode mode)
		{
			string result;
			if (mode != AnimalNameDisplayMode.None)
			{
				if (mode != AnimalNameDisplayMode.TameNamed)
				{
					if (mode != AnimalNameDisplayMode.TameAll)
					{
						throw new NotImplementedException();
					}
					result = "AnimalNameDisplayMode_TameAll".Translate();
				}
				else
				{
					result = "AnimalNameDisplayMode_TameNamed".Translate();
				}
			}
			else
			{
				result = "None".Translate();
			}
			return result;
		}
	}
}
