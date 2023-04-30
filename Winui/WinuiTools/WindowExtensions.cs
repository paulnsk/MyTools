// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using PInvoke;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinuiTools
{
    public static class WindowExtensions
    {
        #region public

        public static bool IsMaximized(this Window @this)
        {
            return GetOverlappedPresenter(@this)?.State == OverlappedPresenterState.Maximized;
        }

        public static bool IsRestored(this Window @this)
        {
            return GetOverlappedPresenter(@this)?.State == OverlappedPresenterState.Restored;
        }

        public static void Maximize(this Window @this)
        {
            GetOverlappedPresenter(@this)?.Maximize();
        }

        public static void Restore(this Window @this)
        {
            GetOverlappedPresenter(@this)?.Restore();
        }

        public static void Minimize(this Window @this)
        {
            GetOverlappedPresenter(@this)?.Minimize();
        }

        public static void MakeModal(this Window @this, Window owner)
        {
            //todo *** сделать из этого диалог
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(@this);
            var hwndOwner = WinRT.Interop.WindowNative.GetWindowHandle(owner);
            User32.SetWindowLongPtr(hwnd, User32.WindowLongIndexFlags.GWLP_HWNDPARENT, hwndOwner);

            var presenter = GetOverlappedPresenter(@this);
            if (presenter != null) presenter.IsModal = true;
        }

        public static void MakeTopmost(this Window @this)
        {
            var presenter = GetOverlappedPresenter(@this);
            if (presenter != null) presenter.IsAlwaysOnTop = true;
        }


        /// <summary>
        /// Attempts to send window to another display, returns true if successful
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool SendToSecondMonitor(this Window @this)
        {

            var otherDisplay = GetOtherDisplay(@this);

            var appWindow = GetAppWindow(@this);

            if (otherDisplay.display != null)
            {
                appWindow.MoveAndResize(new RectInt32
                {
                    Height = appWindow.Size.Height,
                    Width = appWindow.Size.Width,
                    X = otherDisplay.display.WorkArea.X,
                    Y = otherDisplay.display.WorkArea.Y
                });
                return otherDisplay.isOther;
            }

            return false;
        }

        public static void MoveToBottomRightCorner(this Window @this)
        {
            var display = GetCurrentDisplay(@this);
            var appWindow = GetAppWindow(@this);
            appWindow.MoveAndResize(new RectInt32
            {
                Height = appWindow.Size.Height,
                Width = appWindow.Size.Width,
                X = display.WorkArea.Width - appWindow.Size.Width,
                Y = display.WorkArea.Height - appWindow.Size.Height
            });
        }

        public static void SetFullScreen(this Window @this)
        {
            GetAppWindow(@this).SetPresenter(AppWindowPresenterKind.FullScreen);
        }

        public static void ExitFullScreen(this Window @this)
        {
            GetAppWindow(@this).SetPresenter(AppWindowPresenterKind.Overlapped);
        }
        public static void ToggleFullScreen(this Window @this)
        {
            var aw = GetAppWindow(@this);
            var pr = aw.Presenter;
            aw.SetPresenter(pr is OverlappedPresenter
                ? AppWindowPresenterKind.FullScreen
                : AppWindowPresenterKind.Overlapped);
        }

        public static void SetClosingEvent(this Window @this, TypedEventHandler<AppWindow, AppWindowClosingEventArgs> eventHandler)
        {
            GetAppWindow(@this).Closing += eventHandler;
        }

        private static void SetWindowSize(this Window @this, int width, int height)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(@this);
            var dpi = PInvoke.User32.GetDpiForWindow(hwnd);
            var scalingFactor = (float)dpi / 96;
            width = (int)(width * scalingFactor);
            height = (int)(height * scalingFactor);

            User32.SetWindowPos(hwnd, PInvoke.User32.SpecialWindowHandles.HWND_TOP,
                0, 0, width, height,
                User32.SetWindowPosFlags.SWP_NOMOVE);
        }

        #endregion public


        #region private
        private static AppWindow GetAppWindow(this Window @this)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(@this);
            var myWndId = Win32Interop.GetWindowIdFromWindow(hwnd);
            return AppWindow.GetFromWindowId(myWndId);
        }

        private static OverlappedPresenter GetOverlappedPresenter(this Window @this)
        {
            var p = @this.GetAppWindow().Presenter;
            return p as OverlappedPresenter;
        }


        private static DisplayArea GetCurrentDisplay(this Window @this)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(@this);
            var myWndId = Win32Interop.GetWindowIdFromWindow(hwnd);
            return DisplayArea.GetFromWindowId(myWndId, DisplayAreaFallback.Primary);
        }

        private static bool TrulyEquals(this DisplayArea? @this, DisplayArea? other)
        {
            if (@this == null && other == null) return true;
            return
                @this?.WorkArea.X == other?.WorkArea.X &&
                @this?.WorkArea.Y == other?.WorkArea.Y &&
                @this?.WorkArea.Height == other?.WorkArea.Height &&
                @this?.WorkArea.Width == other?.WorkArea.Width;
        }

        private static string RectInt32ToString(this RectInt32 @this)
        {
            return $"X={@this.X}, Y={@this.Y}, H={@this.Height}, W={@this.Width}";
        }

        private static (DisplayArea display, bool isOther) GetOtherDisplay(this Window @this)
        {
            var currentDisplay = GetCurrentDisplay(@this);
            var result = (currentDisplay, false);
            var all = DisplayArea.FindAll();

            //do not convert this to foreach, for some reason it throws invalid cast
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < all.Count; i++)
            {
                var display = all[i];
                var x = display.WorkArea.X;
                if (!display.TrulyEquals(currentDisplay))
                {
                    result = (display, true);
                    break;
                }
            }

            return result;
        }

        //private static DisplayRegion GetOtherDisplayRegion(DisplayRegion currentAppDisplayRegion)
        //{
        //    // Get the list of all DisplayRegions defined for the WindowingEnvironment that our application is currently in
        //    var displayRegions = ApplicationView.GetForCurrentView().WindowingEnvironment.GetDisplayRegions();
        //    foreach (var displayRegion in displayRegions)
        //    {
        //        if (displayRegion != currentAppDisplayRegion && displayRegion.IsVisible)
        //        {
        //            return displayRegion;
        //        }
        //    }

        //    return null;
        //}

        #endregion private


    }
}
