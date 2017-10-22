using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class Command_SetPlantToGrow : Command
	{
		public IPlantToGrowSettable settable;

		private List<IPlantToGrowSettable> settables;

		private static readonly Texture2D SetPlantToGrowTex = ContentFinder<Texture2D>.Get("UI/Commands/SetPlantToGrow", true);

		public Command_SetPlantToGrow()
		{
			base.tutorTag = "GrowingZoneSetPlant";
			ThingDef thingDef = null;
			bool flag = false;
			List<object>.Enumerator enumerator = Find.Selector.SelectedObjects.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					IPlantToGrowSettable plantToGrowSettable = current as IPlantToGrowSettable;
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
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			if (flag)
			{
				base.icon = Command_SetPlantToGrow.SetPlantToGrowTex;
				base.defaultLabel = "CommandSelectPlantToGrowMulti".Translate();
			}
			else
			{
				base.icon = thingDef.uiIcon;
				base.defaultLabel = "CommandSelectPlantToGrow".Translate(thingDef.label);
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
			foreach (ThingDef item in GenPlant.ValidPlantTypesForGrowers(this.settables))
			{
				if (this.IsPlantAvailable(item))
				{
					ThingDef localPlantDef = item;
					string text = item.LabelCap;
					if (item.plant.sowMinSkill > 0)
					{
						string text2 = text;
						text = text2 + " (" + "MinSkill".Translate() + ": " + item.plant.sowMinSkill + ")";
					}
					List<FloatMenuOption> obj = list;
					Func<Rect, bool> extraPartOnGUI = (Func<Rect, bool>)((Rect rect) => Widgets.InfoCardButton((float)(rect.x + 5.0), (float)(rect.y + (rect.height - 24.0) / 2.0), localPlantDef));
					obj.Add(new FloatMenuOption(text, (Action)delegate
					{
						string s = base.tutorTag + "-" + localPlantDef.defName;
						if (TutorSystem.AllowAction(s))
						{
							for (int i = 0; i < this.settables.Count; i++)
							{
								this.settables[i].SetPlantDefToGrow(localPlantDef);
							}
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SetGrowingZonePlant, KnowledgeAmount.Total);
							this.WarnAsAppropriate(localPlantDef);
							TutorSystem.Notify_Event(s);
						}
					}, MenuOptionPriority.Default, null, null, 29f, extraPartOnGUI, null));
				}
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
				foreach (Pawn item in this.settable.Map.mapPawns.FreeColonistsSpawned)
				{
					if (item.skills.GetSkill(SkillDefOf.Growing).Level >= plantDef.plant.sowMinSkill && !item.Downed && item.workSettings.WorkIsActive(WorkTypeDefOf.Growing))
						return;
				}
				Find.WindowStack.Add(new Dialog_MessageBox("NoGrowerCanPlant".Translate(plantDef.label, plantDef.plant.sowMinSkill).CapitalizeFirst(), (string)null, null, (string)null, null, (string)null, false));
			}
		}

		private bool IsPlantAvailable(ThingDef plantDef)
		{
			List<ResearchProjectDef> sowResearchPrerequisites = plantDef.plant.sowResearchPrerequisites;
			if (sowResearchPrerequisites == null)
			{
				return true;
			}
			for (int i = 0; i < sowResearchPrerequisites.Count; i++)
			{
				if (!sowResearchPrerequisites[i].IsFinished)
				{
					return false;
				}
			}
			return true;
		}
	}
}
