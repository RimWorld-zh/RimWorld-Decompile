using System;

namespace Verse
{
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		public bool sendLetterWhenDiscovered = false;

		public string discoverLetterLabel = null;

		public string discoverLetterText = null;

		public MessageTypeDef messageType = null;

		public LetterDef letterType = null;

		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}
	}
}
