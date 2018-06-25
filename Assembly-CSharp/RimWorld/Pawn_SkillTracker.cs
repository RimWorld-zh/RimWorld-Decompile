using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000523 RID: 1315
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x04000E3C RID: 3644
		private Pawn pawn;

		// Token: 0x04000E3D RID: 3645
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x04000E3E RID: 3646
		private int lastXpSinceMidnightResetTimestamp = -1;

		// Token: 0x060017FF RID: 6143 RVA: 0x000D1C80 File Offset: 0x000D0080
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000D1D0C File Offset: 0x000D010C
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x000D1D44 File Offset: 0x000D0144
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

		// Token: 0x06001802 RID: 6146 RVA: 0x000D1DE0 File Offset: 0x000D01E0
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

		// Token: 0x06001803 RID: 6147 RVA: 0x000D1EB1 File Offset: 0x000D02B1
		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000D1EC4 File Offset: 0x000D02C4
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

		// Token: 0x06001805 RID: 6149 RVA: 0x000D1F44 File Offset: 0x000D0344
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

		// Token: 0x06001806 RID: 6150 RVA: 0x000D1FB4 File Offset: 0x000D03B4
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}
	}
}
