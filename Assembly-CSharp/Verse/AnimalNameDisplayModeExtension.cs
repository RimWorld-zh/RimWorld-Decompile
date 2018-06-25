using System;

namespace Verse
{
	// Token: 0x02000FAD RID: 4013
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x06006103 RID: 24835 RVA: 0x00310CB0 File Offset: 0x0030F0B0
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
