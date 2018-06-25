using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085F RID: 2143
	internal static class InspectGizmoGrid
	{
		// Token: 0x04001A49 RID: 6729
		private static List<object> objList = new List<object>();

		// Token: 0x04001A4A RID: 6730
		private static List<Gizmo> gizmoList = new List<Gizmo>();

		// Token: 0x0600308E RID: 12430 RVA: 0x001A6434 File Offset: 0x001A4834
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
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 3427734, false);
			}
		}
	}
}
