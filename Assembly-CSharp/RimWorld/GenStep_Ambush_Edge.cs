using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000409 RID: 1033
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x0009A98C File Offset: 0x00098D8C
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0009A9A8 File Offset: 0x00098DA8
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
