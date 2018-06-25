using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public static class ZoneColorUtility
	{
		private static List<Color> growingZoneColors = new List<Color>();

		private static List<Color> storageZoneColors = new List<Color>();

		private static int nextGrowingZoneColorIndex = 0;

		private static int nextStorageZoneColorIndex = 0;

		private const float ZoneOpacity = 0.09f;

		static ZoneColorUtility()
		{
			foreach (Color color in ZoneColorUtility.GrowingZoneColors())
			{
				Color item = new Color(color.r, color.g, color.b, 0.09f);
				ZoneColorUtility.growingZoneColors.Add(item);
			}
			foreach (Color color2 in ZoneColorUtility.StorageZoneColors())
			{
				Color item2 = new Color(color2.r, color2.g, color2.b, 0.09f);
				ZoneColorUtility.storageZoneColors.Add(item2);
			}
		}

		public static Color NextGrowingZoneColor()
		{
			Color result = ZoneColorUtility.growingZoneColors[ZoneColorUtility.nextGrowingZoneColorIndex];
			ZoneColorUtility.nextGrowingZoneColorIndex++;
			if (ZoneColorUtility.nextGrowingZoneColorIndex >= ZoneColorUtility.growingZoneColors.Count)
			{
				ZoneColorUtility.nextGrowingZoneColorIndex = 0;
			}
			return result;
		}

		public static Color NextStorageZoneColor()
		{
			Color result = ZoneColorUtility.storageZoneColors[ZoneColorUtility.nextStorageZoneColorIndex];
			ZoneColorUtility.nextStorageZoneColorIndex++;
			if (ZoneColorUtility.nextStorageZoneColorIndex >= ZoneColorUtility.storageZoneColors.Count)
			{
				ZoneColorUtility.nextStorageZoneColorIndex = 0;
			}
			return result;
		}

		private static IEnumerable<Color> GrowingZoneColors()
		{
			yield return Color.Lerp(new Color(0f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0.5f), Color.gray, 0.5f);
			yield break;
		}

		private static IEnumerable<Color> StorageZoneColors()
		{
			yield return Color.Lerp(new Color(1f, 0f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0.5f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 0f, 1f), Color.gray, 0.5f);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GrowingZoneColors>c__Iterator0 : IEnumerable, IEnumerable<Color>, IEnumerator, IDisposable, IEnumerator<Color>
		{
			internal Color $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GrowingZoneColors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = Color.Lerp(new Color(0f, 1f, 0f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Color.Lerp(new Color(1f, 1f, 0f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Color.Lerp(new Color(0.5f, 1f, 0f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Color.Lerp(new Color(1f, 1f, 0.5f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Color.Lerp(new Color(0.5f, 1f, 0.5f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Color IEnumerator<Color>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<UnityEngine.Color>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ZoneColorUtility.<GrowingZoneColors>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <StorageZoneColors>c__Iterator1 : IEnumerable, IEnumerable<Color>, IEnumerator, IDisposable, IEnumerator<Color>
		{
			internal Color $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <StorageZoneColors>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = Color.Lerp(new Color(1f, 0f, 0f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Color.Lerp(new Color(1f, 0f, 1f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Color.Lerp(new Color(0f, 0f, 1f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Color.Lerp(new Color(1f, 0f, 0.5f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Color.Lerp(new Color(0f, 0.5f, 1f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Color.Lerp(new Color(0.5f, 0f, 1f), Color.gray, 0.5f);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Color IEnumerator<Color>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<UnityEngine.Color>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ZoneColorUtility.<StorageZoneColors>c__Iterator1();
			}
		}
	}
}
