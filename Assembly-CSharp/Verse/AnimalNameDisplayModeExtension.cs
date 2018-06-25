using System;

namespace Verse
{
	// Token: 0x02000FAC RID: 4012
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x06006103 RID: 24835 RVA: 0x00310A6C File Offset: 0x0030EE6C
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
