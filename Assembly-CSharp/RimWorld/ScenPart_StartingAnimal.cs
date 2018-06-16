using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000649 RID: 1609
	public class ScenPart_StartingAnimal : ScenPart
	{
		// Token: 0x06002153 RID: 8531 RVA: 0x0011ABE4 File Offset: 0x00118FE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<float>(ref this.bondToRandomPlayerPawnChance, "bondToRandomPlayerPawnChance", 0f, false);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x0011AC30 File Offset: 0x00119030
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
				list.Add(new FloatMenuOption("RandomPet".Translate().CapitalizeFirst(), delegate()
				{
					this.animalKind = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				foreach (PawnKindDef localKind2 in this.PossibleAnimals())
				{
					PawnKindDef localKind = localKind2;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
					{
						this.animalKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x0011AD8C File Offset: 0x0011918C
		private IEnumerable<PawnKindDef> PossibleAnimals()
		{
			return from td in DefDatabase<PawnKindDef>.AllDefs
			where td.RaceProps.Animal
			select td;
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0011ADC8 File Offset: 0x001191C8
		private IEnumerable<PawnKindDef> RandomPets()
		{
			return from td in this.PossibleAnimals()
			where td.RaceProps.petness > 0f
			select td;
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x0011AE08 File Offset: 0x00119208
		private string CurrentAnimalLabel()
		{
			return (this.animalKind == null) ? "RandomPet".Translate() : this.animalKind.label;
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x0011AE44 File Offset: 0x00119244
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x0011AE6C File Offset: 0x0011926C
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return this.CurrentAnimalLabel().CapitalizeFirst() + " x" + this.count;
			}
			yield break;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x0011AEA0 File Offset: 0x001192A0
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

		// Token: 0x0600215B RID: 8539 RVA: 0x0011AF1C File Offset: 0x0011931C
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StartingAnimal scenPart_StartingAnimal = other as ScenPart_StartingAnimal;
			bool result;
			if (scenPart_StartingAnimal != null && scenPart_StartingAnimal.animalKind == this.animalKind)
			{
				this.count += scenPart_StartingAnimal.count;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x0011AF6C File Offset: 0x0011936C
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			for (int i = 0; i < this.count; i++)
			{
				PawnKindDef kind;
				if (this.animalKind != null)
				{
					kind = this.animalKind;
				}
				else
				{
					kind = this.RandomPets().RandomElementByWeight((PawnKindDef td) => td.RaceProps.petness);
				}
				Pawn animal = PawnGenerator.GeneratePawn(kind, Faction.OfPlayer);
				if (animal.Name == null || animal.Name.Numerical)
				{
					animal.Name = PawnBioAndNameGenerator.GeneratePawnName(animal, NameStyle.Full, null);
				}
				if (Rand.Value < this.bondToRandomPlayerPawnChance && animal.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted)
				{
					Pawn pawn = (from p in Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount)
					where TrainableUtility.CanBeMaster(p, animal, false) && !p.story.traits.HasTrait(TraitDefOf.Psychopath)
					select p).RandomElementWithFallback(null);
					if (pawn != null)
					{
						animal.training.Train(TrainableDefOf.Obedience, null, true);
						animal.training.SetWantedRecursive(TrainableDefOf.Obedience, true);
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Bond, animal);
						animal.playerSettings.Master = pawn;
					}
				}
				yield return animal;
			}
			yield break;
		}

		// Token: 0x040012F8 RID: 4856
		private PawnKindDef animalKind = null;

		// Token: 0x040012F9 RID: 4857
		private int count = 1;

		// Token: 0x040012FA RID: 4858
		private float bondToRandomPlayerPawnChance = 0.5f;

		// Token: 0x040012FB RID: 4859
		private string countBuf;

		// Token: 0x040012FC RID: 4860
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
	}
}
