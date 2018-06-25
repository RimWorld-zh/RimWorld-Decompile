using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B6 RID: 950
	public class SymbolResolver_PawnGroup : SymbolResolver
	{
		// Token: 0x04000A21 RID: 2593
		private const float DefaultPoints = 250f;

		// Token: 0x06001079 RID: 4217 RVA: 0x0008B5A8 File Offset: 0x000899A8
		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
			}
			else
			{
				result = (from x in rp.rect.Cells
				where x.Standable(BaseGen.globalSettings.map)
				select x).Any<IntVec3>();
			}
			return result;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0008B610 File Offset: 0x00089A10
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
			pawnGroupMakerParms.groupKind = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Combat);
			foreach (Pawn singlePawnToSpawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true))
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = singlePawnToSpawn;
				BaseGen.symbolStack.Push("pawn", resolveParams);
			}
		}
	}
}
