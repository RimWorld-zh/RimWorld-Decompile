using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FB RID: 763
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x0400084C RID: 2124
		public HistoryAutoRecorderDef def = null;

		// Token: 0x0400084D RID: 2125
		public List<float> records;

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0006FF28 File Offset: 0x0006E328
		public HistoryAutoRecorder()
		{
			this.records = new List<float>();
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0006FF44 File Offset: 0x0006E344
		public void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0006FFA0 File Offset: 0x0006E3A0
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
