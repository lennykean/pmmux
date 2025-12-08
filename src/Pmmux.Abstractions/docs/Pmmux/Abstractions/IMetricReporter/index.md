#### [Pmmux\.Abstractions](../../../index.md 'index')
### [Pmmux\.Abstractions](../index.md 'Pmmux\.Abstractions')

## IMetricReporter Interface

Reports metrics from port multiplexer components\.

```csharp
public interface IMetricReporter
```

| Methods | |
| :--- | :--- |
| [MeasureDuration\(string, string, Dictionary&lt;string,string&gt;\)](MeasureDuration.md#Pmmux.Abstractions.IMetricReporter.MeasureDuration(string,string,System.Collections.Generic.Dictionary_string,string_) 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDuration\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>\)') | Create a disposable object that measures and reports duration from creation to disposal\. |
| [MeasureDuration&lt;T&gt;\(string, string, Dictionary&lt;string,string&gt;, Func&lt;T&gt;\)](MeasureDuration.md#Pmmux.Abstractions.IMetricReporter.MeasureDuration_T_(string,string,System.Collections.Generic.Dictionary_string,string_,System.Func_T_) 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDuration\<T\>\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.Func\<T\>\)') | Execute an operation and measure its duration\. |
| [MeasureDurationAsync&lt;T&gt;\(string, string, Dictionary&lt;string,string&gt;, Func&lt;Task&lt;T&gt;&gt;\)](MeasureDurationAsync_T_(string,string,Dictionary_string,string_,Func_Task_T__).md 'Pmmux\.Abstractions\.IMetricReporter\.MeasureDurationAsync\<T\>\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>, System\.Func\<System\.Threading\.Tasks\.Task\<T\>\>\)') | Execute an asynchronous operation and measure its duration\. |
| [ReportCounter\(string, string, double, Dictionary&lt;string,string&gt;\)](ReportCounter(string,string,double,Dictionary_string,string_).md 'Pmmux\.Abstractions\.IMetricReporter\.ReportCounter\(string, string, double, System\.Collections\.Generic\.Dictionary\<string,string\>\)') | Report counter metric with a specified value\. |
| [ReportDuration\(string, string, TimeSpan, Dictionary&lt;string,string&gt;\)](ReportDuration(string,string,TimeSpan,Dictionary_string,string_).md 'Pmmux\.Abstractions\.IMetricReporter\.ReportDuration\(string, string, System\.TimeSpan, System\.Collections\.Generic\.Dictionary\<string,string\>\)') | Report duration metric with a specified time span\. |
| [ReportEvent\(string, string, Dictionary&lt;string,string&gt;\)](ReportEvent(string,string,Dictionary_string,string_).md 'Pmmux\.Abstractions\.IMetricReporter\.ReportEvent\(string, string, System\.Collections\.Generic\.Dictionary\<string,string\>\)') | Report a single count of a metric\. |
| [ReportGauge\(string, string, double, Dictionary&lt;string,string&gt;\)](ReportGauge(string,string,double,Dictionary_string,string_).md 'Pmmux\.Abstractions\.IMetricReporter\.ReportGauge\(string, string, double, System\.Collections\.Generic\.Dictionary\<string,string\>\)') | Report gauge metric with a specified current value\. |
| [ReportMetric&lt;T&gt;\(Metric&lt;T&gt;\)](ReportMetric_T_(Metric_T_).md 'Pmmux\.Abstractions\.IMetricReporter\.ReportMetric\<T\>\(Pmmux\.Abstractions\.Metric\<T\>\)') | Report a typed metric\. |
