using System.Windows;

// 設定 WPF 主題資源字典的位置。預設字典由本組件提供。
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, // 主題專用資源字典的位置。
                                     // 找不到前一層的資源時使用，
                                     // 例如頁面或應用程式的資源字典。
    ResourceDictionaryLocation.SourceAssembly // 通用資源字典的位置。
                                              // 找不到前一層的資源時使用，
                                              // 例如頁面、應用程式或主題專用資源字典。
)]
