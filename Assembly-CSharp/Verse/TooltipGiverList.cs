using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CAE RID: 3246
	public sealed class TooltipGiverList
	{
		// Token: 0x0400308F RID: 12431
		private List<Thing> givers = new List<Thing>();

		// Token: 0x0600478B RID: 18315 RVA: 0x0025BC65 File Offset: 0x0025A065
		public void Notify_ThingSpawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Add(t);
			}
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0025BC90 File Offset: 0x0025A090
		public void Notify_ThingDespawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Remove(t);
			}
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x0025BCBC File Offset: 0x0025A0BC
		public void DispenseAllThingTooltips()
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (Find.WindowStack.FloatMenu == null)
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
							rect.x = vector2.x - vector.x / 2f;
							rect.y = vector2.y - vector.y / 2f;
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
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x0025BE6C File Offset: 0x0025A26C
		private bool ShouldShowShotReport(Thing t)
		{
			return t.def.hasTooltip || t is Hive || t is IAttackTarget;
		}
	}
}
