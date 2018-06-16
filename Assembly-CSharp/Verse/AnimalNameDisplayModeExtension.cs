using System;

namespace Verse
{
	// Token: 0x02000FA9 RID: 4009
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x060060D2 RID: 24786 RVA: 0x0030E26C File Offset: 0x0030C66C
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
