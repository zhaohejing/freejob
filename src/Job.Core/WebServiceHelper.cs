using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Job.Core {
    public class WebServiceHelper {
        #region InvokeWebService

        /// <summary>
        /// 动态调用web服务
        /// </summary>
        /// <param name="url"></param>
        /// <param name="methodname"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeWebService(string url, string methodname, object[] args) {
            return InvokeWebService(url, null, methodname, args);
        }

        /// <summary>
        /// 动态调用web服务
        /// </summary>
        /// <param name="url">ws地址</param>
        /// <param name="classname">名称</param>
        /// <param name="methodname">方法名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args) {
            //命名空间
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            //类名为空
            if (classname == null || classname == "") {
                classname = GetWsClassName(url);
            }
            //试图开始执行方法
            try {
                //获取WSDL
                WebClient wc = new WebClient();
                //wc.Headers.Add("securitydomain", HttpContext.Current.Request.Url.DnsSafeHost);
                //获取流
                System.IO.Stream stream = wc.OpenRead(url + "?WSDL");
                //服务读取
                ServiceDescription sd = ServiceDescription.Read(stream);
                //代理方法
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                //声明命名空间
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                //生成C#的访问实例
                CSharpCodeProvider csc = new CSharpCodeProvider();
                //编译接口
                ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                //编译代理包含错误
                if (true == cr.Errors.HasErrors) {
                    //错误信息
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //循环错误信息 赋值
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors) {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    //throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                //根据命名空间 及类名获取类型
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                //获取现有对象
                object obj = Activator.CreateInstance(t);
                //获取对象的方法
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                return mi.Invoke(obj, args);
            }
            //异常
            catch {

                return null;
                //throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        /// <summary>
        /// 获取WebService类名
        /// </summary>
        /// <param name="wsUrl">ws地址</param>
        /// <returns></returns>
        private static string GetWsClassName(string wsUrl) {
            //数组
            string[] parts = wsUrl.Split('/');
            //类名数组
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion#region InvokeWebService


    }
}
