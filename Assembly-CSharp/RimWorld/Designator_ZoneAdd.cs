using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class Designator_ZoneAdd : Designator_Zone
	{
		protected Type zoneTypeToPlace;

		private Zone SelectedZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
			set
			{
				Find.Selector.ClearSelection();
				if (value != null)
				{
					Find.Selector.Select(value, false, false);
				}
			}
		}

		protected abstract string NewZoneLabel
		{
			get;
		}

		public Designator_ZoneAdd()
		{
			base.soundDragSustain = SoundDefOf.DesignateDragAreaAdd;
			base.soundDragChanged = SoundDefOf.DesignateDragAreaAddChanged;
			base.soundSucceeded = SoundDefOf.DesignateZoneAdd;
			base.useMouseIcon = true;
		}

		protected abstract Zone MakeNewZone();

		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != this.zoneTypeToPlace)
			{
				Find.Selector.Deselect(Find.Selector.SelectedZone);
			}
		}

		public override void DrawMouseAttachments()
		{
			if (base.useMouseIcon)
			{
				string text = string.Empty;
				if (!Input.GetKey(KeyCode.Mouse0))
				{
					Zone selectedZone = Find.Selector.SelectedZone;
					text = ((selectedZone == null) ? "CreateNewZone".Translate(this.NewZoneLabel) : "ExpandOrCreateZone".Translate(selectedZone.label, this.NewZoneLabel));
				}
				GenUI.DrawMouseAttachment(base.icon, text);
			}
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (c.InNoZoneEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			if (zone != null && zone.GetType() != this.zoneTypeToPlace)
			{
				return false;
			}
			foreach (Thing item in base.Map.thingGrid.ThingsAt(c))
			{
				if (!item.def.CanOverlapZones)
				{
					return false;
				}
			}
			return true;
		}

		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			List<IntVec3> list = cells.ToList();
			bool flag = false;
			if (list.Count == 1)
			{
				Zone zone = base.Map.zoneManager.ZoneAt(list[0]);
				if (zone != null)
				{
					if (zone.GetType() == this.zoneTypeToPlace)
					{
						this.SelectedZone = zone;
					}
					return;
				}
			}
			if (this.SelectedZone == null)
			{
				Zone zone2 = null;
				foreach (IntVec3 item in cells)
				{
					Zone zone3 = base.Map.zoneManager.ZoneAt(item);
					if (zone3 != null && zone3.GetType() == this.zoneTypeToPlace)
					{
						if (zone2 == null)
						{
							zone2 = zone3;
						}
						else if (zone3 != zone2)
						{
							zone2 = null;
							break;
						}
					}
				}
				this.SelectedZone = zone2;
			}
			list.RemoveAll((Predicate<IntVec3>)((IntVec3 c) => base.Map.zoneManager.ZoneAt(c) != null));
			if (list.Count != 0 && (!TutorSystem.TutorialMode || TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, list))))
			{
				if (this.SelectedZone == null)
				{
					this.SelectedZone = this.MakeNewZone();
					this.SelectedZone.AddCell(list[0]);
					list.RemoveAt(0);
				}
				while (true)
				{
					flag = true;
					int count = list.Count;
					for (int num = list.Count - 1; num >= 0; num--)
					{
						bool flag2 = false;
						for (int i = 0; i < 4; i++)
						{
							IntVec3 c2 = list[num] + GenAdj.CardinalDirections[i];
							if (c2.InBounds(base.Map) && base.Map.zoneManager.ZoneAt(c2) == this.SelectedZone)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							this.SelectedZone.AddCell(list[num]);
							list.RemoveAt(num);
						}
					}
					if (list.Count != 0)
					{
						if (list.Count == count)
						{
							this.SelectedZone = this.MakeNewZone();
							this.SelectedZone.AddCell(list[0]);
							list.RemoveAt(0);
						}
						continue;
					}
					break;
				}
				this.SelectedZone.CheckContiguous();
				base.Finalize(flag);
				TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, list));
			}
		}
	}
}
