using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000407 RID: 1031
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x0009A82C File Offset: 0x00098C2C
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0009A848 File Offset: 0x00098C48
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
