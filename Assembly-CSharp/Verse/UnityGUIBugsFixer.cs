using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EAB RID: 3755
	public static class UnityGUIBugsFixer
	{
		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06005871 RID: 22641 RVA: 0x002D4F84 File Offset: 0x002D3384
		public static List<Resolution> ScreenResolutionsWithoutDuplicates
		{
			get
			{
				UnityGUIBugsFixer.resolutions.Clear();
				Resolution[] array = Screen.resolutions;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < UnityGUIBugsFixer.resolutions.Count; j++)
					{
						if (UnityGUIBugsFixer.resolutions[j].width == array[i].width && UnityGUIBugsFixer.resolutions[j].height == array[i].height)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						UnityGUIBugsFixer.resolutions.Add(array[i]);
					}
				}
				return UnityGUIBugsFixer.resolutions;
			}
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x002D5053 File Offset: 0x002D3453
		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x002D5060 File Offset: 0x002D3460
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * -6f);
			}
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x002D50C8 File Offset: 0x002D34C8
		private static void FixShift()
		{
			if (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer)
			{
				if (!Event.current.shift)
				{
					Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
				}
			}
		}

		// Token: 0x04003AEE RID: 15086
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x04003AEF RID: 15087
		private const float ScrollFactor = -6f;
	}
}
