using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_PawnGroup : SymbolResolver
	{
		private const float DefaultPoints = 250f;

		[CompilerGenerated]
		private static Func<IntVec3, bool> <>f__am$cache0;

		public SymbolResolver_PawnGroup()
		{
		}

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
			List<PawnKindDef> list = new List<PawnKindDef>();
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true))
			{
				list.Add(pawn.kindDef);
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = pawn;
				BaseGen.symbolStack.Push("pawn", resolveParams);
			}
		}

		[CompilerGenerated]
		private static bool <CanResolve>m__0(IntVec3 x)
		{
			return x.Standable(BaseGen.globalSettings.map);
		}
	}
}
