using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Bill_Medical : Bill
	{
		private BodyPartRecord part = null;

		private int partIndex = -1;

		public ThingDef consumedInitialMedicineDef;

		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		public override bool CompletableEver
		{
			get
			{
				return (byte)((!base.recipe.targetsBodyPart || base.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, base.recipe).Contains(this.part)) ? 1 : 0) != 0;
			}
		}

		public BodyPartRecord Part
		{
			get
			{
				if (this.part == null && this.partIndex >= 0)
				{
					this.part = this.GiverPawn.RaceProps.body.GetPartAtIndex(this.partIndex);
				}
				return this.part;
			}
			set
			{
				if (base.billStack == null)
				{
					Log.Error("Can only set Bill_Medical.Part after the bill has been added to a pawn's bill stack.");
				}
				else
				{
					if (value != null)
					{
						this.partIndex = this.GiverPawn.RaceProps.body.GetIndexOfPart(value);
					}
					else
					{
						this.partIndex = -1;
					}
					this.part = value;
				}
			}
		}

		private Pawn GiverPawn
		{
			get
			{
				Pawn pawn = base.billStack.billGiver as Pawn;
				Corpse corpse = base.billStack.billGiver as Corpse;
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

		public override string Label
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.recipe.Worker.GetLabelWhenUsedOn(this.GiverPawn, this.part));
				if (this.Part != null && !base.recipe.hideBodyPartNames)
				{
					stringBuilder.Append(" (" + this.Part.def.label + ")");
				}
				return stringBuilder.ToString();
			}
		}

		public Bill_Medical()
		{
		}

		public Bill_Medical(RecipeDef recipe) : base(recipe)
		{
		}

		public override bool ShouldDoNow()
		{
			return (byte)((!base.suspended) ? 1 : 0) != 0;
		}

		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			if (this.CompletableEver)
			{
				Pawn giverPawn = this.GiverPawn;
				base.recipe.Worker.ApplyOnPawn(giverPawn, this.Part, billDoer, ingredients, this);
				if (giverPawn.RaceProps.IsFlesh)
				{
					giverPawn.records.Increment(RecordDefOf.OperationsReceived);
					billDoer.records.Increment(RecordDefOf.OperationsPerformed);
				}
			}
			base.billStack.Delete(this);
		}

		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			this.consumedInitialMedicineDef = null;
			if (!this.GiverPawn.Dead && base.recipe.anesthetize && HealthUtility.TryAnesthetize(this.GiverPawn))
			{
				List<ThingStackPartClass> placedThings = billDoer.CurJob.placedThings;
				int num = 0;
				while (true)
				{
					if (num < placedThings.Count)
					{
						if (!(placedThings[num].thing is Medicine))
						{
							num++;
							continue;
						}
						break;
					}
					return;
				}
				base.recipe.Worker.ConsumeIngredient(placedThings[num].thing.SplitOff(1), base.recipe, billDoer.MapHeld);
				placedThings[num].Count--;
				this.consumedInitialMedicineDef = placedThings[num].thing.def;
				if (!placedThings[num].thing.Destroyed && placedThings[num].Count > 0)
					return;
				placedThings.RemoveAt(num);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.partIndex, "partIndex", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.consumedInitialMedicineDef, "consumedInitialMedicineDef");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.partIndex < 0)
				{
					this.part = null;
				}
				else
				{
					this.part = this.GiverPawn.RaceProps.body.GetPartAtIndex(this.partIndex);
				}
			}
		}
	}
}
