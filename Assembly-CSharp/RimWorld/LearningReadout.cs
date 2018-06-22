using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000841 RID: 2113
	[StaticConstructorOnStartup]
	public class LearningReadout : IExposable
	{
		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x0019F05C File Offset: 0x0019D45C
		public int ActiveConceptsCount
		{
			get
			{
				return this.activeConcepts.Count;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x0019F07C File Offset: 0x0019D47C
		public bool ShowAllMode
		{
			get
			{
				return this.showAllMode;
			}
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x0019F098 File Offset: 0x0019D498
		public void ExposeData()
		{
			Scribe_Collections.Look<ConceptDef>(ref this.activeConcepts, "activeConcepts", LookMode.Undefined, new object[0]);
			Scribe_Defs.Look<ConceptDef>(ref this.selectedConcept, "selectedConcept");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.activeConcepts.RemoveAll((ConceptDef c) => PlayerKnowledgeDatabase.IsComplete(c));
			}
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x0019F104 File Offset: 0x0019D504
		public bool TryActivateConcept(ConceptDef conc)
		{
			bool result;
			if (this.activeConcepts.Contains(conc))
			{
				result = false;
			}
			else
			{
				this.activeConcepts.Add(conc);
				SoundDefOf.Lesson_Activated.PlayOneShotOnCamera(null);
				this.lastConceptActivateRealTime = RealTime.LastRealTime;
				result = true;
			}
			return result;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x0019F154 File Offset: 0x0019D554
		public bool IsActive(ConceptDef conc)
		{
			return this.activeConcepts.Contains(conc);
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x0019F175 File Offset: 0x0019D575
		public void LearningReadoutUpdate()
		{
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x0019F178 File Offset: 0x0019D578
		public void Notify_ConceptNewlyLearned(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc) || this.selectedConcept == conc)
			{
				SoundDefOf.Lesson_Deactivated.PlayOneShotOnCamera(null);
				SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			}
			if (this.activeConcepts.Contains(conc))
			{
				this.activeConcepts.Remove(conc);
			}
			if (this.selectedConcept == conc)
			{
				this.selectedConcept = null;
			}
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x0019F1EC File Offset: 0x0019D5EC
		private string FilterSearchStringInput(string input)
		{
			string result;
			if (input == this.searchString)
			{
				result = input;
			}
			else
			{
				if (input.Length > 20)
				{
					input = input.Substring(0, 20);
				}
				result = input;
			}
			return result;
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x0019F234 File Offset: 0x0019D634
		public void LearningReadoutOnGUI()
		{
			if (!TutorSystem.TutorialMode && TutorSystem.AdaptiveTrainingEnabled)
			{
				if (Find.PlaySettings.showLearningHelper || this.activeConcepts.Count != 0)
				{
					if (!Find.WindowStack.IsOpen<Screen_Credits>())
					{
						float b = (float)UI.screenHeight / 2f;
						float a = this.contentHeight + 14f;
						Rect outRect = new Rect((float)UI.screenWidth - 8f - 200f, 8f, 200f, Mathf.Min(a, b));
						Rect outRect2 = outRect;
						Find.WindowStack.ImmediateWindow(76136312, outRect, WindowLayer.Super, delegate
						{
							outRect = outRect.AtZero();
							Rect rect = outRect.ContractedBy(7f);
							Rect viewRect = rect.AtZero();
							bool flag = this.contentHeight > rect.height;
							Widgets.DrawWindowBackgroundTutor(outRect);
							if (flag)
							{
								viewRect.height = this.contentHeight + 40f;
								viewRect.width -= 20f;
								this.scrollPosition = GUI.BeginScrollView(rect, this.scrollPosition, viewRect);
							}
							else
							{
								GUI.BeginGroup(rect);
							}
							float num2 = 0f;
							Text.Font = GameFont.Small;
							Rect rect2 = new Rect(0f, 0f, viewRect.width - 24f, 24f);
							Widgets.Label(rect2, "LearningHelper".Translate());
							num2 = rect2.yMax;
							Rect butRect = new Rect(rect2.xMax, rect2.y, 24f, 24f);
							if (Widgets.ButtonImage(butRect, this.showAllMode ? TexButton.Minus : TexButton.Plus))
							{
								this.showAllMode = !this.showAllMode;
								if (this.showAllMode)
								{
									SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								}
								else
								{
									SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
								}
							}
							if (this.showAllMode)
							{
								Rect rect3 = new Rect(0f, num2, viewRect.width - 20f - 2f, 28f);
								this.searchString = this.FilterSearchStringInput(Widgets.TextField(rect3, this.searchString));
								if (this.searchString == "")
								{
									GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
									Text.Anchor = TextAnchor.MiddleLeft;
									Rect rect4 = rect3;
									rect4.xMin += 7f;
									Widgets.Label(rect4, "Filter".Translate() + "...");
									Text.Anchor = TextAnchor.UpperLeft;
									GUI.color = Color.white;
								}
								Rect butRect2 = new Rect(viewRect.width - 20f, num2 + 14f - 10f, 20f, 20f);
								if (Widgets.ButtonImage(butRect2, TexButton.CloseXSmall))
								{
									this.searchString = "";
									SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
								}
								num2 = rect3.yMax + 4f;
							}
							IEnumerable<ConceptDef> enumerable = this.showAllMode ? DefDatabase<ConceptDef>.AllDefs : this.activeConcepts;
							if (enumerable.Any<ConceptDef>())
							{
								GUI.color = new Color(1f, 1f, 1f, 0.5f);
								Widgets.DrawLineHorizontal(0f, num2, viewRect.width);
								GUI.color = Color.white;
								num2 += 4f;
							}
							if (this.showAllMode)
							{
								enumerable = from c in enumerable
								orderby this.DisplayPriority(c) descending, c.label
								select c;
							}
							foreach (ConceptDef conceptDef2 in enumerable)
							{
								if (!conceptDef2.TriggeredDirect)
								{
									num2 = this.DrawConceptListRow(0f, num2, viewRect.width, conceptDef2).yMax;
								}
							}
							this.contentHeight = num2;
							if (flag)
							{
								GUI.EndScrollView();
							}
							else
							{
								GUI.EndGroup();
							}
						}, false, false, 1f);
						float num = Time.realtimeSinceStartup - this.lastConceptActivateRealTime;
						if (num < 1f && num > 0f)
						{
							GenUI.DrawFlash(outRect2.x, outRect2.center.y, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num) * 0.85f, new Color(0.8f, 0.77f, 0.53f));
						}
						ConceptDef conceptDef = (this.selectedConcept == null) ? this.mouseoverConcept : this.selectedConcept;
						if (conceptDef != null)
						{
							this.DrawInfoPane(conceptDef);
							conceptDef.HighlightAllTags();
						}
						this.mouseoverConcept = null;
					}
				}
			}
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x0019F3D0 File Offset: 0x0019D7D0
		private int DisplayPriority(ConceptDef conc)
		{
			int num = 1;
			if (this.MatchesSearchString(conc))
			{
				num += 10000;
			}
			return num;
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x0019F3FC File Offset: 0x0019D7FC
		private bool MatchesSearchString(ConceptDef conc)
		{
			return this.searchString != "" && conc.label.IndexOf(this.searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x0019F444 File Offset: 0x0019D844
		private Rect DrawConceptListRow(float x, float y, float width, ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool flag = PlayerKnowledgeDatabase.IsComplete(conc);
			bool flag2 = !flag && knowledge > 0f;
			float num = Text.CalcHeight(conc.LabelCap, width);
			if (flag2)
			{
				num = num;
			}
			Rect rect = new Rect(x, y, width, num);
			if (flag2)
			{
				Rect rect2 = new Rect(rect);
				rect2.yMin += 1f;
				rect2.yMax -= 1f;
				Widgets.FillableBar(rect2, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex, LearningReadout.ProgressBarBGTex, false);
			}
			if (flag)
			{
				GUI.DrawTexture(rect, BaseContent.GreyTex);
			}
			if (this.selectedConcept == conc)
			{
				GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			if (this.MatchesSearchString(conc))
			{
				Widgets.DrawHighlight(rect);
			}
			Widgets.Label(rect, conc.LabelCap);
			if (Mouse.IsOver(rect) && this.selectedConcept == null)
			{
				this.mouseoverConcept = conc;
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.selectedConcept == conc)
				{
					this.selectedConcept = null;
				}
				else
				{
					this.selectedConcept = conc;
				}
				SoundDefOf.PageChange.PlayOneShotOnCamera(null);
			}
			return rect;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x0019F5A0 File Offset: 0x0019D9A0
		private Rect DrawInfoPane(ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool complete = PlayerKnowledgeDatabase.IsComplete(conc);
			bool drawProgressBar = !complete && knowledge > 0f;
			Text.Font = GameFont.Medium;
			float titleHeight = Text.CalcHeight(conc.LabelCap, 276f);
			Text.Font = GameFont.Small;
			float textHeight = Text.CalcHeight(conc.HelpTextAdjusted, 296f);
			float num = titleHeight + textHeight + 14f + 5f;
			if (this.selectedConcept == conc)
			{
				num += 40f;
			}
			if (drawProgressBar)
			{
				num += 30f;
			}
			Rect outRect = new Rect((float)UI.screenWidth - 8f - 200f - 8f - 310f, 8f, 310f, num);
			Rect outRect2 = outRect;
			Find.WindowStack.ImmediateWindow(987612111, outRect, WindowLayer.Super, delegate
			{
				outRect = outRect.AtZero();
				Rect rect = outRect.ContractedBy(7f);
				Widgets.DrawShadowAround(outRect);
				Widgets.DrawWindowBackgroundTutor(outRect);
				Rect rect2 = rect;
				rect2.width -= 20f;
				rect2.height = titleHeight + 5f;
				Text.Font = GameFont.Medium;
				Widgets.Label(rect2, conc.LabelCap);
				Text.Font = GameFont.Small;
				Rect rect3 = rect;
				rect3.yMin = rect2.yMax;
				rect3.height = textHeight;
				Widgets.Label(rect3, conc.HelpTextAdjusted);
				if (drawProgressBar)
				{
					Rect rect4 = rect;
					rect4.yMin = rect3.yMax;
					rect4.height = 30f;
					Widgets.FillableBar(rect4, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex);
				}
				if (this.selectedConcept == conc)
				{
					if (Widgets.CloseButtonFor(outRect))
					{
						this.selectedConcept = null;
						SoundDefOf.PageChange.PlayOneShotOnCamera(null);
					}
					Rect rect5 = new Rect(rect.center.x - 70f, rect.yMax - 30f, 140f, 30f);
					if (!complete)
					{
						if (Widgets.ButtonText(rect5, "MarkLearned".Translate(), true, false, true))
						{
							this.selectedConcept = null;
							SoundDefOf.PageChange.PlayOneShotOnCamera(null);
							PlayerKnowledgeDatabase.SetKnowledge(conc, 1f);
						}
					}
					else
					{
						GUI.color = new Color(1f, 1f, 1f, 0.5f);
						Text.Anchor = TextAnchor.MiddleCenter;
						Widgets.Label(rect5, "AlreadyLearned".Translate());
						Text.Anchor = TextAnchor.UpperLeft;
						GUI.color = Color.white;
					}
				}
			}, false, false, 1f);
			return outRect2;
		}

		// Token: 0x040019D8 RID: 6616
		private List<ConceptDef> activeConcepts = new List<ConceptDef>();

		// Token: 0x040019D9 RID: 6617
		private ConceptDef selectedConcept = null;

		// Token: 0x040019DA RID: 6618
		private bool showAllMode = false;

		// Token: 0x040019DB RID: 6619
		private float contentHeight = 0f;

		// Token: 0x040019DC RID: 6620
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040019DD RID: 6621
		private string searchString = "";

		// Token: 0x040019DE RID: 6622
		private float lastConceptActivateRealTime = -999f;

		// Token: 0x040019DF RID: 6623
		private ConceptDef mouseoverConcept;

		// Token: 0x040019E0 RID: 6624
		private const float OuterMargin = 8f;

		// Token: 0x040019E1 RID: 6625
		private const float InnerMargin = 7f;

		// Token: 0x040019E2 RID: 6626
		private const float ReadoutWidth = 200f;

		// Token: 0x040019E3 RID: 6627
		private const float InfoPaneWidth = 310f;

		// Token: 0x040019E4 RID: 6628
		private const float OpenButtonSize = 24f;

		// Token: 0x040019E5 RID: 6629
		public static readonly Texture2D ProgressBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.745098054f, 0.6039216f, 0.2f));

		// Token: 0x040019E6 RID: 6630
		public static readonly Texture2D ProgressBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.509803951f, 0.407843143f, 0.13333334f));
	}
}
