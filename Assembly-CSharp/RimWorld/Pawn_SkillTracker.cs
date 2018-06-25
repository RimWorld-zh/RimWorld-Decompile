using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000523 RID: 1315
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x04000E40 RID: 3648
		private Pawn pawn;

		// Token: 0x04000E41 RID: 3649
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x04000E42 RID: 3650
		private int lastXpSinceMidnightResetTimestamp = -1;

		// Token: 0x060017FE RID: 6142 RVA: 0x000D1EE8 File Offset: 0x000D02E8
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x000D1F74 File Offset: 0x000D0374
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000D1FAC File Offset: 0x000D03AC
		public SkillRecord GetSkill(SkillDef skillDef)
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				if (this.skills[i].def == skillDef)
				{
					return this.skills[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Did not find skill of def ",
				skillDef,
				", returning ",
				this.skills[0]
			}), false);
			return this.skills[0];
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x000D2048 File Offset: 0x000D0448
		public void SkillsTick()
		{
			if (this.pawn.IsHashIntervalTick(200))
			{
				if (GenLocalDate.HourInteger(this.pawn) == 0 && (this.lastXpSinceMidnightResetTimestamp < 0 || Find.TickManager.TicksGame - this.lastXpSinceMidnightResetTimestamp >= 30000))
				{
					for (int i = 0; i < this.skills.Count; i++)
					{
						this.skills[i].xpSinceMidnight = 0f;
					}
					this.lastXpSinceMidnightResetTimestamp = Find.TickManager.TicksGame;
				}
				for (int j = 0; j < this.skills.Count; j++)
				{
					this.skills[j].Interval();
				}
			}
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x000D2119 File Offset: 0x000D0519
		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000D212C File Offset: 0x000D052C
		public float AverageOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			float result;
			if (workDef.relevantSkills.Count == 0)
			{
				result = 3f;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < workDef.relevantSkills.Count; i++)
				{
					num += (float)this.GetSkill(workDef.relevantSkills[i]).Level;
				}
				num /= (float)workDef.relevantSkills.Count;
				result = num;
			}
			return result;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000D21AC File Offset: 0x000D05AC
		public Passion MaxPassionOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			Passion result;
			if (workDef.relevantSkills.Count == 0)
			{
				result = Passion.None;
			}
			else
			{
				Passion passion = Passion.None;
				for (int i = 0; i < workDef.relevantSkills.Count; i++)
				{
					Passion passion2 = this.GetSkill(workDef.relevantSkills[i]).passion;
					if (passion2 > passion)
					{
						passion = passion2;
					}
				}
				result = passion;
			}
			return result;
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000D221C File Offset: 0x000D061C
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}
	}
}
