using System;
using System.Runtime.CompilerServices;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class GenStep_PrisonerWillingToJoin : GenStep_Scatterer
	{
		private const int Size = 8;

		[CompilerGenerated]
		private static Action<Thing> <>f__am$cache0;

		public GenStep_PrisonerWillingToJoin()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 69356099;
			}
		}

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

		[CompilerGenerated]
		private static void <ScatterAt>m__0(Thing x)
		{
			MapGenerator.rootsToUnfog.Add(x.Position);
			((Pawn)x).mindState.willJoinColonyIfRescued = true;
		}
	}
}
