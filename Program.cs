using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConfuserDeobfuscator.StringDeobfuscating;
using OpCodes = dnlib.DotNet.Emit.OpCodes;
using TypeAttributes = dnlib.DotNet.TypeAttributes;
using MethodAttributes = dnlib.DotNet.MethodAttributes;
using MethodImplAttributes = dnlib.DotNet.MethodImplAttributes;
using dnlib.DotNet.Writer;
using dnlib;
using OperandType = System.Reflection.Emit.OperandType;

namespace ConfuserDeobfuscator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Test .NET Reactor 6.3.0.0 deobfuscator by Dark_Bull";
            if (args.Length == 0)
            {
                Console.WriteLine("Drag and drop file");
                Thread.Sleep(2000);
                Environment.Exit(1);
            }
            string pathFile = args[0];
            //string pathFile = "H:\\GitHub\\Madness.NET\\Madness.NET\\Madness.NET\\bin\\x86\\Debug\\MadnessNET_Secure\\MadnessNET.exe";
            ModuleDef moduleDef = ModuleDefMD.Load(pathFile);
            Assembly assemblyDef = Assembly.LoadFile(pathFile);
            MethodInfo methodInfo = (MethodInfo)assemblyDef.EntryPoint;
            StringDecrypt stringDecrypt = new StringDecrypt(ref moduleDef, ref methodInfo);
            //var writerOptions = new dnlib.DotNet.Writer.ModuleWriterOptions(moduleDef);
            //writerOptions.Logger = DummyLogger.NoThrowInstance;
            moduleDef.Write(Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileNameWithoutExtension(args[0]) + "_strCleaned" + Path.GetExtension(args[0]));
            //moduleDef.Write(Path.GetDirectoryName(pathFile) + "\\" + Path.GetFileNameWithoutExtension(pathFile) + "_cleaned" + Path.GetExtension(pathFile));
            Console.Write("OK!\n");

        }
    }
}
