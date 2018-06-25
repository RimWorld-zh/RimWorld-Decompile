using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class TimedForcedExit : WorldObjectComp
	{
		private int ticksLeftToForceExitAndRemoveMap = -1;

		public const int DefaultForceExitAndRemoveMapCountdownHours = 24;

		private static List<Pawn> tmpPawns = new List<Pawn>();

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache2;

		public TimedForcedExit()
		{
		}

		public bool ForceExitAndRemoveMapCountdownActive
		{
			get
			{
				return this.ticksLeftToForceExitAndRemoveMap >= 0;
			}
		}

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

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToForceExitAndRemoveMap, "ticksLeftToForceExitAndRemoveMap", -1, false);
		}

		public void StartForceExitAndRemoveMapCountdown()
		{
			this.StartForceExitAndRemoveMapCountdown(60000);
		}

		public void StartForceExitAndRemoveMapCountdown(int duration)
		{
			this.ticksLeftToForceExitAndRemoveMap = duration;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static TimedForcedExit()
		{
		}

		[CompilerGenerated]
		private static bool <ForceReform>m__0(Pawn x)
		{
			return x.IsColonist;
		}

		[CompilerGenerated]
		private static bool <ForceReform>m__1(Pawn x)
		{
			return x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer;
		}

		[CompilerGenerated]
		private static bool <ForceReform>m__2(Pawn x)
		{
			return CaravanUtility.IsOwner(x, Faction.OfPlayer);
		}

		[CompilerGenerated]
		private sealed class <ForceReform>c__AnonStorey0
		{
			internal MapParent mapParent;

			public <ForceReform>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (this.mapParent.HasMap)
				{
					Find.WorldObjects.Remove(this.mapParent);
				}
			}
		}
	}
}
