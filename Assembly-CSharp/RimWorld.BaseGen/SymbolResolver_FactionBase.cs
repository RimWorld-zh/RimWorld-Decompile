using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FactionBase : SymbolResolver
	{
		private static readonly FloatRange NeolithicPawnsPoints = new FloatRange(880f, 1250f);

		private static readonly FloatRange NonNeolithicPawnsPoints = new FloatRange(1150f, 1600f);

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			int num = 0;
			int? edgeDefenseWidth = rp.edgeDefenseWidth;
			if (edgeDefenseWidth.HasValue)
			{
				num = rp.edgeDefenseWidth.Value;
			}
			else if (rp.rect.Width >= 20 && rp.rect.Height >= 20 && ((int)faction.def.techLevel >= 4 || Rand.Bool))
			{
				num = ((!Rand.Bool) ? 4 : 2);
			}
			float num2 = (float)((float)rp.rect.Area / 144.0 * 0.17000000178813934);
			BaseGen.globalSettings.minEmptyNodes = ((!(num2 < 1.0)) ? GenMath.RoundRandom(num2) : 0);
			Lord singlePawnLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell), map, null);
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			ResolveParams resolveParams = rp;
			resolveParams.rect = rp.rect;
			resolveParams.faction = faction;
			resolveParams.singlePawnLord = singlePawnLord;
			resolveParams.pawnGroupKindDef = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.FactionBase);
			object obj = rp.singlePawnSpawnCellExtraPredicate;
			obj = (resolveParams.singlePawnSpawnCellExtraPredicate = (Predicate<IntVec3>)((IntVec3 x) => map.reachability.CanReachMapEdge(x, traverseParms)));
			if (resolveParams.pawnGroupMakerParams == null)
			{
				float num3 = (!faction.def.techLevel.IsNeolithicOrWorse()) ? SymbolResolver_FactionBase.NonNeolithicPawnsPoints.RandomInRange : SymbolResolver_FactionBase.NeolithicPawnsPoints.RandomInRange;
				float? factionBasePawnGroupPointsFactor = rp.factionBasePawnGroupPointsFactor;
				if (factionBasePawnGroupPointsFactor.HasValue)
				{
					num3 *= rp.factionBasePawnGroupPointsFactor.Value;
				}
				resolveParams.pawnGroupMakerParams = new PawnGroupMakerParms();
				resolveParams.pawnGroupMakerParams.tile = map.Tile;
				resolveParams.pawnGroupMakerParams.faction = faction;
				resolveParams.pawnGroupMakerParams.points = num3;
				resolveParams.pawnGroupMakerParams.inhabitants = true;
			}
			BaseGen.symbolStack.Push("pawnGroup", resolveParams);
			BaseGen.symbolStack.Push("outdoorLighting", rp);
			if ((int)faction.def.techLevel >= 4)
			{
				int num4 = Rand.Chance(0.75f) ? GenMath.RoundRandom((float)((float)rp.rect.Area / 400.0)) : 0;
				for (int num5 = 0; num5 < num4; num5++)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.faction = faction;
					BaseGen.symbolStack.Push("firefoamPopper", resolveParams2);
				}
			}
			if (num > 0)
			{
				ResolveParams resolveParams3 = rp;
				resolveParams3.faction = faction;
				resolveParams3.edgeDefenseWidth = new int?(num);
				BaseGen.symbolStack.Push("edgeDefense", resolveParams3);
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = rp.rect.ContractedBy(num);
			resolveParams4.faction = faction;
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", resolveParams4);
			ResolveParams resolveParams5 = rp;
			resolveParams5.rect = rp.rect.ContractedBy(num);
			resolveParams5.faction = faction;
			BaseGen.symbolStack.Push("basePart_outdoors", resolveParams5);
		}
	}
}
