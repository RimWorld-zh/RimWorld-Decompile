using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Pawn_RecordsTracker : IExposable
	{
		private const int UpdateTimeRecordsIntervalTicks = 80;

		private Pawn pawn;

		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void RecordsTick()
		{
			if (!this.pawn.Dead && this.pawn.IsHashIntervalTick(80))
			{
				List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].type == RecordType.Time && allDefsListForReading[i].Worker.ShouldMeasureTimeNow(this.pawn))
					{
						DefMap<RecordDef, float> defMap;
						DefMap<RecordDef, float> obj = defMap = this.records;
						RecordDef def;
						RecordDef def2 = def = allDefsListForReading[i];
						float num = defMap[def];
						obj[def2] = (float)(num + 80.0);
					}
				}
			}
		}

		public void Increment(RecordDef def)
		{
			if (def.type != RecordType.Int)
			{
				Log.Error("Tried to increment record \"" + def.defName + "\" whose record type is \"" + def.type + "\".");
			}
			else
			{
				this.records[def] = Mathf.Round((float)(this.records[def] + 1.0));
			}
		}

		public void AddTo(RecordDef def, float value)
		{
			if (def.type == RecordType.Int)
			{
				this.records[def] = Mathf.Round(this.records[def] + Mathf.Round(value));
			}
			else if (def.type == RecordType.Float)
			{
				DefMap<RecordDef, float> defMap;
				DefMap<RecordDef, float> obj = defMap = this.records;
				RecordDef def2;
				RecordDef def3 = def2 = def;
				float num = defMap[def2];
				obj[def3] = num + value;
			}
			else
			{
				Log.Error("Tried to add value to record \"" + def.defName + "\" whose record type is \"" + def.type + "\".");
			}
		}

		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			if (((def.type != RecordType.Int) ? def.type : RecordType.Time) != 0)
			{
				return num;
			}
			return Mathf.Round(num);
		}

		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", new object[0]);
		}
	}
}
