using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Shared
{
    public static class CurrentCultureInfo
    {
        #region Fields & Properties

        private static int _lpdwProcessId;
        private static readonly InputLanguageCollection InstalledInputLanguages = InputLanguage.InstalledInputLanguages;
        private static CultureInfo _currentInputLanguage;

        public static string InputLangTwoLetterISOLanguageName => _currentInputLanguage.TwoLetterISOLanguageName;

        public static string InputLangThreeLetterWindowsLanguageName => _currentInputLanguage.ThreeLetterWindowsLanguageName;

        public static string InputLangThreeLetterISOLanguageName => _currentInputLanguage.ThreeLetterISOLanguageName;

        public static string InputLangNativeName => _currentInputLanguage.NativeName;

        public static string InputLangName => _currentInputLanguage.Name;

        public static int InputLangLCID => _currentInputLanguage.LCID;

        public static int InputLangKeyboardLayoutId => _currentInputLanguage.KeyboardLayoutId;

        public static string InputLangEnglishName => _currentInputLanguage.EnglishName;

        public static string InputLangDisplayName => _currentInputLanguage.DisplayName;

        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetKeyboardLayout(int windowsThreadProcessId);

        public static int GetKeyboardLayoutIdAtTime()
        {
            IntPtr hWnd = GetForegroundWindow();
            int winThreadProcId = GetWindowThreadProcessId(hWnd, out _lpdwProcessId);
            IntPtr keybLayout = GetKeyboardLayout(winThreadProcId);
            for (int i = 0; i < InstalledInputLanguages.Count; i++)
            {
                if (keybLayout == InstalledInputLanguages[i].Handle)
                    _currentInputLanguage = InstalledInputLanguages[i].Culture;
            }
           return _currentInputLanguage.KeyboardLayoutId;
        }
    }
}
