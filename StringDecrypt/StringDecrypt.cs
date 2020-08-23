using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace ConfuserDeobfuscator.StringDeobfuscating
{
    public class StringDecrypt
    {
        private ModuleDef moduleDef { get; }
        private MethodInfo methodInfo { get; }
        public StringDecrypt(ref ModuleDef moduleDef, ref MethodInfo methodInfo)
        {
            this.moduleDef = moduleDef;
            this.methodInfo = methodInfo;
            Decrypt();
        }

        void Decrypt()
        {
            foreach (var typeDef in moduleDef.Types)
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    
                    if (methodDef.Body == null) continue;
                    for (int i = 0; i < methodDef.Body.Instructions.Count(); i++)
                    {
                        if (methodDef.Body.Instructions[i].IsLdcI4() &&
                            methodDef.Body.Instructions[i + 1].OpCode.Name == "call")
                        {
                            dynamic operand_call = methodDef.Body.Instructions[i + 1].Operand;
                            if (operand_call.ReturnType.TypeName == "String" && operand_call.Parameters[0].Type.TypeName == "Int32")
                            {
                                dynamic operand_ldc = methodDef.Body.Instructions[i].Operand;
                                string decrypt_string = InvokeDecryptMethod(operand_ldc, operand_call.Module.Name, operand_call.DeclaringType2.Name, operand_call.Name.String);
                                methodDef.Body.Instructions[i].OpCode = OpCodes.Nop;
                                methodDef.Body.Instructions[i + 1] = new Instruction(OpCodes.Ldstr, decrypt_string);
                                methodDef.Body.OptimizeBranches();
                                methodDef.Body.SimplifyBranches();
                            }
                        }
                    }
                }
            }
        }


        string InvokeDecryptMethod(int seed, string moduleName, string typeName, string methodName)
        {
            string decryptedString;
            var module = methodInfo.Module.Assembly.GetModule(moduleName);
            foreach (var type in module.GetTypes())
            {
                if (type.Name == typeName)
                {
                    foreach (var methodType in type.GetRuntimeMethods())
                    {
                        if (methodType.Name == methodName)
                        {
                            decryptedString = (string)methodType.Invoke(null, new object[] { seed });
                            return decryptedString;
                        }
                    }
                }
            }
            return "NULL";
        }
    }
}
