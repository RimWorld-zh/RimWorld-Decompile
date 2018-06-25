using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000625 RID: 1573
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x04001272 RID: 4722
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x04001273 RID: 4723
		public const int DefaultForceExitAndRemoveMapCountdownHours = 24;

		// Token: 0x04001274 RID: 4724
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x001134BC File Offset: 0x001118BC
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x001134E0 File Offset: 0x001118E0
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

		// Token: 0x06001FFA RID: 8186 RVA: 0x00113516 File Offset: 0x00111916
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00113531 File Offset: 0x00111931
		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0011353F File Offset: 0x0011193F
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0011354C File Offset: 0x0011194C
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

		// Token: 0x06001FFE RID: 8190 RVA: 0x00113598 File Offset: 0x00111998
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

		// Token: 0x06001FFF RID: 8191 RVA: 0x001135F8 File Offset: 0x001119F8
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

		// Token: 0x06002000 RID: 8192 RVA: 0x00113628 File Offset: 0x00111A28
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
	}
}
