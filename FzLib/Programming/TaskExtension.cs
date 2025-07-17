using System;
using System.Threading;
using System.Threading.Tasks;

namespace FzLib.Programming
{
    public static class TaskExtension
    {
        /// <summary>
        /// 为任务设置超时时间，超时后抛出 <see cref="TimeoutException"/>。
        /// </summary>
        /// <param name="task">要监视的任务</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>原始任务的结果（如果未超时）</returns>
        /// <exception cref="TimeoutException">任务未在指定时间内完成</exception>
        /// <exception cref="OperationCanceledException">任务被取消</exception>
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (task.IsCompleted)
            {
                await task;
                return;
            }

            using var timeoutCts = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, timeoutCts.Token);
            var completedTask = await Task.WhenAny(task, delayTask).ConfigureAwait(false);

            if (completedTask == task)
            {
                timeoutCts.Cancel(); // 取消 Delay 任务
                await task.ConfigureAwait(false); // 传播可能的异常（包括取消）
            }
            else
            {
                throw new TimeoutException($"任务未在 {timeout.TotalSeconds} 秒内完成。");
            }
        }

        /// <summary>
        /// 为任务设置超时时间，超时后抛出 <see cref="TimeoutException"/>（泛型版本）。
        /// </summary>
        /// <typeparam name="TResult">任务返回类型</typeparam>
        /// <param name="task">要监视的任务</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>原始任务的结果（如果未超时）</returns>
        /// <exception cref="TimeoutException">任务未在指定时间内完成</exception>
        /// <exception cref="OperationCanceledException">任务被取消</exception>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (task.IsCompleted)
            {
                return await task.ConfigureAwait(false);
            }

            using var timeoutCts = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, timeoutCts.Token);
            var completedTask = await Task.WhenAny(task, delayTask).ConfigureAwait(false);

            if (completedTask == task)
            {
                timeoutCts.Cancel();
                return await task.ConfigureAwait(false); // 传播异常
            }
            else
            {
                throw new TimeoutException($"任务未在 {timeout.TotalSeconds} 秒内完成。");
            }
        }
    }
}