using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000096 RID: 150
	public class JobDriver_FoodFeedPatient : JobDriver
	{
		// Token: 0x04000259 RID: 601
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x0400025A RID: 602
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x0400025B RID: 603
		private const float FeedDurationMultiplier = 1.5f;

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0002AF18 File Offset: 0x00029318
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0002AF40 File Offset: 0x00029340
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0002AF6C File Offset: 0x0002936C
		public override string GetReport()
		{
			string result;
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				result = this.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", this.Deliveree.LabelShort);
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0002AFF0 File Offset: 0x000293F0
		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (this.pawn.inventory == null || !this.pawn.inventory.Contains(base.TargetThingA)))
				{
					if (!this.pawn.Reserve(this.Food, this.job, 1, -1, null))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0002B09C File Offset: 0x0002949C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(this.Deliveree));
			if (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (base.TargetThingA is Building_NutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
			yield break;
		}
	}
}
