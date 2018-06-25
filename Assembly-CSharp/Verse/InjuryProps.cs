using System;

namespace Verse
{
	public class InjuryProps
	{
		public float painPerSeverity = 1f;

		public float averagePainPerSeverityPermanent = 0.5f;

		public float bleedRate = 0f;

		public bool canMerge = false;

		public string destroyedLabel = null;

		public string destroyedOutLabel = null;

		public bool useRemovedLabel = false;

		public InjuryProps()
		{
		}
	}
}
