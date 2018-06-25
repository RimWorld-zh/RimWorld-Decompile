using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class Command_SetPlantToGrow : Command
	{
		public IPlantToGrowSettable settable;

		private List<IPlantToGrowSettable> settables;

		private static List<ThingDef> tmpAvailablePlants = new List<ThingDef>();

		private static readonly Texture2D SetPlantToGrowTex = ContentFinder<Texture2D>.Get("UI/Commands/SetPlantToGrow", true);

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache0;

		public Command_SetPlantToGrow()
		{
			this.tutorTag = "GrowingZoneSetPlant";
			ThingDef thingDef = null;
			bool flag = false;
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				IPlantToGrowSettable plantToGrowSettable = obj as IPlantToGrowSettable;
				if (plantToGrowSettable != null)
				{
					if (thingDef != null && thingDef != plantToGrowSettable.GetPlantDefToGrow())
					{
						flag = true;
						break;
					}
					thingDef = plantToGrowSettable.GetPlantDefToGrow();
				}
			}
			if (flag)
			{
				this.icon = Command_SetPlantToGrow.SetPlantToGrowTex;
				this.defaultLabel = "CommandSelectPlantToGrowMulti".Translate();
			}
			else
			{
				this.icon = thingDef.uiIcon;
				this.iconAngle = thingDef.uiIconAngle;
				this.iconOffset = thingDef.uiIconOffset;
				this.defaultLabel = "CommandSelectPlantToGrow".Translate(new object[]
				{
					thingDef.LabelCap
				});
			}
		}

		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (this.settables == null)
			{
				this.settables = new List<IPlantToGrowSettable>();
			}
			if (!this.settables.Contains(this.settable))
			{
				this.settables.Add(this.settable);
			}
			Command_SetPlantToGrow.tmpAvailablePlants.Clear();
			foreach (ThingDef thingDef in GenPlant.ValidPlantTypesForGrowers(this.settables))
			{
				if (this.IsPlantAvailable(thingDef, this.settable.Map))
				{
					Command_SetPlantToGrow.tmpAvailablePlants.Add(thingDef);
				}
			}
			Command_SetPlantToGrow.tmpAvailablePlants.SortBy((ThingDef x) => -this.GetPlantListPriority(x), (ThingDef x) => x.label);
			for (int i = 0; i < Command_SetPlantToGrow.tmpAvailablePlants.Count; i++)
			{
				ThingDef plantDef = Command_SetPlantToGrow.tmpAvailablePlants[i];
				string text = plantDef.LabelCap;
				if (plantDef.plant.sowMinSkill > 0)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						" (",
						"MinSkill".Translate(),
						": ",
						plantDef.plant.sowMinSkill,
						")"
					});
				}
				list.Add(new FloatMenuOption(text, delegate()
				{
					string s = this.tutorTag + "-" + plantDef.defName;
					if (TutorSystem.AllowAction(s))
					{
						bool flag = true;
						for (int j = 0; j < this.settables.Count; j++)
						{
							this.settables[j].SetPlantDefToGrow(plantDef);
							if (flag && plantDef.plant.interferesWithRoof)
							{
								foreach (IntVec3 c in this.settables[j].Cells)
								{
									if (c.Roofed(this.settables[j].Map))
									{
										Messages.Message("MessagePlantIncompatibleWithRoof".Translate(new object[]
										{
											Find.ActiveLanguageWorker.Pluralize(plantDef.LabelCap, -1)
										}), MessageTypeDefOf.CautionInput, false);
										flag = false;
										break;
									}
								}
							}
						}
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SetGrowingZonePlant, KnowledgeAmount.Total);
						this.WarnAsAppropriate(plantDef);
						TutorSystem.Notify_Event(s);
					}
				}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, plantDef), null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.settables == null)
			{
				this.settables = new List<IPlantToGrowSettable>();
			}
			this.settables.Add(((Command_SetPlantToGrow)other).settable);
			return false;
		}

		private void WarnAsAppropriate(ThingDef plantDef)
		{
			if (plantDef.plant.sowMinSkill > 0)
			{
				foreach (Pawn pawn in this.settable.Map.mapPawns.FreeColonistsSpawned)
				{
					if (pawn.skills.GetSkill(SkillDefOf.Plants).Level >= plantDef.plant.sowMinSkill && !pawn.Downed && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Growing))
					{
						return;
					}
				}
				Find.WindowStack.Add(new Dialog_MessageBox("NoGrowerCanPlant".Translate(new object[]
				{
					plantDef.label,
					plantDef.plant.sowMinSkill
				}).CapitalizeFirst(), null, null, null, null, null, false, null, null));
			}
			if (plantDef.plant.cavePlant)
			{
				IntVec3 cell = IntVec3.Invalid;
				for (int i = 0; i < this.settables.Count; i++)
				{
					foreach (IntVec3 intVec in this.settables[i].Cells)
					{
						if (!intVec.Roofed(this.settables[i].Map) || this.settables[i].Map.glowGrid.GameGlowAt(intVec, true) > 0f)
						{
							cell = intVec;
							break;
						}
					}
					if (cell.IsValid)
					{
						break;
					}
				}
				if (cell.IsValid)
				{
					Messages.Message("MessageWarningCavePlantsExposedToLight".Translate(new object[]
					{
						plantDef.LabelCap
					}).CapitalizeFirst(), new TargetInfo(cell, this.settable.Map, false), MessageTypeDefOf.RejectInput, true);
				}
			}
		}

		private bool IsPlantAvailable(ThingDef plantDef, Map map)
		{
			List<ResearchProjectDef> sowResearchPrerequisites = plantDef.plant.sowResearchPrerequisites;
			bool result;
			if (sowResearchPrerequisites == null)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < sowResearchPrerequisites.Count; i++)
				{
					if (!sowResearchPrerequisites[i].IsFinished)
					{
						return false;
					}
				}
				result = (!plantDef.plant.mustBeWildToSow || map.Biome.AllWildPlants.Contains(plantDef));
			}
			return result;
		}

		private float GetPlantListPriority(ThingDef plantDef)
		{
			float result;
			if (plantDef.plant.IsTree)
			{
				result = 1f;
			}
			else
			{
				switch (plantDef.plant.purpose)
				{
				case PlantPurpose.Food:
					result = 4f;
					break;
				case PlantPurpose.Health:
					result = 3f;
					break;
				case PlantPurpose.Beauty:
					result = 2f;
					break;
				case PlantPurpose.Misc:
					result = 0f;
					break;
				default:
					result = 0f;
					break;
				}
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Command_SetPlantToGrow()
		{
		}

		[CompilerGenerated]
		private float <ProcessInput>m__0(ThingDef x)
		{
			return -this.GetPlantListPriority(x);
		}

		[CompilerGenerated]
		private static string <ProcessInput>m__1(ThingDef x)
		{
			return x.label;
		}

		[CompilerGenerated]
		private sealed class <ProcessInput>c__AnonStorey0
		{
			internal ThingDef plantDef;

			internal Command_SetPlantToGrow $this;

			public <ProcessInput>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				string s = this.$this.tutorTag + "-" + this.plantDef.defName;
				if (TutorSystem.AllowAction(s))
				{
					bool flag = true;
					for (int i = 0; i < this.$this.settables.Count; i++)
					{
						this.$this.settables[i].SetPlantDefToGrow(this.plantDef);
						if (flag && this.plantDef.plant.interferesWithRoof)
						{
							foreach (IntVec3 c in this.$this.settables[i].Cells)
							{
								if (c.Roofed(this.$this.settables[i].Map))
								{
									Messages.Message("MessagePlantIncompatibleWithRoof".Translate(new object[]
									{
										Find.ActiveLanguageWorker.Pluralize(this.plantDef.LabelCap, -1)
									}), MessageTypeDefOf.CautionInput, false);
									flag = false;
									break;
								}
							}
						}
					}
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SetGrowingZonePlant, KnowledgeAmount.Total);
					this.$this.WarnAsAppropriate(this.plantDef);
					TutorSystem.Notify_Event(s);
				}
			}

			internal bool <>m__1(Rect rect)
			{
				return Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, this.plantDef);
			}
		}
	}
}
