using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_StartingAnimal : ScenPart
	{
		private PawnKindDef animalKind = null;

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

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pair<int, float>, float> <>f__am$cache2;

		public ScenPart_StartingAnimal()
		{
		}

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

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return this.CurrentAnimalLabel().CapitalizeFirst() + " x" + this.count;
			}
			yield break;
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

		// Note: this type is marked as 'beforefieldinit'.
		static ScenPart_StartingAnimal()
		{
		}

		[CompilerGenerated]
		private void <DoEditInterface>m__0()
		{
			this.animalKind = null;
		}

		[CompilerGenerated]
		private static bool <PossibleAnimals>m__1(PawnKindDef td)
		{
			return td.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static bool <RandomPets>m__2(PawnKindDef td)
		{
			return td.RaceProps.petness > 0f;
		}

		[CompilerGenerated]
		private static float <Randomize>m__3(Pair<int, float> pa)
		{
			return pa.Second;
		}

		[CompilerGenerated]
		private sealed class <DoEditInterface>c__AnonStorey2
		{
			internal PawnKindDef localKind;

			internal ScenPart_StartingAnimal $this;

			public <DoEditInterface>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.$this.animalKind = this.localKind;
			}
		}

		[CompilerGenerated]
		private sealed class <GetSummaryListEntries>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal string tag;

			internal ScenPart_StartingAnimal $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetSummaryListEntries>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (tag == "PlayerStartsWith")
					{
						this.$current = base.CurrentAnimalLabel().CapitalizeFirst() + " x" + this.count;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ScenPart_StartingAnimal.<GetSummaryListEntries>c__Iterator0 <GetSummaryListEntries>c__Iterator = new ScenPart_StartingAnimal.<GetSummaryListEntries>c__Iterator0();
				<GetSummaryListEntries>c__Iterator.$this = this;
				<GetSummaryListEntries>c__Iterator.tag = tag;
				return <GetSummaryListEntries>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PlayerStartingThings>c__Iterator1 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal int <i>__1;

			internal PawnKindDef <kind>__2;

			internal ScenPart_StartingAnimal $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private ScenPart_StartingAnimal.<PlayerStartingThings>c__Iterator1.<PlayerStartingThings>c__AnonStorey3 $locvar0;

			private static Func<PawnKindDef, float> <>f__am$cache0;

			[DebuggerHidden]
			public <PlayerStartingThings>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < this.count)
				{
					if (this.animalKind != null)
					{
						kind = this.animalKind;
					}
					else
					{
						kind = base.RandomPets().RandomElementByWeight((PawnKindDef td) => td.RaceProps.petness);
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
					this.$current = animal;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ScenPart_StartingAnimal.<PlayerStartingThings>c__Iterator1 <PlayerStartingThings>c__Iterator = new ScenPart_StartingAnimal.<PlayerStartingThings>c__Iterator1();
				<PlayerStartingThings>c__Iterator.$this = this;
				return <PlayerStartingThings>c__Iterator;
			}

			private static float <>m__0(PawnKindDef td)
			{
				return td.RaceProps.petness;
			}

			private sealed class <PlayerStartingThings>c__AnonStorey3
			{
				internal Pawn animal;

				internal ScenPart_StartingAnimal.<PlayerStartingThings>c__Iterator1 <>f__ref$1;

				public <PlayerStartingThings>c__AnonStorey3()
				{
				}

				internal bool <>m__0(Pawn p)
				{
					return TrainableUtility.CanBeMaster(p, this.animal, false) && !p.story.traits.HasTrait(TraitDefOf.Psychopath);
				}
			}
		}
	}
}
