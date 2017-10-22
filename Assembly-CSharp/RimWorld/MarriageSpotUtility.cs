using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class MarriageSpotUtility
	{
		public static bool IsValidMarriageSpot(IntVec3 cell, Map map, StringBuilder outFailReason = null)
		{
			bool result;
			if (!cell.Standable(map))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotNotStandable".Translate());
				}
				result = false;
			}
			else
			{
				result = ((byte)((cell.Roofed(map) || JoyUtility.EnjoyableOutsideNow(map, outFailReason)) ? 1 : 0) != 0);
			}
			return result;
		}

		public static bool IsValidMarriageSpotFor(IntVec3 cell, Pawn firstFiance, Pawn secondFiance, StringBuilder outFailReason = null)
		{
			bool result;
			if (!firstFiance.Spawned || !secondFiance.Spawned)
			{
				Log.Warning("Can't check if a marriage spot is valid because one of the fiances isn't spawned.");
				result = false;
			}
			else if (firstFiance.Map != secondFiance.Map)
			{
				result = false;
			}
			else if (!MarriageSpotUtility.IsValidMarriageSpot(cell, firstFiance.Map, outFailReason))
			{
				result = false;
			}
			else
			{
				if (!cell.Roofed(firstFiance.Map))
				{
					if (!JoyUtility.EnjoyableOutsideNow(firstFiance, outFailReason))
					{
						result = false;
						goto IL_02b5;
					}
					if (!JoyUtility.EnjoyableOutsideNow(secondFiance, outFailReason))
					{
						result = false;
						goto IL_02b5;
					}
				}
				if (cell.GetDangerFor(firstFiance, firstFiance.Map) != Danger.None)
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotDangerous".Translate(firstFiance.LabelShort));
					}
					result = false;
				}
				else if (cell.GetDangerFor(secondFiance, secondFiance.Map) != Danger.None)
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotDangerous".Translate(secondFiance.LabelShort));
					}
					result = false;
				}
				else if (cell.IsForbidden(firstFiance))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotForbidden".Translate(firstFiance.LabelShort));
					}
					result = false;
				}
				else if (cell.IsForbidden(secondFiance))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotForbidden".Translate(secondFiance.LabelShort));
					}
					result = false;
				}
				else if (!firstFiance.CanReserve(cell, 1, -1, null, false) || !secondFiance.CanReserve(cell, 1, -1, null, false))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotReserved".Translate());
					}
					result = false;
				}
				else if (!firstFiance.CanReach(cell, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotUnreachable".Translate(firstFiance.LabelShort));
					}
					result = false;
				}
				else if (!secondFiance.CanReach(cell, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotUnreachable".Translate(secondFiance.LabelShort));
					}
					result = false;
				}
				else
				{
					if (!firstFiance.IsPrisoner && !secondFiance.IsPrisoner)
					{
						Room room = cell.GetRoom(firstFiance.Map, RegionType.Set_Passable);
						if (room != null && room.isPrisonCell)
						{
							if (outFailReason != null)
							{
								outFailReason.Append("MarriageSpotInPrisonCell".Translate());
							}
							result = false;
							goto IL_02b5;
						}
					}
					result = true;
				}
			}
			goto IL_02b5;
			IL_02b5:
			return result;
		}
	}
}
