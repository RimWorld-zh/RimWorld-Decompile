using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061E RID: 1566
	public class DefeatAllEnemiesQuestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06001FC1 RID: 8129 RVA: 0x00111E7B File Offset: 0x0011027B
		public DefeatAllEnemiesQuestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x00111E90 File Offset: 0x00110290
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x00111EAB File Offset: 0x001102AB
		public void StartQuest(Faction requestingFaction, int relationsImprovement, List<Thing> rewards)
		{
			this.StopQuest();
			this.active = true;
			this.requestingFaction = requestingFaction;
			this.relationsImprovement = relationsImprovement;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			this.rewards.TryAddRangeOrTransfer(rewards, true, false);
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x00111EE3 File Offset: 0x001102E3
		public void StopQuest()
		{
			this.active = false;
			this.requestingFaction = null;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x00111F00 File Offset: 0x00110300
		public override void CompTick()
		{
			base.CompTick();
			if (this.active)
			{
				MapParent mapParent = this.parent as MapParent;
				if (mapParent != null)
				{
					this.CheckAllEnemiesDefeated(mapParent);
				}
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x00111F3A File Offset: 0x0011033A
		private void CheckAllEnemiesDefeated(MapParent mapParent)
		{
			if (mapParent.HasMap)
			{
				if (!GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map))
				{
					this.GiveRewardsAndSendLetter();
					this.StopQuest();
				}
			}
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00111F70 File Offset: 0x00110370
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.relationsImprovement, "relationsImprovement", 0, false);
			Scribe_References.Look<Faction>(ref this.requestingFaction, "requestingFaction", false);
			Scribe_Deep.Look<ThingOwner>(ref this.rewards, "rewards", new object[]
			{
				this
			});
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x00111FD4 File Offset: 0x001103D4
		private void GiveRewardsAndSendLetter()
		{
			Map map = Find.AnyPlayerHomeMap ?? ((MapParent)this.parent).Map;
			DefeatAllEnemiesQuestComp.tmpRewards.AddRange(this.rewards);
			this.rewards.Clear();
			IntVec3 intVec = DropCellFinder.TradeDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, DefeatAllEnemiesQuestComp.tmpRewards, 110, false, false, false, false);
			DefeatAllEnemiesQuestComp.tmpRewards.Clear();
			FactionRelationKind playerRelationKind = this.requestingFaction.PlayerRelationKind;
			string text = "LetterDefeatAllEnemiesQuestCompleted".Translate(new object[]
			{
				this.requestingFaction.Name,
				this.relationsImprovement.ToString()
			});
			this.requestingFaction.TryAffectGoodwillWith(Faction.OfPlayer, this.relationsImprovement, false, false, null, null);
			this.requestingFaction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, this.requestingFaction.PlayerRelationKind, null);
			Find.LetterStack.ReceiveLetter("LetterLabelDefeatAllEnemiesQuestCompleted".Translate(), text, LetterDefOf.PositiveEvent, new GlobalTargetInfo(intVec, map, false), this.requestingFaction, null);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x001120E8 File Offset: 0x001104E8
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x001120F8 File Offset: 0x001104F8
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x00112113 File Offset: 0x00110513
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x00112128 File Offset: 0x00110528
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.active)
			{
				result = "QuestTargetDestroyInspectString".Translate(new object[]
				{
					this.requestingFaction.Name,
					this.rewards[0].LabelCap
				}).CapitalizeFirst();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04001267 RID: 4711
		private bool active;

		// Token: 0x04001268 RID: 4712
		public Faction requestingFaction;

		// Token: 0x04001269 RID: 4713
		public int relationsImprovement;

		// Token: 0x0400126A RID: 4714
		public ThingOwner rewards;

		// Token: 0x0400126B RID: 4715
		private static List<Thing> tmpRewards = new List<Thing>();
	}
}
