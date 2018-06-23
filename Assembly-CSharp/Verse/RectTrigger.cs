using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DC8 RID: 3528
	public class RectTrigger : Thing
	{
		// Token: 0x0400348C RID: 13452
		private CellRect rect;

		// Token: 0x0400348D RID: 13453
		public bool destroyIfUnfogged;

		// Token: 0x0400348E RID: 13454
		public bool activateOnExplosion;

		// Token: 0x0400348F RID: 13455
		public string signalTag;

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06004F09 RID: 20233 RVA: 0x00293CEC File Offset: 0x002920EC
		// (set) Token: 0x06004F0A RID: 20234 RVA: 0x00293D07 File Offset: 0x00292107
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

		// Token: 0x06004F0B RID: 20235 RVA: 0x00293D2E File Offset: 0x0029212E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x00293D4C File Offset: 0x0029214C
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

		// Token: 0x06004F0D RID: 20237 RVA: 0x00293E8D File Offset: 0x0029228D
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

		// Token: 0x06004F0E RID: 20238 RVA: 0x00293EC4 File Offset: 0x002922C4
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
