using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000533 RID: 1331
	public class Thought_Memory : Thought
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x000D8174 File Offset: 0x000D6574
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x000D81A0 File Offset: 0x000D65A0
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x000D81BC File Offset: 0x000D65BC
		public virtual bool ShouldDiscard
		{
			get
			{
				return this.age > this.def.DurationTicks;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x000D81E4 File Offset: 0x000D65E4
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

		// Token: 0x060018B9 RID: 6329 RVA: 0x000D8288 File Offset: 0x000D6688
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x000D8294 File Offset: 0x000D6694
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x000D82F3 File Offset: 0x000D66F3
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x000D8308 File Offset: 0x000D6708
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000D8314 File Offset: 0x000D6714
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

		// Token: 0x060018BE RID: 6334 RVA: 0x000D839C File Offset: 0x000D679C
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x000D83FC File Offset: 0x000D67FC
		public override float MoodOffset()
		{
			float num = base.MoodOffset();
			return num * this.moodPowerFactor;
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x000D8424 File Offset: 0x000D6824
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

		// Token: 0x04000E9B RID: 3739
		public float moodPowerFactor = 1f;

		// Token: 0x04000E9C RID: 3740
		public Pawn otherPawn;

		// Token: 0x04000E9D RID: 3741
		public int age = 0;

		// Token: 0x04000E9E RID: 3742
		private int forcedStage = 0;

		// Token: 0x04000E9F RID: 3743
		private string cachedLabelCap;

		// Token: 0x04000EA0 RID: 3744
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x04000EA1 RID: 3745
		private int cachedLabelCapForStageIndex = -1;
	}
}
