using System;

namespace Navigation
{
    public static class Functional
    {
        public static Func<TResult> Apply<TArg, TResult>(this Func<TArg, TResult> fn, TArg arg)
        {
            return () => fn(arg);
        }
    }
}
