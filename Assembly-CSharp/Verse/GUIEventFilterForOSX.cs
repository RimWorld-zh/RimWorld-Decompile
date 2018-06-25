using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E65 RID: 3685
	internal static class GUIEventFilterForOSX
	{
		// Token: 0x04003981 RID: 14721
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x04003982 RID: 14722
		private static int lastRecordedFrame = -1;

		// Token: 0x060056CA RID: 22218 RVA: 0x002CBDD0 File Offset: 0x002CA1D0
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

		// Token: 0x060056CB RID: 22219 RVA: 0x002CBE84 File Offset: 0x002CA284
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x002CBECC File Offset: 0x002CA2CC
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
