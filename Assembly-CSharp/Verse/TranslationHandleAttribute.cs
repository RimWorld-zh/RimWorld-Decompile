using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse
{
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int <Priority>k__BackingField;

		public TranslationHandleAttribute()
		{
		}

		public int Priority
		{
			[CompilerGenerated]
			get
			{
				return this.<Priority>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Priority>k__BackingField = value;
			}
		}
	}
}
