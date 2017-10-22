using System;

namespace Verse
{
	public static class AnimalNameDisplayModeExtension
	{
		public static string ToStringHuman(this AnimalNameDisplayMode mode)
		{
			string result;
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
			{
				result = "None".Translate();
				break;
			}
			case AnimalNameDisplayMode.TameNamed:
			{
				result = "AnimalNameDisplayMode_TameNamed".Translate();
				break;
			}
			case AnimalNameDisplayMode.TameAll:
			{
				result = "AnimalNameDisplayMode_TameAll".Translate();
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
			return result;
		}
	}
}
