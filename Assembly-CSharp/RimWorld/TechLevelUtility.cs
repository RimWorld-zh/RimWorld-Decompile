using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200037C RID: 892
	public static class TechLevelUtility
	{
		// Token: 0x06000F65 RID: 3941 RVA: 0x00082E28 File Offset: 0x00081228
		public static string ToStringHuman(this TechLevel tl)
		{
			string result;
			switch (tl)
			{
			case TechLevel.Undefined:
				result = "Undefined".Translate();
				break;
			case TechLevel.Animal:
				result = "TechLevel_Animal".Translate();
				break;
			case TechLevel.Neolithic:
				result = "TechLevel_Neolithic".Translate();
				break;
			case TechLevel.Medieval:
				result = "TechLevel_Medieval".Translate();
				break;
			case TechLevel.Industrial:
				result = "TechLevel_Industrial".Translate();
				break;
			case TechLevel.Spacer:
				result = "TechLevel_Spacer".Translate();
				break;
			case TechLevel.Ultra:
				result = "TechLevel_Ultra".Translate();
				break;
			case TechLevel.Archotech:
				result = "TechLevel_Archotech".Translate();
				break;
			default:
				throw new NotImplementedException();
			}
			return result;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00082EE8 File Offset: 0x000812E8
		public static bool CanSpawnWithEquipmentFrom(this TechLevel pawnLevel, TechLevel gearLevel)
		{
			bool result;
			if (gearLevel == TechLevel.Undefined)
			{
				result = false;
			}
			else
			{
				switch (pawnLevel)
				{
				case TechLevel.Undefined:
					return false;
				case TechLevel.Neolithic:
					return gearLevel <= TechLevel.Neolithic;
				case TechLevel.Medieval:
					return gearLevel <= TechLevel.Medieval;
				case TechLevel.Industrial:
					return gearLevel == TechLevel.Industrial;
				case TechLevel.Spacer:
					return gearLevel == TechLevel.Spacer || gearLevel == TechLevel.Industrial;
				case TechLevel.Ultra:
					return gearLevel == TechLevel.Ultra || gearLevel == TechLevel.Spacer;
				case TechLevel.Archotech:
					return gearLevel == TechLevel.Archotech;
				}
				Log.Error(string.Concat(new object[]
				{
					"Unknown tech levels ",
					pawnLevel,
					", ",
					gearLevel
				}), false);
				result = true;
			}
			return result;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00082FC8 File Offset: 0x000813C8
		public static bool IsNeolithicOrWorse(this TechLevel techLevel)
		{
			return techLevel != TechLevel.Undefined && techLevel <= TechLevel.Neolithic;
		}
	}
}
