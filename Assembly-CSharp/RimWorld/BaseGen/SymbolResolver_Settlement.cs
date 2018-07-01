using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Settlement : SymbolResolver
	{
		private static readonly FloatRange DefaultPawnsPoints = new FloatRange(1150f, 1600f);

		public SymbolResolver_Settlement()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_Settlement.<Resolve>c__AnonStorey0 <Resolve>c__AnonStorey = new SymbolResolver_Settlement.<Resolve>c__AnonStorey0();
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
			resolveParams.pawnGroupKindDef = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Settlement);
			resolveParams.singlePawnSpawnCellExtraPredicate = (rp.singlePawnSpawnCellExtraPredicate ?? ((IntVec3 x) => <Resolve>c__AnonStorey.map.reachability.CanReachMapEdge(x, traverseParms)));
			if (resolveParams.pawnGroupMakerParams == null)
			{
				resolveParams.pawnGroupMakerParams = new PawnGroupMakerParms();
				resolveParams.pawnGroupMakerParams.tile = <Resolve>c__AnonStorey.map.Tile;
				resolveParams.pawnGroupMakerParams.faction = faction;
				PawnGroupMakerParms pawnGroupMakerParams = resolveParams.pawnGroupMakerParams;
				float? settlementPawnGroupPoints = rp.settlementPawnGroupPoints;
				pawnGroupMakerParams.points = ((settlementPawnGroupPoints == null) ? SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange : settlementPawnGroupPoints.Value);
				resolveParams.pawnGroupMakerParams.inhabitants = true;
				resolveParams.pawnGroupMakerParams.seed = rp.settlementPawnGroupSeed;
			}
			BaseGen.symbolStack.Push("pawnGroup", resolveParams);
			BaseGen.symbolStack.Push("outdoorLighting", rp);
			if (faction.def.techLevel >= TechLevel.Industrial)
			{
				int num3 = (!Rand.Chance(0.75f)) ? 0 : GenMath.RoundRandom((float)rp.rect.Area / 400f);
				for (int i = 0; i < num3; i++)
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

		// Note: this type is marked as 'beforefieldinit'.
		static SymbolResolver_Settlement()
		{
		}

		[CompilerGenerated]
		private sealed class <Resolve>c__AnonStorey0
		{
			internal Map map;

			public <Resolve>c__AnonStorey0()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <Resolve>c__AnonStorey1
		{
			internal TraverseParms traverseParms;

			internal SymbolResolver_Settlement.<Resolve>c__AnonStorey0 <>f__ref$0;

			public <Resolve>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return this.<>f__ref$0.map.reachability.CanReachMapEdge(x, this.traverseParms);
			}
		}
	}
}
