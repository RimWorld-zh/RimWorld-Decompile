using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class MainTabWindow_Research : MainTabWindow
	{
		private const float LeftAreaWidth = 200f;

		private const int ModeSelectButHeight = 40;

		private const float ProjectTitleHeight = 50f;

		private const float ProjectTitleLeftMargin = 0f;

		private const float localPadding = 20f;

		private const int ResearchItemW = 140;

		private const int ResearchItemH = 50;

		private const int ResearchItemPaddingW = 50;

		private const int ResearchItemPaddingH = 50;

		private const float PrereqsLineSpacing = 15f;

		private const int ColumnMaxProjects = 6;

		protected ResearchProjectDef selectedProject;

		private bool noBenchWarned;

		private bool requiredByThisFound;

		private Vector2 leftScrollPosition = Vector2.zero;

		private float leftScrollViewHeight;

		private Vector2 rightScrollPosition = default(Vector2);

		private float rightViewWidth;

		private float rightViewHeight;

		private ResearchTabDef curTabInt;

		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

		private static readonly Color FulfilledPrerequisiteColor = Color.green;

		private static readonly Color MissingPrerequisiteColor = Color.red;

		private static readonly Color ProjectWithMissingPrerequisiteLabelColor = Color.gray;

		private static List<Building> tmpAllBuildings = new List<Building>();

		private ResearchTabDef CurTab
		{
			get
			{
				return this.curTabInt;
			}
			set
			{
				if (value == this.curTabInt)
				{
					return;
				}
				this.curTabInt = value;
				Vector2 vector = this.ViewSize(this.CurTab);
				this.rightViewWidth = vector.x;
				this.rightViewHeight = vector.y;
				this.rightScrollPosition = Vector2.zero;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				Vector2 initialSize = base.InitialSize;
				float b = (float)(UI.screenHeight - 35);
				float b2 = 52f + DefDatabase<ResearchTabDef>.AllDefs.Max((ResearchTabDef tab) => this.ViewSize(tab).y) + 50f + 10f + 10f;
				float a = Mathf.Max(initialSize.y, b2);
				initialSize.y = Mathf.Min(a, b);
				return initialSize;
			}
		}

		private Vector2 ViewSize(ResearchTabDef tab)
		{
			IEnumerable<ResearchProjectDef> source = from d in DefDatabase<ResearchProjectDef>.AllDefs
			where d.tab == tab
			select d;
			if (!source.Any<ResearchProjectDef>())
			{
				return Vector2.zero;
			}
			return new Vector2(source.Max((ResearchProjectDef d) => this.PosX(d)) + 140f + 10f + 40f, source.Max((ResearchProjectDef d) => this.PosY(d)) + 50f + 10f);
		}

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
					Find.WindowStack.Add(new Dialog_MessageBox("ResearchMenuWithoutBench".Translate(), null, null, null, null, null, false));
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

		private void DrawLeftRect(Rect leftOutRect)
		{
			Rect position = leftOutRect;
			GUI.BeginGroup(position);
			if (this.selectedProject != null)
			{
				Rect outRect = new Rect(0f, 0f, position.width, 500f);
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.leftScrollViewHeight);
				Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect);
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
				string label = string.Concat(new string[]
				{
					"ProjectTechLevel".Translate().CapitalizeFirst(),
					": ",
					this.selectedProject.techLevel.ToStringHuman(),
					"\n",
					"YourTechLevel".Translate().CapitalizeFirst(),
					": ",
					Faction.OfPlayer.def.techLevel.ToStringHuman(),
					"\n",
					"ResearchCostMultiplier".Translate().CapitalizeFirst(),
					": ",
					this.selectedProject.CostFactor(Faction.OfPlayer.def.techLevel).ToStringPercent(),
					"\n",
					"ResearchCostComparison".Translate(new object[]
					{
						this.selectedProject.baseCost.ToString("F0"),
						this.selectedProject.CostApparent.ToString("F0")
					})
				});
				Rect rect3 = new Rect(0f, num, viewRect.width, 0f);
				Widgets.LabelCacheHeight(ref rect3, label, true, false);
				num = rect3.yMax + 10f;
				Rect rect4 = new Rect(0f, num, viewRect.width, 500f);
				float num2 = this.DrawResearchPrereqs(this.selectedProject, rect4);
				if (num2 > 0f)
				{
					num += num2 + 15f;
				}
				Rect rect5 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawResearchBenchRequirements(this.selectedProject, rect5);
				num += 3f;
				this.leftScrollViewHeight = num;
				Widgets.EndScrollView();
				bool flag = Prefs.DevMode && this.selectedProject.PrerequisitesCompleted && this.selectedProject != Find.ResearchManager.currentProj && !this.selectedProject.IsFinished;
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
					Widgets.DrawMenuSection(rect6, true);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect6, "Finished".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (this.selectedProject == Find.ResearchManager.currentProj)
				{
					Widgets.DrawMenuSection(rect6, true);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect6, "InProgress".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (!this.selectedProject.CanStartNow)
				{
					Widgets.DrawMenuSection(rect6, true);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect6, "Locked".Translate());
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (Widgets.ButtonText(rect6, "Research".Translate(), true, false, true))
				{
					SoundDef.Named("ResearchStart").PlayOneShotOnCamera(null);
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

		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}

		private float PosX(ResearchProjectDef d)
		{
			return this.CoordToPixelsX(d.ResearchViewX);
		}

		private float PosY(ResearchProjectDef d)
		{
			return this.CoordToPixelsY(d.ResearchViewY);
		}

		private void DrawRightRect(Rect rightOutRect)
		{
			rightOutRect.yMin += 32f;
			Widgets.DrawMenuSection(rightOutRect, true);
			List<TabRecord> list = new List<TabRecord>();
			foreach (ResearchTabDef current in DefDatabase<ResearchTabDef>.AllDefs)
			{
				ResearchTabDef localTabDef = current;
				list.Add(new TabRecord(localTabDef.LabelCap, delegate
				{
					this.CurTab = localTabDef;
				}, this.CurTab == localTabDef));
			}
			TabDrawer.DrawTabs(rightOutRect, list);
			Rect outRect = rightOutRect.ContractedBy(10f);
			Rect rect = new Rect(0f, 0f, this.rightViewWidth, this.rightViewHeight);
			Rect position = rect.ContractedBy(10f);
			rect.width = this.rightViewWidth;
			position = rect.ContractedBy(10f);
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			Widgets.ScrollHorizontal(outRect, ref this.rightScrollPosition, rect, 20f);
			Widgets.BeginScrollView(outRect, ref this.rightScrollPosition, rect);
			GUI.BeginGroup(position);
			for (int i = 0; i < 2; i++)
			{
				foreach (ResearchProjectDef current2 in from d in DefDatabase<ResearchProjectDef>.AllDefs
				where d.tab == this.CurTab
				select d)
				{
					start.x = this.PosX(current2);
					start.y = this.PosY(current2) + 25f;
					for (int j = 0; j < current2.prerequisites.CountAllowNull<ResearchProjectDef>(); j++)
					{
						ResearchProjectDef researchProjectDef = current2.prerequisites[j];
						if (researchProjectDef != null && researchProjectDef.tab == this.CurTab)
						{
							end.x = this.PosX(researchProjectDef) + 140f;
							end.y = this.PosY(researchProjectDef) + 25f;
							if (this.selectedProject == current2 || this.selectedProject == researchProjectDef)
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
			foreach (ResearchProjectDef current3 in from d in DefDatabase<ResearchProjectDef>.AllDefs
			where d.tab == this.CurTab
			select d)
			{
				Rect source = new Rect(this.PosX(current3), this.PosY(current3), 140f, 50f);
				string label = current3.LabelCap + "\n(" + current3.CostApparent.ToString("F0") + ")";
				Rect rect2 = new Rect(source);
				Color textColor = Widgets.NormalOptionColor;
				Color color = default(Color);
				Color borderColor = default(Color);
				bool flag = !current3.IsFinished && !current3.CanStartNow;
				if (current3 == Find.ResearchManager.currentProj)
				{
					color = TexUI.ActiveResearchColor;
				}
				else if (current3.IsFinished)
				{
					color = TexUI.FinishedResearchColor;
				}
				else if (flag)
				{
					color = TexUI.LockedResearchColor;
				}
				else if (current3.CanStartNow)
				{
					color = TexUI.AvailResearchColor;
				}
				if (this.selectedProject == current3)
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
				for (int k = 0; k < current3.prerequisites.CountAllowNull<ResearchProjectDef>(); k++)
				{
					ResearchProjectDef researchProjectDef2 = current3.prerequisites[k];
					if (researchProjectDef2 != null && this.selectedProject == researchProjectDef2)
					{
						borderColor = TexUI.HighlightLineResearchColor;
					}
				}
				if (this.requiredByThisFound)
				{
					for (int l = 0; l < current3.requiredByThis.CountAllowNull<ResearchProjectDef>(); l++)
					{
						ResearchProjectDef researchProjectDef3 = current3.requiredByThis[l];
						if (this.selectedProject == researchProjectDef3)
						{
							borderColor = TexUI.HighlightLineResearchColor;
						}
					}
				}
				if (Widgets.CustomButtonText(ref rect2, label, color, textColor, borderColor, true, 1, true, true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.selectedProject = current3;
				}
			}
			GUI.EndGroup();
			Widgets.EndScrollView();
		}

		private float DrawResearchPrereqs(ResearchProjectDef project, Rect rect)
		{
			if (project.prerequisites.NullOrEmpty<ResearchProjectDef>())
			{
				return 0f;
			}
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
			return rect.yMin - yMin;
		}

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

		private void SetPrerequisiteStatusColor(bool present, ResearchProjectDef project)
		{
			if (project.IsFinished)
			{
				return;
			}
			if (present)
			{
				GUI.color = MainTabWindow_Research.FulfilledPrerequisiteColor;
			}
			else
			{
				GUI.color = MainTabWindow_Research.MissingPrerequisiteColor;
			}
		}

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
	}
}
