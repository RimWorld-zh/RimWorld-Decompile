using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DCC RID: 3532
	public class RectTrigger : Thing
	{
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x00292730 File Offset: 0x00290B30
		// (set) Token: 0x06004EF7 RID: 20215 RVA: 0x0029274B File Offset: 0x00290B4B
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

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00292772 File Offset: 0x00290B72
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x00292790 File Offset: 0x00290B90
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

		// Token: 0x06004EFA RID: 20218 RVA: 0x002928D1 File Offset: 0x00290CD1
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

		// Token: 0x06004EFB RID: 20219 RVA: 0x00292908 File Offset: 0x00290D08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04003483 RID: 13443
		private CellRect rect;

		// Token: 0x04003484 RID: 13444
		public bool destroyIfUnfogged;

		// Token: 0x04003485 RID: 13445
		public bool activateOnExplosion;

		// Token: 0x04003486 RID: 13446
		public string signalTag;
	}
}
