using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E7 RID: 2023
	public abstract class Designator_ZoneAdd : Designator_Zone
	{
		// Token: 0x040017B4 RID: 6068
		protected Type zoneTypeToPlace;

		// Token: 0x06002CF7 RID: 11511 RVA: 0x0017A5CB File Offset: 0x001789CB
		public Designator_ZoneAdd()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x0017A5F8 File Offset: 0x001789F8
		// (set) Token: 0x06002CF9 RID: 11513 RVA: 0x0017A617 File Offset: 0x00178A17
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

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002CFA RID: 11514
		protected abstract string NewZoneLabel { get; }

		// Token: 0x06002CFB RID: 11515
		protected abstract Zone MakeNewZone();

		// Token: 0x06002CFC RID: 11516 RVA: 0x0017A638 File Offset: 0x00178A38
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != this.zoneTypeToPlace)
			{
				Find.Selector.Deselect(Find.Selector.SelectedZone);
			}
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x0017A68C File Offset: 0x00178A8C
		public override void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				string text = "";
				if (!Input.GetKey(KeyCode.Mouse0))
				{
					Zone selectedZone = Find.Selector.SelectedZone;
					if (selectedZone != null)
					{
						text = "ExpandOrCreateZone".Translate(new object[]
						{
							selectedZone.label,
							this.NewZoneLabel
						});
					}
					else
					{
						text = "CreateNewZone".Translate(new object[]
						{
							this.NewZoneLabel
						});
					}
				}
				GenUI.DrawMouseAttachment(this.icon, text, 0f, default(Vector2), null);
			}
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x0017A734 File Offset: 0x00178B34
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (c.Fogged(base.Map))
			{
				result = false;
			}
			else if (c.InNoZoneEdgeArea(base.Map))
			{
				result = "TooCloseToMapEdge".Translate();
			}
			else
			{
				Zone zone = base.Map.zoneManager.ZoneAt(c);
				if (zone != null && zone.GetType() != this.zoneTypeToPlace)
				{
					result = false;
				}
				else
				{
					foreach (Thing thing in base.Map.thingGrid.ThingsAt(c))
					{
						if (!thing.def.CanOverlapZones)
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x0017A854 File Offset: 0x00178C54
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			List<IntVec3> list = cells.ToList<IntVec3>();
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
				foreach (IntVec3 c3 in cells)
				{
					Zone zone3 = base.Map.zoneManager.ZoneAt(c3);
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
			list.RemoveAll((IntVec3 c) => base.Map.zoneManager.ZoneAt(c) != null);
			if (list.Count != 0)
			{
				if (!TutorSystem.TutorialMode || TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, list)))
				{
					if (this.SelectedZone == null)
					{
						this.SelectedZone = this.MakeNewZone();
						base.Map.zoneManager.RegisterZone(this.SelectedZone);
						this.SelectedZone.AddCell(list[0]);
						list.RemoveAt(0);
					}
					bool somethingSucceeded;
					for (;;)
					{
						somethingSucceeded = true;
						int count = list.Count;
						for (int i = list.Count - 1; i >= 0; i--)
						{
							bool flag = false;
							for (int j = 0; j < 4; j++)
							{
								IntVec3 c2 = list[i] + GenAdj.CardinalDirections[j];
								if (c2.InBounds(base.Map))
								{
									if (base.Map.zoneManager.ZoneAt(c2) == this.SelectedZone)
									{
										flag = true;
										break;
									}
								}
							}
							if (flag)
							{
								this.SelectedZone.AddCell(list[i]);
								list.RemoveAt(i);
							}
						}
						if (list.Count == 0)
						{
							break;
						}
						if (list.Count == count)
						{
							this.SelectedZone = this.MakeNewZone();
							base.Map.zoneManager.RegisterZone(this.SelectedZone);
							this.SelectedZone.AddCell(list[0]);
							list.RemoveAt(0);
						}
					}
					this.SelectedZone.CheckContiguous();
					base.Finalize(somethingSucceeded);
					TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, list));
				}
			}
		}
	}
}
