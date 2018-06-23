using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000406 RID: 1030
	public abstract class GenStep_Ambush : GenStep
	{
		// Token: 0x04000AD2 RID: 2770
		public FloatRange pointsRange = new FloatRange(180f, 340f);

		// Token: 0x060011B8 RID: 4536 RVA: 0x0009A6BC File Offset: 0x00098ABC
		public override void Generate(Map map)
		{
			CellRect rectToDefend;
			IntVec3 root;
			if (SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out rectToDefend, out root, map))
			{
				this.SpawnTrigger(rectToDefend, root, map);
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0009A6E8 File Offset: 0x00098AE8
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

		// Token: 0x060011BA RID: 4538 RVA: 0x0009A784 File Offset: 0x00098B84
		protected virtual RectTrigger MakeRectTrigger()
		{
			return (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0009A7AC File Offset: 0x00098BAC
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
