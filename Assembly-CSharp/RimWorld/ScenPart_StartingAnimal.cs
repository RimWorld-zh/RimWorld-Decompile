using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_StartingAnimal : ScenPart
	{
		private PawnKindDef animalKind;

		private int count = 1;

		private float bondToRandomPlayerPawnChance = 0.5f;

		private string countBuf;

		private static readonly List<Pair<int, float>> PetCountChances = new List<Pair<int, float>>
		{
			new Pair<int, float>(1, 20f),
			new Pair<int, float>(2, 10f),
			new Pair<int, float>(3, 5f),
			new Pair<int, float>(4, 3f),
			new Pair<int, float>(5, 1f),
			new Pair<int, float>(6, 1f),
			new Pair<int, float>(7, 1f),
			new Pair<int, float>(8, 1f),
			new Pair<int, float>(9, 1f),
			new Pair<int, float>(10, 0.1f),
			new Pair<int, float>(11, 0.1f),
			new Pair<int, float>(12, 0.1f),
			new Pair<int, float>(13, 0.1f),
			new Pair<int, float>(14, 0.1f)
		};

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<float>(ref this.bondToRandomPlayerPawnChance, "bondToRandomPlayerPawnChance", 0f, false);
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(scenPartRect.TopHalf());
			listing_Standard.ColumnWidth = scenPartRect.width;
			listing_Standard.TextFieldNumeric<int>(ref this.count, ref this.countBuf, 1f, 1E+09f);
			listing_Standard.End();
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.CurrentAnimalLabel().CapitalizeFirst(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("RandomPet".Translate().CapitalizeFirst(), delegate
				{
					this.animalKind = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				foreach (PawnKindDef current in this.PossibleAnimals())
				{
					PawnKindDef localKind = current;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate
					{
						this.animalKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		private IEnumerable<PawnKindDef> PossibleAnimals()
		{
			return from td in DefDatabase<PawnKindDef>.AllDefs
			where td.RaceProps.Animal
			select td;
		}

		private IEnumerable<PawnKindDef> RandomPets()
		{
			return from td in this.PossibleAnimals()
			where td.RaceProps.petness > 0f
			select td;
		}

		private string CurrentAnimalLabel()
		{
			return (this.animalKind == null) ? "RandomPet".Translate() : this.animalKind.label;
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		[DebuggerHidden]
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_StartingAnimal.<GetSummaryListEntries>c__Iterator11B <GetSummaryListEntries>c__Iterator11B = new ScenPart_StartingAnimal.<GetSummaryListEntries>c__Iterator11B();
			<GetSummaryListEntries>c__Iterator11B.tag = tag;
			<GetSummaryListEntries>c__Iterator11B.<$>tag = tag;
			<GetSummaryListEntries>c__Iterator11B.<>f__this = this;
			ScenPart_StartingAnimal.<GetSummaryListEntries>c__Iterator11B expr_1C = <GetSummaryListEntries>c__Iterator11B;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public override void Randomize()
		{
			if (Rand.Value < 0.5f)
			{
				this.animalKind = null;
			}
			else
			{
				this.animalKind = this.PossibleAnimals().RandomElement<PawnKindDef>();
			}
			this.count = ScenPart_StartingAnimal.PetCountChances.RandomElementByWeight((Pair<int, float> pa) => pa.Second).First;
			this.bondToRandomPlayerPawnChance = 0f;
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StartingAnimal scenPart_StartingAnimal = other as ScenPart_StartingAnimal;
			if (scenPart_StartingAnimal != null && scenPart_StartingAnimal.animalKind == this.animalKind)
			{
				this.count += scenPart_StartingAnimal.count;
				return true;
			}
			return false;
		}

		[DebuggerHidden]
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			ScenPart_StartingAnimal.<PlayerStartingThings>c__Iterator11C <PlayerStartingThings>c__Iterator11C = new ScenPart_StartingAnimal.<PlayerStartingThings>c__Iterator11C();
			<PlayerStartingThings>c__Iterator11C.<>f__this = this;
			ScenPart_StartingAnimal.<PlayerStartingThings>c__Iterator11C expr_0E = <PlayerStartingThings>c__Iterator11C;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
