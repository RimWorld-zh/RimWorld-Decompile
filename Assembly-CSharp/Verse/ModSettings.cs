using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse
{
	public abstract class ModSettings : IExposable
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Mod <Mod>k__BackingField;

		protected ModSettings()
		{
		}

		public Mod Mod
		{
			[CompilerGenerated]
			get
			{
				return this.<Mod>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<Mod>k__BackingField = value;
			}
		}

		public virtual void ExposeData()
		{
		}

		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
		}
	}
}
