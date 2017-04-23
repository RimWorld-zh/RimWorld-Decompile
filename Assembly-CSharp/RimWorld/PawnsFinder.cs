using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public static class PawnsFinder
	{
		public static IEnumerable<Pawn> AllMapsAndWorld_AliveOrDead
		{
			get
			{
				PawnsFinder.<>c__IteratorCA <>c__IteratorCA = new PawnsFinder.<>c__IteratorCA();
				PawnsFinder.<>c__IteratorCA expr_07 = <>c__IteratorCA;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMapsAndWorld_Alive
		{
			get
			{
				PawnsFinder.<>c__IteratorCB <>c__IteratorCB = new PawnsFinder.<>c__IteratorCB();
				PawnsFinder.<>c__IteratorCB expr_07 = <>c__IteratorCB;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps
		{
			get
			{
				PawnsFinder.<>c__IteratorCC <>c__IteratorCC = new PawnsFinder.<>c__IteratorCC();
				PawnsFinder.<>c__IteratorCC expr_07 = <>c__IteratorCC;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps_Spawned
		{
			get
			{
				PawnsFinder.<>c__IteratorCD <>c__IteratorCD = new PawnsFinder.<>c__IteratorCD();
				PawnsFinder.<>c__IteratorCD expr_07 = <>c__IteratorCD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods
		{
			get
			{
				PawnsFinder.<>c__IteratorCE <>c__IteratorCE = new PawnsFinder.<>c__IteratorCE();
				PawnsFinder.<>c__IteratorCE expr_07 = <>c__IteratorCE;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Colonists
		{
			get
			{
				PawnsFinder.<>c__IteratorCF <>c__IteratorCF = new PawnsFinder.<>c__IteratorCF();
				PawnsFinder.<>c__IteratorCF expr_07 = <>c__IteratorCF;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_FreeColonists
		{
			get
			{
				PawnsFinder.<>c__IteratorD0 <>c__IteratorD = new PawnsFinder.<>c__IteratorD0();
				PawnsFinder.<>c__IteratorD0 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_PrisonersOfColony
		{
			get
			{
				PawnsFinder.<>c__IteratorD1 <>c__IteratorD = new PawnsFinder.<>c__IteratorD1();
				PawnsFinder.<>c__IteratorD1 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				PawnsFinder.<>c__IteratorD2 <>c__IteratorD = new PawnsFinder.<>c__IteratorD2();
				PawnsFinder.<>c__IteratorD2 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				PawnsFinder.<>c__IteratorD3 <>c__IteratorD = new PawnsFinder.<>c__IteratorD3();
				PawnsFinder.<>c__IteratorD3 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonists
		{
			get
			{
				PawnsFinder.<>c__IteratorD4 <>c__IteratorD = new PawnsFinder.<>c__IteratorD4();
				PawnsFinder.<>c__IteratorD4 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.<>c__IteratorD5 <>c__IteratorD = new PawnsFinder.<>c__IteratorD5();
				PawnsFinder.<>c__IteratorD5 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				PawnsFinder.<>c__IteratorD6 <>c__IteratorD = new PawnsFinder.<>c__IteratorD6();
				PawnsFinder.<>c__IteratorD6 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				PawnsFinder.<>c__IteratorD7 <>c__IteratorD = new PawnsFinder.<>c__IteratorD7();
				PawnsFinder.<>c__IteratorD7 expr_07 = <>c__IteratorD;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		[DebuggerHidden]
		public static IEnumerable<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			PawnsFinder.<AllMaps_SpawnedPawnsInFaction>c__IteratorD8 <AllMaps_SpawnedPawnsInFaction>c__IteratorD = new PawnsFinder.<AllMaps_SpawnedPawnsInFaction>c__IteratorD8();
			<AllMaps_SpawnedPawnsInFaction>c__IteratorD.faction = faction;
			<AllMaps_SpawnedPawnsInFaction>c__IteratorD.<$>faction = faction;
			PawnsFinder.<AllMaps_SpawnedPawnsInFaction>c__IteratorD8 expr_15 = <AllMaps_SpawnedPawnsInFaction>c__IteratorD;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
