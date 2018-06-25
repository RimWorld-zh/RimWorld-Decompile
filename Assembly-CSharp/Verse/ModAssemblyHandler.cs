using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Verse
{
	// Token: 0x02000CC2 RID: 3266
	public class ModAssemblyHandler
	{
		// Token: 0x040030D2 RID: 12498
		private ModContentPack mod;

		// Token: 0x040030D3 RID: 12499
		public List<Assembly> loadedAssemblies = new List<Assembly>();

		// Token: 0x040030D4 RID: 12500
		private static bool globalResolverIsSet = false;

		// Token: 0x0600480F RID: 18447 RVA: 0x0025F035 File Offset: 0x0025D435
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x0025F050 File Offset: 0x0025D450
		public void ReloadAll()
		{
			if (!ModAssemblyHandler.globalResolverIsSet)
			{
				ResolveEventHandler @object = (object obj, ResolveEventArgs args) => Assembly.GetExecutingAssembly();
				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyResolve += @object.Invoke;
				ModAssemblyHandler.globalResolverIsSet = true;
			}
			string path = Path.Combine(this.mod.RootDir, "Assemblies");
			string path2 = Path.Combine(GenFilePaths.CoreModsFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(path2);
			if (directoryInfo.Exists)
			{
				foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
				{
					if (!(fileInfo.Extension.ToLower() != ".dll"))
					{
						Assembly assembly = null;
						try
						{
							byte[] rawAssembly = File.ReadAllBytes(fileInfo.FullName);
							string fileName = Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.FullName)) + ".pdb";
							FileInfo fileInfo2 = new FileInfo(fileName);
							if (fileInfo2.Exists)
							{
								byte[] rawSymbolStore = File.ReadAllBytes(fileInfo2.FullName);
								assembly = AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
							}
							else
							{
								assembly = AppDomain.CurrentDomain.Load(rawAssembly);
							}
						}
						catch (Exception ex)
						{
							Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString(), false);
							break;
						}
						if (assembly != null)
						{
							if (this.AssemblyIsUsable(assembly))
							{
								this.loadedAssemblies.Add(assembly);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0025F220 File Offset: 0x0025D620
		private bool AssemblyIsUsable(Assembly asm)
		{
			try
			{
				asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"ReflectionTypeLoadException getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex
				}));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Loader exceptions:");
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception ex2 in ex.LoaderExceptions)
					{
						stringBuilder.AppendLine("   => " + ex2.ToString());
					}
				}
				Log.Error(stringBuilder.ToString(), false);
				return false;
			}
			catch (Exception ex3)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex3
				}), false);
				return false;
			}
			return true;
		}
	}
}
