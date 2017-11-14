using UnityEngine;

namespace Verse
{
	public static class UnityGUIBugsFixer
	{
		private const float ScrollFactor = -6f;

		public static void OnGUI()
		{
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
		}

		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel)
			{
				if (Application.platform != RuntimePlatform.LinuxEditor && Application.platform != RuntimePlatform.LinuxPlayer)
					return;
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, (float)(delta.y * -6.0));
			}
		}

		private static void FixShift()
		{
			if (Application.platform != RuntimePlatform.LinuxEditor && Application.platform != RuntimePlatform.LinuxPlayer)
				return;
			if (!Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}
	}
}
