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
			int numKinds = this.kindCountRange.RandomInRange;
			int count = base.countRange.RandomInRange;
			List<PawnKindDef> kinds = new List<PawnKindDef>();
			int i = 0;
			PawnKindDef kind;
			while (i < numKinds && (from k in DefDatabase<PawnKindDef>.AllDefs
			where !((_003CGenerateThings_003Ec__Iterator17F)/*Error near IL_0069: stateMachine*/)._003Ckinds_003E__2.Contains(k) && ((_003CGenerateThings_003Ec__Iterator17F)/*Error near IL_0069: stateMachine*/)._003C_003Ef__this.PawnKindAllowed(k, ((_003CGenerateThings_003Ec__Iterator17F)/*Error near IL_0069: stateMachine*/).forTile)
			select k).TryRandomElementByWeight<PawnKindDef>((Func<PawnKindDef, float>)((PawnKindDef k) => ((_003CGenerateThings_003Ec__Iterator17F)/*Error near IL_007a: stateMachine*/)._003C_003Ef__this.SelectionChance(k)), out kind))
			{
				kinds.Add(kind);
				i++;
			}
			int j = 0;
			PawnKindDef kind2;
			while (j < count && ((IEnumerable<PawnKindDef>)kinds).TryRandomElement<PawnKindDef>(out kind2))
			{
				PawnGenerationRequest request = new PawnGenerationRequest(kind2, null, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, false, true, true, false, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
				yield return (Thing)PawnGenerator.GeneratePawn(request);
				j++;
			}
		}

		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.RaceProps.wildness);
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal;
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
				if (this.tradeTags.Find((Predicate<string>)((string x) => kind.race.tradeTags.Contains(x))) == null)
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
