using System;
using System.Threading.Tasks;

namespace GenTools
{
    // TODO: move to Extensions Library
    public static class TaskHelpers
    {
        public static void LogExceptions(this Task task)
        {
            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                {
                    Console.WriteLine(exception.Message);
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);
        }

        public static Task<T> LogExceptions<T>(this Task<T> task) where T : new()
        {
            return task.ContinueWith<T>((antecedent) =>
            {
                bool isError = false;
                var aggException = antecedent.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                {
                    isError = true;
                    Console.WriteLine(" Task Exception - ", exception.Message);
                }

                if (isError)
                    return new T();
                else
                    return antecedent.Result;
            },
            TaskContinuationOptions.OnlyOnFaulted);   // may need to change to .None
        }

        public static Task<T> IgnoreExceptions<T>(this Task<T> task)
        {
            task.ContinueWith(c => { var ignored = c.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        public static Task IgnoreExceptions(this Task task)
        {
            task.ContinueWith(c => { var ignored = c.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        public static Task<TResult> FromResult<TResult>(TResult result)
        {
            var completionSource = new TaskCompletionSource<TResult>();
            completionSource.SetResult(result);
            return completionSource.Task;
        }



    }


}
