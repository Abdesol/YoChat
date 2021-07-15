using System;
using System.Collections.Generic;
using System.Text;

namespace YoChat
{
    public interface IThemeService
    {
        event EventHandler ThemeChanged;
        bool IsLightTheme { get; set; }
    }

    public class ThemeService : IThemeService
    {
        private bool _isLightTheme;
        public bool IsLightTheme
        {
            get => _isLightTheme;
            set
            {
                _isLightTheme = value;
                ThemeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ThemeChanged;

    }
}
