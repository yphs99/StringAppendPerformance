# 使用 BenchmarkDotNet 簡單地測試效能

### 前言

相信大家在實作時都會遇上跟效能有關的疑問！

內心OS：
* X跟Y寫法，哪個好？
* 突然想起之前聽前輩說A寫法比B寫法快？
* 我這寫法感覺還比較快！

而一般常見手法大多都是使用 StopWatch 記錄，之後手動在程式碼內加上記錄時間開始結束時間，之後手動記錄時間、整理結果，最後復原程式碼，而這種記錄採樣也就一次性不夠公正、客觀，也忽略記憶體空間成本。

今天來介紹OpenSource、免費工具 [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) 可以讓我們比對多種不同程式寫法，並告訴我們平均執行時間、耗用多少記憶體...等等。

![BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet/raw/master/docs/logo/logo-wide.png)
### 安裝

打開 Nuget 尋找 `BenchmarkDotnet` 並安裝，或直接下指令安裝

```
Install-Package BenchmarkDotNet
```

### 範例 StringAppendPerformance

#### 建立1個類別與實作方法

```
/// <summary>
/// 常用通用元件
/// </summary>
public class Utility
{
    /// <summary>
    /// 字串串接(使用+=)
    /// </summary>
    /// <param name="n">串接次數</param>
    /// <returns></returns>
    public string Concat(int n)
    {
        string resilt = string.Empty;
        for (int i = 0; i < n; i++)
        {
            resilt += i.ToString();
        }
        return resilt;
    }

    /// <summary>
    /// 字串串接(StringBuilder)
    /// </summary>
    /// <param name="n">串接次數</param>
    /// <returns></returns>
    public string Append(int n)
    {
        StringBuilder resilt = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            resilt.Append(i.ToString());
        }
        return resilt.ToString();
    }
}
```

#### 在每個相關方法上套上 [Benchmark] 屬性標籤，並在count指定 [Params] 標籤分別執行10、100、1000次串接
```
/// <summary>
/// 使用 Benchmark 評測字串串接效能
/// </summary>
[MemoryDiagnoser]
public class StringAppendBenchmark
{
    /// <summary>
    /// 通用元件
    /// </summary>
    public Utility Utility;

    /// <summary>
    /// 建構子
    /// </summary>
    public StringAppendBenchmark()
    {
        Utility = new Utility();
    }

    /// <summary>
    /// 串接次數
    /// </summary>
    [Params(10, 100, 1000)]
    public int Count;

    /// <summary>
    /// 字串串接(使用+=)
    /// </summary>
    [Benchmark]
    public void Concat()
    {
        Utility.Concat(Count);
    }

    /// <summary>
    /// 字串串接(StringBuilder)
    /// </summary>
    [Benchmark]
    public void Append()
    {
        Utility.Append(Count);
    }
}
```
#### 在主控台上呼叫 BenchmarkRunner.Run 開始評測

請注意 BenchmarkDotnet 必須在 release 環境下啟動

```
public class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<StringAppendBenchmark>();
    }
}
```

#### 報告結果

* 測試報告提供多種呈現方式，常見格式亦有支援如 csv、markdown、html，預設儲存於 release 資料夾裡面的 `BenchmarkDotNet.Artifacts`

| Method | Count |          Mean |        Error |       StdDev |     Gen0 |   Gen1 | Allocated |
|------- |------ |--------------:|-------------:|-------------:|---------:|-------:|----------:|
| **Concat** |    **10** |     **191.53 ns** |     **3.696 ns** |     **6.277 ns** |   **0.0534** |      **-** |     **336 B** |
| Append |    10 |      99.97 ns |     0.955 ns |     0.797 ns |   0.0242 |      - |     152 B |
| **Concat** |   **100** |   **4,620.79 ns** |    **63.722 ns** |    **59.605 ns** |   **3.7766** |      **-** |   **23736 B** |
| Append |   100 |   2,094.37 ns |    39.971 ns |    50.550 ns |   0.6599 |      - |    4160 B |
| **Concat** |  **1000** | **269,336.32 ns** | **4,773.401 ns** | **7,144.602 ns** | **454.1016** | **6.3477** | **2849736 B** |
| Append |  1000 |  24,107.01 ns |   448.293 ns |   479.669 ns |   7.3547 | 0.2136 |   46328 B |

### 其他

* 當你在完成優化或重構程式後，需要有個客觀的前後量化數據對照，可以在 Benchmark後面加上baseline設定。

```
[Benchmark(Baseline = true)]
public void 原始方法()
{
....
}
[Benchmark]
public void 重構優化後方法()
{
....
}
```

* 支援評測不同平台下(NET Framework, .NET Core, Mono and CoreRT.)的效能。

```
[ClrJob, MonoJob, CoreJob, CoreRtJob]
public class StringAppendBenchmark
```
或

```
[SimpleJob(RuntimeMoniker.Net472)]
[SimpleJob(RuntimeMoniker.NetCoreApp30)]
public class StringAppendBenchmark
```

> 若結果出現NA的話，這時需要開啟專案 csproj 檔案，手動調整專案內容，詳見[F&Q](https://benchmarkdotnet.org/articles/faq.html)
 
```
<TargetFrameworks>netcoreapp3.0;net472</TargetFrameworks>
```

* 瞭解記憶體分配、使用狀況，其中報告內的Allocated即是記憶體使用狀況。

```
[MemoryDiagnoser]
public class StringAppendBenchmark
```

* 想瞭解其他的設定或更多使用方式可以到底下的參考文件的官網查詢。

#### 範例下載

> [GitHub](https://github.com/yphs99/StringAppendPerformance)

### 參考文件

> [入門指引 Getting started](https://benchmarkdotnet.org/articles/guides/getting-started.html)
