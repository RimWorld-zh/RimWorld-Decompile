using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Steamworks
{
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
	internal class CallbackIdentityAttribute : Attribute
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int <Identity>k__BackingField;

		public CallbackIdentityAttribute(int callbackNum)
		{
			this.Identity = callbackNum;
		}

		public int Identity
		{
			[CompilerGenerated]
			get
			{
				return this.<Identity>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Identity>k__BackingField = value;
			}
		}
	}
}
