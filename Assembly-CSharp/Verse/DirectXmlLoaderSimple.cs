using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Verse
{
	public static class DirectXmlLoaderSimple
	{
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(FileInfo file)
		{
			XDocument doc = XDocument.Load(file.FullName, LoadOptions.SetLineInfo);
			foreach (XElement element in doc.Root.Elements())
			{
				string key = element.Name.ToString();
				string value = element.Value;
				value = value.Replace("\\n", "\n");
				yield return new DirectXmlLoaderSimple.XmlKeyValuePair
				{
					key = key,
					value = value,
					lineNumber = ((IXmlLineInfo)element).LineNumber
				};
			}
			yield break;
		}

		public struct XmlKeyValuePair
		{
			public string key;

			public string value;

			public int lineNumber;
		}

		[CompilerGenerated]
		private sealed class <ValuesFromXmlFile>c__Iterator0 : IEnumerable, IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair>, IEnumerator, IDisposable, IEnumerator<DirectXmlLoaderSimple.XmlKeyValuePair>
		{
			internal FileInfo file;

			internal XDocument <doc>__0;

			internal IEnumerator<XElement> $locvar0;

			internal XElement <element>__1;

			internal string <key>__2;

			internal string <value>__2;

			internal DirectXmlLoaderSimple.XmlKeyValuePair <pair>__2;

			internal DirectXmlLoaderSimple.XmlKeyValuePair $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ValuesFromXmlFile>c__Iterator0()
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
					doc = XDocument.Load(file.FullName, LoadOptions.SetLineInfo);
					enumerator = doc.Root.Elements().GetEnumerator();
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
						element = enumerator.Current;
						key = element.Name.ToString();
						value = element.Value;
						value = value.Replace("\\n", "\n");
						DirectXmlLoaderSimple.XmlKeyValuePair pair = default(DirectXmlLoaderSimple.XmlKeyValuePair);
						pair.key = key;
						pair.value = value;
						pair.lineNumber = ((IXmlLineInfo)element).LineNumber;
						this.$current = pair;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			DirectXmlLoaderSimple.XmlKeyValuePair IEnumerator<DirectXmlLoaderSimple.XmlKeyValuePair>.Current
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
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
				return this.System.Collections.Generic.IEnumerable<Verse.DirectXmlLoaderSimple.XmlKeyValuePair>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<DirectXmlLoaderSimple.XmlKeyValuePair> IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DirectXmlLoaderSimple.<ValuesFromXmlFile>c__Iterator0 <ValuesFromXmlFile>c__Iterator = new DirectXmlLoaderSimple.<ValuesFromXmlFile>c__Iterator0();
				<ValuesFromXmlFile>c__Iterator.file = file;
				return <ValuesFromXmlFile>c__Iterator;
			}
		}
	}
}
