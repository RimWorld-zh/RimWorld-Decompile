using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_BedroomJealous : ThoughtWorker
	{
		public ThoughtWorker_BedroomJealous()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.IsColonist)
			{
				result = false;
			}
			else
			{
				float num = 0f;
				Room ownedRoom = p.ownership.OwnedRoom;
				if (ownedRoom != null)
				{
					num = ownedRoom.GetStat(RoomStatDefOf.Impressiveness);
				}
				List<Pawn> list = p.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				Pawn pawn = null;
				float num2 = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].HostFaction == null && p.RaceProps.Humanlike && list[i].ownership != null)
					{
						Room ownedRoom2 = list[i].ownership.OwnedRoom;
						if (ownedRoom2 != null)
						{
							float stat = ownedRoom2.GetStat(RoomStatDefOf.Impressiveness);
							if (stat - num >= Mathf.Abs(num * 0.1f) && (pawn == null || stat > num2))
							{
								pawn = list[i];
								num2 = stat;
							}
						}
					}
				}
				if (pawn != null)
				{
					result = ThoughtState.ActiveWithReason(pawn.LabelShort);
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
