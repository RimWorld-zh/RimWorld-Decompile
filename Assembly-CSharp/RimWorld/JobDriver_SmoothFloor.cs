using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200003E RID: 62
	public class JobDriver_SmoothFloor : JobDriver_AffectFloor
	{
		// Token: 0x06000216 RID: 534 RVA: 0x00016384 File Offset: 0x00014784
		public JobDriver_SmoothFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00016394 File Offset: 0x00014794
		protected override int BaseWorkAmount
		{
			get
			{
				return 2800;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000218 RID: 536 RVA: 0x000163B0 File Offset: 0x000147B0
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000163CC File Offset: 0x000147CC
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000163E8 File Offset: 0x000147E8
		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef smoothedTerrain = base.TargetLocA.GetTerrain(base.Map).smoothedTerrain;
			base.Map.terrainGrid.SetTerrain(base.TargetLocA, smoothedTerrain);
			FilthMaker.RemoveAllFilth(base.TargetLocA, base.Map);
		}
	}
}
