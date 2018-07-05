using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class InspectGizmoGrid
	{
		private static List<object> objList = new List<object>();

		private static List<Gizmo> gizmoList = new List<Gizmo>();

		public static void DrawInspectGizmoGridFor(IEnumerable<object> selectedObjects, out Gizmo mouseoverGizmo)
		{
			mouseoverGizmo = null;
			try
			{
				InspectGizmoGrid.objList.Clear();
				InspectGizmoGrid.objList.AddRange(selectedObjects);
				InspectGizmoGrid.gizmoList.Clear();
				for (int i = 0; i < InspectGizmoGrid.objList.Count; i++)
				{
					ISelectable selectable = InspectGizmoGrid.objList[i] as ISelectable;
					if (selectable != null)
					{
						InspectGizmoGrid.gizmoList.AddRange(selectable.GetGizmos());
					}
				}
				for (int j = 0; j < InspectGizmoGrid.objList.Count; j++)
				{
					Thing t = InspectGizmoGrid.objList[j] as Thing;
					if (t != null)
					{
						List<Designator> allDesignators = Find.ReverseDesignatorDatabase.AllDesignators;
						for (int k = 0; k < allDesignators.Count; k++)
						{
							Designator des = allDesignators[k];
							if (des.CanDesignateThing(t).Accepted)
							{
								Command_Action command_Action = new Command_Action();
								command_Action.defaultLabel = des.LabelCapReverseDesignating(t);
								float iconAngle;
								Vector2 iconOffset;
								command_Action.icon = des.IconReverseDesignating(t, out iconAngle, out iconOffset);
								command_Action.iconAngle = iconAngle;
								command_Action.iconOffset = iconOffset;
								command_Action.defaultDesc = des.DescReverseDesignating(t);
								command_Action.order = ((!(des is Designator_Uninstall)) ? -20f : -11f);
								command_Action.action = delegate()
								{
									if (TutorSystem.AllowAction(des.TutorTagDesignate))
									{
										des.DesignateThing(t);
										des.Finalize(true);
									}
								};
								command_Action.hotKey = des.hotKey;
								command_Action.groupKey = des.groupKey;
								InspectGizmoGrid.gizmoList.Add(command_Action);
							}
						}
					}
				}
				InspectGizmoGrid.objList.Clear();
				GizmoGridDrawer.DrawGizmoGrid(InspectGizmoGrid.gizmoList, InspectPaneUtility.PaneWidthFor(Find.WindowStack.WindowOfType<IInspectPane>()) + 20f, out mouseoverGizmo);
				InspectGizmoGrid.gizmoList.Clear();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 3427734, false);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static InspectGizmoGrid()
		{
		}

		[CompilerGenerated]
		private sealed class <DrawInspectGizmoGridFor>c__AnonStorey1
		{
			internal Thing t;

			public <DrawInspectGizmoGridFor>c__AnonStorey1()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DrawInspectGizmoGridFor>c__AnonStorey0
		{
			internal Designator des;

			internal InspectGizmoGrid.<DrawInspectGizmoGridFor>c__AnonStorey1 <>f__ref$1;

			public <DrawInspectGizmoGridFor>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (TutorSystem.AllowAction(this.des.TutorTagDesignate))
				{
					this.des.DesignateThing(this.<>f__ref$1.t);
					this.des.Finalize(true);
				}
			}
		}
	}
}
