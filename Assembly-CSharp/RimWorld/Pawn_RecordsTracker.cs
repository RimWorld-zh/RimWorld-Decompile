using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200051A RID: 1306
	public class Pawn_RecordsTracker : IExposable
	{
		// Token: 0x060017AD RID: 6061 RVA: 0x000CECD0 File Offset: 0x000CD0D0
		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
			Rand.PushState();
			Rand.Seed = pawn.thingIDNumber * 681;
			this.storyRelevanceBonus = Rand.Range(0f, 100f);
			Rand.PopState();
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x000CED50 File Offset: 0x000CD150
		public float StoryRelevance
		{
			get
			{
				return (float)this.storyRelevance + this.storyRelevanceBonus;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x000CED74 File Offset: 0x000CD174
		public Battle BattleActive
		{
			get
			{
				Battle result;
				if (this.battleExitTick < Find.TickManager.TicksGame)
				{
					result = null;
				}
				else if (this.battleActive == null)
				{
					result = null;
				}
				else
				{
					while (this.battleActive.AbsorbedBy != null)
					{
						this.battleActive = this.battleActive.AbsorbedBy;
					}
					result = this.battleActive;
				}
				return result;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x000CEDE8 File Offset: 0x000CD1E8
		public int LastBattleTick
		{
			get
			{
				return this.battleExitTick;
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x000CEE03 File Offset: 0x000CD203
		public void RecordsTick()
		{
			if (!this.pawn.Dead)
			{
				if (this.pawn.IsHashIntervalTick(80))
				{
					this.RecordsTickUpdate(80);
					this.battleActive = this.BattleActive;
				}
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000CEE43 File Offset: 0x000CD243
		public void RecordsTickMothballed(int interval)
		{
			this.RecordsTickUpdate(interval);
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000CEE50 File Offset: 0x000CD250
		private void RecordsTickUpdate(int interval)
		{
			List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].type == RecordType.Time)
				{
					if (allDefsListForReading[i].Worker.ShouldMeasureTimeNow(this.pawn))
					{
						DefMap<RecordDef, float> defMap;
						RecordDef def;
						(defMap = this.records)[def = allDefsListForReading[i]] = defMap[def] + (float)interval;
					}
				}
			}
			this.storyRelevance *= Math.Pow(0.20000000298023224, (double)(0 * interval));
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x000CEEEC File Offset: 0x000CD2EC
		public void Increment(RecordDef def)
		{
			if (def.type != RecordType.Int)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to increment record \"",
					def.defName,
					"\" whose record type is \"",
					def.type,
					"\"."
				}), false);
			}
			else
			{
				this.records[def] = Mathf.Round(this.records[def] + 1f);
			}
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000CEF70 File Offset: 0x000CD370
		public void AddTo(RecordDef def, float value)
		{
			if (def.type == RecordType.Int)
			{
				this.records[def] = Mathf.Round(this.records[def] + Mathf.Round(value));
			}
			else if (def.type == RecordType.Float)
			{
				DefMap<RecordDef, float> defMap;
				(defMap = this.records)[def] = defMap[def] + value;
			}
			else
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add value to record \"",
					def.defName,
					"\" whose record type is \"",
					def.type,
					"\"."
				}), false);
			}
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x000CF024 File Offset: 0x000CD424
		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			float result;
			if (def.type == RecordType.Int || def.type == RecordType.Time)
			{
				result = Mathf.Round(num);
			}
			else
			{
				result = num;
			}
			return result;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x000CF06C File Offset: 0x000CD46C
		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x000CF092 File Offset: 0x000CD492
		public void AccumulateStoryEvent(StoryEventDef def)
		{
			this.storyRelevance += (double)def.importance;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x000CF0A9 File Offset: 0x000CD4A9
		public void EnterBattle(Battle battle)
		{
			this.battleActive = battle;
			this.battleExitTick = Find.TickManager.TicksGame + 5000;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000CF0CC File Offset: 0x000CD4CC
		public void ExposeData()
		{
			this.battleActive = this.BattleActive;
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", new object[0]);
			Scribe_Values.Look<double>(ref this.storyRelevance, "storyRelevance", 0.0, false);
			Scribe_References.Look<Battle>(ref this.battleActive, "battleActive", false);
			Scribe_Values.Look<int>(ref this.battleExitTick, "battleExitTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.RecordsTrackerPostLoadInit(this);
			}
		}

		// Token: 0x04000DF3 RID: 3571
		public Pawn pawn;

		// Token: 0x04000DF4 RID: 3572
		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		// Token: 0x04000DF5 RID: 3573
		private double storyRelevance = 0.0;

		// Token: 0x04000DF6 RID: 3574
		private Battle battleActive = null;

		// Token: 0x04000DF7 RID: 3575
		private int battleExitTick = 0;

		// Token: 0x04000DF8 RID: 3576
		private float storyRelevanceBonus = 0f;

		// Token: 0x04000DF9 RID: 3577
		private const int UpdateTimeRecordsIntervalTicks = 80;

		// Token: 0x04000DFA RID: 3578
		private const float StoryRelevanceBonusRange = 100f;

		// Token: 0x04000DFB RID: 3579
		private const float StoryRelevanceMultiplierPerYear = 0.2f;
	}
}
