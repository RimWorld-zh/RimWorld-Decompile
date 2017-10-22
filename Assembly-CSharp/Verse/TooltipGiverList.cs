using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public sealed class TooltipGiverList
	{
		private List<Thing> givers = new List<Thing>();

		public void Notify_ThingSpawned(Thing t)
		{
			if (!t.def.hasTooltip && !this.ShouldShowShotReport(t))
				return;
			this.givers.Add(t);
		}

		public void Notify_ThingDespawned(Thing t)
		{
			if (!t.def.hasTooltip && !this.ShouldShowShotReport(t))
				return;
			this.givers.Remove(t);
		}

		public void DispenseAllThingTooltips()
		{
			if (Event.current.type == EventType.Repaint && Find.WindowStack.FloatMenu == null)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				float cellSizePixels = Find.CameraDriver.CellSizePixels;
				Vector2 vector = new Vector2(cellSizePixels, cellSizePixels);
				Rect rect = new Rect(0f, 0f, vector.x, vector.y);
				int num = 0;
				for (int i = 0; i < this.givers.Count; i++)
				{
					Thing thing = this.givers[i];
					if (currentViewRect.Contains(thing.Position) && !thing.Position.Fogged(thing.Map))
					{
						Vector2 vector2 = thing.DrawPos.MapToUIPosition();
						rect.x = (float)(vector2.x - vector.x / 2.0);
						rect.y = (float)(vector2.y - vector.y / 2.0);
						if (rect.Contains(Event.current.mousePosition))
						{
							string text = (!this.ShouldShowShotReport(thing)) ? null : TooltipUtility.ShotCalculationTipString(thing);
							if (thing.def.hasTooltip || !text.NullOrEmpty())
							{
								TipSignal tooltip = thing.GetTooltip();
								if (!text.NullOrEmpty())
								{
									tooltip.text = tooltip.text + "\n\n" + text;
								}
								TooltipHandler.TipRegion(rect, tooltip);
							}
						}
						num++;
					}
				}
			}
		}

		private bool ShouldShowShotReport(Thing t)
		{
			return t.def.hasTooltip || t is Hive || t is IAttackTarget;
		}
	}
}
