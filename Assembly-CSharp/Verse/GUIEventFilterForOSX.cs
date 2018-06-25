using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E64 RID: 3684
	internal static class GUIEventFilterForOSX
	{
		// Token: 0x04003979 RID: 14713
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x0400397A RID: 14714
		private static int lastRecordedFrame = -1;

		// Token: 0x060056CA RID: 22218 RVA: 0x002CBBE4 File Offset: 0x002C9FE4
		public static void CheckRejectGUIEvent()
		{
			if (UnityData.platform == RuntimePlatform.OSXPlayer)
			{
				if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp)
				{
					if (Time.frameCount != GUIEventFilterForOSX.lastRecordedFrame)
					{
						GUIEventFilterForOSX.eventsThisFrame.Clear();
						GUIEventFilterForOSX.lastRecordedFrame = Time.frameCount;
					}
					for (int i = 0; i < GUIEventFilterForOSX.eventsThisFrame.Count; i++)
					{
						if (GUIEventFilterForOSX.EventsAreEquivalent(GUIEventFilterForOSX.eventsThisFrame[i], Event.current))
						{
							GUIEventFilterForOSX.RejectEvent();
						}
					}
					GUIEventFilterForOSX.eventsThisFrame.Add(Event.current);
				}
			}
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x002CBC98 File Offset: 0x002CA098
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x002CBCE0 File Offset: 0x002CA0E0
		private static void RejectEvent()
		{
			if (DebugViewSettings.logInput)
			{
				Log.Message(string.Concat(new object[]
				{
					"Frame ",
					Time.frameCount,
					": REJECTED ",
					Event.current.ToStringFull()
				}), false);
			}
			Event.current.Use();
		}
	}
}
