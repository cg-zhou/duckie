using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Duckie.Utils.Localization
{
    /// <summary>
    /// XAML本地化标记扩展，用于在XAML中绑定本地化字符串
    /// 使用方式：{local:Localize Key=ResourceKey}
    /// </summary>
    [MarkupExtensionReturnType(typeof(BindingExpression))]
    public class LocalizeExtension : MarkupExtension
    {
        /// <summary>
        /// 资源键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 格式化参数（可选）
        /// </summary>
        public object[] Args { get; set; }

        public LocalizeExtension()
        {
        }

        public LocalizeExtension(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 提供本地化值
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>绑定表达式</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return "[Missing Key]";
            }

            // 在设计时返回键名，便于设计器显示
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return $"[{Key}]";
            }

            // 创建绑定到嵌入式本地化管理器的绑定
            var binding = new Binding("CurrentCulture")
            {
                Source = EmbeddedLocalizationManager.Instance,
                Converter = new LocalizationConverter(),
                ConverterParameter = new LocalizationParameter { Key = Key, Args = Args },
                Mode = BindingMode.OneWay
            };

            // 如果可以获取目标对象，返回绑定表达式
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target)
            {
                if (target.TargetObject is DependencyObject dependencyObject &&
                    target.TargetProperty is DependencyProperty dependencyProperty)
                {
                    return binding.ProvideValue(serviceProvider);
                }
            }

            // 否则直接返回当前值
            return LocUtils.GetString(Key, Args);
        }
    }

    /// <summary>
    /// 本地化参数
    /// </summary>
    public class LocalizationParameter
    {
        public string Key { get; set; }
        public object[] Args { get; set; }
    }

    /// <summary>
    /// 本地化转换器
    /// </summary>
    public class LocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is LocalizationParameter param && !string.IsNullOrEmpty(param.Key))
            {
                return LocUtils.GetString(param.Key, param.Args);
            }

            return "[Invalid Parameter]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
