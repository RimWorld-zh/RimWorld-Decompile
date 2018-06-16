using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F8 RID: 1784
	public class ThingSetMaker_MapGen_AncientPodContents : ThingSetMaker
	{
		// Token: 0x060026CA RID: 9930 RVA: 0x0014CA84 File Offset: 0x0014AE84
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

		// Token: 0x060026CB RID: 9931 RVA: 0x0014CB60 File Offset: 0x0014AF60
		private Pawn GenerateFriendlyAncient()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0014CBDC File Offset: 0x0014AFDC
		private Pawn GenerateIncappedAncient()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			HealthUtility.DamageUntilDowned(pawn);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0014CC60 File Offset: 0x0014B060
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

		// Token: 0x060026CE RID: 9934 RVA: 0x0014CCF8 File Offset: 0x0014B0F8
		private Pawn GenerateAngryAncient()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncientsHostile, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, true, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0014CD74 File Offset: 0x0014B174
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

		// Token: 0x060026D0 RID: 9936 RVA: 0x0014CE44 File Offset: 0x0014B244
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

		// Token: 0x060026D1 RID: 9937 RVA: 0x0014CEAC File Offset: 0x0014B2AC
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

		// Token: 0x060026D2 RID: 9938 RVA: 0x0014CF5C File Offset: 0x0014B35C
		private void MakeIntoContainer(ThingOwner container, ThingDef def, int count)
		{
			if (count > 0)
			{
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = count;
				container.TryAdd(thing, true);
			}
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0014CF90 File Offset: 0x0014B390
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
	}
}
