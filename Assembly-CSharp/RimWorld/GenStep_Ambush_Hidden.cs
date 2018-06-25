using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200040A RID: 1034
	public class GenStep_Ambush_Hidden : GenStep_Ambush
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0009A9C8 File Offset: 0x00098DC8
		public override int SeedPart
		{
			get
			{
				return 921085483;
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0009A9E4 File Offset: 0x00098DE4
		protected override RectTrigger MakeRectTrigger()
		{
			RectTrigger rectTrigger = base.MakeRectTrigger();
			rectTrigger.activateOnExplosion = true;
			return rectTrigger;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0009AA08 File Offset: 0x00098E08
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root);
			if (root.IsValid)
			{
				signalAction_Ambush.spawnNear = root;
			}
			else
			{
				signalAction_Ambush.spawnAround = rectToDefend;
			}
			return signalAction_Ambush;
		}
	}
}
