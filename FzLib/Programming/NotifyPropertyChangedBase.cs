using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FzLib.Programming
{
    /// <summary>
    /// 实现 INotifyPropertyChanged 的基类，简化属性通知。
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 设置字段值并触发 PropertyChanged 事件（如果值已更改）。
        /// </summary>
        /// <typeparam name="T">字段类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名（自动捕获）</param>
        /// <returns>如果值已更改，则返回 true</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 触发 PropertyChanged 事件。
        /// </summary>
        /// <param name="propertyName">属性名（自动捕获）</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 批量触发多个属性的 PropertyChanged 事件。
        /// </summary>
        /// <param name="propertyNames">属性名集合</param>
        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                OnPropertyChanged(name);
            }
        }
    }
}