using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class KeyBindingDef : Def
	{
		public KeyBindingCategoryDef category;

		public KeyCode defaultKeyCodeA;

		public KeyCode defaultKeyCodeB;

		public bool devModeOnly;

		public List<string> extraConflictTags;

		public KeyCode MainKey
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != 0)
					{
						return keyBindingData.keyBindingA;
					}
					if (keyBindingData.keyBindingB != 0)
					{
						return keyBindingData.keyBindingB;
					}
				}
				return KeyCode.None;
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
				if (Event.current.type == EventType.KeyDown && Event.current.keyCode != 0 && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != KeyCode.LeftCommand && keyBindingData.keyBindingA != KeyCode.RightCommand && keyBindingData.keyBindingB != KeyCode.LeftCommand && keyBindingData.keyBindingB != KeyCode.RightCommand && Event.current.command)
					{
						return false;
					}
					return Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB;
				}
				return false;
			}
		}

		public bool IsDownEvent
		{
			get
			{
				if (Event.current == null)
				{
					return false;
				}
				KeyBindingData keyBindingData = default(KeyBindingData);
				if (!KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					return false;
				}
				if (this.KeyDownEvent)
				{
					return true;
				}
				if (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift))
				{
					return true;
				}
				if (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl))
				{
					return true;
				}
				if (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt))
				{
					return true;
				}
				if (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand))
				{
					return true;
				}
				return this.IsDown;
			}
		}

		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					return Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB);
				}
				return false;
			}
		}

		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData = default(KeyBindingData);
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					return Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB);
				}
				return false;
			}
		}

		public KeyCode GetDefaultKeyCode(KeyPrefs.BindingSlot slot)
		{
			switch (slot)
			{
			case KeyPrefs.BindingSlot.A:
				return this.defaultKeyCodeA;
			case KeyPrefs.BindingSlot.B:
				return this.defaultKeyCodeB;
			default:
				throw new InvalidOperationException();
			}
		}

		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}
	}
}
