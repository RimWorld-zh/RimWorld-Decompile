using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000772 RID: 1906
	[HasDebugOutput]
	public class StockGenerator_Animals : StockGenerator
	{
		// Token: 0x06002A1C RID: 10780 RVA: 0x001654CC File Offset: 0x001638CC
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			int numKinds = this.kindCountRange.RandomInRange;
			int count = this.countRange.RandomInRange;
			List<PawnKindDef> kinds = new List<PawnKindDef>();
			for (int j = 0; j < numKinds; j++)
			{
				PawnKindDef item;
				if (!(from k in DefDatabase<PawnKindDef>.AllDefs
				where !kinds.Contains(k) && this.PawnKindAllowed(k, forTile)
				select k).TryRandomElementByWeight((PawnKindDef k) => this.SelectionChance(k), out item))
				{
					break;
				}
				kinds.Add(item);
			}
			for (int i = 0; i < count; i++)
			{
				PawnKindDef kind;
				if (!kinds.TryRandomElement(out kind))
				{
					yield break;
				}
				PawnKindDef kind2 = kind;
				int forTile2 = forTile;
				PawnGenerationRequest request = new PawnGenerationRequest(kind2, null, PawnGenerationContext.NonPlayer, forTile2, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				yield return PawnGenerator.GeneratePawn(request);
			}
			yield break;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x00165500 File Offset: 0x00163900
		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.RaceProps.wildness);
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x0016552C File Offset: 0x0016392C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.None && (this.tradeTagsSell.Any((string tag) => thingDef.tradeTags.Contains(tag)) || this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags.Contains(tag)));
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x001655C0 File Offset: 0x001639C0
		private bool PawnKindAllowed(PawnKindDef kind, int forTile)
		{
			bool result;
			if (!kind.RaceProps.Animal || kind.RaceProps.wildness < this.minWildness || kind.RaceProps.wildness > this.maxWildness || kind.RaceProps.wildness > 1f)
			{
				result = false;
			}
			else
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
				result = (kind.race.tradeTags != null && this.tradeTagsSell.Any((string x) => kind.race.tradeTags.Contains(x)) && kind.race.tradeability.TraderCanSell());
			}
			return result;
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x0016570C File Offset: 0x00163B0C
		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(pawnKindDef.defName + ": " + this.SelectionChance(pawnKindDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x001657A0 File Offset: 0x00163BA0
		[DebugOutput]
		private static void StockGenerationAnimals()
		{
			new StockGenerator_Animals
			{
				tradeTagsSell = new List<string>(),
				tradeTagsSell = 
				{
					"AnimalCommon",
					"AnimalUncommon"
				}
			}.LogAnimalChances();
		}

		// Token: 0x040016B9 RID: 5817
		[NoTranslate]
		private List<string> tradeTagsSell = null;

		// Token: 0x040016BA RID: 5818
		[NoTranslate]
		private List<string> tradeTagsBuy = null;

		// Token: 0x040016BB RID: 5819
		private IntRange kindCountRange = new IntRange(1, 1);

		// Token: 0x040016BC RID: 5820
		private float minWildness = 0f;

		// Token: 0x040016BD RID: 5821
		private float maxWildness = 1f;

		// Token: 0x040016BE RID: 5822
		private bool checkTemperature = false;

		// Token: 0x040016BF RID: 5823
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
	}
}
