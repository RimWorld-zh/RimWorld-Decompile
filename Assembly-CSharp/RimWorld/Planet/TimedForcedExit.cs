using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000627 RID: 1575
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x0011310C File Offset: 0x0011150C
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x00113130 File Offset: 0x00111530
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

		// Token: 0x06002000 RID: 8192 RVA: 0x00113166 File Offset: 0x00111566
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x00113181 File Offset: 0x00111581
		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0011318F File Offset: 0x0011158F
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0011319C File Offset: 0x0011159C
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

		// Token: 0x06002004 RID: 8196 RVA: 0x001131E8 File Offset: 0x001115E8
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

		// Token: 0x06002005 RID: 8197 RVA: 0x00113248 File Offset: 0x00111648
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

		// Token: 0x06002006 RID: 8198 RVA: 0x00113278 File Offset: 0x00111678
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

		// Token: 0x04001274 RID: 4724
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x04001275 RID: 4725
		public const int DefaultForceExitAndRemoveMapCountdownHours = 24;

		// Token: 0x04001276 RID: 4726
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
