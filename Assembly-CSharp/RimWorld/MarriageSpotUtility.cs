using System;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006A8 RID: 1704
	public static class MarriageSpotUtility
	{
		// Token: 0x0600248B RID: 9355 RVA: 0x00138F54 File Offset: 0x00137354
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
				if (!cell.Roofed(map))
				{
					if (!JoyUtility.EnjoyableOutsideNow(map, outFailReason))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x00138FB8 File Offset: 0x001373B8
		public static bool IsValidMarriageSpotFor(IntVec3 cell, Pawn firstFiance, Pawn secondFiance, StringBuilder outFailReason = null)
		{
			bool result;
			if (!firstFiance.Spawned || !secondFiance.Spawned)
			{
				Log.Warning("Can't check if a marriage spot is valid because one of the fiances isn't spawned.", false);
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
						return false;
					}
					if (!JoyUtility.EnjoyableOutsideNow(secondFiance, outFailReason))
					{
						return false;
					}
				}
				if (cell.GetDangerFor(firstFiance, firstFiance.Map) != Danger.None)
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotDangerous".Translate(new object[]
						{
							firstFiance.LabelShort
						}));
					}
					result = false;
				}
				else if (cell.GetDangerFor(secondFiance, secondFiance.Map) != Danger.None)
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotDangerous".Translate(new object[]
						{
							secondFiance.LabelShort
						}));
					}
					result = false;
				}
				else if (cell.IsForbidden(firstFiance))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotForbidden".Translate(new object[]
						{
							firstFiance.LabelShort
						}));
					}
					result = false;
				}
				else if (cell.IsForbidden(secondFiance))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotForbidden".Translate(new object[]
						{
							secondFiance.LabelShort
						}));
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
						outFailReason.Append("MarriageSpotUnreachable".Translate(new object[]
						{
							firstFiance.LabelShort
						}));
					}
					result = false;
				}
				else if (!secondFiance.CanReach(cell, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn))
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotUnreachable".Translate(new object[]
						{
							secondFiance.LabelShort
						}));
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
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}
	}
}
