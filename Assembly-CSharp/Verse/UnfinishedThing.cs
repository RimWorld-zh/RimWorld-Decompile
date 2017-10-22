using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class UnfinishedThing : ThingWithComps
	{
		private Pawn creatorInt;

		private string creatorName = "ErrorCreatorName";

		private RecipeDef recipeInt;

		public List<Thing> ingredients = new List<Thing>();

		private Bill_ProductionWithUft boundBillInt;

		public float workLeft = -10000f;

		private const float CancelIngredientRecoveryFraction = 0.75f;

		public Pawn Creator
		{
			get
			{
				return this.creatorInt;
			}
			set
			{
				if (value == null)
				{
					Log.Error("Cannot set creator to null.");
				}
				else
				{
					this.creatorInt = value;
					this.creatorName = value.NameStringShort;
				}
			}
		}

		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		public Bill_ProductionWithUft BoundBill
		{
			get
			{
				if (this.boundBillInt != null && (this.boundBillInt.DeletedOrDereferenced || this.boundBillInt.BoundUft != this))
				{
					this.boundBillInt = null;
				}
				return this.boundBillInt;
			}
			set
			{
				if (value != this.boundBillInt)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = this.boundBillInt;
					this.boundBillInt = value;
					if (bill_ProductionWithUft != null && bill_ProductionWithUft.BoundUft == this)
					{
						bill_ProductionWithUft.SetBoundUft(null, false);
					}
					if (value != null)
					{
						this.recipeInt = value.recipe;
						if (value.BoundUft != this)
						{
							value.SetBoundUft(this, false);
						}
					}
				}
			}
		}

		public Thing BoundWorkTable
		{
			get
			{
				Thing result;
				if (this.BoundBill == null)
				{
					result = null;
				}
				else
				{
					IBillGiver billGiver = this.BoundBill.billStack.billGiver;
					Thing thing = billGiver as Thing;
					result = ((!thing.Destroyed) ? thing : null);
				}
				return result;
			}
		}

		public override string LabelNoCount
		{
			get
			{
				return (this.Recipe != null) ? ((base.Stuff != null) ? "UnfinishedItemWithStuff".Translate(base.Stuff.LabelAsStuff, this.Recipe.products[0].thingDef.label) : "UnfinishedItem".Translate(this.Recipe.products[0].thingDef.label)) : base.LabelNoCount;
			}
		}

		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000.0;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.boundBillInt != null && this.boundBillInt.DeletedOrDereferenced)
			{
				this.boundBillInt = null;
			}
			Scribe_References.Look<Pawn>(ref this.creatorInt, "creator", false);
			Scribe_Values.Look<string>(ref this.creatorName, "creatorName", (string)null, false);
			Scribe_References.Look<Bill_ProductionWithUft>(ref this.boundBillInt, "bill", false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipeInt, "recipe");
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.ingredients, "ingredients", LookMode.Deep, new object[0]);
		}

		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Cancel)
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					int num = GenMath.RoundRandom((float)((float)this.ingredients[i].stackCount * 0.75));
					if (num > 0)
					{
						this.ingredients[i].stackCount = num;
						GenPlace.TryPlaceThing(this.ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null);
					}
				}
				this.ingredients.Clear();
			}
			base.Destroy(mode);
			this.BoundBill = null;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "CommandCancelConstructionLabel".Translate(),
				defaultDesc = "CommandCancelConstructionDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true),
				hotKey = KeyBindingDefOf.DesignatorCancel,
				action = (Action)delegate
				{
					((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_0119: stateMachine*/)._0024this.Destroy(DestroyMode.Cancel);
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0153:
			/*Error near IL_0154: Unexpected return in MoveNext()*/;
		}

		public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
		{
			Bill_ProductionWithUft bill_ProductionWithUft;
			if (this.Recipe.AllRecipeUsers.Contains(workTable.def))
			{
				IBillGiver billGiver = (IBillGiver)workTable;
				for (int i = 0; i < billGiver.BillStack.Count; i++)
				{
					bill_ProductionWithUft = (billGiver.BillStack[i] as Bill_ProductionWithUft);
					if (bill_ProductionWithUft != null && bill_ProductionWithUft.ShouldDoNow() && bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == this.Recipe)
						goto IL_0070;
				}
			}
			Bill_ProductionWithUft result = null;
			goto IL_0095;
			IL_0095:
			return result;
			IL_0070:
			result = bill_ProductionWithUft;
			goto IL_0095;
		}

		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			string text2;
			text = (text2 = text + "Author".Translate() + ": " + this.creatorName);
			return text2 + "\n" + "WorkLeft".Translate() + ": " + this.workLeft.ToStringWorkAmount();
		}
	}
}
