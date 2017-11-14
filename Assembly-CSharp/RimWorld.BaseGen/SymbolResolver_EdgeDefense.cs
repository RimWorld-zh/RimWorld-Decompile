using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EdgeDefense : SymbolResolver
	{
		private const int DefaultCellsPerTurret = 30;

		private const int DefaultCellsPerMortar = 75;

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			int? edgeDefenseGuardsCount = rp.edgeDefenseGuardsCount;
			int num = edgeDefenseGuardsCount.HasValue ? edgeDefenseGuardsCount.Value : 0;
			int width;
			if (rp.edgeDefenseWidth.HasValue)
			{
				width = rp.edgeDefenseWidth.Value;
			}
			else if (rp.edgeDefenseMortarsCount.HasValue && rp.edgeDefenseMortarsCount.Value > 0)
			{
				width = 4;
			}
			else
			{
				width = ((!Rand.Bool) ? 4 : 2);
			}
			width = Mathf.Clamp(width, 1, Mathf.Min(rp.rect.Width, rp.rect.Height) / 2);
			int num2;
			int num3;
			bool flag;
			bool flag2;
			bool flag3;
			switch (width)
			{
			case 1:
			{
				int? edgeDefenseTurretsCount4 = rp.edgeDefenseTurretsCount;
				num2 = (edgeDefenseTurretsCount4.HasValue ? edgeDefenseTurretsCount4.Value : 0);
				num3 = 0;
				flag = false;
				flag2 = true;
				flag3 = true;
				break;
			}
			case 2:
			{
				int? edgeDefenseTurretsCount3 = rp.edgeDefenseTurretsCount;
				num2 = ((!edgeDefenseTurretsCount3.HasValue) ? (rp.rect.EdgeCellsCount / 30) : edgeDefenseTurretsCount3.Value);
				num3 = 0;
				flag = false;
				flag2 = false;
				flag3 = true;
				break;
			}
			case 3:
			{
				int? edgeDefenseTurretsCount2 = rp.edgeDefenseTurretsCount;
				num2 = ((!edgeDefenseTurretsCount2.HasValue) ? (rp.rect.EdgeCellsCount / 30) : edgeDefenseTurretsCount2.Value);
				int? edgeDefenseMortarsCount2 = rp.edgeDefenseMortarsCount;
				num3 = ((!edgeDefenseMortarsCount2.HasValue) ? (rp.rect.EdgeCellsCount / 75) : edgeDefenseMortarsCount2.Value);
				flag = (num3 == 0);
				flag2 = false;
				flag3 = true;
				break;
			}
			default:
			{
				int? edgeDefenseTurretsCount = rp.edgeDefenseTurretsCount;
				num2 = ((!edgeDefenseTurretsCount.HasValue) ? (rp.rect.EdgeCellsCount / 30) : edgeDefenseTurretsCount.Value);
				int? edgeDefenseMortarsCount = rp.edgeDefenseMortarsCount;
				num3 = ((!edgeDefenseMortarsCount.HasValue) ? (rp.rect.EdgeCellsCount / 75) : edgeDefenseMortarsCount.Value);
				flag = true;
				flag2 = false;
				flag3 = false;
				break;
			}
			}
			if (faction != null && (int)faction.def.techLevel < 4)
			{
				num2 = 0;
				num3 = 0;
			}
			if (num > 0)
			{
				Lord singlePawnLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell), map, null);
				for (int i = 0; i < num; i++)
				{
					PawnGenerationRequest value = new PawnGenerationRequest(faction.RandomPawnKind(), faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, true, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null);
					ResolveParams resolveParams = rp;
					resolveParams.faction = faction;
					resolveParams.singlePawnLord = singlePawnLord;
					resolveParams.singlePawnGenerationRequest = value;
					resolveParams.singlePawnSpawnCellExtraPredicate = (resolveParams.singlePawnSpawnCellExtraPredicate ?? ((Predicate<IntVec3>)delegate(IntVec3 x)
					{
						CellRect cellRect = rp.rect;
						for (int m = 0; m < width; m++)
						{
							if (cellRect.IsOnEdge(x))
							{
								return true;
							}
							cellRect = cellRect.ContractedBy(1);
						}
						return true;
					}));
					BaseGen.symbolStack.Push("pawn", resolveParams);
				}
			}
			CellRect rect = rp.rect;
			for (int j = 0; j < width; j++)
			{
				if (j % 2 == 0)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.faction = faction;
					resolveParams2.rect = rect;
					BaseGen.symbolStack.Push("edgeSandbags", resolveParams2);
					if (!flag)
						break;
				}
				rect = rect.ContractedBy(1);
			}
			CellRect rect2 = (!flag3) ? rp.rect.ContractedBy(1) : rp.rect;
			for (int k = 0; k < num3; k++)
			{
				ResolveParams resolveParams3 = rp;
				resolveParams3.faction = faction;
				resolveParams3.rect = rect2;
				BaseGen.symbolStack.Push("edgeMannedMortar", resolveParams3);
			}
			CellRect rect3 = (!flag2) ? rp.rect.ContractedBy(1) : rp.rect;
			for (int l = 0; l < num2; l++)
			{
				ResolveParams resolveParams4 = rp;
				resolveParams4.faction = faction;
				resolveParams4.singleThingDef = ThingDefOf.TurretGun;
				resolveParams4.rect = rect3;
				bool? edgeThingAvoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings;
				resolveParams4.edgeThingAvoidOtherEdgeThings = (!edgeThingAvoidOtherEdgeThings.HasValue || edgeThingAvoidOtherEdgeThings.Value);
				BaseGen.symbolStack.Push("edgeThing", resolveParams4);
			}
		}
	}
}
