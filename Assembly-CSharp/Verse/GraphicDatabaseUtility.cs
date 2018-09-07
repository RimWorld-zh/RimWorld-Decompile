using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public static class GraphicDatabaseUtility
	{
		public static IEnumerable<string> GraphicNamesInFolder(string folderPath)
		{
			HashSet<string> loadedAssetNames = new HashSet<string>();
			foreach (Texture2D tex in Resources.LoadAll<Texture2D>("Textures/" + folderPath))
			{
				string origAssetName = tex.name;
				string[] pieces = origAssetName.Split(new char[]
				{
					'_'
				});
				string assetName = string.Empty;
				if (pieces.Length <= 2)
				{
					assetName = pieces[0];
				}
				else if (pieces.Length == 3)
				{
					assetName = pieces[0] + "_" + pieces[1];
				}
				else if (pieces.Length == 4)
				{
					assetName = string.Concat(new string[]
					{
						pieces[0],
						"_",
						pieces[1],
						"_",
						pieces[2]
					});
				}
				else
				{
					Log.Error("Cannot load assets with >3 pieces.", false);
				}
				if (!loadedAssetNames.Contains(assetName))
				{
					loadedAssetNames.Add(assetName);
					yield return assetName;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <GraphicNamesInFolder>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal HashSet<string> <loadedAssetNames>__0;

			internal string folderPath;

			internal Texture2D[] $locvar0;

			internal int $locvar1;

			internal Texture2D <tex>__1;

			internal string <origAssetName>__2;

			internal string[] <pieces>__2;

			internal string <assetName>__2;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GraphicNamesInFolder>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					loadedAssetNames = new HashSet<string>();
					array = Resources.LoadAll<Texture2D>("Textures/" + folderPath);
					i = 0;
					break;
				case 1u:
					IL_19F:
					i++;
					break;
				default:
					return false;
				}
				if (i >= array.Length)
				{
					this.$PC = -1;
				}
				else
				{
					tex = array[i];
					origAssetName = tex.name;
					pieces = origAssetName.Split(new char[]
					{
						'_'
					});
					assetName = string.Empty;
					if (pieces.Length <= 2)
					{
						assetName = pieces[0];
					}
					else if (pieces.Length == 3)
					{
						assetName = pieces[0] + "_" + pieces[1];
					}
					else if (pieces.Length == 4)
					{
						assetName = string.Concat(new string[]
						{
							pieces[0],
							"_",
							pieces[1],
							"_",
							pieces[2]
						});
					}
					else
					{
						Log.Error("Cannot load assets with >3 pieces.", false);
					}
					if (loadedAssetNames.Contains(assetName))
					{
						goto IL_19F;
					}
					loadedAssetNames.Add(assetName);
					this.$current = assetName;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator0 <GraphicNamesInFolder>c__Iterator = new GraphicDatabaseUtility.<GraphicNamesInFolder>c__Iterator0();
				<GraphicNamesInFolder>c__Iterator.folderPath = folderPath;
				return <GraphicNamesInFolder>c__Iterator;
			}
		}
	}
}
