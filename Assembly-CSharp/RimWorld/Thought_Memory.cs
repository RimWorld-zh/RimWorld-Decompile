using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000535 RID: 1333
	public class Thought_Memory : Thought
	{
		// Token: 0x04000E9F RID: 3743
		public float moodPowerFactor = 1f;

		// Token: 0x04000EA0 RID: 3744
		public Pawn otherPawn;

		// Token: 0x04000EA1 RID: 3745
		public int age = 0;

		// Token: 0x04000EA2 RID: 3746
		private int forcedStage = 0;

		// Token: 0x04000EA3 RID: 3747
		private string cachedLabelCap;

		// Token: 0x04000EA4 RID: 3748
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x04000EA5 RID: 3749
		private int cachedLabelCapForStageIndex = -1;

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x000D852C File Offset: 0x000D692C
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x000D8558 File Offset: 0x000D6958
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060018BA RID: 6330 RVA: 0x000D8574 File Offset: 0x000D6974
		public virtual bool ShouldDiscard
		{
			get
			{
				return this.age > this.def.DurationTicks;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x000D859C File Offset: 0x000D699C
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

		// Token: 0x060018BC RID: 6332 RVA: 0x000D8640 File Offset: 0x000D6A40
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000D864C File Offset: 0x000D6A4C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x000D86AB File Offset: 0x000D6AAB
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x000D86C0 File Offset: 0x000D6AC0
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x000D86CC File Offset: 0x000D6ACC
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

		// Token: 0x060018C1 RID: 6337 RVA: 0x000D8754 File Offset: 0x000D6B54
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x000D87B4 File Offset: 0x000D6BB4
		public override float MoodOffset()
		{
			float num = base.MoodOffset();
			return num * this.moodPowerFactor;
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x000D87DC File Offset: 0x000D6BDC
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
	}
}
