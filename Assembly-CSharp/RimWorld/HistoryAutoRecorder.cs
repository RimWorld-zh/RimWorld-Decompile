using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F9 RID: 761
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x0400084C RID: 2124
		public HistoryAutoRecorderDef def = null;

		// Token: 0x0400084D RID: 2125
		public List<float> records;

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0006FDD8 File Offset: 0x0006E1D8
		public HistoryAutoRecorder()
		{
			this.records = new List<float>();
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0006FDF4 File Offset: 0x0006E1F4
		public void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0006FE50 File Offset: 0x0006E250
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderDef>(ref this.def, "def");
			byte[] array = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				array = new byte[this.records.Count * 4];
				for (int i = 0; i < this.records.Count; i++)
				{
					byte[] bytes = BitConverter.GetBytes(this.records[i]);
					for (int j = 0; j < 4; j++)
					{
						array[i * 4 + j] = bytes[j];
					}
				}
			}
			DataExposeUtility.ByteArray(ref array, "records");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num = array.Length / 4;
				this.records.Clear();
				for (int k = 0; k < num; k++)
				{
					float item = BitConverter.ToSingle(array, k * 4);
					this.records.Add(item);
				}
			}
		}
	}
}
