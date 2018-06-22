using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000521 RID: 1313
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x060017FB RID: 6139 RVA: 0x000D1B30 File Offset: 0x000CFF30
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x000D1BBC File Offset: 0x000CFFBC
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x000D1BF4 File Offset: 0x000CFFF4
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

		// Token: 0x060017FE RID: 6142 RVA: 0x000D1C90 File Offset: 0x000D0090
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

		// Token: 0x060017FF RID: 6143 RVA: 0x000D1D61 File Offset: 0x000D0161
		public void Learn(SkillDef sDef, float xp, bool direct = false)
		{
			this.GetSkill(sDef).Learn(xp, direct);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000D1D74 File Offset: 0x000D0174
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

		// Token: 0x06001801 RID: 6145 RVA: 0x000D1DF4 File Offset: 0x000D01F4
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

		// Token: 0x06001802 RID: 6146 RVA: 0x000D1E64 File Offset: 0x000D0264
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}

		// Token: 0x04000E3C RID: 3644
		private Pawn pawn;

		// Token: 0x04000E3D RID: 3645
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x04000E3E RID: 3646
		private int lastXpSinceMidnightResetTimestamp = -1;
	}
}
