using RimWorld.Planet;
using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_SinglePawn : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec = default(IntVec3);
			return (byte)(base.CanResolve(rp) ? ((rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned) ? 1 : (SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out intVec) ? 1 : 0)) : 0) != 0;
		}

		public override void Resolve(ResolveParams rp)
		{
			if (rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned)
				return;
			Map map = BaseGen.globalSettings.map;
			IntVec3 loc = default(IntVec3);
			if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out loc))
			{
				if (rp.singlePawnToSpawn != null)
				{
					Find.WorldPawns.PassToWorld(rp.singlePawnToSpawn, PawnDiscardDecideMode.Discard);
				}
			}
			else
			{
				Pawn pawn;
				if (rp.singlePawnToSpawn == null)
				{
					PawnGenerationRequest request = default(PawnGenerationRequest);
					if (rp.singlePawnGenerationRequest.HasValue)
					{
						request = rp.singlePawnGenerationRequest.Value;
					}
					else
					{
						PawnKindDef pawnKindDef = rp.singlePawnKindDef ?? (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
						where x.defaultFactionType == null || !x.defaultFactionType.isPlayer
						select x).RandomElement();
						Faction faction = rp.faction;
						if (faction == null && pawnKindDef.RaceProps.Humanlike)
						{
							if (pawnKindDef.defaultFactionType != null)
							{
								faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
								if (faction == null)
									return;
							}
							else if (!(from x in Find.FactionManager.AllFactions
							where !x.IsPlayer
							select x).TryRandomElement<Faction>(out faction))
								return;
						}
						PawnKindDef kind = pawnKindDef;
						Faction faction2 = faction;
						int tile = map.Tile;
						request = new PawnGenerationRequest(kind, faction2, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, default(float?), default(float?), default(float?), default(Gender?), default(float?), (string)null);
					}
					pawn = PawnGenerator.GeneratePawn(request);
					if ((object)rp.postThingGenerate != null)
					{
						rp.postThingGenerate(pawn);
					}
				}
				else
				{
					pawn = rp.singlePawnToSpawn;
				}
				if (!pawn.Dead && rp.disableSinglePawn.HasValue && rp.disableSinglePawn.Value)
				{
					pawn.mindState.Active = false;
				}
				GenSpawn.Spawn(pawn, loc, map);
				if (rp.singlePawnLord != null)
				{
					rp.singlePawnLord.AddPawn(pawn);
				}
				if ((object)rp.postThingSpawn != null)
				{
					rp.postThingSpawn(pawn);
				}
			}
		}

		public static bool TryFindSpawnCell(ResolveParams rp, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rp.rect, (Predicate<IntVec3>)((IntVec3 x) => x.Standable(map) && ((object)rp.singlePawnSpawnCellExtraPredicate == null || rp.singlePawnSpawnCellExtraPredicate(x))), out cell);
		}
	}
}
