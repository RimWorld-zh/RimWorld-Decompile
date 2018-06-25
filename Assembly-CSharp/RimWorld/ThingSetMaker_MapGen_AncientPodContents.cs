using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class ThingSetMaker_MapGen_AncientPodContents : ThingSetMaker
	{
		public ThingSetMaker_MapGen_AncientPodContents()
		{
		}

		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PodContentsType? podContentsType = parms.podContentsType;
			PodContentsType podContentsType2 = (podContentsType == null) ? Gen.RandomEnumValue<PodContentsType>(true) : podContentsType.Value;
			switch (podContentsType2)
			{
			case PodContentsType.Empty:
				break;
			case PodContentsType.AncientFriendly:
				outThings.Add(this.GenerateFriendlyAncient());
				break;
			case PodContentsType.AncientIncapped:
				outThings.Add(this.GenerateIncappedAncient());
				break;
			case PodContentsType.AncientHalfEaten:
				outThings.Add(this.GenerateHalfEatenAncient());
				outThings.AddRange(this.GenerateScarabs());
				break;
			case PodContentsType.AncientHostile:
				outThings.Add(this.GenerateAngryAncient());
				break;
			case PodContentsType.Slave:
				outThings.Add(this.GenerateSlave());
				break;
			default:
				Log.Error("Pod contents type not handled: " + podContentsType2, false);
				break;
			}
		}

		private Pawn GenerateFriendlyAncient()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		private Pawn GenerateIncappedAncient()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			HealthUtility.DamageUntilDowned(pawn);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		private Pawn GenerateSlave()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Slave, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			HealthUtility.DamageUntilDowned(pawn);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			if (Rand.Value < 0.5f)
			{
				HealthUtility.DamageUntilDead(pawn);
			}
			return pawn;
		}

		private Pawn GenerateAngryAncient()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncientsHostile, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		private Pawn GenerateHalfEatenAncient()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			int num = Rand.Range(6, 10);
			for (int i = 0; i < num; i++)
			{
				Thing thing = pawn;
				DamageDef bite = DamageDefOf.Bite;
				float amount = (float)Rand.Range(3, 8);
				Pawn instigator = pawn;
				thing.TakeDamage(new DamageInfo(bite, amount, -1f, instigator, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		private List<Thing> GenerateScarabs()
		{
			List<Thing> list = new List<Thing>();
			int num = Rand.Range(3, 6);
			for (int i = 0; i < num; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Megascarab, null);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false);
				list.Add(pawn);
			}
			return list;
		}

		private void GiveRandomLootInventoryForTombPawn(Pawn p)
		{
			if (Rand.Value < 0.65f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Gold, Rand.Range(10, 50));
			}
			else
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Plasteel, Rand.Range(10, 50));
			}
			if (Rand.Value < 0.7f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentIndustrial, Rand.Range(-2, 4));
			}
			else
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentSpacer, Rand.Range(-2, 4));
			}
		}

		private void MakeIntoContainer(ThingOwner container, ThingDef def, int count)
		{
			if (count > 0)
			{
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = count;
				container.TryAdd(thing, true);
			}
		}

		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.AncientSoldier.race;
			yield return PawnKindDefOf.Slave.race;
			yield return PawnKindDefOf.Megascarab.race;
			yield return ThingDefOf.Gold;
			yield return ThingDefOf.Plasteel;
			yield return ThingDefOf.ComponentIndustrial;
			yield return ThingDefOf.ComponentSpacer;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <AllGeneratableThingsDebugSub>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllGeneratableThingsDebugSub>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = PawnKindDefOf.AncientSoldier.race;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = PawnKindDefOf.Slave.race;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = PawnKindDefOf.Megascarab.race;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = ThingDefOf.Gold;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = ThingDefOf.Plasteel;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = ThingDefOf.ComponentIndustrial;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = ThingDefOf.ComponentSpacer;
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ThingSetMaker_MapGen_AncientPodContents.<AllGeneratableThingsDebugSub>c__Iterator0();
			}
		}
	}
}
