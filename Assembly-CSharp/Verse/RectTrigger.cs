using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DCB RID: 3531
	public class RectTrigger : Thing
	{
		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x00292710 File Offset: 0x00290B10
		// (set) Token: 0x06004EF5 RID: 20213 RVA: 0x0029272B File Offset: 0x00290B2B
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

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00292752 File Offset: 0x00290B52
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00292770 File Offset: 0x00290B70
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

		// Token: 0x06004EF8 RID: 20216 RVA: 0x002928B1 File Offset: 0x00290CB1
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

		// Token: 0x06004EF9 RID: 20217 RVA: 0x002928E8 File Offset: 0x00290CE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04003481 RID: 13441
		private CellRect rect;

		// Token: 0x04003482 RID: 13442
		public bool destroyIfUnfogged;

		// Token: 0x04003483 RID: 13443
		public bool activateOnExplosion;

		// Token: 0x04003484 RID: 13444
		public string signalTag;
	}
}
