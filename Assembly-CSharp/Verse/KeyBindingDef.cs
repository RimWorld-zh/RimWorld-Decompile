using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class KeyBindingDef : Def
	{
		public KeyBindingCategoryDef category;

		public KeyCode defaultKeyCodeA;

		public KeyCode defaultKeyCodeB;

		public bool devModeOnly = false;

		public List<string> extraConflictTags;

		public KeyCode MainKey
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				KeyCode result;
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != 0)
					{
						result = keyBindingData.keyBindingA;
						goto IL_004f;
					}
					if (keyBindingData.keyBindingB != 0)
					{
						result = keyBindingData.keyBindingB;
						goto IL_004f;
					}
				}
				result = KeyCode.None;
				goto IL_004f;
				IL_004f:
				return result;
			}
		}

		public string MainKeyLabel
		{
			get
			{
				return this.MainKey.ToStringReadable();
			}
		}

		public bool KeyDownEvent
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				return Event.current.type == EventType.KeyDown && Event.current.keyCode != 0 && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB);
			}
		}

		public bool IsDownEvent
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				return Event.current != null && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (this.KeyDownEvent || (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift)) || (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl)) || (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt)) || (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand)) || this.IsDown);
			}
		}

		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB));
			}
		}

		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB));
			}
		}

		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}
	}
}
