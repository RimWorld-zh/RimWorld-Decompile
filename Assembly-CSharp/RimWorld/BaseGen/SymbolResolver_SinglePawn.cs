using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B9 RID: 953
	public class SymbolResolver_SinglePawn : SymbolResolver
	{
		// Token: 0x06001081 RID: 4225 RVA: 0x0008B854 File Offset: 0x00089C54
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && ((rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned) || SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out intVec));
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0008B8B8 File Offset: 0x00089CB8
		public override void Resolve(ResolveParams rp)
		{
			if (rp.singlePawnToSpawn == null || !rp.singlePawnToSpawn.Spawned)
			{
				Map map = BaseGen.globalSettings.map;
				IntVec3 loc;
				if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out loc))
				{
					if (rp.singlePawnToSpawn != null)
					{
						Find.WorldPawns.PassToWorld(rp.singlePawnToSpawn, PawnDiscardDecideMode.Decide);
					}
				}
				else
				{
					Pawn pawn;
					if (rp.singlePawnToSpawn == null)
					{
						PawnGenerationRequest value;
						if (rp.singlePawnGenerationRequest != null)
						{
							value = rp.singlePawnGenerationRequest.Value;
						}
						else
						{
							PawnKindDef pawnKindDef;
							if ((pawnKindDef = rp.singlePawnKindDef) == null)
							{
								pawnKindDef = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
								where x.defaultFactionType == null || !x.defaultFactionType.isPlayer
								select x).RandomElement<PawnKindDef>();
							}
							PawnKindDef pawnKindDef2 = pawnKindDef;
							Faction faction = rp.faction;
							if (faction == null && pawnKindDef2.RaceProps.Humanlike)
							{
								if (pawnKindDef2.defaultFactionType != null)
								{
									faction = FactionUtility.DefaultFactionFrom(pawnKindDef2.defaultFactionType);
									if (faction == null)
									{
										return;
									}
								}
								else if (!(from x in Find.FactionManager.AllFactions
								where !x.IsPlayer
								select x).TryRandomElement(out faction))
								{
									return;
								}
							}
							PawnKindDef kind = pawnKindDef2;
							Faction faction2 = faction;
							int tile = map.Tile;
							value = new PawnGenerationRequest(kind, faction2, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
						}
						pawn = PawnGenerator.GeneratePawn(value);
						if (rp.postThingGenerate != null)
						{
							rp.postThingGenerate(pawn);
						}
					}
					else
					{
						pawn = rp.singlePawnToSpawn;
					}
					if (!pawn.Dead && rp.disableSinglePawn != null && rp.disableSinglePawn.Value)
					{
						pawn.mindState.Active = false;
					}
					GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
					if (rp.singlePawnLord != null)
					{
						rp.singlePawnLord.AddPawn(pawn);
					}
					if (rp.postThingSpawn != null)
					{
						rp.postThingSpawn(pawn);
					}
				}
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0008BB2C File Offset: 0x00089F2C
		public static bool TryFindSpawnCell(ResolveParams rp, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rp.rect, (IntVec3 x) => x.Standable(map) && (rp.singlePawnSpawnCellExtraPredicate == null || rp.singlePawnSpawnCellExtraPredicate(x)), out cell);
		}
	}
}
