using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Scripter
{
    public static class ScripterExtensions
    {
        



        /// <summary>
        /// Validates supplied value with the expression. Expression is assumed to return bool; value must be referred to as x
        /// </summary>
        /// <typeparam name="Tx"></typeparam>
        /// <param name="x"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static async Task<bool> TestX<Tx>(this Tx x, string expression)
        {            
            var state = await CSharpScript.RunAsync<Func<Tx, bool>>($"Func<{typeof(Tx).FullName}, bool> f = (x) => {expression};", Options);
            state = await state.ContinueWithAsync<Func<Tx, bool>>("f");
            var func = state.ReturnValue;
            return func(x);            
        }

        public static async Task<string> DatetimeXToString(DateTime x, string expression)
        {
            var state = await CSharpScript.RunAsync<Func<DateTime,string>>($"Func<DateTime, string> f = (x) => {expression};", Options);
            state = await state.ContinueWithAsync<Func<DateTime, string>>("f");
            var func = state.ReturnValue;
            return func(x);
        }



        
        private static ScriptOptions Options { get; set; }

        static ScripterExtensions()
        {
            Options= ScriptOptions.Default.AddReferences(typeof(IQueryable).Assembly, typeof(Enumerable).Assembly);
            Options = Options.AddImports("System.Linq");
            Options = Options.AddImports("System");
        }

    }
}
