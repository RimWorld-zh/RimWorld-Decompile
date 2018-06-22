using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045F RID: 1119
	public class Bill_Medical : Bill
	{
		// Token: 0x060013A0 RID: 5024 RVA: 0x000A989B File Offset: 0x000A7C9B
		public Bill_Medical()
		{
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x000A98A4 File Offset: 0x000A7CA4
		public Bill_Medical(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x000A98B0 File Offset: 0x000A7CB0
		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x000A98C8 File Offset: 0x000A7CC8
		protected override bool CanCopy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x000A98E0 File Offset: 0x000A7CE0
		public override bool CompletableEver
		{
			get
			{
				return !this.recipe.targetsBodyPart || this.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, this.recipe).Contains(this.part);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x000A993C File Offset: 0x000A7D3C
		// (set) Token: 0x060013A6 RID: 5030 RVA: 0x000A9958 File Offset: 0x000A7D58
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
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x000A99F0 File Offset: 0x000A7DF0
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
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x000A9A48 File Offset: 0x000A7E48
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

		// Token: 0x060013A9 RID: 5033 RVA: 0x000A9AC8 File Offset: 0x000A7EC8
		public override bool ShouldDoNow()
		{
			return !this.suspended;
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x000A9AF0 File Offset: 0x000A7EF0
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

		// Token: 0x060013AB RID: 5035 RVA: 0x000A9B74 File Offset: 0x000A7F74
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

		// Token: 0x060013AC RID: 5036 RVA: 0x000A9C90 File Offset: 0x000A8090
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

		// Token: 0x060013AD RID: 5037 RVA: 0x000A9CE8 File Offset: 0x000A80E8
		public override Bill Clone()
		{
			Bill_Medical bill_Medical = (Bill_Medical)base.Clone();
			bill_Medical.part = this.part;
			bill_Medical.consumedInitialMedicineDef = this.consumedInitialMedicineDef;
			return bill_Medical;
		}

		// Token: 0x04000BF0 RID: 3056
		private BodyPartRecord part;

		// Token: 0x04000BF1 RID: 3057
		public ThingDef consumedInitialMedicineDef;

		// Token: 0x04000BF2 RID: 3058
		public int temp_partIndexToSetLater;
	}
}
