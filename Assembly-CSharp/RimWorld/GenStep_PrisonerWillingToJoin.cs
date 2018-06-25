using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000410 RID: 1040
	public class GenStep_PrisonerWillingToJoin : GenStep_Scatterer
	{
		// Token: 0x04000AE3 RID: 2787
		private const int Size = 8;

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0009B37C File Offset: 0x0009977C
		public override int SeedPart
		{
			get
			{
				return 69356099;
			}
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0009B398 File Offset: 0x00099798
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (!base.CanScatterAt(c, map))
			{
				result = false;
			}
			else if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				result = false;
			}
			else if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				result = false;
			}
			else
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(c, 8, 8).GetIterator();
				while (!iterator.Done())
				{
					if (!iterator.Current.InBounds(map) || iterator.Current.GetEdifice(map) != null)
					{
						return false;
					}
					iterator.MoveNext();
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0009B450 File Offset: 0x00099850
		protected override void ScatterAt(IntVec3 loc, Map map, int count = 1)
		{
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			CellRect cellRect = CellRect.CenteredOn(loc, 8, 8).ClipInsideMap(map);
			PrisonerWillingToJoinComp component = map.Parent.GetComponent<PrisonerWillingToJoinComp>();
			Pawn singlePawnToSpawn;
			if (component != null && component.pawn.Any)
			{
				singlePawnToSpawn = component.pawn.Take(component.pawn[0]);
			}
			else
			{
				singlePawnToSpawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(map.Tile, faction);
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("prisonCell", resolveParams);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = cellRect;
			resolveParams2.faction = faction;
			resolveParams2.singlePawnToSpawn = singlePawnToSpawn;
			resolveParams2.postThingSpawn = delegate(Thing x)
			{
				MapGenerator.rootsToUnfog.Add(x.Position);
				((Pawn)x).mindState.willJoinColonyIfRescued = true;
			};
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("pawn", resolveParams2);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
		}
	}
}
