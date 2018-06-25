using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse.Sound
{
	public struct SoundInfo
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <IsOnCamera>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private TargetInfo <Maker>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MaintenanceType <Maintenance>k__BackingField;

		private Dictionary<string, float> parameters;

		public float volumeFactor;

		public float pitchFactor;

		public bool testPlay;

		public bool IsOnCamera
		{
			[CompilerGenerated]
			get
			{
				return this.<IsOnCamera>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsOnCamera>k__BackingField = value;
			}
		}

		public TargetInfo Maker
		{
			[CompilerGenerated]
			get
			{
				return this.<Maker>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Maker>k__BackingField = value;
			}
		}

		public MaintenanceType Maintenance
		{
			[CompilerGenerated]
			get
			{
				return this.<Maintenance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Maintenance>k__BackingField = value;
			}
		}

		public IEnumerable<KeyValuePair<string, float>> DefinedParameters
		{
			get
			{
				if (this.parameters == null)
				{
					yield break;
				}
				foreach (KeyValuePair<string, float> kvp in this.parameters)
				{
					yield return kvp;
				}
				yield break;
			}
		}

		public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = true;
			result.Maintenance = maint;
			result.Maker = TargetInfo.Invalid;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = false;
			result.Maintenance = maint;
			result.Maker = maker;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		public override string ToString()
		{
			string text = null;
			if (this.parameters != null && this.parameters.Count > 0)
			{
				text = "parameters=";
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						keyValuePair.Key.ToString(),
						"-",
						keyValuePair.Value.ToString(),
						" "
					});
				}
			}
			string text3 = null;
			if (this.Maker.HasThing || this.Maker.Cell.IsValid)
			{
				text3 = this.Maker.ToString();
			}
			string text4 = null;
			if (this.Maintenance != MaintenanceType.None)
			{
				text4 = ", Maint=" + this.Maintenance;
			}
			return string.Concat(new string[]
			{
				"(",
				(!this.IsOnCamera) ? "World from " : "Camera",
				text3,
				text,
				text4,
				")"
			});
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<KeyValuePair<string, float>>, IEnumerator, IDisposable, IEnumerator<KeyValuePair<string, float>>
		{
			internal Dictionary<string, float>.Enumerator $locvar0;

			internal KeyValuePair<string, float> <kvp>__1;

			internal SoundInfo $this;

			internal KeyValuePair<string, float> $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (this.parameters == null)
					{
						return false;
					}
					enumerator = this.parameters.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						kvp = enumerator.Current;
						this.$current = kvp;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			KeyValuePair<string, float> IEnumerator<KeyValuePair<string, float>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string,float>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<KeyValuePair<string, float>> IEnumerable<KeyValuePair<string, float>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SoundInfo.<>c__Iterator0 <>c__Iterator = new SoundInfo.<>c__Iterator0();
				<>c__Iterator.$this = ref this;
				return <>c__Iterator;
			}
		}
	}
}
