using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class GenStep_Turrets : GenStep
	{
		public IntRange widthRange = new IntRange(3, 4);

		public IntRange turretsCountRange = new IntRange(4, 5);

		public IntRange mortarsCountRange = new IntRange(0, 1);

		public IntRange guardsCountRange = IntRange.one;

		private const int Padding = 7;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache0;

		public GenStep_Turrets()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 895502705;
			}
		}

		public override void Generate(Map map)
		{
			CellRect cellRect;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				cellRect = this.FindRandomRectToDefend(map);
			}
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = (from x in Find.FactionManager.AllFactions
				where !x.defeated && x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.techLevel >= TechLevel.Industrial
				select x).RandomElementWithFallback(Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined));
			}
			else
			{
				faction = map.ParentFaction;
			}
			int randomInRange = this.widthRange.RandomInRange;
			CellRect rect = cellRect.ExpandedBy(7 + randomInRange).ClipInsideMap(map);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = new int?(randomInRange);
			resolveParams.edgeDefenseTurretsCount = new int?(this.turretsCountRange.RandomInRange);
			resolveParams.edgeDefenseMortarsCount = new int?(this.mortarsCountRange.RandomInRange);
			resolveParams.edgeDefenseGuardsCount = new int?(this.guardsCountRange.RandomInRange);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("edgeDefense", resolveParams);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = rect;
			resolveParams2.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("outdoorLighting", resolveParams2);
			BaseGen.Generate();
		}

		private CellRect FindRandomRectToDefend(Map map)
		{
			int rectRadius = Mathf.Max(Mathf.RoundToInt((float)Mathf.Min(map.Size.x, map.Size.z) * 0.07f), 1);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
			IntVec3 center;
			CellRect result;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(delegate(IntVec3 x)
			{
				bool result2;
				if (!map.reachability.CanReachMapEdge(x, traverseParams))
				{
					result2 = false;
				}
				else
				{
					CellRect cellRect = CellRect.CenteredOn(x, rectRadius);
					int num = 0;
					CellRect.CellRectIterator iterator = cellRect.GetIterator();
					while (!iterator.Done())
					{
						if (!iterator.Current.InBounds(map))
						{
							return false;
						}
						if (iterator.Current.Standable(map) || iterator.Current.GetPlant(map) != null)
						{
							num++;
						}
						iterator.MoveNext();
					}
					result2 = ((float)num / (float)cellRect.Area >= 0.6f);
				}
				return result2;
			}, map, out center))
			{
				result = CellRect.CenteredOn(center, rectRadius);
			}
			else if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map), map, out center))
			{
				result = CellRect.CenteredOn(center, rectRadius);
			}
			else
			{
				result = CellRect.CenteredOn(CellFinder.RandomCell(map), rectRadius).ClipInsideMap(map);
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <Generate>m__0(Faction x)
		{
			return !x.defeated && x.HostileTo(Faction.OfPlayer) && !x.def.hidden && x.def.techLevel >= TechLevel.Industrial;
		}

		[CompilerGenerated]
		private sealed class <FindRandomRectToDefend>c__AnonStorey0
		{
			internal Map map;

			internal TraverseParms traverseParams;

			internal int rectRadius;

			public <FindRandomRectToDefend>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				bool result;
				if (!this.map.reachability.CanReachMapEdge(x, this.traverseParams))
				{
					result = false;
				}
				else
				{
					CellRect cellRect = CellRect.CenteredOn(x, this.rectRadius);
					int num = 0;
					CellRect.CellRectIterator iterator = cellRect.GetIterator();
					while (!iterator.Done())
					{
						if (!iterator.Current.InBounds(this.map))
						{
							return false;
						}
						if (iterator.Current.Standable(this.map) || iterator.Current.GetPlant(this.map) != null)
						{
							num++;
						}
						iterator.MoveNext();
					}
					result = ((float)num / (float)cellRect.Area >= 0.6f);
				}
				return result;
			}

			internal bool <>m__1(IntVec3 x)
			{
				return x.Standable(this.map);
			}
		}
	}
}
