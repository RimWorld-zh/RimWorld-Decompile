using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000408 RID: 1032
	public abstract class GenStep_Ambush : GenStep
	{
		// Token: 0x04000AD5 RID: 2773
		public FloatRange pointsRange = new FloatRange(180f, 340f);

		// Token: 0x060011BB RID: 4539 RVA: 0x0009A81C File Offset: 0x00098C1C
		public override void Generate(Map map)
		{
			CellRect rectToDefend;
			IntVec3 root;
			if (SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out rectToDefend, out root, map))
			{
				this.SpawnTrigger(rectToDefend, root, map);
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0009A848 File Offset: 0x00098C48
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

		// Token: 0x060011BD RID: 4541 RVA: 0x0009A8E4 File Offset: 0x00098CE4
		protected virtual RectTrigger MakeRectTrigger()
		{
			return (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0009A90C File Offset: 0x00098D0C
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
