using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000537 RID: 1335
	public class Thought_Memory : Thought
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060018BE RID: 6334 RVA: 0x000D8168 File Offset: 0x000D6568
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x000D8194 File Offset: 0x000D6594
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x000D81B0 File Offset: 0x000D65B0
		public virtual bool ShouldDiscard
		{
			get
			{
				return this.age > this.def.DurationTicks;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x000D81D8 File Offset: 0x000D65D8
		public override string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null || this.cachedLabelCapForOtherPawn != this.otherPawn || this.cachedLabelCapForStageIndex != this.CurStageIndex)
				{
					if (this.otherPawn != null)
					{
						this.cachedLabelCap = string.Format(base.CurStage.label, this.otherPawn.LabelShort).CapitalizeFirst();
					}
					else
					{
						this.cachedLabelCap = base.LabelCap;
					}
					this.cachedLabelCapForOtherPawn = this.otherPawn;
					this.cachedLabelCapForStageIndex = this.CurStageIndex;
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x000D827C File Offset: 0x000D667C
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x000D8288 File Offset: 0x000D6688
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x000D82E7 File Offset: 0x000D66E7
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x000D82FC File Offset: 0x000D66FC
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x000D8308 File Offset: 0x000D6708
		public virtual bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= this.def.stackLimit)
			{
				Thought_Memory thought_Memory = thoughts.memories.OldestMemoryInGroup(this);
				if (thought_Memory != null)
				{
					showBubble = (thought_Memory.age > thought_Memory.def.DurationTicks / 2);
					thought_Memory.Renew();
					return true;
				}
			}
			showBubble = true;
			return false;
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x000D8390 File Offset: 0x000D6790
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x000D83F0 File Offset: 0x000D67F0
		public override float MoodOffset()
		{
			float num = base.MoodOffset();
			return num * this.moodPowerFactor;
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x000D8418 File Offset: 0x000D6818
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.def.defName,
				", moodPowerFactor=",
				this.moodPowerFactor,
				", age=",
				this.age,
				")"
			});
		}

		// Token: 0x04000E9E RID: 3742
		public float moodPowerFactor = 1f;

		// Token: 0x04000E9F RID: 3743
		public Pawn otherPawn;

		// Token: 0x04000EA0 RID: 3744
		public int age = 0;

		// Token: 0x04000EA1 RID: 3745
		private int forcedStage = 0;

		// Token: 0x04000EA2 RID: 3746
		private string cachedLabelCap;

		// Token: 0x04000EA3 RID: 3747
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x04000EA4 RID: 3748
		private int cachedLabelCapForStageIndex = -1;
	}
}
