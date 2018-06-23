using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000847 RID: 2119
	public class ITab_Bills : ITab
	{
		// Token: 0x040019F5 RID: 6645
		private float viewHeight = 1000f;

		// Token: 0x040019F6 RID: 6646
		private Vector2 scrollPosition = default(Vector2);

		// Token: 0x040019F7 RID: 6647
		private Bill mouseoverBill;

		// Token: 0x040019F8 RID: 6648
		private static readonly Vector2 WinSize = new Vector2(420f, 480f);

		// Token: 0x040019F9 RID: 6649
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteX = 48f;

		// Token: 0x040019FA RID: 6650
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteY = 3f;

		// Token: 0x040019FB RID: 6651
		[TweakValue("Interface", 0f, 32f)]
		private static float PasteSize = 24f;

		// Token: 0x06003002 RID: 12290 RVA: 0x001A186C File Offset: 0x0019FC6C
		public ITab_Bills()
		{
			this.size = ITab_Bills.WinSize;
			this.labelKey = "TabBills";
			this.tutorTag = "Bills";
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x001A18BC File Offset: 0x0019FCBC
		protected Building_WorkTable SelTable
		{
			get
			{
				return (Building_WorkTable)base.SelThing;
			}
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x001A18DC File Offset: 0x0019FCDC
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BillsTab, KnowledgeAmount.FrameDisplayed);
			Rect rect = new Rect(ITab_Bills.WinSize.x - ITab_Bills.PasteX, ITab_Bills.PasteY, ITab_Bills.PasteSize, ITab_Bills.PasteSize);
			if (BillUtility.Clipboard == null)
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				TooltipHandler.TipRegion(rect, "PasteBillTip".Translate());
			}
			else if (!this.SelTable.def.AllRecipes.Contains(BillUtility.Clipboard.recipe) || !BillUtility.Clipboard.recipe.AvailableNow)
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				TooltipHandler.TipRegion(rect, "ClipboardBillNotAvailableHere".Translate());
			}
			else if (this.SelTable.billStack.Count >= 15)
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				TooltipHandler.TipRegion(rect, "PasteBillTip".Translate() + " (" + "PasteBillTip_LimitReached".Translate() + ")");
			}
			else
			{
				if (Widgets.ButtonImageFitted(rect, TexButton.Paste, Color.white))
				{
					Bill bill = BillUtility.Clipboard.Clone();
					bill.InitializeAfterClone();
					this.SelTable.billStack.AddBill(bill);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegion(rect, "PasteBillTip".Translate());
			}
			Rect rect2 = new Rect(0f, 0f, ITab_Bills.WinSize.x, ITab_Bills.WinSize.y).ContractedBy(10f);
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				for (int i = 0; i < this.SelTable.def.AllRecipes.Count; i++)
				{
					if (this.SelTable.def.AllRecipes[i].AvailableNow)
					{
						RecipeDef recipe = this.SelTable.def.AllRecipes[i];
						list.Add(new FloatMenuOption(recipe.LabelCap, delegate()
						{
							if (!this.SelTable.Map.mapPawns.FreeColonists.Any((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)))
							{
								Bill.CreateNoPawnsWithSkillDialog(recipe);
							}
							Bill bill2 = recipe.MakeNewBill();
							this.SelTable.billStack.AddBill(bill2);
							if (recipe.conceptLearned != null)
							{
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
							}
							if (TutorSystem.TutorialMode)
							{
								TutorSystem.Notify_Event("AddBill-" + recipe.LabelCap);
							}
						}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe), null));
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				return list;
			};
			this.mouseoverBill = this.SelTable.billStack.DoListing(rect2, recipeOptionsMaker, ref this.scrollPosition, ref this.viewHeight);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x001A1B16 File Offset: 0x0019FF16
		public override void TabUpdate()
		{
			if (this.mouseoverBill != null)
			{
				this.mouseoverBill.TryDrawIngredientSearchRadiusOnMap(this.SelTable.Position);
				this.mouseoverBill = null;
			}
		}
	}
}
