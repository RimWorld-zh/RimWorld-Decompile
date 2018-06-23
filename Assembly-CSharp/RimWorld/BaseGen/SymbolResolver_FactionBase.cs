using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C4 RID: 964
	public class SymbolResolver_FactionBase : SymbolResolver
	{
		// Token: 0x04000A30 RID: 2608
		private static readonly FloatRange NeolithicPawnsPoints = new FloatRange(880f, 1250f);

		// Token: 0x04000A31 RID: 2609
		private static readonly FloatRange MedievalPawnsPoints = new FloatRange(1150f, 1600f);

		// Token: 0x060010A8 RID: 4264 RVA: 0x0008D9C8 File Offset: 0x0008BDC8
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_FactionBase.<Resolve>c__AnonStorey0 <Resolve>c__AnonStorey = new SymbolResolver_FactionBase.<Resolve>c__AnonStorey0();
			<Resolve>c__AnonStorey.map = BaseGen.globalSettings.map;
			Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			int num = 0;
			int? edgeDefenseWidth = rp.edgeDefenseWidth;
			if (edgeDefenseWidth != null)
			{
				num = rp.edgeDefenseWidth.Value;
			}
			else if (rp.rect.Width >= 20 && rp.rect.Height >= 20 && (faction.def.techLevel >= TechLevel.Industrial || Rand.Bool))
			{
				num = ((!Rand.Bool) ? 4 : 2);
			}
			float num2 = (float)rp.rect.Area / 144f * 0.17f;
			BaseGen.globalSettings.minEmptyNodes = ((num2 >= 1f) ? GenMath.RoundRandom(num2) : 0);
			Lord singlePawnLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell), <Resolve>c__AnonStorey.map, null);
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			ResolveParams resolveParams = rp;
			resolveParams.rect = rp.rect;
			resolveParams.faction = faction;
			resolveParams.singlePawnLord = singlePawnLord;
			resolveParams.pawnGroupKindDef = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.FactionBase);
			resolveParams.singlePawnSpawnCellExtraPredicate = (rp.singlePawnSpawnCellExtraPredicate ?? ((IntVec3 x) => <Resolve>c__AnonStorey.map.reachability.CanReachMapEdge(x, traverseParms)));
			if (resolveParams.pawnGroupMakerParams == null)
			{
				float num3 = (!faction.def.techLevel.IsNeolithicOrWorse()) ? SymbolResolver_FactionBase.MedievalPawnsPoints.RandomInRange : SymbolResolver_FactionBase.NeolithicPawnsPoints.RandomInRange;
				float? factionBasePawnGroupPointsFactor = rp.factionBasePawnGroupPointsFactor;
				if (factionBasePawnGroupPointsFactor != null)
				{
					num3 *= rp.factionBasePawnGroupPointsFactor.Value;
				}
				resolveParams.pawnGroupMakerParams = new PawnGroupMakerParms();
				resolveParams.pawnGroupMakerParams.tile = <Resolve>c__AnonStorey.map.Tile;
				resolveParams.pawnGroupMakerParams.faction = faction;
				resolveParams.pawnGroupMakerParams.points = num3;
				resolveParams.pawnGroupMakerParams.inhabitants = true;
			}
			BaseGen.symbolStack.Push("pawnGroup", resolveParams);
			BaseGen.symbolStack.Push("outdoorLighting", rp);
			if (faction.def.techLevel >= TechLevel.Industrial)
			{
				int num4 = (!Rand.Chance(0.75f)) ? 0 : GenMath.RoundRandom((float)rp.rect.Area / 400f);
				for (int i = 0; i < num4; i++)
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
