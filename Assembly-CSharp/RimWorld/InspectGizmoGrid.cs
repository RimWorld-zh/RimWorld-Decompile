using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal static class InspectGizmoGrid
	{
		public static Gizmo mouseoverGizmo;

		private static List<object> objList = new List<object>();

		private static List<Gizmo> gizmoList = new List<Gizmo>();

		public static void DrawInspectGizmoGridFor(IEnumerable<object> selectedObjects)
		{
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
						foreach (Gizmo gizmo in selectable.GetGizmos())
						{
							InspectGizmoGrid.gizmoList.Add(gizmo);
						}
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
								command_Action.icon = des.IconReverseDesignating(t);
								command_Action.defaultDesc = des.DescReverseDesignating(t);
								command_Action.action = (Action)delegate
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
				List<Gizmo> gizmos = InspectGizmoGrid.gizmoList;
				Vector2 paneSize = InspectPaneUtility.PaneSize;
				GizmoGridDrawer.DrawGizmoGrid((IEnumerable<Gizmo>)gizmos, (float)(paneSize.x + 20.0), out InspectGizmoGrid.mouseoverGizmo);
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 3427734);
			}
		}
	}
}
