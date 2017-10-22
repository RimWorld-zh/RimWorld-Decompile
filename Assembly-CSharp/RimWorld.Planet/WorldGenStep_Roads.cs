using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldGenStep_Roads : WorldGenStep
	{
		private struct Link
		{
			public float distance;

			public int indexA;

			public int indexB;
		}

		private class Connectedness
		{
			public Connectedness parent;

			public Connectedness Group()
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Group();
			}
		}

		private const float ChanceExtraNonSpanningTreeLink = 0.015f;

		private const float ChanceHideSpanningTreeLink = 0.1f;

		private const float ChanceWorldObjectReclusive = 0.05f;

		private const int PotentialSpanningTreeLinksPerSettlement = 8;

		private static readonly FloatRange ExtraRoadNodesPer100kTiles = new FloatRange(30f, 50f);

		private static readonly IntRange RoadDistanceFromSettlement = new IntRange(-4, 4);

		public override void GenerateFresh(string seed)
		{
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadEndpoints();
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork();
			Rand.RandomizeStateFromTime();
		}

		public override void GenerateFromScribe(string seed)
		{
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork();
			Rand.RandomizeStateFromTime();
		}

		private void GenerateRoadEndpoints()
		{
			List<int> list = (from wo in Find.WorldObjects.AllWorldObjects
			where Rand.Value > 0.05000000074505806
			select wo.Tile).ToList();
			int num = GenMath.RoundRandom((float)((float)Find.WorldGrid.TilesCount / 100000.0 * WorldGenStep_Roads.ExtraRoadNodesPer100kTiles.RandomInRange));
			for (int num2 = 0; num2 < num; num2++)
			{
				list.Add(TileFinder.RandomFactionBaseTileFor(null, false));
			}
			List<int> list2 = new List<int>();
			for (int i = 0; i < list.Count; i++)
			{
				int num3 = Mathf.Max(0, WorldGenStep_Roads.RoadDistanceFromSettlement.RandomInRange);
				int num4 = list[i];
				for (int num5 = 0; num5 < num3; num5++)
				{
					Find.WorldGrid.GetTileNeighbors(num4, list2);
					num4 = list2.RandomElement();
				}
				if (Find.WorldReachability.CanReach(list[i], num4))
				{
					list[i] = num4;
				}
			}
			list = (Find.World.genData.roadNodes = list.Distinct().ToList());
		}

		private void GenerateRoadNetwork()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts(Season.Spring.GetMiddleYearPct(0f));
			List<Link> linkProspective = this.GenerateProspectiveLinks(Find.World.genData.roadNodes);
			List<Link> linkFinal = this.GenerateFinalLinks(linkProspective, Find.World.genData.roadNodes.Count);
			this.DrawLinksOnWorld(linkFinal, Find.World.genData.roadNodes);
		}

		private List<Link> GenerateProspectiveLinks(List<int> indexToTile)
		{
			Dictionary<int, int> tileToIndexLookup = new Dictionary<int, int>();
			for (int i = 0; i < indexToTile.Count; i++)
			{
				tileToIndexLookup[indexToTile[i]] = i;
			}
			List<Link> linkProspective = new List<Link>();
			List<int> list = new List<int>();
			int srcIndex;
			for (srcIndex = 0; srcIndex < indexToTile.Count; srcIndex++)
			{
				int srcTile = indexToTile[srcIndex];
				list.Clear();
				list.Add(srcTile);
				int found = 0;
				WorldPathFinder worldPathFinder = Find.WorldPathFinder;
				Func<int, float, bool> terminator = (Func<int, float, bool>)delegate(int tile, float distance)
				{
					if (tile != srcTile && tileToIndexLookup.ContainsKey(tile))
					{
						found++;
						linkProspective.Add(new Link
						{
							distance = distance,
							indexA = srcIndex,
							indexB = tileToIndexLookup[tile]
						});
					}
					return found >= 8;
				};
				worldPathFinder.FloodPathsWithCost(list, (Func<int, int, int>)((int src, int dst) => WorldPathFinder.StandardPathCost(src, dst, null)), null, terminator);
			}
			linkProspective.Sort((Comparison<Link>)((Link lhs, Link rhs) => lhs.distance.CompareTo(rhs.distance)));
			return linkProspective;
		}

		private List<Link> GenerateFinalLinks(List<Link> linkProspective, int endpointCount)
		{
			List<Connectedness> list = new List<Connectedness>();
			for (int num = 0; num < endpointCount; num++)
			{
				list.Add(new Connectedness());
			}
			List<Link> list2 = new List<Link>();
			for (int i = 0; i < linkProspective.Count; i++)
			{
				Link prospective = linkProspective[i];
				if (list[prospective.indexA].Group() != list[prospective.indexB].Group() || (!(Rand.Value > 0.014999999664723873) && !list2.Any((Predicate<Link>)((Link link) => link.indexB == prospective.indexA && link.indexA == prospective.indexB))))
				{
					if (Rand.Value > 0.10000000149011612)
					{
						list2.Add(prospective);
					}
					if (list[prospective.indexA].Group() != list[prospective.indexB].Group())
					{
						Connectedness parent = list[prospective.indexA].Group().parent = new Connectedness();
						list[prospective.indexB].Group().parent = parent;
					}
				}
			}
			return list2;
		}

		private void DrawLinksOnWorld(List<Link> linkFinal, List<int> indexToTile)
		{
			List<Link>.Enumerator enumerator = linkFinal.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Link current = enumerator.Current;
					WorldPath worldPath = Find.WorldPathFinder.FindPath(indexToTile[current.indexA], indexToTile[current.indexB], null, null);
					List<int> nodesReversed = worldPath.NodesReversed;
					RoadDef roadDef = (from rd in DefDatabase<RoadDef>.AllDefsListForReading
					where !rd.ancientOnly
					select rd).RandomElementWithFallback(null);
					for (int i = 0; i < nodesReversed.Count - 1; i++)
					{
						Find.WorldGrid.OverlayRoad(nodesReversed[i], nodesReversed[i + 1], roadDef);
					}
					worldPath.ReleaseToPool();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
