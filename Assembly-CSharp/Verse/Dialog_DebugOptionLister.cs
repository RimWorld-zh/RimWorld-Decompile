using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		protected void DebugAction(string label, Action action)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (base.listing.ButtonDebug(label))
			{
				this.Close(true);
				action();
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				base.totalOptionsHeight += 24f;
			}
		}

		protected void DebugToolMap(string label, Action toolAction)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				if (!base.FilterAllows(label))
				{
					GUI.color = new Color(1f, 1f, 1f, 0.3f);
				}
				if (base.listing.ButtonDebug(label))
				{
					this.Close(true);
					DebugTools.curTool = new DebugTool(label, toolAction, null);
				}
				GUI.color = Color.white;
				if (Event.current.type == EventType.Layout)
				{
					base.totalOptionsHeight += 24f;
				}
			}
		}

		protected void DebugToolMapForPawns(string label, Action<Pawn> pawnAction)
		{
			this.DebugToolMap(label, (Action)delegate()
			{
				if (UI.MouseCell().InBounds(Find.VisibleMap))
				{
					List<Pawn>.Enumerator enumerator = (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().ToList().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Pawn current = enumerator.Current;
							pawnAction(current);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
				}
			});
		}

		protected void DebugToolWorld(string label, Action toolAction)
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (!base.FilterAllows(label))
				{
					GUI.color = new Color(1f, 1f, 1f, 0.3f);
				}
				if (base.listing.ButtonDebug(label))
				{
					this.Close(true);
					DebugTools.curTool = new DebugTool(label, toolAction, null);
				}
				GUI.color = Color.white;
				if (Event.current.type == EventType.Layout)
				{
					base.totalOptionsHeight += 24f;
				}
			}
		}

		protected void CheckboxLabeledDebug(string label, ref bool checkOn)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			base.listing.LabelCheckboxDebug(label, ref checkOn);
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				base.totalOptionsHeight += 24f;
			}
		}
	}
}
