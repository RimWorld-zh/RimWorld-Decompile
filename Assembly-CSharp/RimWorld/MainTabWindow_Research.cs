using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000873 RID: 2163
	[StaticConstructorOnStartup]
	public class MainTabWindow_Research : MainTabWindow
	{
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x001ABB04 File Offset: 0x001A9F04
		// (set) Token: 0x06003150 RID: 12624 RVA: 0x001ABB20 File Offset: 0x001A9F20
		private ResearchTabDef CurTab
		{
			get
			{
				return this.curTabInt;
			}
			set
			{
				if (value != this.curTabInt)
				{
					this.curTabInt = value;
					Vector2 vector = this.ViewSize(this.CurTab);
					this.rightViewWidth = vector.x;
					this.rightViewHeight = vector.y;
					this.rightScrollPosition = Vector2.zero;
				}
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06003151 RID: 12625 RVA: 0x001ABB78 File Offset: 0x001A9F78
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 initialSize = base.InitialSize;
				float b = (float)(UI.screenHeight - 35);
				float b2 = this.Margin + 10f + 32f + 10f + DefDatabase<ResearchTabDef>.AllDefs.Max((ResearchTabDef tab) => this.ViewSize(tab).y) + 10f + 10f + this.Margin;
				float a = Mathf.Max(initialSize.y, b2);
				initialSize.y = Mathf.Min(a, b);
				return initialSize;
			}
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x001ABC00 File Offset: 0x001AA000
		private Vector2 ViewSize(ResearchTabDef tab)
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			float num = 0f;
			float num2 = 0f;
			Text.Font = GameFont.Small;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ResearchProjectDef researchProjectDef = allDefsListForReading[i];
				if (researchProjectDef.tab == tab)
				{
					Rect rect = new Rect(0f, 0f, 140f, 0f);
					Widgets.LabelCacheHeight(ref rect, this.GetLabel(researchProjectDef), false, false);
					num = Mathf.Max(num, this.PosX(researchProjectDef) + 140f);
					num2 = Mathf.Max(num2, this.PosY(researchProjectDef) + rect.height);
				}
			}
			return new Vector2(num + 20f, num2 + 20f);
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x001ABCD0 File Offset: 0x001AA0D0
		public override void PreOpen()
		{
			base.PreOpen();
			this.selectedProject = Find.ResearchManager.currentProj;
			if (this.CurTab == null)
			{
				if (this.selectedProject != null)
				{
					this.CurTab = this.selectedProject.tab;
				}
				else
				{
					this.CurTab = ResearchTabDefOf.Main;
				}
			}
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x001ABD30 File Offset: 0x001AA130
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			this.windowRect.width = (float)UI.screenWidth;
			if (!this.noBenchWarned)
			{
				bool flag = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].listerBuildings.ColonistsHaveResearchBench())
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Find.WindowStack.Add(new Dialog_MessageBox("ResearchMenuWithoutBench".Translate(), null, null, null, null, null, false, null, null));
				}
				this.noBenchWarned = true;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			Rect leftOutRect = new Rect(0f, 0f, 200f, inRect.height);
			Rect rightOutRect = new Rect(leftOutRect.xMax + 10f, 0f, inRect.width - leftOutRect.width - 10f, inRect.height);
			this.DrawLeftRect(leftOutRect);
			this.DrawRightRect(rightOutRect);
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x001ABE40 File Offset: 0x001AA240
		private void DrawLeftRect(Rect leftOutRect)
		{
			Rect position = leftOutRect;
			GUI.BeginGroup(position);
			if (this.selectedProject != null)
			{
				Rect outRect = new Rect(0f, 0f, position.width, 500f);
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.leftScrollViewHeight);
				Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect, true);
				float num = 0f;
				Text.Font = GameFont.Medium;
				GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
				Rect rect = new Rect(0f, num, viewRect.width, 50f);
				Widgets.LabelCacheHeight(ref rect, this.selectedProject.LabelCap, true, false);
				GenUI.ResetLabelAlign();
				Text.Font = GameFont.Small;
				num += rect.height;
				Rect rect2 = new Rect(0f, num, viewRect.width, 0f);
				Widgets.LabelCacheHeight(ref rect2, this.selectedProject.description, true, false);
				num += rect2.height + 10f;
				string text = string.Concat(new string[]
				{
					"ProjectTechLevel".Translate().CapitalizeFirst(),
					": ",
					this.selectedProject.techLevel.ToStringHuman().CapitalizeFirst(),
					"\n",
					"YourTechLevel".Translate().CapitalizeFirst(),
					": ",
					Faction.OfPlayer.def.techLevel.ToStringHuman().CapitalizeFirst()
				});
				float num2 = this.selectedProject.CostFactor(Faction.OfPlayer.def.techLevel);
				if (num2 != 1f)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n\n",
						"ResearchCostMultiplier".Translate().CapitalizeFirst(),
						": ",
						num2.ToStringPercent(),
						"\n",
						"ResearchCostComparison".Translate(new object[]
						{
							this.selectedProject.baseCost.ToString("F0"),
							this.selectedProject.CostApparent.ToString("F0")
						})
					});
				}
				Rect rect3 = new Rect(0f, num, viewRect.width, 0f);
				Widgets.LabelCacheHeight(ref rect3, text, true, false);
				num = rect3.yMax + 10f;
				Rect rect4 = new Rect(0f, num, viewRect.width, 500f);
				float num3 = this.DrawResearchPrereqs(this.selectedProject, rect4);
				if (num3 > 0f)
				{
					num += num3 + 15f;
				}
				Rect rect5 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawResearchBenchRequirements(this.selectedProject, rect5);
				num += 3f;
				this.leftScrollViewHeight = num;
				Widgets.EndScrollView();
				bool flag = Prefs.DevMode && this.selectedProject != Find.ResearchManager.currentProj && !this.selectedProject.IsFinished;
				Rect rect6 = new Rect(0f, 0f, 90f, 50f);
				if (flag)
				{
					rect6.x = (outRect.width - (rect6.width * 2f + 20f)) / 2f;
				}
				else
				{
					rect6.x = (outRect.width - rect6.width) / 2f;
				}
				rect6.y = outRect.y + outRect.height + 20f;
				if (this.selectedProject.IsFinished)
				{
					Widgets.DrawMenuSection(rect6);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect6, "Finished".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (this.selectedProject == Find.ResearchManager.currentProj)
				{
					Widgets.DrawMenuSection(rect6);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect6, "InProgress".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (!this.selectedProject.CanStartNow)
				{
					Widgets.DrawMenuSection(rect6);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect6, "Locked".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (Widgets.ButtonText(rect6, "Research".Translate(), true, false, true))
				{
					SoundDefOf.ResearchStart.PlayOneShotOnCamera(null);
					Find.ResearchManager.currentProj = this.selectedProject;
					TutorSystem.Notify_Event("StartResearchProject");
				}
				if (flag)
				{
					Rect rect7 = rect6;
					rect7.x += rect7.width + 20f;
					if (Widgets.ButtonText(rect7, "Debug Insta-finish", true, false, true))
					{
						Find.ResearchManager.currentProj = this.selectedProject;
						Find.ResearchManager.InstantFinish(this.selectedProject, false);
					}
				}
				Rect rect8 = new Rect(15f, rect6.y + rect6.height + 20f, position.width - 30f, 35f);
				Widgets.FillableBar(rect8, this.selectedProject.ProgressPercent, MainTabWindow_Research.ResearchBarFillTex, MainTabWindow_Research.ResearchBarBGTex, true);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect8, this.selectedProject.ProgressApparent.ToString("F0") + " / " + this.selectedProject.CostApparent.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}
			GUI.EndGroup();
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x001AC3EC File Offset: 0x001AA7EC
		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x001AC408 File Offset: 0x001AA808
		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x001AC424 File Offset: 0x001AA824
		private float PixelsToCoordX(float x)
		{
			return x / 190f;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x001AC440 File Offset: 0x001AA840
		private float PixelsToCoordY(float y)
		{
			return y / 100f;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x001AC45C File Offset: 0x001AA85C
		private float PosX(ResearchProjectDef d)
		{
			return this.CoordToPixelsX(d.ResearchViewX);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x001AC480 File Offset: 0x001AA880
		private float PosY(ResearchProjectDef d)
		{
			return this.CoordToPixelsY(d.ResearchViewY);
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x001AC4A4 File Offset: 0x001AA8A4
		private void DrawRightRect(Rect rightOutRect)
		{
			rightOutRect.yMin += 32f;
			Widgets.DrawMenuSection(rightOutRect);
			List<TabRecord> list = new List<TabRecord>();
			foreach (ResearchTabDef localTabDef2 in DefDatabase<ResearchTabDef>.AllDefs)
			{
				ResearchTabDef localTabDef = localTabDef2;
				list.Add(new TabRecord(localTabDef.LabelCap, delegate()
				{
					this.CurTab = localTabDef;
				}, this.CurTab == localTabDef));
			}
			TabDrawer.DrawTabs(rightOutRect, list, 200f);
			if (Prefs.DevMode)
			{
				Rect rect = rightOutRect;
				rect.yMax = rect.yMin + 20f;
				rect.xMin = rect.xMax - 80f;
				Rect butRect = rect.RightPartPixels(30f);
				rect = rect.LeftPartPixels(rect.width - 30f);
				Widgets.CheckboxLabeled(rect, "Edit", ref this.editMode, false, null, null, false);
				if (Widgets.ButtonImageFitted(butRect, TexButton.Copy))
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (ResearchProjectDef researchProjectDef in from def in DefDatabase<ResearchProjectDef>.AllDefsListForReading
					where def.Debug_IsPositionModified()
					select def)
					{
						stringBuilder.AppendLine(researchProjectDef.defName);
						stringBuilder.AppendLine(string.Format("  <researchViewX>{0}</researchViewX>", researchProjectDef.ResearchViewX.ToString("F2")));
						stringBuilder.AppendLine(string.Format("  <researchViewY>{0}</researchViewY>", researchProjectDef.ResearchViewY.ToString("F2")));
						stringBuilder.AppendLine();
					}
					GUIUtility.systemCopyBuffer = stringBuilder.ToString();
					Messages.Message("Modified data copied to clipboard.", MessageTypeDefOf.SituationResolved, false);
				}
			}
			else
			{
				this.editMode = false;
			}
			Rect outRect = rightOutRect.ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, this.rightViewWidth, this.rightViewHeight);
			Rect position = rect2.ContractedBy(10f);
			rect2.width = this.rightViewWidth;
			position = rect2.ContractedBy(10f);
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			Widgets.ScrollHorizontal(outRect, ref this.rightScrollPosition, rect2, 20f);
			Widgets.BeginScrollView(outRect, ref this.rightScrollPosition, rect2, true);
			GUI.BeginGroup(position);
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					ResearchProjectDef researchProjectDef2 = allDefsListForReading[j];
					if (researchProjectDef2.tab == this.CurTab)
					{
						start.x = this.PosX(researchProjectDef2);
						start.y = this.PosY(researchProjectDef2) + 25f;
						for (int k = 0; k < researchProjectDef2.prerequisites.CountAllowNull<ResearchProjectDef>(); k++)
						{
							ResearchProjectDef researchProjectDef3 = researchProjectDef2.prerequisites[k];
							if (researchProjectDef3 != null && researchProjectDef3.tab == this.CurTab)
							{
								end.x = this.PosX(researchProjectDef3) + 140f;
								end.y = this.PosY(researchProjectDef3) + 25f;
								if (this.selectedProject == researchProjectDef2 || this.selectedProject == researchProjectDef3)
								{
									if (i == 1)
									{
										Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
									}
								}
								else if (i == 0)
								{
									Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
								}
							}
						}
					}
				}
			}
			for (int l = 0; l < allDefsListForReading.Count; l++)
			{
				ResearchProjectDef researchProjectDef4 = allDefsListForReading[l];
				if (researchProjectDef4.tab == this.CurTab)
				{
					Rect source = new Rect(this.PosX(researchProjectDef4), this.PosY(researchProjectDef4), 140f, 50f);
					string label = this.GetLabel(researchProjectDef4);
					Rect rect3 = new Rect(source);
					Color textColor = Widgets.NormalOptionColor;
					Color color = default(Color);
					Color borderColor = default(Color);
					bool flag = !researchProjectDef4.IsFinished && !researchProjectDef4.CanStartNow;
					if (researchProjectDef4 == Find.ResearchManager.currentProj)
					{
						color = TexUI.ActiveResearchColor;
					}
					else if (researchProjectDef4.IsFinished)
					{
						color = TexUI.FinishedResearchColor;
					}
					else if (flag)
					{
						color = TexUI.LockedResearchColor;
					}
					else if (researchProjectDef4.CanStartNow)
					{
						color = TexUI.AvailResearchColor;
					}
					if (this.selectedProject == researchProjectDef4)
					{
						color += TexUI.HighlightBgResearchColor;
						borderColor = TexUI.HighlightBorderResearchColor;
					}
					else
					{
						borderColor = TexUI.DefaultBorderResearchColor;
					}
					if (flag)
					{
						textColor = MainTabWindow_Research.ProjectWithMissingPrerequisiteLabelColor;
					}
					for (int m = 0; m < researchProjectDef4.prerequisites.CountAllowNull<ResearchProjectDef>(); m++)
					{
						ResearchProjectDef researchProjectDef5 = researchProjectDef4.prerequisites[m];
						if (researchProjectDef5 != null)
						{
							if (this.selectedProject == researchProjectDef5)
							{
								borderColor = TexUI.HighlightLineResearchColor;
							}
						}
					}
					if (this.requiredByThisFound)
					{
						for (int n = 0; n < researchProjectDef4.requiredByThis.CountAllowNull<ResearchProjectDef>(); n++)
						{
							ResearchProjectDef researchProjectDef6 = researchProjectDef4.requiredByThis[n];
							if (this.selectedProject == researchProjectDef6)
							{
								borderColor = TexUI.HighlightLineResearchColor;
							}
						}
					}
					if (Widgets.CustomButtonText(ref rect3, label, color, textColor, borderColor, true, 1, true, true))
					{
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						this.selectedProject = researchProjectDef4;
					}
					if (this.editMode && Mouse.IsOver(rect3) && Input.GetMouseButtonDown(0))
					{
						this.draggingTab = researchProjectDef4;
					}
				}
			}
			GUI.EndGroup();
			Widgets.EndScrollView();
			if (!Input.GetMouseButton(0))
			{
				this.draggingTab = null;
			}
			if (this.draggingTab != null && !Input.GetMouseButtonDown(0) && Event.current.type == EventType.Layout)
			{
				this.draggingTab.Debug_ApplyPositionDelta(new Vector2(this.PixelsToCoordX(Event.current.delta.x), this.PixelsToCoordY(Event.current.delta.y)));
			}
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x001ACB98 File Offset: 0x001AAF98
		private float DrawResearchPrereqs(ResearchProjectDef project, Rect rect)
		{
			float result;
			if (project.prerequisites.NullOrEmpty<ResearchProjectDef>())
			{
				result = 0f;
			}
			else
			{
				float yMin = rect.yMin;
				Widgets.LabelCacheHeight(ref rect, "ResearchPrerequisites".Translate() + ":", true, false);
				rect.yMin += rect.height;
				for (int i = 0; i < project.prerequisites.Count; i++)
				{
					this.SetPrerequisiteStatusColor(project.prerequisites[i].IsFinished, project);
					Widgets.LabelCacheHeight(ref rect, "  " + project.prerequisites[i].LabelCap, true, false);
					rect.yMin += rect.height;
				}
				GUI.color = Color.white;
				result = rect.yMin - yMin;
			}
			return result;
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x001ACC84 File Offset: 0x001AB084
		private float DrawResearchBenchRequirements(ResearchProjectDef project, Rect rect)
		{
			float yMin = rect.yMin;
			if (project.requiredResearchBuilding != null)
			{
				bool present = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].listerBuildings.allBuildingsColonist.Find((Building x) => x.def == project.requiredResearchBuilding) != null)
					{
						present = true;
						break;
					}
				}
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBench".Translate() + ":", true, false);
				rect.yMin += rect.height;
				this.SetPrerequisiteStatusColor(present, project);
				Widgets.LabelCacheHeight(ref rect, "  " + project.requiredResearchBuilding.LabelCap, true, false);
				rect.yMin += rect.height;
				GUI.color = Color.white;
			}
			if (!project.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBenchFacilities".Translate() + ":", true, false);
				rect.yMin += rect.height;
				Building_ResearchBench building_ResearchBench = this.FindBenchFulfillingMostRequirements(project.requiredResearchBuilding, project.requiredResearchFacilities);
				CompAffectedByFacilities bestMatchingBench = null;
				if (building_ResearchBench != null)
				{
					bestMatchingBench = building_ResearchBench.TryGetComp<CompAffectedByFacilities>();
				}
				for (int j = 0; j < project.requiredResearchFacilities.Count; j++)
				{
					this.DrawResearchBenchFacilityRequirement(project.requiredResearchFacilities[j], bestMatchingBench, project, ref rect);
					rect.yMin += 15f;
				}
			}
			GUI.color = Color.white;
			return rect.yMin - yMin;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x001ACE84 File Offset: 0x001AB284
		private string GetLabel(ResearchProjectDef r)
		{
			return r.LabelCap + "\n(" + r.CostApparent.ToString("F0") + ")";
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x001ACEC1 File Offset: 0x001AB2C1
		private void SetPrerequisiteStatusColor(bool present, ResearchProjectDef project)
		{
			if (!project.IsFinished)
			{
				if (present)
				{
					GUI.color = MainTabWindow_Research.FulfilledPrerequisiteColor;
				}
				else
				{
					GUI.color = MainTabWindow_Research.MissingPrerequisiteColor;
				}
			}
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x001ACEF4 File Offset: 0x001AB2F4
		private void DrawResearchBenchFacilityRequirement(ThingDef requiredFacility, CompAffectedByFacilities bestMatchingBench, ResearchProjectDef project, ref Rect rect)
		{
			Thing thing = null;
			Thing thing2 = null;
			if (bestMatchingBench != null)
			{
				thing = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility);
				thing2 = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility && bestMatchingBench.IsFacilityActive(x));
			}
			this.SetPrerequisiteStatusColor(thing2 != null, project);
			string text = requiredFacility.LabelCap;
			if (thing != null && thing2 == null)
			{
				text = text + " (" + "InactiveFacility".Translate() + ")";
			}
			Widgets.LabelCacheHeight(ref rect, "  " + text, true, false);
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x001ACFB8 File Offset: 0x001AB3B8
		private Building_ResearchBench FindBenchFulfillingMostRequirements(ThingDef requiredResearchBench, List<ThingDef> requiredFacilities)
		{
			MainTabWindow_Research.tmpAllBuildings.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				MainTabWindow_Research.tmpAllBuildings.AddRange(maps[i].listerBuildings.allBuildingsColonist);
			}
			float num = 0f;
			Building_ResearchBench building_ResearchBench = null;
			for (int j = 0; j < MainTabWindow_Research.tmpAllBuildings.Count; j++)
			{
				Building_ResearchBench building_ResearchBench2 = MainTabWindow_Research.tmpAllBuildings[j] as Building_ResearchBench;
				if (building_ResearchBench2 != null)
				{
					if (requiredResearchBench == null || building_ResearchBench2.def == requiredResearchBench)
					{
						float researchBenchRequirementsScore = this.GetResearchBenchRequirementsScore(building_ResearchBench2, requiredFacilities);
						if (building_ResearchBench == null || researchBenchRequirementsScore > num)
						{
							num = researchBenchRequirementsScore;
							building_ResearchBench = building_ResearchBench2;
						}
					}
				}
			}
			MainTabWindow_Research.tmpAllBuildings.Clear();
			return building_ResearchBench;
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x001AD0A0 File Offset: 0x001AB4A0
		private float GetResearchBenchRequirementsScore(Building_ResearchBench bench, List<ThingDef> requiredFacilities)
		{
			float num = 0f;
			int i;
			for (i = 0; i < requiredFacilities.Count; i++)
			{
				CompAffectedByFacilities benchComp = bench.GetComp<CompAffectedByFacilities>();
				if (benchComp != null)
				{
					List<Thing> linkedFacilitiesListForReading = benchComp.LinkedFacilitiesListForReading;
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacilities[i] && benchComp.IsFacilityActive(x)) != null)
					{
						num += 1f;
					}
					else if (linkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacilities[i]) != null)
					{
						num += 0.6f;
					}
				}
			}
			return num;
		}

		// Token: 0x04001AA1 RID: 6817
		protected ResearchProjectDef selectedProject = null;

		// Token: 0x04001AA2 RID: 6818
		private bool noBenchWarned = false;

		// Token: 0x04001AA3 RID: 6819
		private bool requiredByThisFound = false;

		// Token: 0x04001AA4 RID: 6820
		private Vector2 leftScrollPosition = Vector2.zero;

		// Token: 0x04001AA5 RID: 6821
		private float leftScrollViewHeight = 0f;

		// Token: 0x04001AA6 RID: 6822
		private Vector2 rightScrollPosition = default(Vector2);

		// Token: 0x04001AA7 RID: 6823
		private float rightViewWidth;

		// Token: 0x04001AA8 RID: 6824
		private float rightViewHeight;

		// Token: 0x04001AA9 RID: 6825
		private ResearchTabDef curTabInt = null;

		// Token: 0x04001AAA RID: 6826
		private bool editMode = false;

		// Token: 0x04001AAB RID: 6827
		private ResearchProjectDef draggingTab = null;

		// Token: 0x04001AAC RID: 6828
		private const float LeftAreaWidth = 200f;

		// Token: 0x04001AAD RID: 6829
		private const int ModeSelectButHeight = 40;

		// Token: 0x04001AAE RID: 6830
		private const float ProjectTitleHeight = 50f;

		// Token: 0x04001AAF RID: 6831
		private const float ProjectTitleLeftMargin = 0f;

		// Token: 0x04001AB0 RID: 6832
		private const float localPadding = 20f;

		// Token: 0x04001AB1 RID: 6833
		private const int ResearchItemW = 140;

		// Token: 0x04001AB2 RID: 6834
		private const int ResearchItemH = 50;

		// Token: 0x04001AB3 RID: 6835
		private const int ResearchItemPaddingW = 50;

		// Token: 0x04001AB4 RID: 6836
		private const int ResearchItemPaddingH = 50;

		// Token: 0x04001AB5 RID: 6837
		private const float PrereqsLineSpacing = 15f;

		// Token: 0x04001AB6 RID: 6838
		private const int ColumnMaxProjects = 6;

		// Token: 0x04001AB7 RID: 6839
		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x04001AB8 RID: 6840
		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

		// Token: 0x04001AB9 RID: 6841
		private static readonly Color FulfilledPrerequisiteColor = Color.green;

		// Token: 0x04001ABA RID: 6842
		private static readonly Color MissingPrerequisiteColor = Color.red;

		// Token: 0x04001ABB RID: 6843
		private static readonly Color ProjectWithMissingPrerequisiteLabelColor = Color.gray;

		// Token: 0x04001ABC RID: 6844
		private static List<Building> tmpAllBuildings = new List<Building>();
	}
}
