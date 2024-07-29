using BudgetAppLibray;
using Microsoft.Extensions.Logging;

namespace BudgetAppProject {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<LocalDbService>();
            builder.Services.AddTransient<MainPage>();

#if WINDOWS // GOnna need this for all the other platforms too
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("NoLabel", (handler, view) => {
                if (view is BudgetAppLibray.CustomSwitch1) {
                    handler.PlatformView.OnContent = "Percent";
                    handler.PlatformView.OffContent = "Static";

                    // Add this to remove the padding around the switch as well
                    handler.PlatformView.MinWidth = 0;
                }
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
