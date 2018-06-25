using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000461 RID: 1121
	public class Bill_Medical : Bill
	{
		// Token: 0x04000BF0 RID: 3056
		private BodyPartRecord part;

		// Token: 0x04000BF1 RID: 3057
		public ThingDef consumedInitialMedicineDef;

		// Token: 0x04000BF2 RID: 3058
		public int temp_partIndexToSetLater;

		// Token: 0x060013A4 RID: 5028 RVA: 0x000A99EB File Offset: 0x000A7DEB
		public Bill_Medical()
		{
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x000A99F4 File Offset: 0x000A7DF4
		public Bill_Medical(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x000A9A00 File Offset: 0x000A7E00
		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x000A9A18 File Offset: 0x000A7E18
		protected override bool CanCopy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x000A9A30 File Offset: 0x000A7E30
		public override bool CompletableEver
		{
			get
			{
				return !this.recipe.targetsBodyPart || this.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, this.recipe).Contains(this.part);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x000A9A8C File Offset: 0x000A7E8C
		// (set) Token: 0x060013AA RID: 5034 RVA: 0x000A9AA8 File Offset: 0x000A7EA8
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.billStack == null && this.part != null)
				{
					Log.Error("Can only set Bill_Medical.Part after the bill has been added to a pawn's bill stack.", false);
				}
				else if (UnityData.isDebugBuild && this.part != null && !this.GiverPawn.RaceProps.body.AllParts.Contains(this.part))
				{
					Log.Error("Cannot set BodyPartRecord which doesn't belong to the pawn " + this.GiverPawn.ToStringSafe<Pawn>(), false);
				}
				else
				{
					this.part = value;
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x000A9B40 File Offset: 0x000A7F40
		public Pawn GiverPawn
		{
			get
			{
				Pawn pawn = this.billStack.billGiver as Pawn;
				Corpse corpse = this.billStack.billGiver as Corpse;
				if (corpse != null)
				{
					pawn = corpse.InnerPawn;
				}
				if (pawn == null)
				{
					throw new InvalidOperationException("Medical bill on non-pawn.");
				}
				return pawn;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x000A9B98 File Offset: 0x000A7F98
		public override string Label
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.recipe.Worker.GetLabelWhenUsedOn(this.GiverPawn, this.part));
				if (this.Part != null && !this.recipe.hideBodyPartNames)
				{
					stringBuilder.Append(" (" + this.Part.Label + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x000A9C18 File Offset: 0x000A8018
		public override bool ShouldDoNow()
		{
			return !this.suspended;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x000A9C40 File Offset: 0x000A8040
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			if (this.CompletableEver)
			{
				Pawn giverPawn = this.GiverPawn;
				this.recipe.Worker.ApplyOnPawn(giverPawn, this.Part, billDoer, ingredients, this);
				if (giverPawn.RaceProps.IsFlesh)
				{
					giverPawn.records.Increment(RecordDefOf.OperationsReceived);
					billDoer.records.Increment(RecordDefOf.OperationsPerformed);
				}
			}
			this.billStack.Delete(this);
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x000A9CC4 File Offset: 0x000A80C4
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			this.consumedInitialMedicineDef = null;
			if (!this.GiverPawn.Dead && this.recipe.anesthetize)
			{
				if (HealthUtility.TryAnesthetize(this.GiverPawn))
				{
					List<ThingCountClass> placedThings = billDoer.CurJob.placedThings;
					for (int i = 0; i < placedThings.Count; i++)
					{
						if (placedThings[i].thing is Medicine)
						{
							this.recipe.Worker.ConsumeIngredient(placedThings[i].thing.SplitOff(1), this.recipe, billDoer.MapHeld);
							placedThings[i].Count--;
							this.consumedInitialMedicineDef = placedThings[i].thing.def;
							if (placedThings[i].thing.Destroyed || placedThings[i].Count <= 0)
							{
								placedThings.RemoveAt(i);
							}
							break;
						}
					}
				}
			}
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x000A9DE0 File Offset: 0x000A81E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Defs.Look<ThingDef>(ref this.consumedInitialMedicineDef, "consumedInitialMedicineDef");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.BillMedicalLoadingVars(this);
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				BackCompatibility.BillMedicalResolvingCrossRefs(this);
			}
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x000A9E38 File Offset: 0x000A8238
		public override Bill Clone()
		{
			Bill_Medical bill_Medical = (Bill_Medical)base.Clone();
			bill_Medical.part = this.part;
			bill_Medical.consumedInitialMedicineDef = this.consumedInitialMedicineDef;
			return bill_Medical;
		}
	}
}
