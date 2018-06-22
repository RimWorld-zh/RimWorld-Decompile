using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x02000DAB RID: 3499
	public static class DebugSoundEventsLog
	{
		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06004E2F RID: 20015 RVA: 0x0028EBFC File Offset: 0x0028CFFC
		public static string EventsListingDebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LogMessage logMessage in DebugSoundEventsLog.queue.Messages.Reverse<LogMessage>())
				{
					stringBuilder.AppendLine(logMessage.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x0028EC7C File Offset: 0x0028D07C
		public static void Notify_SoundEvent(SoundDef def, SoundInfo info)
		{
			if (DebugViewSettings.writeSoundEventsRecord)
			{
				string str;
				if (def == null)
				{
					str = "null: ";
				}
				else if (def.isUndefined)
				{
					str = "Undefined: ";
				}
				else
				{
					str = ((!def.sustain) ? "OneShot: " : "SustainerSpawn: ");
				}
				string str2 = (def == null) ? "null" : def.defName;
				string str3 = str + str2 + " - " + info.ToString();
				DebugSoundEventsLog.CreateRecord(str3);
			}
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x0028ED14 File Offset: 0x0028D114
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			string str = "SustainerEnd: " + sustainer.def.defName + " - " + info.ToString();
			DebugSoundEventsLog.CreateRecord(str);
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x0028ED50 File Offset: 0x0028D150
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		// Token: 0x0400341F RID: 13343
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
