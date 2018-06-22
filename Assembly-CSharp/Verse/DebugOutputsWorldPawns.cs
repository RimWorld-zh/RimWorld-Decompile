using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E20 RID: 3616
	[HasDebugOutput]
	internal class DebugOutputsWorldPawns
	{
		// Token: 0x060054E9 RID: 21737 RVA: 0x002B9A1C File Offset: 0x002B7E1C
		[DebugOutput]
		[Category("World pawns")]
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

		// Token: 0x060054EA RID: 21738 RVA: 0x002B9BD8 File Offset: 0x002B7FD8
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void KidnappedPawns()
		{
			Find.FactionManager.LogKidnappedPawns();
		}

		// Token: 0x060054EB RID: 21739 RVA: 0x002B9BE5 File Offset: 0x002B7FE5
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnList()
		{
			Find.WorldPawns.LogWorldPawns();
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x002B9BF2 File Offset: 0x002B7FF2
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnMothballInfo()
		{
			Find.WorldPawns.LogWorldPawnMothballPrevention();
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x002B9BFF File Offset: 0x002B7FFF
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnGcBreakdown()
		{
			Find.WorldPawns.gc.LogGC();
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x002B9C11 File Offset: 0x002B8011
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnDotgraph()
		{
			Find.WorldPawns.gc.LogDotgraph();
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x002B9C23 File Offset: 0x002B8023
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void RunWorldPawnGc()
		{
			Find.WorldPawns.gc.RunGC();
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x002B9C35 File Offset: 0x002B8035
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void RunWorldPawnMothball()
		{
			Find.WorldPawns.DebugRunMothballProcessing();
		}
	}
}
