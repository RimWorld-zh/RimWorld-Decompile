using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000E23 RID: 3619
	[HasDebugOutput]
	internal class DebugOutputsWorldPawns
	{
		// Token: 0x060054CD RID: 21709 RVA: 0x002B7E64 File Offset: 0x002B6264
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

		// Token: 0x060054CE RID: 21710 RVA: 0x002B8020 File Offset: 0x002B6420
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void KidnappedPawns()
		{
			Find.FactionManager.LogKidnappedPawns();
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x002B802D File Offset: 0x002B642D
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnList()
		{
			Find.WorldPawns.LogWorldPawns();
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x002B803A File Offset: 0x002B643A
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnMothballInfo()
		{
			Find.WorldPawns.LogWorldPawnMothballPrevention();
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x002B8047 File Offset: 0x002B6447
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnGcBreakdown()
		{
			Find.WorldPawns.gc.LogGC();
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x002B8059 File Offset: 0x002B6459
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void WorldPawnDotgraph()
		{
			Find.WorldPawns.gc.LogDotgraph();
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x002B806B File Offset: 0x002B646B
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void RunWorldPawnGc()
		{
			Find.WorldPawns.gc.RunGC();
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x002B807D File Offset: 0x002B647D
		[DebugOutput]
		[Category("World pawns")]
		[ModeRestrictionPlay]
		public static void RunWorldPawnMothball()
		{
			Find.WorldPawns.DebugRunMothballProcessing();
		}
	}
}
