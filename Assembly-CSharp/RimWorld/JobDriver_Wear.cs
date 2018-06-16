using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200009F RID: 159
	public class JobDriver_Wear : JobDriver
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0002F38C File Offset: 0x0002D78C
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0002F3BA File Offset: 0x0002D7BA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0002F3E8 File Offset: 0x0002D7E8
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0002F41C File Offset: 0x0002D81C
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
				}
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0002F4D4 File Offset: 0x0002D8D4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil prepare = new Toil();
			prepare.tickAction = delegate()
			{
				this.unequipBuffer++;
				this.TryUnequipSomething();
			};
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			prepare.defaultCompleteMode = ToilCompleteMode.Delay;
			prepare.defaultDuration = this.duration;
			yield return prepare;
			yield return Toils_General.Do(delegate
			{
				Apparel apparel = this.Apparel;
				this.pawn.apparel.Wear(apparel, true);
				if (this.pawn.outfits != null && this.job.playerForced)
				{
					this.pawn.outfits.forcedHandler.SetForced(apparel, true);
				}
			});
			yield break;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0002F500 File Offset: 0x0002D900
		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
					if (this.unequipBuffer >= num)
					{
						bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
						Apparel apparel2;
						if (!this.pawn.apparel.TryDrop(wornApparel[i], out apparel2, this.pawn.PositionHeld, forbid))
						{
							Log.Error(this.pawn + " could not drop " + wornApparel[i].ToStringSafe<Apparel>(), false);
							base.EndJobWith(JobCondition.Errored);
						}
					}
					break;
				}
			}
		}

		// Token: 0x0400026B RID: 619
		private int duration;

		// Token: 0x0400026C RID: 620
		private int unequipBuffer;

		// Token: 0x0400026D RID: 621
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
