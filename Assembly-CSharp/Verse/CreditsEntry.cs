using System;
using UnityEngine;

namespace Verse
{
	public abstract class CreditsEntry
	{
		protected CreditsEntry()
		{
		}

		public abstract float DrawHeight(float width);

		public abstract void Draw(Rect rect);
	}
}
