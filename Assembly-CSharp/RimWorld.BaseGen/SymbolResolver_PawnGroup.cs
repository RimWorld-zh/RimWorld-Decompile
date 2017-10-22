using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_PawnGroup : SymbolResolver
	{
		private const float DefaultPoints = 250f;

		public override bool CanResolve(ResolveParams rp)
		{
			return (byte)(base.CanResolve(rp) ? ((from x in rp.rect.Cells
			where x.Standable(BaseGen.globalSettings.map)
			select x).Any() ? 1 : 0) : 0) != 0;
		}

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			PawnGroupMakerParms pawnGroupMakerParms = rp.pawnGroupMakerParams;
			if (pawnGroupMakerParms == null)
			{
				pawnGroupMakerParms = new PawnGroupMakerParms();
				pawnGroupMakerParms.tile = map.Tile;
				pawnGroupMakerParms.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
				pawnGroupMakerParms.points = 250f;
			}
			PawnGroupKindDef groupKind = rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Normal;
			foreach (Pawn item in PawnGroupMakerUtility.GeneratePawns(groupKind, pawnGroupMakerParms, true))
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = item;
				BaseGen.symbolStack.Push("pawn", resolveParams);
			}
		}
	}
}
