using RimWorld.BaseGen;
using RimWorld.Planet;
using System;
using Verse;

namespace RimWorld
{
	public class GenStep_PrisonerWillingToJoin : GenStep_Scatterer
	{
		private const int Size = 8;

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (!base.CanScatterAt(c, map))
			{
				result = false;
			}
			else if (!c.SupportsStructureType(map, TerrainAffordance.Heavy))
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
					if (iterator.Current.InBounds(map) && iterator.Current.GetEdifice(map) == null)
					{
						iterator.MoveNext();
						continue;
					}
					goto IL_0084;
				}
				result = true;
			}
			goto IL_00a6;
			IL_0084:
			result = false;
			goto IL_00a6;
			IL_00a6:
			return result;
		}

		protected override void ScatterAt(IntVec3 loc, Map map, int count = 1)
		{
			Faction faction = (map.ParentFaction != null && map.ParentFaction != Faction.OfPlayer) ? map.ParentFaction : Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			CellRect cellRect = CellRect.CenteredOn(loc, 8, 8).ClipInsideMap(map);
			PrisonerWillingToJoinComp component = ((WorldObject)map.info.parent).GetComponent<PrisonerWillingToJoinComp>();
			Pawn singlePawnToSpawn = (component == null || !component.pawn.Any) ? PrisonerWillingToJoinQuestUtility.GeneratePrisoner(map.Tile, faction) : component.pawn.Take(component.pawn[0]);
			ResolveParams resolveParams = new ResolveParams
			{
				rect = cellRect,
				faction = faction
			};
			RimWorld.BaseGen.BaseGen.globalSettings.map = map;
			RimWorld.BaseGen.BaseGen.symbolStack.Push("prisonCell", resolveParams);
			RimWorld.BaseGen.BaseGen.Generate();
			ResolveParams resolveParams2 = new ResolveParams
			{
				rect = cellRect,
				faction = faction,
				singlePawnToSpawn = singlePawnToSpawn,
				postThingSpawn = (Action<Thing>)delegate(Thing x)
				{
					MapGenerator.rootsToUnfog.Add(x.Position);
					((Pawn)x).mindState.willJoinColonyIfRescued = true;
				}
			};
			RimWorld.BaseGen.BaseGen.globalSettings.map = map;
			RimWorld.BaseGen.BaseGen.symbolStack.Push("pawn", resolveParams2);
			RimWorld.BaseGen.BaseGen.Generate();
			MapGenerator.SetVar("RectOfInterest", cellRect);
		}
	}
}
