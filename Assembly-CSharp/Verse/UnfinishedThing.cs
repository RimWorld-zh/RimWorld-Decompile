using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFE RID: 3582
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x04003546 RID: 13638
		private Pawn creatorInt;

		// Token: 0x04003547 RID: 13639
		private string creatorName = "ErrorCreatorName";

		// Token: 0x04003548 RID: 13640
		private RecipeDef recipeInt;

		// Token: 0x04003549 RID: 13641
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x0400354A RID: 13642
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x0400354B RID: 13643
		public float workLeft = -10000f;

		// Token: 0x0400354C RID: 13644
		private const float CancelIngredientRecoveryFraction = 0.75f;

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x0600511C RID: 20764 RVA: 0x0029B1D4 File Offset: 0x002995D4
		// (set) Token: 0x0600511D RID: 20765 RVA: 0x0029B1EF File Offset: 0x002995EF
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

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x0600511E RID: 20766 RVA: 0x0029B21C File Offset: 0x0029961C
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600511F RID: 20767 RVA: 0x0029B238 File Offset: 0x00299638
		// (set) Token: 0x06005120 RID: 20768 RVA: 0x0029B288 File Offset: 0x00299688
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

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06005121 RID: 20769 RVA: 0x0029B2F8 File Offset: 0x002996F8
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

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06005122 RID: 20770 RVA: 0x0029B34C File Offset: 0x0029974C
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

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06005123 RID: 20771 RVA: 0x0029B3F4 File Offset: 0x002997F4
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

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06005124 RID: 20772 RVA: 0x0029B430 File Offset: 0x00299830
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

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x0029B46C File Offset: 0x0029986C
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x0029B490 File Offset: 0x00299890
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

		// Token: 0x06005127 RID: 20775 RVA: 0x0029B544 File Offset: 0x00299944
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

		// Token: 0x06005128 RID: 20776 RVA: 0x0029B5EC File Offset: 0x002999EC
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

		// Token: 0x06005129 RID: 20777 RVA: 0x0029B618 File Offset: 0x00299A18
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

		// Token: 0x0600512A RID: 20778 RVA: 0x0029B6BB File Offset: 0x00299ABB
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x0029B6E8 File Offset: 0x00299AE8
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
