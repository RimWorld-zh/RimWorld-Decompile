using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Dialog_BillConfig : Window
	{
		private IntVec3 billGiverPos;

		private Bill_Production bill;

		private Vector2 thingFilterScrollPosition;

		private string repeatCountEditBuffer;

		private string targetCountEditBuffer;

		private string unpauseCountEditBuffer;

		[TweakValue("Interface", 0f, 400f)]
		private static int RepeatModeSubdialogHeight = 300;

		[TweakValue("Interface", 0f, 400f)]
		private static int StoreModeSubdialogHeight = 30;

		[TweakValue("Interface", 0f, 400f)]
		private static int WorkerSelectionSubdialogHeight = 85;

		[TweakValue("Interface", 0f, 400f)]
		private static int IngredientRadiusSubdialogHeight = 50;

		[CompilerGenerated]
		private static Func<Bill_Production, Zone_Stockpile> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<BillStoreModeDef, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Bill_Production, Pawn> <>f__am$cache2;

		public Dialog_BillConfig(Bill_Production bill, IntVec3 billGiverPos)
		{
			this.billGiverPos = billGiverPos;
			this.bill = bill;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(800f, 610f);
			}
		}

		private void AdjustCount(int offset)
		{
			if (offset > 0)
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
			}
			else
			{
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
			}
			this.bill.repeatCount += offset;
			if (this.bill.repeatCount < 1)
			{
				this.bill.repeatCount = 1;
			}
		}

		public override void WindowUpdate()
		{
			this.bill.TryDrawIngredientSearchRadiusOnMap(this.billGiverPos);
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(0f, 0f, 400f, 50f);
			Widgets.Label(rect, this.bill.LabelCap);
			float width = (float)((int)((inRect.width - 34f) / 3f));
			Rect rect2 = new Rect(0f, 80f, width, inRect.height - 80f);
			Rect rect3 = new Rect(rect2.xMax + 17f, 50f, width, inRect.height - 50f - this.CloseButSize.y);
			Rect rect4 = new Rect(rect3.xMax + 17f, 50f, 0f, inRect.height - 50f - this.CloseButSize.y);
			rect4.xMax = inRect.xMax;
			Text.Font = GameFont.Small;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect3);
			Listing_Standard listing_Standard2 = listing_Standard.BeginSection((float)Dialog_BillConfig.RepeatModeSubdialogHeight);
			if (listing_Standard2.ButtonText(this.bill.repeatMode.LabelCap, null))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this.bill);
			}
			listing_Standard2.Gap(12f);
			if (this.bill.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				listing_Standard2.Label("RepeatCount".Translate(new object[]
				{
					this.bill.repeatCount
				}), -1f, null);
				listing_Standard2.IntEntry(ref this.bill.repeatCount, ref this.repeatCountEditBuffer, 1);
			}
			else if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				string text = "CurrentlyHave".Translate() + ": ";
				text += this.bill.recipe.WorkerCounter.CountProducts(this.bill);
				text += " / ";
				text += ((this.bill.targetCount >= 999999) ? "Infinite".Translate().ToLower() : this.bill.targetCount.ToString());
				string text2 = this.bill.recipe.WorkerCounter.ProductsDescription(this.bill);
				if (!text2.NullOrEmpty())
				{
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						"\n",
						"CountingProducts".Translate(),
						": ",
						text2
					});
				}
				listing_Standard2.Label(text, -1f, null);
				int targetCount = this.bill.targetCount;
				listing_Standard2.IntEntry(ref this.bill.targetCount, ref this.targetCountEditBuffer, this.bill.recipe.targetCountAdjustment);
				this.bill.unpauseWhenYouHave = Mathf.Max(0, this.bill.unpauseWhenYouHave + (this.bill.targetCount - targetCount));
				ThingDef producedThingDef = this.bill.recipe.ProducedThingDef;
				if (producedThingDef != null)
				{
					if (producedThingDef.IsWeapon || producedThingDef.IsApparel)
					{
						listing_Standard2.CheckboxLabeled("IncludeEquipped".Translate(), ref this.bill.includeEquipped, null);
					}
					if (producedThingDef.IsApparel && producedThingDef.apparel.careIfWornByCorpse)
					{
						listing_Standard2.CheckboxLabeled("IncludeTainted".Translate(), ref this.bill.includeTainted, null);
					}
					Widgets.Dropdown<Bill_Production, Zone_Stockpile>(listing_Standard2.GetRect(30f), this.bill, (Bill_Production b) => b.includeFromZone, (Bill_Production b) => this.GenerateStockpileInclusion(), (this.bill.includeFromZone != null) ? "IncludeSpecific".Translate(new object[]
					{
						this.bill.includeFromZone.label
					}) : "IncludeFromAll".Translate(), null, null, null, null, false);
					Widgets.FloatRange(listing_Standard2.GetRect(28f), 10, ref this.bill.hpRange, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
					if (producedThingDef.HasComp(typeof(CompQuality)))
					{
						Widgets.QualityRange(listing_Standard2.GetRect(28f), 2, ref this.bill.qualityRange);
					}
					if (producedThingDef.MadeFromStuff)
					{
						listing_Standard2.CheckboxLabeled("LimitToAllowedStuff".Translate(), ref this.bill.limitToAllowedStuff, null);
					}
				}
			}
			if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				listing_Standard2.Gap(12f);
				listing_Standard2.Gap(12f);
				listing_Standard2.CheckboxLabeled("PauseWhenSatisfied".Translate(), ref this.bill.pauseWhenSatisfied, null);
				if (this.bill.pauseWhenSatisfied)
				{
					listing_Standard2.Label("UnpauseWhenYouHave".Translate() + ": " + this.bill.unpauseWhenYouHave.ToString("F0"), -1f, null);
					listing_Standard2.IntEntry(ref this.bill.unpauseWhenYouHave, ref this.unpauseCountEditBuffer, this.bill.recipe.targetCountAdjustment);
					if (this.bill.unpauseWhenYouHave >= this.bill.targetCount)
					{
						this.bill.unpauseWhenYouHave = this.bill.targetCount - 1;
						this.unpauseCountEditBuffer = this.bill.unpauseWhenYouHave.ToStringCached();
					}
				}
			}
			listing_Standard.EndSection(listing_Standard2);
			listing_Standard.Gap(12f);
			Listing_Standard listing_Standard3 = listing_Standard.BeginSection((float)Dialog_BillConfig.StoreModeSubdialogHeight);
			string text4 = string.Format(this.bill.GetStoreMode().LabelCap, (this.bill.GetStoreZone() == null) ? "" : this.bill.GetStoreZone().SlotYielderLabel());
			if (this.bill.GetStoreZone() != null && !this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone()))
			{
				text4 += string.Format(" ({0})", "IncompatibleLower".Translate());
				Text.Font = GameFont.Tiny;
			}
			if (listing_Standard3.ButtonText(text4, null))
			{
				Text.Font = GameFont.Small;
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (BillStoreModeDef billStoreModeDef in from bsm in DefDatabase<BillStoreModeDef>.AllDefs
				orderby bsm.listOrder
				select bsm)
				{
					if (billStoreModeDef == BillStoreModeDefOf.SpecificStockpile)
					{
						List<SlotGroup> allGroupsListInPriorityOrder = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
						int count = allGroupsListInPriorityOrder.Count;
						for (int i = 0; i < count; i++)
						{
							SlotGroup group = allGroupsListInPriorityOrder[i];
							Zone_Stockpile zone_Stockpile = group.parent as Zone_Stockpile;
							if (zone_Stockpile != null)
							{
								if (!this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, zone_Stockpile))
								{
									list.Add(new FloatMenuOption(string.Format("{0} ({1})", string.Format(billStoreModeDef.LabelCap, group.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null));
								}
								else
								{
									list.Add(new FloatMenuOption(string.Format(billStoreModeDef.LabelCap, group.parent.SlotYielderLabel()), delegate()
									{
										this.bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, (Zone_Stockpile)group.parent);
									}, MenuOptionPriority.Default, null, null, 0f, null, null));
								}
							}
						}
					}
					else
					{
						BillStoreModeDef smLocal = billStoreModeDef;
						list.Add(new FloatMenuOption(smLocal.LabelCap, delegate()
						{
							this.bill.SetStoreMode(smLocal, null);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Text.Font = GameFont.Small;
			listing_Standard.EndSection(listing_Standard3);
			listing_Standard.Gap(12f);
			Listing_Standard listing_Standard4 = listing_Standard.BeginSection((float)Dialog_BillConfig.WorkerSelectionSubdialogHeight);
			Widgets.Dropdown<Bill_Production, Pawn>(listing_Standard4.GetRect(30f), this.bill, (Bill_Production b) => b.pawnRestriction, (Bill_Production b) => this.GeneratePawnRestrictionOptions(), (this.bill.pawnRestriction != null) ? this.bill.pawnRestriction.LabelShortCap : "AnyWorker".Translate(), null, null, null, null, false);
			if (this.bill.pawnRestriction == null && this.bill.recipe.workSkill != null)
			{
				listing_Standard4.Label("AllowedSkillRange".Translate(new object[]
				{
					this.bill.recipe.workSkill.label
				}), -1f, null);
				listing_Standard4.IntRange(ref this.bill.allowedSkillRange, 0, 20);
			}
			listing_Standard.EndSection(listing_Standard4);
			listing_Standard.End();
			Rect rect5 = rect4;
			rect5.yMin = rect5.yMax - (float)Dialog_BillConfig.IngredientRadiusSubdialogHeight;
			rect4.yMax = rect5.yMin - 17f;
			bool flag = this.bill.GetStoreZone() == null || this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone());
			ThingFilterUI.DoThingFilterConfigWindow(rect4, ref this.thingFilterScrollPosition, this.bill.ingredientFilter, this.bill.recipe.fixedIngredientFilter, 4, null, this.bill.recipe.forceHiddenSpecialFilters, this.bill.recipe.GetPremultipliedSmallIngredients(), this.bill.Map);
			bool flag2 = this.bill.GetStoreZone() == null || this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone());
			if (flag && !flag2)
			{
				Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(new object[]
				{
					this.bill.LabelCap,
					this.bill.billStack.billGiver.LabelShort.CapitalizeFirst(),
					this.bill.GetStoreZone().label
				}), this.bill.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
			}
			Listing_Standard listing_Standard5 = new Listing_Standard();
			listing_Standard5.Begin(rect5);
			listing_Standard5.Label("IngredientSearchRadius".Translate() + ": " + ((this.bill.ingredientSearchRadius != 999f) ? this.bill.ingredientSearchRadius.ToString("F0") : "Unlimited".Translate()), -1f, null);
			this.bill.ingredientSearchRadius = listing_Standard5.Slider(this.bill.ingredientSearchRadius, 3f, 100f);
			if (this.bill.ingredientSearchRadius >= 100f)
			{
				this.bill.ingredientSearchRadius = 999f;
			}
			listing_Standard5.End();
			Listing_Standard listing_Standard6 = new Listing_Standard();
			listing_Standard6.Begin(rect2);
			if (this.bill.suspended)
			{
				if (listing_Standard6.ButtonText("Suspended".Translate(), null))
				{
					this.bill.suspended = false;
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
			else if (listing_Standard6.ButtonText("NotSuspended".Translate(), null))
			{
				this.bill.suspended = true;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.bill.recipe.description != null)
			{
				stringBuilder.AppendLine(this.bill.recipe.description);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("WorkAmount".Translate() + ": " + this.bill.recipe.WorkAmountTotal(null).ToStringWorkAmount());
			stringBuilder.AppendLine();
			for (int j = 0; j < this.bill.recipe.ingredients.Count; j++)
			{
				IngredientCount ingredientCount = this.bill.recipe.ingredients[j];
				if (!ingredientCount.filter.Summary.NullOrEmpty())
				{
					stringBuilder.AppendLine(this.bill.recipe.IngredientValueGetter.BillRequirementsDescription(this.bill.recipe, ingredientCount));
				}
			}
			stringBuilder.AppendLine();
			string text5 = this.bill.recipe.IngredientValueGetter.ExtraDescriptionLine(this.bill.recipe);
			if (text5 != null)
			{
				stringBuilder.AppendLine(text5);
				stringBuilder.AppendLine();
			}
			if (!this.bill.recipe.skillRequirements.NullOrEmpty<SkillRequirement>())
			{
				stringBuilder.AppendLine("MinimumSkills".Translate());
				stringBuilder.AppendLine(this.bill.recipe.MinSkillString);
			}
			Text.Font = GameFont.Small;
			string text6 = stringBuilder.ToString();
			if (Text.CalcHeight(text6, rect2.width) > rect2.height)
			{
				Text.Font = GameFont.Tiny;
			}
			listing_Standard6.Label(text6, -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard6.End();
			if (this.bill.recipe.products.Count == 1)
			{
				ThingDef thingDef = this.bill.recipe.products[0].thingDef;
				Widgets.InfoCardButton(rect2.x, rect4.y, thingDef, GenStuff.DefaultStuffFor(thingDef));
			}
		}

		private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("AnyWorker".Translate(), delegate()
				{
					this.bill.pawnRestriction = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null),
				payload = null
			};
			SkillDef workSkill = this.bill.recipe.workSkill;
			IEnumerable<Pawn> pawns = PawnsFinder.AllMaps_FreeColonists;
			pawns = from pawn in pawns
			orderby pawn.LabelShortCap
			select pawn;
			if (workSkill != null)
			{
				pawns = from pawn in pawns
				orderby pawn.skills.GetSkill(this.bill.recipe.workSkill).Level descending
				select pawn;
			}
			WorkGiverDef workGiver = this.bill.billStack.billGiver.GetWorkgiver();
			if (workGiver == null)
			{
				Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148, false);
				yield break;
			}
			pawns = from pawn in pawns
			orderby pawn.workSettings.WorkIsActive(workGiver.workType) descending
			select pawn;
			pawns = from pawn in pawns
			orderby pawn.story.WorkTypeIsDisabled(workGiver.workType)
			select pawn;
			using (IEnumerator<Pawn> enumerator = pawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					if (pawn.story.WorkTypeIsDisabled(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(new object[]
							{
								workGiver.verb
							})), null, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
					else if (this.bill.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
							{
								pawn.LabelShortCap,
								pawn.skills.GetSkill(this.bill.recipe.workSkill).Level,
								this.bill.recipe.workSkill.label,
								"NotAssigned".Translate()
							}), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
					else if (!pawn.workSettings.WorkIsActive(workGiver.workType))
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
					else if (this.bill.recipe.workSkill != null)
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0} ({1} {2})", pawn.LabelShortCap, pawn.skills.GetSkill(this.bill.recipe.workSkill).Level, this.bill.recipe.workSkill.label), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
					else
					{
						yield return new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
					}
				}
			}
			yield break;
		}

		private IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>> GenerateStockpileInclusion()
		{
			yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
			{
				option = new FloatMenuOption("IncludeFromAll".Translate(), delegate()
				{
					this.bill.includeFromZone = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null),
				payload = null
			};
			List<SlotGroup> groupList = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
			int groupCount = groupList.Count;
			for (int i = 0; i < groupCount; i++)
			{
				SlotGroup group = groupList[i];
				Zone_Stockpile stockpile = group.parent as Zone_Stockpile;
				if (stockpile != null)
				{
					if (!this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, stockpile))
					{
						yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", "IncludeSpecific".Translate(new object[]
							{
								group.parent.SlotYielderLabel()
							}), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = stockpile
						};
					}
					else
					{
						yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
						{
							option = new FloatMenuOption("IncludeSpecific".Translate(new object[]
							{
								group.parent.SlotYielderLabel()
							}), delegate()
							{
								this.bill.includeFromZone = stockpile;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = stockpile
						};
					}
				}
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Dialog_BillConfig()
		{
		}

		[CompilerGenerated]
		private static Zone_Stockpile <DoWindowContents>m__0(Bill_Production b)
		{
			return b.includeFromZone;
		}

		[CompilerGenerated]
		private IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>> <DoWindowContents>m__1(Bill_Production b)
		{
			return this.GenerateStockpileInclusion();
		}

		[CompilerGenerated]
		private static int <DoWindowContents>m__2(BillStoreModeDef bsm)
		{
			return bsm.listOrder;
		}

		[CompilerGenerated]
		private static Pawn <DoWindowContents>m__3(Bill_Production b)
		{
			return b.pawnRestriction;
		}

		[CompilerGenerated]
		private IEnumerable<Widgets.DropdownMenuElement<Pawn>> <DoWindowContents>m__4(Bill_Production b)
		{
			return this.GeneratePawnRestrictionOptions();
		}

		[CompilerGenerated]
		private sealed class <DoWindowContents>c__AnonStorey3
		{
			internal BillStoreModeDef smLocal;

			internal Dialog_BillConfig $this;

			public <DoWindowContents>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				this.$this.bill.SetStoreMode(this.smLocal, null);
			}
		}

		[CompilerGenerated]
		private sealed class <DoWindowContents>c__AnonStorey2
		{
			internal SlotGroup group;

			internal Dialog_BillConfig.<DoWindowContents>c__AnonStorey3 <>f__ref$3;

			public <DoWindowContents>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$3.$this.bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, (Zone_Stockpile)this.group.parent);
			}
		}

		[CompilerGenerated]
		private sealed class <GeneratePawnRestrictionOptions>c__Iterator0 : IEnumerable, IEnumerable<Widgets.DropdownMenuElement<Pawn>>, IEnumerator, IDisposable, IEnumerator<Widgets.DropdownMenuElement<Pawn>>
		{
			internal SkillDef <workSkill>__0;

			internal IEnumerable<Pawn> <pawns>__0;

			internal IEnumerator<Pawn> $locvar0;

			internal Dialog_BillConfig $this;

			internal Widgets.DropdownMenuElement<Pawn> $current;

			internal bool $disposing;

			internal int $PC;

			private Dialog_BillConfig.<GeneratePawnRestrictionOptions>c__Iterator0.<GeneratePawnRestrictionOptions>c__AnonStorey4 $locvar1;

			private static Func<Pawn, string> <>f__am$cache0;

			private Dialog_BillConfig.<GeneratePawnRestrictionOptions>c__Iterator0.<GeneratePawnRestrictionOptions>c__AnonStorey5 $locvar2;

			[DebuggerHidden]
			public <GeneratePawnRestrictionOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					this.$current = new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption("AnyWorker".Translate(), delegate()
						{
							this.bill.pawnRestriction = null;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = null
					};
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					workSkill = this.bill.recipe.workSkill;
					pawns = PawnsFinder.AllMaps_FreeColonists;
					pawns = from pawn in pawns
					orderby pawn.LabelShortCap
					select pawn;
					if (workSkill != null)
					{
						pawns = from pawn in pawns
						orderby pawn.skills.GetSkill(<GeneratePawnRestrictionOptions>c__AnonStorey.<>f__ref$0.$this.bill.recipe.workSkill).Level descending
						select pawn;
					}
					<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver = this.bill.billStack.billGiver.GetWorkgiver();
					if (<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver == null)
					{
						Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148, false);
						return false;
					}
					pawns = from pawn in pawns
					orderby pawn.workSettings.WorkIsActive(<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver.workType) descending
					select pawn;
					pawns = from pawn in pawns
					orderby pawn.story.WorkTypeIsDisabled(<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver.workType)
					select pawn;
					enumerator = pawns.GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
				case 3u:
				case 4u:
				case 5u:
				case 6u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						Pawn pawn = enumerator.Current;
						if (pawn.story.WorkTypeIsDisabled(<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver.workType))
						{
							this.$current = new Widgets.DropdownMenuElement<Pawn>
							{
								option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(new object[]
								{
									<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver.verb
								})), null, MenuOptionPriority.Default, null, null, 0f, null, null),
								payload = pawn
							};
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
						if (this.bill.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver.workType))
						{
							this.$current = new Widgets.DropdownMenuElement<Pawn>
							{
								option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
								{
									pawn.LabelShortCap,
									pawn.skills.GetSkill(this.bill.recipe.workSkill).Level,
									this.bill.recipe.workSkill.label,
									"NotAssigned".Translate()
								}), delegate()
								{
									this.bill.pawnRestriction = pawn;
								}, MenuOptionPriority.Default, null, null, 0f, null, null),
								payload = pawn
							};
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
						if (!pawn.workSettings.WorkIsActive(<GeneratePawnRestrictionOptions>c__AnonStorey.workGiver.workType))
						{
							this.$current = new Widgets.DropdownMenuElement<Pawn>
							{
								option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate()
								{
									this.bill.pawnRestriction = pawn;
								}, MenuOptionPriority.Default, null, null, 0f, null, null),
								payload = pawn
							};
							if (!this.$disposing)
							{
								this.$PC = 4;
							}
							flag = true;
							return true;
						}
						if (this.bill.recipe.workSkill != null)
						{
							this.$current = new Widgets.DropdownMenuElement<Pawn>
							{
								option = new FloatMenuOption(string.Format("{0} ({1} {2})", pawn.LabelShortCap, pawn.skills.GetSkill(this.bill.recipe.workSkill).Level, this.bill.recipe.workSkill.label), delegate()
								{
									this.bill.pawnRestriction = pawn;
								}, MenuOptionPriority.Default, null, null, 0f, null, null),
								payload = pawn
							};
							if (!this.$disposing)
							{
								this.$PC = 5;
							}
							flag = true;
							return true;
						}
						this.$current = new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate()
							{
								this.bill.pawnRestriction = pawn;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = pawn
						};
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Widgets.DropdownMenuElement<Pawn> IEnumerator<Widgets.DropdownMenuElement<Pawn>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
				case 3u:
				case 4u:
				case 5u:
				case 6u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Widgets.DropdownMenuElement<Verse.Pawn>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Widgets.DropdownMenuElement<Pawn>> IEnumerable<Widgets.DropdownMenuElement<Pawn>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Dialog_BillConfig.<GeneratePawnRestrictionOptions>c__Iterator0 <GeneratePawnRestrictionOptions>c__Iterator = new Dialog_BillConfig.<GeneratePawnRestrictionOptions>c__Iterator0();
				<GeneratePawnRestrictionOptions>c__Iterator.$this = this;
				return <GeneratePawnRestrictionOptions>c__Iterator;
			}

			private static string <>m__0(Pawn pawn)
			{
				return pawn.LabelShortCap;
			}

			private sealed class <GeneratePawnRestrictionOptions>c__AnonStorey4
			{
				internal WorkGiverDef workGiver;

				internal Dialog_BillConfig.<GeneratePawnRestrictionOptions>c__Iterator0 <>f__ref$0;

				public <GeneratePawnRestrictionOptions>c__AnonStorey4()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.bill.pawnRestriction = null;
				}

				internal int <>m__1(Pawn pawn)
				{
					return pawn.skills.GetSkill(this.<>f__ref$0.$this.bill.recipe.workSkill).Level;
				}

				internal bool <>m__2(Pawn pawn)
				{
					return pawn.workSettings.WorkIsActive(this.workGiver.workType);
				}

				internal bool <>m__3(Pawn pawn)
				{
					return pawn.story.WorkTypeIsDisabled(this.workGiver.workType);
				}
			}

			private sealed class <GeneratePawnRestrictionOptions>c__AnonStorey5
			{
				internal Pawn pawn;

				internal Dialog_BillConfig.<GeneratePawnRestrictionOptions>c__Iterator0 <>f__ref$0;

				internal Dialog_BillConfig.<GeneratePawnRestrictionOptions>c__Iterator0.<GeneratePawnRestrictionOptions>c__AnonStorey4 <>f__ref$4;

				public <GeneratePawnRestrictionOptions>c__AnonStorey5()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.bill.pawnRestriction = this.pawn;
				}

				internal void <>m__1()
				{
					this.<>f__ref$0.$this.bill.pawnRestriction = this.pawn;
				}

				internal void <>m__2()
				{
					this.<>f__ref$0.$this.bill.pawnRestriction = this.pawn;
				}

				internal void <>m__3()
				{
					this.<>f__ref$0.$this.bill.pawnRestriction = this.pawn;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateStockpileInclusion>c__Iterator1 : IEnumerable, IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>>, IEnumerator, IDisposable, IEnumerator<Widgets.DropdownMenuElement<Zone_Stockpile>>
		{
			internal List<SlotGroup> <groupList>__0;

			internal int <groupCount>__0;

			internal int <i>__1;

			internal SlotGroup <group>__2;

			internal Dialog_BillConfig $this;

			internal Widgets.DropdownMenuElement<Zone_Stockpile> $current;

			internal bool $disposing;

			internal int $PC;

			private Dialog_BillConfig.<GenerateStockpileInclusion>c__Iterator1.<GenerateStockpileInclusion>c__AnonStorey6 $locvar0;

			[DebuggerHidden]
			public <GenerateStockpileInclusion>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = new Widgets.DropdownMenuElement<Zone_Stockpile>
					{
						option = new FloatMenuOption("IncludeFromAll".Translate(), delegate()
						{
							this.bill.includeFromZone = null;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = null
					};
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					groupList = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
					groupCount = groupList.Count;
					i = 0;
					goto IL_272;
				case 2u:
					break;
				case 3u:
					break;
				default:
					return false;
				}
				IL_264:
				i++;
				IL_272:
				if (i >= groupCount)
				{
					this.$PC = -1;
				}
				else
				{
					group = groupList[i];
					Zone_Stockpile stockpile = group.parent as Zone_Stockpile;
					if (stockpile == null)
					{
						goto IL_264;
					}
					if (!this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, stockpile))
					{
						this.$current = new Widgets.DropdownMenuElement<Zone_Stockpile>
						{
							option = new FloatMenuOption(string.Format("{0} ({1})", "IncludeSpecific".Translate(new object[]
							{
								group.parent.SlotYielderLabel()
							}), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = stockpile
						};
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					this.$current = new Widgets.DropdownMenuElement<Zone_Stockpile>
					{
						option = new FloatMenuOption("IncludeSpecific".Translate(new object[]
						{
							group.parent.SlotYielderLabel()
						}), delegate()
						{
							this.bill.includeFromZone = stockpile;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = stockpile
					};
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				return false;
			}

			Widgets.DropdownMenuElement<Zone_Stockpile> IEnumerator<Widgets.DropdownMenuElement<Zone_Stockpile>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Widgets.DropdownMenuElement<RimWorld.Zone_Stockpile>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Widgets.DropdownMenuElement<Zone_Stockpile>> IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Dialog_BillConfig.<GenerateStockpileInclusion>c__Iterator1 <GenerateStockpileInclusion>c__Iterator = new Dialog_BillConfig.<GenerateStockpileInclusion>c__Iterator1();
				<GenerateStockpileInclusion>c__Iterator.$this = this;
				return <GenerateStockpileInclusion>c__Iterator;
			}

			internal void <>m__0()
			{
				this.bill.includeFromZone = null;
			}

			private sealed class <GenerateStockpileInclusion>c__AnonStorey6
			{
				internal Zone_Stockpile stockpile;

				internal Dialog_BillConfig.<GenerateStockpileInclusion>c__Iterator1 <>f__ref$1;

				public <GenerateStockpileInclusion>c__AnonStorey6()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$1.$this.bill.includeFromZone = this.stockpile;
				}
			}
		}
	}
}
