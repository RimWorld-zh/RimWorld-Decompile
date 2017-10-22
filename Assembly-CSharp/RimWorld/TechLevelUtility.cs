using System;
using Verse;

namespace RimWorld
{
	public static class TechLevelUtility
	{
		public static string ToStringHuman(this TechLevel tl)
		{
			string result;
			switch (tl)
			{
			case TechLevel.Undefined:
			{
				result = "Undefined".Translate();
				break;
			}
			case TechLevel.Animal:
			{
				result = "TechLevel_Animal".Translate();
				break;
			}
			case TechLevel.Neolithic:
			{
				result = "TechLevel_Neolithic".Translate();
				break;
			}
			case TechLevel.Medieval:
			{
				result = "TechLevel_Medieval".Translate();
				break;
			}
			case TechLevel.Industrial:
			{
				result = "TechLevel_Industrial".Translate();
				break;
			}
			case TechLevel.Spacer:
			{
				result = "TechLevel_Spacer".Translate();
				break;
			}
			case TechLevel.Ultra:
			{
				result = "TechLevel_Ultra".Translate();
				break;
			}
			case TechLevel.Transcendent:
			{
				result = "TechLevel_Transcendent".Translate();
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
			return result;
		}

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
				{
					result = false;
					break;
				}
				case TechLevel.Neolithic:
				{
					result = ((int)gearLevel <= 2);
					break;
				}
				case TechLevel.Medieval:
				{
					result = ((int)gearLevel <= 3);
					break;
				}
				case TechLevel.Industrial:
				{
					result = (gearLevel == TechLevel.Industrial);
					break;
				}
				case TechLevel.Spacer:
				{
					result = (gearLevel == TechLevel.Spacer || gearLevel == TechLevel.Industrial);
					break;
				}
				case TechLevel.Ultra:
				{
					result = (gearLevel == TechLevel.Ultra || gearLevel == TechLevel.Spacer);
					break;
				}
				case TechLevel.Transcendent:
				{
					result = (gearLevel == TechLevel.Transcendent);
					break;
				}
				default:
				{
					Log.Error("Unknown tech levels " + pawnLevel + ", " + gearLevel);
					result = true;
					break;
				}
				}
			}
			return result;
		}

		public static bool IsNeolithicOrWorse(this TechLevel techLevel)
		{
			return techLevel != 0 && (int)techLevel <= 2;
		}
	}
}
