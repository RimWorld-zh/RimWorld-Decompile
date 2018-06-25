using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061C RID: 1564
	public class DefeatAllEnemiesQuestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x04001268 RID: 4712
		private bool active;

		// Token: 0x04001269 RID: 4713
		public Faction requestingFaction;

		// Token: 0x0400126A RID: 4714
		public int relationsImprovement;

		// Token: 0x0400126B RID: 4715
		public ThingOwner rewards;

		// Token: 0x0400126C RID: 4716
		private static List<Thing> tmpRewards = new List<Thing>();

		// Token: 0x06001FBB RID: 8123 RVA: 0x00112287 File Offset: 0x00110687
		public DefeatAllEnemiesQuestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x0011229C File Offset: 0x0011069C
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x001122B7 File Offset: 0x001106B7
		public void StartQuest(Faction requestingFaction, int relationsImprovement, List<Thing> rewards)
		{
			this.StopQuest();
			this.active = true;
			this.requestingFaction = requestingFaction;
			this.relationsImprovement = relationsImprovement;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			this.rewards.TryAddRangeOrTransfer(rewards, true, false);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x001122EF File Offset: 0x001106EF
		public void StopQuest()
		{
			this.active = false;
			this.requestingFaction = null;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0011230C File Offset: 0x0011070C
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

		// Token: 0x06001FC0 RID: 8128 RVA: 0x00112346 File Offset: 0x00110746
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

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0011237C File Offset: 0x0011077C
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

		// Token: 0x06001FC2 RID: 8130 RVA: 0x001123E0 File Offset: 0x001107E0
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

		// Token: 0x06001FC3 RID: 8131 RVA: 0x001124F4 File Offset: 0x001108F4
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x00112504 File Offset: 0x00110904
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0011251F File Offset: 0x0011091F
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x00112534 File Offset: 0x00110934
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
	}
}
