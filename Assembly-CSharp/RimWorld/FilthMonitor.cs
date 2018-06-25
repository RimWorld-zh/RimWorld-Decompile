using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097B RID: 2427
	internal static class FilthMonitor
	{
		// Token: 0x04002336 RID: 9014
		private static int lastUpdate = 0;

		// Token: 0x04002337 RID: 9015
		private static int filthAccumulated = 0;

		// Token: 0x04002338 RID: 9016
		private static int filthDropped = 0;

		// Token: 0x04002339 RID: 9017
		private static int filthAnimalGenerated = 0;

		// Token: 0x0400233A RID: 9018
		private static int filthHumanGenerated = 0;

		// Token: 0x0400233B RID: 9019
		private static int filthSpawned = 0;

		// Token: 0x0400233C RID: 9020
		private const int SampleDuration = 2500;

		// Token: 0x0600368D RID: 13965 RVA: 0x001D1784 File Offset: 0x001CFB84
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

		// Token: 0x0600368E RID: 13966 RVA: 0x001D1909 File Offset: 0x001CFD09
		public static void Notify_FilthAccumulated(ThingDef filth)
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthAccumulated++;
			}
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x001D1927 File Offset: 0x001CFD27
		public static void Notify_FilthDropped(ThingDef filth)
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthDropped++;
			}
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x001D1945 File Offset: 0x001CFD45
		public static void Notify_FilthAnimalGenerated(ThingDef filth)
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthAnimalGenerated++;
			}
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x001D1963 File Offset: 0x001CFD63
		public static void Notify_FilthHumanGenerated(ThingDef filth)
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthHumanGenerated++;
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x001D1981 File Offset: 0x001CFD81
		public static void Notify_FilthSpawned(ThingDef filth)
		{
			if (DebugViewSettings.logFilthSummary)
			{
				FilthMonitor.filthSpawned++;
			}
		}
	}
}
