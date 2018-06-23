using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFB RID: 3579
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x0400353F RID: 13631
		private Pawn creatorInt;

		// Token: 0x04003540 RID: 13632
		private string creatorName = "ErrorCreatorName";

		// Token: 0x04003541 RID: 13633
		private RecipeDef recipeInt;

		// Token: 0x04003542 RID: 13634
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x04003543 RID: 13635
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x04003544 RID: 13636
		public float workLeft = -10000f;

		// Token: 0x04003545 RID: 13637
		private const float CancelIngredientRecoveryFraction = 0.75f;

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06005118 RID: 20760 RVA: 0x0029ADC8 File Offset: 0x002991C8
		// (set) Token: 0x06005119 RID: 20761 RVA: 0x0029ADE3 File Offset: 0x002991E3
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
					Log.Error("Cannot set creator to null.", false);
				}
				else
				{
					this.creatorInt = value;
					this.creatorName = value.LabelShort;
				}
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600511A RID: 20762 RVA: 0x0029AE10 File Offset: 0x00299210
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x0600511B RID: 20763 RVA: 0x0029AE2C File Offset: 0x0029922C
		// (set) Token: 0x0600511C RID: 20764 RVA: 0x0029AE7C File Offset: 0x0029927C
		public Bill_ProductionWithUft BoundBill
		{
			get
			{
				if (this.boundBillInt != null)
				{
					if (this.boundBillInt.DeletedOrDereferenced || this.boundBillInt.BoundUft != this)
					{
						this.boundBillInt = null;
					}
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

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x0600511D RID: 20765 RVA: 0x0029AEEC File Offset: 0x002992EC
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
					if (thing.Destroyed)
					{
						result = null;
					}
					else
					{
						result = thing;
					}
				}
				return result;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x0600511E RID: 20766 RVA: 0x0029AF40 File Offset: 0x00299340
		public override string LabelNoCount
		{
			get
			{
				string result;
				if (this.Recipe == null)
				{
					result = base.LabelNoCount;
				}
				else if (base.Stuff == null)
				{
					result = "UnfinishedItem".Translate(new object[]
					{
						this.Recipe.products[0].thingDef.label
					});
				}
				else
				{
					result = "UnfinishedItemWithStuff".Translate(new object[]
					{
						base.Stuff.LabelAsStuff,
						this.Recipe.products[0].thingDef.label
					});
				}
				return result;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x0600511F RID: 20767 RVA: 0x0029AFE8 File Offset: 0x002993E8
		public override string DescriptionDetailed
		{
			get
			{
				string result;
				if (this.Recipe == null)
				{
					result = base.LabelNoCount;
				}
				else
				{
					result = this.Recipe.ProducedThingDef.DescriptionDetailed;
				}
				return result;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06005120 RID: 20768 RVA: 0x0029B024 File Offset: 0x00299424
		public override string DescriptionFlavor
		{
			get
			{
				string result;
				if (this.Recipe == null)
				{
					result = base.LabelNoCount;
				}
				else
				{
					result = this.Recipe.ProducedThingDef.description;
				}
				return result;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06005121 RID: 20769 RVA: 0x0029B060 File Offset: 0x00299460
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x0029B084 File Offset: 0x00299484
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.boundBillInt != null && this.boundBillInt.DeletedOrDereferenced)
				{
					this.boundBillInt = null;
				}
			}
			Scribe_References.Look<Pawn>(ref this.creatorInt, "creator", false);
			Scribe_Values.Look<string>(ref this.creatorName, "creatorName", null, false);
			Scribe_References.Look<Bill_ProductionWithUft>(ref this.boundBillInt, "bill", false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipeInt, "recipe");
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.ingredients, "ingredients", LookMode.Deep, new object[0]);
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x0029B138 File Offset: 0x00299538
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Cancel)
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					int num = GenMath.RoundRandom((float)this.ingredients[i].stackCount * 0.75f);
					if (num > 0)
					{
						this.ingredients[i].stackCount = num;
						GenPlace.TryPlaceThing(this.ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null, null);
					}
				}
				this.ingredients.Clear();
			}
			base.Destroy(mode);
			this.BoundBill = null;
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x0029B1E0 File Offset: 0x002995E0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			yield return new Command_Action
			{
				defaultLabel = "CommandCancelConstructionLabel".Translate(),
				defaultDesc = "CommandCancelConstructionDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true),
				hotKey = KeyBindingDefOf.Designator_Cancel,
				action = delegate()
				{
					this.Destroy(DestroyMode.Cancel);
				}
			};
			yield break;
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x0029B20C File Offset: 0x0029960C
		public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
		{
			if (this.Recipe.AllRecipeUsers.Contains(workTable.def))
			{
				IBillGiver billGiver = (IBillGiver)workTable;
				for (int i = 0; i < billGiver.BillStack.Count; i++)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = billGiver.BillStack[i] as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null)
					{
						if (bill_ProductionWithUft.ShouldDoNow())
						{
							if (bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == this.Recipe)
							{
								return bill_ProductionWithUft;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x0029B2AF File Offset: 0x002996AF
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x0029B2DC File Offset: 0x002996DC
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text = text + "Author".Translate() + ": " + this.creatorName;
			string text2 = text;
			return string.Concat(new string[]
			{
				text2,
				"\n",
				"WorkLeft".Translate(),
				": ",
				this.workLeft.ToStringWorkAmount()
			});
		}
	}
}
