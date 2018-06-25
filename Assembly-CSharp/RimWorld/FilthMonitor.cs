using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	internal static class FilthMonitor
	{
		private static int lastUpdate = 0;

		private static int filthAccumulated = 0;

		private static int filthDropped = 0;

		private static int filthAnimalGenerated = 0;

		private static int filthHumanGenerated = 0;

		private static int filthSpawned = 0;

		private const int SampleDuration = 2500;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache2;

		public static void FilthMonitorTick()
		{
			if (DebugViewSettings.logFilthSummary)
			{
				if (FilthMonitor.lastUpdate + 2500 <= Find.TickManager.TicksAbs)
				{
					int num = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer);
					int num2 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && pawn.RaceProps.Humanlike);
					int num3 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && !pawn.RaceProps.Humanlike);
					Log.Message(string.Format("Filth data, per day:\n  {0} filth spawned per pawn\n  {1} filth human-generated per human\n  {2} filth animal-generated per animal\n  {3} filth accumulated per pawn\n  {4} filth dropped per pawn", new object[]
					{
						(float)FilthMonitor.filthSpawned / (float)num / 2500f * 60000f,
						(float)FilthMonitor.filthHumanGenerated / (float)num2 / 2500f * 60000f,
						(float)FilthMonitor.filthAnimalGenerated / (float)num3 / 2500f * 60000f,
						(float)FilthMonitor.filthAccumulated / (float)num / 2500f * 60000f,
						(float)FilthMonitor.filthDropped / (float)num / 2500f * 60000f
					}), false);
					FilthMonitor.filthSpawned = 0;
					FilthMonitor.filthAnimalGenerated = 0;
					FilthMonitor.filthHumanGenerated = 0;
					FilthMonitor.filthAccumulated = 0;
					FilthMonitor.filthDropped = 0;
					FilthMonitor.lastUpdate = Find.TickManager.TicksAbs;
				}
			}
		}

		public static void Notify_FilthAccumulated()
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthAccumulated++;
			}
		}

		public static void Notify_FilthDropped()
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthDropped++;
			}
		}

		public static void Notify_FilthAnimalGenerated()
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthAnimalGenerated++;
			}
		}

		public static void Notify_FilthHumanGenerated()
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthHumanGenerated++;
			}
		}

		public static void Notify_FilthSpawned()
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthSpawned++;
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FilthMonitor()
		{
		}

		[CompilerGenerated]
		private static bool <FilthMonitorTick>m__0(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}

		[CompilerGenerated]
		private static bool <FilthMonitorTick>m__1(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && pawn.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static bool <FilthMonitorTick>m__2(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer && !pawn.RaceProps.Humanlike;
		}
	}
}
