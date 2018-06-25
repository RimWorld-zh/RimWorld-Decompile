using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000408 RID: 1032
	public abstract class GenStep_Ambush : GenStep
	{
		// Token: 0x04000AD2 RID: 2770
		public FloatRange pointsRange = new FloatRange(180f, 340f);

		// Token: 0x060011BC RID: 4540 RVA: 0x0009A80C File Offset: 0x00098C0C
		public override void Generate(Map map)
		{
			CellRect rectToDefend;
			IntVec3 root;
			if (SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out rectToDefend, out root, map))
			{
				this.SpawnTrigger(rectToDefend, root, map);
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0009A838 File Offset: 0x00098C38
		private void SpawnTrigger(CellRect rectToDefend, IntVec3 root, Map map)
		{
			int nextSignalTagID = Find.UniqueIDsManager.GetNextSignalTagID();
			string signalTag = "ambushActivated-" + nextSignalTagID;
			CellRect rect;
			if (root.IsValid)
			{
				rect = CellRect.CenteredOn(root, 17);
			}
			else
			{
				rect = rectToDefend.ExpandedBy(12);
			}
			SignalAction_Ambush signalAction_Ambush = this.MakeAmbushSignalAction(rectToDefend, root);
			signalAction_Ambush.signalTag = signalTag;
			GenSpawn.Spawn(signalAction_Ambush, rect.CenterCell, map, WipeMode.Vanish);
			RectTrigger rectTrigger = this.MakeRectTrigger();
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0009A8D4 File Offset: 0x00098CD4
		protected virtual RectTrigger MakeRectTrigger()
		{
			return (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0009A8FC File Offset: 0x00098CFC
		protected virtual SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			signalAction_Ambush.points = this.pointsRange.RandomInRange;
			int num = Rand.RangeInclusive(0, 2);
			if (num == 0)
			{
				signalAction_Ambush.manhunters = true;
			}
			else if (num == 1 && PawnGroupMakerUtility.CanGenerateAnyNormalGroup(Faction.OfMechanoids, signalAction_Ambush.points))
			{
				signalAction_Ambush.mechanoids = true;
			}
			return signalAction_Ambush;
		}
	}
}
