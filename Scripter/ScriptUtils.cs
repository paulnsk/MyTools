using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Scripter
{
    public static class ScriptUtils
    {
        

        /// <summary>
        /// Validates supplied value with the expression. Expression is assumed to return bool; value must be referred to as x
        /// </summary>
        /// <typeparam name="Tx"></typeparam>
        /// <param name="x"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static async Task<Func<Tx, bool>> BoolFuncAsync<Tx>(string expression)
        {            
            var state = await CSharpScript.RunAsync<Func<Tx, bool>>($"Func<{typeof(Tx).FullName}, bool> f = (x) => {expression};", Options);
            state = await state.ContinueWithAsync<Func<Tx, bool>>("f");
            return state.ReturnValue;            
        }

        public static Func<Tx, bool> BoolFunc<Tx>(string expression)
        {
            return BoolFuncAsync<Tx>(expression).GetAwaiter().GetResult();
        }

        public static async Task<Func<DateTime, string>> DatetimeXToStringFuncAsync(string expression)
        {
            var state = await CSharpScript.RunAsync<Func<DateTime,string>>($"Func<DateTime, string> f = (x) => {expression};", Options);
            state = await state.ContinueWithAsync<Func<DateTime, string>>("f");
            return state.ReturnValue;            
        }

        public static Func<DateTime, string> DatetimeXToStringFunc(string expression)
        {
            return DatetimeXToStringFuncAsync(expression).GetAwaiter().GetResult();
        }




        private static ScriptOptions Options { get; set; }

        static ScriptUtils()
        {
            Options= ScriptOptions.Default.AddReferences(typeof(IQueryable).Assembly, typeof(Enumerable).Assembly);
            Options = Options.AddImports("System.Linq");
            Options = Options.AddImports("System");
        }

    }
}
