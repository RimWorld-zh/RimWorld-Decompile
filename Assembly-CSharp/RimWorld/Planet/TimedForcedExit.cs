using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000625 RID: 1573
	public class TimedForcedExit : WorldObjectComp
	{
		// Token: 0x0400126E RID: 4718
		private int ticksLeftToForceExitAndRemoveMap = -1;

		// Token: 0x0400126F RID: 4719
		public const int DefaultForceExitAndRemoveMapCountdownHours = 24;

		// Token: 0x04001270 RID: 4720
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x00113254 File Offset: 0x00111654
		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001FFA RID: 8186 RVA: 0x00113278 File Offset: 0x00111678
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

		// Token: 0x06001FFB RID: 8187 RVA: 0x001132AE File Offset: 0x001116AE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x001132C9 File Offset: 0x001116C9
		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x001132D7 File Offset: 0x001116D7
		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x001132E4 File Offset: 0x001116E4
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

		// Token: 0x06001FFF RID: 8191 RVA: 0x00113330 File Offset: 0x00111730
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

		// Token: 0x06002000 RID: 8192 RVA: 0x00113390 File Offset: 0x00111790
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

		// Token: 0x06002001 RID: 8193 RVA: 0x001133C0 File Offset: 0x001117C0
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
