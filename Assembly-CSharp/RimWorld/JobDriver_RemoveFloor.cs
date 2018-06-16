using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200003D RID: 61
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000212 RID: 530 RVA: 0x000162EC File Offset: 0x000146EC
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00016308 File Offset: 0x00014708
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00016324 File Offset: 0x00014724
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0001633E File Offset: 0x0001473E
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
