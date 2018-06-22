using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200003D RID: 61
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000212 RID: 530 RVA: 0x000162F4 File Offset: 0x000146F4
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00016310 File Offset: 0x00014710
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0001632C File Offset: 0x0001472C
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00016346 File Offset: 0x00014746
		protected override void DoEffect(IntVec3 c)
		{
			if (base.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				base.Map.terrainGrid.RemoveTopLayer(base.TargetLocA, true);
				FilthMaker.RemoveAllFilth(c, base.Map);
			}
		}
	}
}
