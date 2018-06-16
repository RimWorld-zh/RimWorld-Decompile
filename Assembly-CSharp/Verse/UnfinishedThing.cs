using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFF RID: 3583
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06005106 RID: 20742 RVA: 0x0029980C File Offset: 0x00297C0C
		// (set) Token: 0x06005107 RID: 20743 RVA: 0x00299827 File Offset: 0x00297C27
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
		// (get) Token: 0x06005108 RID: 20744 RVA: 0x00299854 File Offset: 0x00297C54
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06005109 RID: 20745 RVA: 0x00299870 File Offset: 0x00297C70
		// (set) Token: 0x0600510A RID: 20746 RVA: 0x002998C0 File Offset: 0x00297CC0
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
		// (get) Token: 0x0600510B RID: 20747 RVA: 0x00299930 File Offset: 0x00297D30
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
		// (get) Token: 0x0600510C RID: 20748 RVA: 0x00299984 File Offset: 0x00297D84
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
		// (get) Token: 0x0600510D RID: 20749 RVA: 0x00299A2C File Offset: 0x00297E2C
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
		// (get) Token: 0x0600510E RID: 20750 RVA: 0x00299A68 File Offset: 0x00297E68
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
		// (get) Token: 0x0600510F RID: 20751 RVA: 0x00299AA4 File Offset: 0x00297EA4
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x00299AC8 File Offset: 0x00297EC8
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

		// Token: 0x06005111 RID: 20753 RVA: 0x00299B7C File Offset: 0x00297F7C
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

		// Token: 0x06005112 RID: 20754 RVA: 0x00299C24 File Offset: 0x00298024
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

		// Token: 0x06005113 RID: 20755 RVA: 0x00299C50 File Offset: 0x00298050
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

		// Token: 0x06005114 RID: 20756 RVA: 0x00299CF3 File Offset: 0x002980F3
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x00299D20 File Offset: 0x00298120
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

		// Token: 0x0400353A RID: 13626
		private Pawn creatorInt;

		// Token: 0x0400353B RID: 13627
		private string creatorName = "ErrorCreatorName";

		// Token: 0x0400353C RID: 13628
		private RecipeDef recipeInt;

		// Token: 0x0400353D RID: 13629
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x0400353E RID: 13630
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x0400353F RID: 13631
		public float workLeft = -10000f;

		// Token: 0x04003540 RID: 13632
		private const float CancelIngredientRecoveryFraction = 0.75f;
	}
}
