using System;
using UnityEngine;

namespace Verse
{
	public class KeyBindingData
	{
		public KeyCode keyBindingA;

		public KeyCode keyBindingB;

		public KeyBindingData()
		{
		}

		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		public override string ToString()
		{
			string str = "[";
			if (this.keyBindingA != 0)
			{
				str += ((Enum)(object)this.keyBindingA).ToString();
			}
			if (this.keyBindingB != 0)
			{
				str = str + ", " + ((Enum)(object)this.keyBindingB).ToString();
			}
			return str + "]";
		}
	}
}
