using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	[HasDebugOutput]
	internal class DebugOutputsWorldPawns
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public DebugOutputsWorldPawns()
		{
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void ColonistRelativeChance()
		{
			HashSet<Pawn> hashSet = new HashSet<Pawn>(Find.WorldPawns.AllPawnsAliveOrDead);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < 500; i++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				list.Add(pawn);
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.KeepForever);
				}
			}
			int num = list.Count((Pawn x) => PawnRelationUtility.GetMostImportantColonyRelative(x) != null);
			Log.Message(string.Concat(new object[]
			{
				"Colony relatives: ",
				((float)num / 500f).ToStringPercent(),
				" (",
				num,
				" of ",
				500,
				")"
			}), false);
			foreach (Pawn pawn2 in Find.WorldPawns.AllPawnsAliveOrDead.ToList<Pawn>())
			{
				if (!hashSet.Contains(pawn2))
				{
					Find.WorldPawns.RemovePawn(pawn2);
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
				}
			}
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void KidnappedPawns()
		{
			Find.FactionManager.LogKidnappedPawns();
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WorldPawnList()
		{
			Find.WorldPawns.LogWorldPawns();
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WorldPawnMothballInfo()
		{
			Find.WorldPawns.LogWorldPawnMothballPrevention();
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WorldPawnGcBreakdown()
		{
			Find.WorldPawns.gc.LogGC();
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WorldPawnDotgraph()
		{
			Find.WorldPawns.gc.LogDotgraph();
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void RunWorldPawnGc()
		{
			Find.WorldPawns.gc.RunGC();
		}

		[Category("World pawns")]
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void RunWorldPawnMothball()
		{
			Find.WorldPawns.DebugRunMothballProcessing();
		}

		[CompilerGenerated]
		private static bool <ColonistRelativeChance>m__0(Pawn x)
		{
			return PawnRelationUtility.GetMostImportantColonyRelative(x) != null;
		}
	}
}
