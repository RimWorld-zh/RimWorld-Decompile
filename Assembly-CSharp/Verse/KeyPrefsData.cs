using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class KeyPrefsData
	{
		public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();

		public void ResetToDefaults()
		{
			this.keyPrefs.Clear();
			this.AddMissingDefaultBindings();
		}

		public void AddMissingDefaultBindings()
		{
			foreach (KeyBindingDef allDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (!this.keyPrefs.ContainsKey(allDef))
				{
					this.keyPrefs.Add(allDef, new KeyBindingData(allDef.defaultKeyCodeA, allDef.defaultKeyCodeB));
				}
			}
		}

		public bool SetBinding(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot, KeyCode keyCode)
		{
			KeyBindingData keyBindingData = default(KeyBindingData);
			if (this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				switch (slot)
				{
				case KeyPrefs.BindingSlot.A:
				{
					keyBindingData.keyBindingA = keyCode;
					break;
				}
				case KeyPrefs.BindingSlot.B:
				{
					keyBindingData.keyBindingB = keyCode;
					break;
				}
				default:
				{
					Log.Error("Tried to set a key binding for \"" + keyDef.LabelCap + "\" on a nonexistent slot: " + ((Enum)(object)slot).ToString());
					return false;
				}
				}
				return true;
			}
			Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
			return false;
		}

		public KeyCode GetBoundKeyCode(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyBindingData keyBindingData = default(KeyBindingData);
			if (!this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
				return KeyCode.None;
			}
			switch (slot)
			{
			case KeyPrefs.BindingSlot.A:
			{
				return keyBindingData.keyBindingA;
			}
			case KeyPrefs.BindingSlot.B:
			{
				return keyBindingData.keyBindingB;
			}
			default:
			{
				throw new InvalidOperationException();
			}
			}
		}

		private IEnumerable<KeyBindingDef> ConflictingBindings(KeyBindingDef keyDef, KeyCode code)
		{
			foreach (KeyBindingDef allDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				KeyBindingData prefData;
				if (allDef != keyDef && (allDef.category == keyDef.category || keyDef.category.checkForConflicts.Contains(allDef.category)) && this.keyPrefs.TryGetValue(allDef, out prefData) && (prefData.keyBindingA == code || prefData.keyBindingB == code))
				{
					yield return allDef;
				}
			}
		}

		public void EraseConflictingBindingsForKeyCode(KeyBindingDef keyDef, KeyCode keyCode, Action<KeyBindingDef> callBackOnErase = null)
		{
			foreach (KeyBindingDef item in this.ConflictingBindings(keyDef, keyCode))
			{
				KeyBindingData keyBindingData = this.keyPrefs[item];
				if (keyBindingData.keyBindingA == keyCode)
				{
					keyBindingData.keyBindingA = KeyCode.None;
				}
				if (keyBindingData.keyBindingB == keyCode)
				{
					keyBindingData.keyBindingB = KeyCode.None;
				}
				if ((object)callBackOnErase != null)
				{
					callBackOnErase(item);
				}
			}
		}

		public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != 0)
			{
				this.EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode, null);
				this.SetBinding(keyDef, slot, boundKeyCode);
			}
		}

		public KeyPrefsData Clone()
		{
			KeyPrefsData keyPrefsData = new KeyPrefsData();
			Dictionary<KeyBindingDef, KeyBindingData>.Enumerator enumerator = this.keyPrefs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<KeyBindingDef, KeyBindingData> current = enumerator.Current;
					keyPrefsData.keyPrefs[current.Key] = new KeyBindingData(current.Value.keyBindingA, current.Value.keyBindingB);
				}
				return keyPrefsData;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}

		public void ErrorCheck()
		{
			foreach (KeyBindingDef allDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				this.ErrorCheckOn(allDef, KeyPrefs.BindingSlot.A);
				this.ErrorCheckOn(allDef, KeyPrefs.BindingSlot.B);
			}
		}

		private void ErrorCheckOn(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, KeyPrefs.BindingSlot.A);
			if (boundKeyCode != 0)
			{
				foreach (KeyBindingDef item in this.ConflictingBindings(keyDef, boundKeyCode))
				{
					Log.Error("Key binding conflict: " + item + " and " + keyDef + " are both bound to " + boundKeyCode);
				}
			}
		}
	}
}
