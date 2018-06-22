using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000623 RID: 1571
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x00113104 File Offset: 0x00111504
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x00113128 File Offset: 0x00111528
		public string ForceExitAndRemoveMapCountdownTimeLeftString
		{
			get
			{
				string result;
				if (!this.ForceExitAndRemoveMapCountdownActive)
				{
					result = "";
				}
				else
				{
					result = TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(this.ticksLeftToForceExitAndRemoveMap);
				}
				return result;
			}
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0011315E File Offset: 0x0011155E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x00113179 File Offset: 0x00111579
		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x00113187 File Offset: 0x00111587
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x00113194 File Offset: 0x00111594
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				result = "ForceExitAndRemoveMapCountdown".Translate(new object[]
				{
					this.ForceExitAndRemoveMapCountdownTimeLeftString
				}) + ".";
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x001131E0 File Offset: 0x001115E0
		public override void CompTick()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (this.ForceExitAndRemoveMapCountdownActive)
			{
				if (mapParent.HasMap)
				{
					this.ticksLeftToForceExitAndRemoveMap--;
					if (this.ticksLeftToForceExitAndRemoveMap <= 0)
					{
						TimedForcedExit.ForceReform(mapParent);
					}
				}
				else
				{
					this.ticksLeftToForceExitAndRemoveMap = -1;
				}
			}
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x00113240 File Offset: 0x00111640
		public static string GetForceExitAndRemoveMapCountdownTimeLeftString(int ticksLeft)
		{
			string result;
			if (ticksLeft < 0)
			{
				result = "";
			}
			else
			{
				result = ticksLeft.ToStringTicksToPeriod();
			}
			return result;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x00113270 File Offset: 0x00111670
		public static void ForceReform(MapParent mapParent)
		{
			if (Dialog_FormCaravan.AllSendablePawns(mapParent.Map, true).Any((Pawn x) => x.IsColonist))
			{
				Messages.Message("MessageYouHaveToReformCaravanNow".Translate(), new GlobalTargetInfo(mapParent.Tile), MessageTypeDefOf.NeutralEvent, true);
				Current.Game.CurrentMap = mapParent.Map;
				Dialog_FormCaravan window = new Dialog_FormCaravan(mapParent.Map, true, delegate()
				{
					if (mapParent.HasMap)
					{
						Find.WorldObjects.Remove(mapParent);
					}
				}, true);
				Find.WindowStack.Add(window);
			}
			else
			{
				TimedForcedExit.tmpPawns.Clear();
				TimedForcedExit.tmpPawns.AddRange(from x in mapParent.Map.mapPawns.AllPawns
				where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
				select x);
				if (TimedForcedExit.tmpPawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
				{
					CaravanExitMapUtility.ExitMapAndCreateCaravan(TimedForcedExit.tmpPawns, Faction.OfPlayer, mapParent.Tile, mapParent.Tile, -1, true);
				}
				TimedForcedExit.tmpPawns.Clear();
				Find.WorldObjects.Remove(mapParent);
			}
		}

		// Token: 0x0400126E RID: 4718
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x0400126F RID: 4719
		public const int DefaultForceExitAndRemoveMapCountdownHours = 24;

		// Token: 0x04001270 RID: 4720
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
