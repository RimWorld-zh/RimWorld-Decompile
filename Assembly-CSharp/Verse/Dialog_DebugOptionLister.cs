using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		private const float DebugOptionsGap = 7f;

		protected Dialog_DebugOptionLister()
		{
		}

		protected bool DebugAction(string label, Action action)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label))
			{
				this.Close(true);
				action();
				result = true;
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
			return result;
		}

		protected void DebugToolMap(string label, Action toolAction)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				if (!base.FilterAllows(label))
				{
					GUI.color = new Color(1f, 1f, 1f, 0.3f);
				}
				if (this.listing.ButtonDebug(label))
				{
					this.Close(true);
					DebugTools.curTool = new DebugTool(label, toolAction, null);
				}
				GUI.color = Color.white;
				if (Event.current.type == EventType.Layout)
				{
					this.totalOptionsHeight += 24f;
				}
			}
		}

		protected void DebugToolMapForPawns(string label, Action<Pawn> pawnAction)
		{
			this.DebugToolMap(label, delegate
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					foreach (Pawn obj in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().ToList<Pawn>())
					{
						pawnAction(obj);
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
				if (this.listing.ButtonDebug(label))
				{
					this.Close(true);
					DebugTools.curTool = new DebugTool(label, toolAction, null);
				}
				GUI.color = Color.white;
				if (Event.current.type == EventType.Layout)
				{
					this.totalOptionsHeight += 24f;
				}
			}
		}

		protected void CheckboxLabeledDebug(string label, ref bool checkOn)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			this.listing.LabelCheckboxDebug(label, ref checkOn);
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		protected void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
		}

		protected void DoGap()
		{
			this.listing.Gap(7f);
			this.totalOptionsHeight += 7f;
		}

		[CompilerGenerated]
		private sealed class <DebugToolMapForPawns>c__AnonStorey0
		{
			internal Action<Pawn> pawnAction;

			private static Func<Thing, bool> <>f__am$cache0;

			public <DebugToolMapForPawns>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					foreach (Pawn obj in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().ToList<Pawn>())
					{
						this.pawnAction(obj);
					}
				}
			}

			private static bool <>m__1(Thing t)
			{
				return t is Pawn;
			}
		}
	}
}
