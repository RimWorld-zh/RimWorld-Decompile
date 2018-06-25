using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E68 RID: 3688
	[StaticConstructorOnStartup]
	public class Command_SetPlantToGrow : Command
	{
		// Token: 0x04003992 RID: 14738
		public IPlantToGrowSettable settable;

		// Token: 0x04003993 RID: 14739
		private List<IPlantToGrowSettable> settables;

		// Token: 0x04003994 RID: 14740
		private static List<ThingDef> tmpAvailablePlants = new List<ThingDef>();

		// Token: 0x04003995 RID: 14741
		private static readonly Texture2D SetPlantToGrowTex = ContentFinder<Texture2D>.Get("UI/Commands/SetPlantToGrow", true);

		// Token: 0x060056DF RID: 22239 RVA: 0x002CBF58 File Offset: 0x002CA358
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

		// Token: 0x060056E0 RID: 22240 RVA: 0x002CC06C File Offset: 0x002CA46C
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

		// Token: 0x060056E1 RID: 22241 RVA: 0x002CC26C File Offset: 0x002CA66C
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.settables == null)
			{
				this.settables = new List<IPlantToGrowSettable>();
			}
			this.settables.Add(((Command_SetPlantToGrow)other).settable);
			return false;
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x002CC2B0 File Offset: 0x002CA6B0
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

		// Token: 0x060056E3 RID: 22243 RVA: 0x002CC4F4 File Offset: 0x002CA8F4
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

		// Token: 0x060056E4 RID: 22244 RVA: 0x002CC584 File Offset: 0x002CA984
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
	}
}
