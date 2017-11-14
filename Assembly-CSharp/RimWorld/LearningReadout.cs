using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class LearningReadout : IExposable
	{
		private List<ConceptDef> activeConcepts = new List<ConceptDef>();

		private ConceptDef selectedConcept;

		private bool showAllMode;

		private float contentHeight;

		private Vector2 scrollPosition = Vector2.zero;

		private string searchString = string.Empty;

		private float lastConceptActivateRealTime = -999f;

		private ConceptDef mouseoverConcept;

		private const float OuterMargin = 8f;

		private const float InnerMargin = 7f;

		private const float ReadoutWidth = 200f;

		private const float InfoPaneWidth = 310f;

		private const float OpenButtonSize = 24f;

		public static readonly Texture2D ProgressBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.745098054f, 0.6039216f, 0.2f));

		public static readonly Texture2D ProgressBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.509803951f, 0.407843143f, 0.13333334f));

		[CompilerGenerated]
		private static Predicate<ConceptDef> _003C_003Ef__mg_0024cache0;

		public int ActiveConceptsCount
		{
			get
			{
				return this.activeConcepts.Count;
			}
		}

		public bool ShowAllMode
		{
			get
			{
				return this.showAllMode;
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<ConceptDef>(ref this.activeConcepts, "activeConcepts", LookMode.Undefined, new object[0]);
			Scribe_Defs.Look<ConceptDef>(ref this.selectedConcept, "selectedConcept");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.activeConcepts.RemoveAll(PlayerKnowledgeDatabase.IsComplete);
			}
		}

		public bool TryActivateConcept(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc))
			{
				return false;
			}
			this.activeConcepts.Add(conc);
			SoundDefOf.LessonActivated.PlayOneShotOnCamera(null);
			this.lastConceptActivateRealTime = RealTime.LastRealTime;
			return true;
		}

		public bool IsActive(ConceptDef conc)
		{
			return this.activeConcepts.Contains(conc);
		}

		public void LearningReadoutUpdate()
		{
		}

		public void Notify_ConceptNewlyLearned(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc) || this.selectedConcept == conc)
			{
				SoundDefOf.LessonDeactivated.PlayOneShotOnCamera(null);
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

		private string FilterSearchStringInput(string input)
		{
			if (input == this.searchString)
			{
				return input;
			}
			if (input.Length > 20)
			{
				input = input.Substring(0, 20);
			}
			return input;
		}

		public void LearningReadoutOnGUI()
		{
			if (!TutorSystem.TutorialMode && TutorSystem.AdaptiveTrainingEnabled && (Find.PlaySettings.showLearningHelper || this.activeConcepts.Count != 0) && !Find.WindowStack.IsOpen<Screen_Credits>())
			{
				float b = (float)((float)UI.screenHeight / 2.0);
				float a = (float)(this.contentHeight + 14.0);
				Rect outRect = new Rect((float)((float)UI.screenWidth - 8.0 - 200.0), 8f, 200f, Mathf.Min(a, b));
				Rect rect = outRect;
				Find.WindowStack.ImmediateWindow(76136312, outRect, WindowLayer.Super, delegate
				{
					outRect = outRect.AtZero();
					Rect rect2 = outRect.ContractedBy(7f);
					Rect viewRect = rect2.AtZero();
					bool flag = this.contentHeight > rect2.height;
					Widgets.DrawWindowBackgroundTutor(outRect);
					if (flag)
					{
						viewRect.height = (float)(this.contentHeight + 40.0);
						viewRect.width -= 20f;
						this.scrollPosition = GUI.BeginScrollView(rect2, this.scrollPosition, viewRect);
					}
					else
					{
						GUI.BeginGroup(rect2);
					}
					float num2 = 0f;
					Text.Font = GameFont.Small;
					Rect rect3 = new Rect(0f, 0f, (float)(viewRect.width - 24.0), 24f);
					Widgets.Label(rect3, "LearningHelper".Translate());
					num2 = rect3.yMax;
					Rect butRect = new Rect(rect3.xMax, rect3.y, 24f, 24f);
					if (Widgets.ButtonImage(butRect, this.showAllMode ? TexButton.Minus : TexButton.Plus))
					{
						this.showAllMode = !this.showAllMode;
						if (this.showAllMode)
						{
							SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
						}
						else
						{
							SoundDefOf.TickLow.PlayOneShotOnCamera(null);
						}
					}
					if (this.showAllMode)
					{
						Rect rect4 = new Rect(0f, num2, (float)(viewRect.width - 20.0 - 2.0), 28f);
						this.searchString = this.FilterSearchStringInput(Widgets.TextField(rect4, this.searchString));
						if (this.searchString == string.Empty)
						{
							GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
							Text.Anchor = TextAnchor.MiddleLeft;
							Rect rect5 = rect4;
							rect5.xMin += 7f;
							Widgets.Label(rect5, "Filter".Translate() + "...");
							Text.Anchor = TextAnchor.UpperLeft;
							GUI.color = Color.white;
						}
						Rect butRect2 = new Rect((float)(viewRect.width - 20.0), (float)(num2 + 14.0 - 10.0), 20f, 20f);
						if (Widgets.ButtonImage(butRect2, TexButton.CloseXSmall))
						{
							this.searchString = string.Empty;
							SoundDefOf.TickTiny.PlayOneShotOnCamera(null);
						}
						num2 = (float)(rect4.yMax + 4.0);
					}
					IEnumerable<ConceptDef> enumerable = this.showAllMode ? DefDatabase<ConceptDef>.AllDefs : this.activeConcepts;
					if (enumerable.Any())
					{
						GUI.color = new Color(1f, 1f, 1f, 0.5f);
						Widgets.DrawLineHorizontal(0f, num2, viewRect.width);
						GUI.color = Color.white;
						num2 = (float)(num2 + 4.0);
					}
					if (this.showAllMode)
					{
						enumerable = from c in enumerable
						orderby this.DisplayPriority(c) descending, c.label
						select c;
					}
					foreach (ConceptDef item in enumerable)
					{
						if (!item.TriggeredDirect)
						{
							num2 = this.DrawConceptListRow(0f, num2, viewRect.width, item).yMax;
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
				if (num < 1.0 && num > 0.0)
				{
					float x = rect.x;
					Vector2 center = rect.center;
					GenUI.DrawFlash(x, center.y, (float)((float)UI.screenWidth * 0.60000002384185791), (float)(Pulser.PulseBrightness(1f, 1f, num) * 0.85000002384185791), new Color(0.8f, 0.77f, 0.53f));
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

		private int DisplayPriority(ConceptDef conc)
		{
			int num = 1;
			if (this.MatchesSearchString(conc))
			{
				num += 10000;
			}
			return num;
		}

		private bool MatchesSearchString(ConceptDef conc)
		{
			return this.searchString != string.Empty && conc.label.IndexOf(this.searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private Rect DrawConceptListRow(float x, float y, float width, ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool flag = PlayerKnowledgeDatabase.IsComplete(conc);
			bool flag2 = !flag && knowledge > 0.0;
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

		private Rect DrawInfoPane(ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool complete = PlayerKnowledgeDatabase.IsComplete(conc);
			bool drawProgressBar = !complete && knowledge > 0.0;
			Text.Font = GameFont.Medium;
			float titleHeight = Text.CalcHeight(conc.LabelCap, 276f);
			Text.Font = GameFont.Small;
			float textHeight = Text.CalcHeight(conc.HelpTextAdjusted, 296f);
			float num = (float)(titleHeight + textHeight + 14.0 + 5.0);
			if (this.selectedConcept == conc)
			{
				num = (float)(num + 40.0);
			}
			if (drawProgressBar)
			{
				num = (float)(num + 30.0);
			}
			Rect outRect = new Rect((float)((float)UI.screenWidth - 8.0 - 200.0 - 8.0 - 310.0), 8f, 310f, num);
			Rect result = outRect;
			Find.WindowStack.ImmediateWindow(987612111, outRect, WindowLayer.Super, delegate
			{
				outRect = outRect.AtZero();
				Rect rect = outRect.ContractedBy(7f);
				Widgets.DrawShadowAround(outRect);
				Widgets.DrawWindowBackgroundTutor(outRect);
				Rect rect2 = rect;
				rect2.width -= 20f;
				rect2.height = (float)(titleHeight + 5.0);
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
					Vector2 center = rect.center;
					Rect rect5 = new Rect((float)(center.x - 70.0), (float)(rect.yMax - 30.0), 140f, 30f);
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
			return result;
		}
	}
}
