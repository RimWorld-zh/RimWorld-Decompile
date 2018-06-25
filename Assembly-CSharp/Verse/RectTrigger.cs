using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DCB RID: 3531
	public class RectTrigger : Thing
	{
		// Token: 0x04003493 RID: 13459
		private CellRect rect;

		// Token: 0x04003494 RID: 13460
		public bool destroyIfUnfogged;

		// Token: 0x04003495 RID: 13461
		public bool activateOnExplosion;

		// Token: 0x04003496 RID: 13462
		public string signalTag;

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004F0D RID: 20237 RVA: 0x002940F8 File Offset: 0x002924F8
		// (set) Token: 0x06004F0E RID: 20238 RVA: 0x00294113 File Offset: 0x00292513
		public CellRect Rect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
				if (base.Spawned)
				{
					this.rect.ClipInsideMap(base.Map);
				}
			}
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x0029413A File Offset: 0x0029253A
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x00294158 File Offset: 0x00292558
		public override void Tick()
		{
			if (this.destroyIfUnfogged && !this.rect.CenterCell.Fogged(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else if (this.IsHashIntervalTick(60))
			{
				Map map = base.Map;
				for (int i = this.rect.minZ; i <= this.rect.maxZ; i++)
				{
					for (int j = this.rect.minX; j <= this.rect.maxX; j++)
					{
						IntVec3 c = new IntVec3(j, 0, i);
						List<Thing> thingList = c.GetThingList(map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.category == ThingCategory.Pawn && thingList[k].def.race.intelligence == Intelligence.Humanlike && thingList[k].Faction == Faction.OfPlayer)
							{
								this.ActivatedBy((Pawn)thingList[k]);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x00294299 File Offset: 0x00292699
		public void ActivatedBy(Pawn p)
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, new object[]
			{
				p
			}));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x002942D0 File Offset: 0x002926D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}
	}
}
