using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class StockGenerator_Animals : StockGenerator
	{
		private IntRange kindCountRange = new IntRange(1, 1);

		private float minWildness;

		private float maxWildness = 1f;

		private List<string> tradeTags;

		private bool checkTemperature;

		private static readonly SimpleCurve SelectionChanceFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 100f),
				true
			},
			{
				new CurvePoint(0.25f, 60f),
				true
			},
			{
				new CurvePoint(0.5f, 30f),
				true
			},
			{
				new CurvePoint(0.75f, 12f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};

		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			_003CGenerateThings_003Ec__Iterator0 _003CGenerateThings_003Ec__Iterator = (_003CGenerateThings_003Ec__Iterator0)/*Error near IL_0032: stateMachine*/;
			int numKinds = this.kindCountRange.RandomInRange;
			int count = base.countRange.RandomInRange;
			List<PawnKindDef> kinds = new List<PawnKindDef>();
			int num = 0;
			PawnKindDef item = default(PawnKindDef);
			while (num < numKinds && (from k in DefDatabase<PawnKindDef>.AllDefs
			where !kinds.Contains(k) && _003CGenerateThings_003Ec__Iterator._0024this.PawnKindAllowed(k, forTile)
			select k).TryRandomElementByWeight<PawnKindDef>((Func<PawnKindDef, float>)((PawnKindDef k) => _003CGenerateThings_003Ec__Iterator._0024this.SelectionChance(k)), out item))
			{
				kinds.Add(item);
				num++;
			}
			int i = 0;
			PawnKindDef kind;
			if (i < count && ((IEnumerable<PawnKindDef>)kinds).TryRandomElement<PawnKindDef>(out kind))
			{
				PawnKindDef kind2 = kind;
				int tile = forTile;
				PawnGenerationRequest request = new PawnGenerationRequest(kind2, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null);
				yield return (Thing)PawnGenerator.GeneratePawn(request);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.RaceProps.wildness);
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.Never;
		}

		private bool PawnKindAllowed(PawnKindDef kind, int forTile)
		{
			if (kind.RaceProps.Animal && !(kind.RaceProps.wildness < this.minWildness) && !(kind.RaceProps.wildness > this.maxWildness) && !(kind.RaceProps.wildness > 1.0))
			{
				if (this.checkTemperature)
				{
					int num = forTile;
					if (num == -1 && Find.AnyPlayerHomeMap != null)
					{
						num = Find.AnyPlayerHomeMap.Tile;
					}
					if (num != -1 && !Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(num, kind.race))
					{
						return false;
					}
				}
				if (kind.race.tradeTags == null)
				{
					return false;
				}
				if (this.tradeTags.Find((string x) => kind.race.tradeTags.Contains(x)) == null)
				{
					return false;
				}
				if (kind.race.tradeability != Tradeability.Stockable)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef allDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(allDef.defName + ": " + this.SelectionChance(allDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString());
		}

		internal static void LogStockGeneration()
		{
			StockGenerator_Animals stockGenerator_Animals = new StockGenerator_Animals();
			stockGenerator_Animals.tradeTags = new List<string>();
			stockGenerator_Animals.tradeTags.Add("StandardAnimal");
			stockGenerator_Animals.tradeTags.Add("BadassAnimal");
			stockGenerator_Animals.LogAnimalChances();
		}
	}
}
