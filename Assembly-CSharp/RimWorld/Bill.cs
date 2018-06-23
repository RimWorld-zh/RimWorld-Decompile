using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000025 RID: 37
	public abstract class Bill : IExposable, ILoadReferenceable
	{
		// Token: 0x0400018B RID: 395
		[Unsaved]
		public BillStack billStack = null;

		// Token: 0x0400018C RID: 396
		private int loadID = -1;

		// Token: 0x0400018D RID: 397
		public RecipeDef recipe;

		// Token: 0x0400018E RID: 398
		public bool suspended = false;

		// Token: 0x0400018F RID: 399
		public ThingFilter ingredientFilter;

		// Token: 0x04000190 RID: 400
		public float ingredientSearchRadius = 999f;

		// Token: 0x04000191 RID: 401
		public IntRange allowedSkillRange = new IntRange(0, 20);

		// Token: 0x04000192 RID: 402
		public Pawn pawnRestriction = null;

		// Token: 0x04000193 RID: 403
		public bool deleted = false;

		// Token: 0x04000194 RID: 404
		public int lastIngredientSearchFailTicks = -99999;

		// Token: 0x04000195 RID: 405
		public const int MaxIngredientSearchRadius = 999;

		// Token: 0x04000196 RID: 406
		public const float ButSize = 24f;

		// Token: 0x04000197 RID: 407
		private const float InterfaceBaseHeight = 53f;

		// Token: 0x04000198 RID: 408
		private const float InterfaceStatusLineHeight = 17f;

		// Token: 0x0600013C RID: 316 RVA: 0x0000D040 File Offset: 0x0000B440
		public Bill()
		{
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000D09C File Offset: 0x0000B49C
		public Bill(RecipeDef recipe)
		{
			this.recipe = recipe;
			this.ingredientFilter = new ThingFilter();
			this.ingredientFilter.CopyAllowancesFrom(recipe.defaultIngredientFilter);
			this.InitializeAfterClone();
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000D120 File Offset: 0x0000B520
		public Map Map
		{
			get
			{
				return this.billStack.billGiver.Map;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000D148 File Offset: 0x0000B548
		public virtual string Label
		{
			get
			{
				return this.recipe.label;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000D168 File Offset: 0x0000B568
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000D188 File Offset: 0x0000B588
		public virtual bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000D1A0 File Offset: 0x0000B5A0
		public virtual bool CompletableEver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000D1B8 File Offset: 0x0000B5B8
		protected virtual string StatusString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000D1D0 File Offset: 0x0000B5D0
		protected virtual float StatusLineMinHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000D1EC File Offset: 0x0000B5EC
		protected virtual bool CanCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000D204 File Offset: 0x0000B604
		public bool DeletedOrDereferenced
		{
			get
			{
				bool result;
				if (this.deleted)
				{
					result = true;
				}
				else
				{
					Thing thing = this.billStack.billGiver as Thing;
					result = (thing != null && thing.Destroyed);
				}
				return result;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000D255 File Offset: 0x0000B655
		public void InitializeAfterClone()
		{
			this.loadID = Find.UniqueIDsManager.GetNextBillID();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000D268 File Offset: 0x0000B668
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipe, "recipe");
			Scribe_Values.Look<bool>(ref this.suspended, "suspended", false, false);
			Scribe_Values.Look<float>(ref this.ingredientSearchRadius, "ingredientSearchRadius", 999f, false);
			Scribe_Values.Look<IntRange>(ref this.allowedSkillRange, "allowedSkillRange", default(IntRange), false);
			Scribe_References.Look<Pawn>(ref this.pawnRestriction, "pawnRestriction", false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.recipe.fixedIngredientFilter != null)
				{
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
					{
						if (!this.recipe.fixedIngredientFilter.Allows(thingDef))
						{
							this.ingredientFilter.SetAllow(thingDef, false);
						}
					}
				}
			}
			Scribe_Deep.Look<ThingFilter>(ref this.ingredientFilter, "ingredientFilter", new object[0]);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000D38C File Offset: 0x0000B78C
		public virtual bool PawnAllowedToStartAnew(Pawn p)
		{
			bool result;
			if (this.pawnRestriction != null)
			{
				result = (this.pawnRestriction == p);
			}
			else
			{
				if (this.recipe.workSkill != null)
				{
					int level = p.skills.GetSkill(this.recipe.workSkill).Level;
					if (level < this.allowedSkillRange.min)
					{
						JobFailReason.Is("UnderAllowedSkill".Translate(new object[]
						{
							this.allowedSkillRange.min
						}), this.Label);
						return false;
					}
					if (level > this.allowedSkillRange.max)
					{
						JobFailReason.Is("AboveAllowedSkill".Translate(new object[]
						{
							this.allowedSkillRange.max
						}), this.Label);
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000D479 File Offset: 0x0000B879
		public virtual void Notify_PawnDidWork(Pawn p)
		{
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000D47C File Offset: 0x0000B87C
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x0600014C RID: 332
		public abstract bool ShouldDoNow();

		// Token: 0x0600014D RID: 333 RVA: 0x0000D47F File Offset: 0x0000B87F
		public virtual void Notify_DoBillStarted(Pawn billDoer)
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000D484 File Offset: 0x0000B884
		protected virtual void DoConfigInterface(Rect rect, Color baseColor)
		{
			rect.yMin += 29f;
			float y = rect.center.y;
			float num = rect.xMax - (rect.yMax - y);
			Widgets.InfoCardButton(num - 12f, y - 12f, this.recipe);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000D4E1 File Offset: 0x0000B8E1
		public virtual void DoStatusLineInterface(Rect rect)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000D4E4 File Offset: 0x0000B8E4
		public Rect DoInterface(float x, float y, float width, int index)
		{
			Rect rect = new Rect(x, y, width, 53f);
			float num = 0f;
			if (!this.StatusString.NullOrEmpty())
			{
				num = Mathf.Max(17f, this.StatusLineMinHeight);
			}
			rect.height += num;
			Color white = Color.white;
			if (!this.ShouldDoNow())
			{
				white = new Color(1f, 0.7f, 0.7f, 0.7f);
			}
			GUI.color = white;
			Text.Font = GameFont.Small;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 24f, 24f);
			if (this.billStack.IndexOf(this) > 0)
			{
				if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, white))
				{
					this.billStack.Reorder(this, -1);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegion(rect2, "ReorderBillUpTip".Translate());
			}
			if (this.billStack.IndexOf(this) < this.billStack.Count - 1)
			{
				Rect rect3 = new Rect(0f, 24f, 24f, 24f);
				if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, white))
				{
					this.billStack.Reorder(this, 1);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegion(rect3, "ReorderBillDownTip".Translate());
			}
			Rect rect4 = new Rect(28f, 0f, rect.width - 48f - 20f, rect.height + 5f);
			Widgets.Label(rect4, this.LabelCap);
			this.DoConfigInterface(rect.AtZero(), white);
			Rect rect5 = new Rect(rect.width - 24f, 0f, 24f, 24f);
			if (Widgets.ButtonImage(rect5, TexButton.DeleteX, white, white * GenUI.SubtleMouseoverColor))
			{
				this.billStack.Delete(this);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegion(rect5, "DeleteBillTip".Translate());
			Rect rect7;
			if (this.CanCopy)
			{
				Rect rect6 = new Rect(rect5);
				rect6.x -= rect6.width + 4f;
				if (Widgets.ButtonImageFitted(rect6, TexButton.Copy, white))
				{
					BillUtility.Clipboard = this.Clone();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegion(rect6, "CopyBillTip".Translate());
				rect7 = new Rect(rect6);
			}
			else
			{
				rect7 = new Rect(rect5);
			}
			rect7.x -= rect7.width + 4f;
			if (Widgets.ButtonImage(rect7, TexButton.Suspend, white))
			{
				this.suspended = !this.suspended;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegion(rect7, "SuspendBillTip".Translate());
			if (!this.StatusString.NullOrEmpty())
			{
				Text.Font = GameFont.Tiny;
				Rect rect8 = new Rect(24f, rect.height - num, rect.width - 24f, num);
				Widgets.Label(rect8, this.StatusString);
				this.DoStatusLineInterface(rect8);
			}
			GUI.EndGroup();
			if (this.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect9 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect9, TexUI.GrayTextBG);
				Widgets.Label(rect9, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			return rect;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000D910 File Offset: 0x0000BD10
		public bool IsFixedOrAllowedIngredient(Thing thing)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(thing))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(thing) && this.ingredientFilter.Allows(thing);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000D9A4 File Offset: 0x0000BDA4
		public bool IsFixedOrAllowedIngredient(ThingDef def)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(def))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(def) && this.ingredientFilter.Allows(def);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000DA38 File Offset: 0x0000BE38
		public static void CreateNoPawnsWithSkillDialog(RecipeDef recipe)
		{
			string text = "RecipeRequiresSkills".Translate(new object[]
			{
				recipe.LabelCap
			});
			text += "\n\n";
			text += recipe.MinSkillString;
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000DA94 File Offset: 0x0000BE94
		public virtual BillStoreModeDef GetStoreMode()
		{
			return BillStoreModeDefOf.BestStockpile;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000DAB0 File Offset: 0x0000BEB0
		public virtual Zone_Stockpile GetStoreZone()
		{
			return null;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000DAC6 File Offset: 0x0000BEC6
		public virtual void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			Log.ErrorOnce("Tried to set store mode of a non-production bill", 27190980, false);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000DADC File Offset: 0x0000BEDC
		public virtual Bill Clone()
		{
			Bill bill = (Bill)Activator.CreateInstance(base.GetType());
			bill.recipe = this.recipe;
			bill.suspended = this.suspended;
			bill.ingredientFilter = new ThingFilter();
			bill.ingredientFilter.CopyAllowancesFrom(this.ingredientFilter);
			bill.ingredientSearchRadius = this.ingredientSearchRadius;
			bill.allowedSkillRange = this.allowedSkillRange;
			bill.pawnRestriction = this.pawnRestriction;
			return bill;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000DB5C File Offset: 0x0000BF5C
		public virtual void ValidateSettings()
		{
			if (this.pawnRestriction != null)
			{
				if (this.pawnRestriction.Dead || this.pawnRestriction.Faction != Faction.OfPlayer || this.pawnRestriction.IsKidnapped())
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationPawnUnavailable".Translate(new object[]
						{
							this.pawnRestriction.LabelShortCap,
							this.LabelCap,
							this.billStack.billGiver.LabelShort.CapitalizeFirst()
						}), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.pawnRestriction = null;
				}
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000DC24 File Offset: 0x0000C024
		public string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"Bill_",
				this.recipe.defName,
				"_",
				this.loadID
			});
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000DC70 File Offset: 0x0000C070
		public override string ToString()
		{
			return this.GetUniqueLoadID();
		}
	}
}
