using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000525 RID: 1317
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x06001803 RID: 6147 RVA: 0x000D1AE4 File Offset: 0x000CFEE4
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000D1B70 File Offset: 0x000CFF70
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000D1BA8 File Offset: 0x000CFFA8
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

		// Token: 0x06001806 RID: 6150 RVA: 0x000D1C44 File Offset: 0x000D0044
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

		// Token: 0x06001807 RID: 6151 RVA: 0x000D1D15 File Offset: 0x000D0115
		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000D1D28 File Offset: 0x000D0128
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

		// Token: 0x06001809 RID: 6153 RVA: 0x000D1DA8 File Offset: 0x000D01A8
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

		// Token: 0x0600180A RID: 6154 RVA: 0x000D1E18 File Offset: 0x000D0218
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}

		// Token: 0x04000E3F RID: 3647
		private Pawn pawn;

		// Token: 0x04000E40 RID: 3648
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x04000E41 RID: 3649
		private int lastXpSinceMidnightResetTimestamp = -1;
	}
}
