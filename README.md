# Stratysis
#### A pluggable automated trading system backtesting engine.

### Builds
Master Branch: [![Build Status](https://jonblankenship.visualstudio.com/stratysis/_apis/build/status/Stratysis%20-%20CI%20Build?branchName=master)](https://jonblankenship.visualstudio.com/stratysis/_build/latest?definitionId=1&branchName=master)



## Overview

Stratysis an automated trading system backtesting engine written in C#.  It is designed to be pluggable, allowing for the easy addition of new strategies, indicators, data providers, etc...

Stratysis is a pet project of mine and very much a work-in-progress.  I'm developing it to satisfy my own creativity and to support my personal trading interests.

#### Progress

- 2019.04 - Start project; built original engine skeleton; console app
- 2020.09 - Dust off project; add Oanda web client; upgrade to .NET Core 3.1
- 2020.10 - Start (ugly) WPF client to support changing parameters and visualization of results



## Getting Started

Stratysis currently supports data from [Oanda](https://www.oanda.com/) and [Quandl](https://www.quandl.com/).  My current focus is on the Forex market, and Oanda will be the default data provided for the foreseeable future.

Prior to running either the console app or the WPF app, obtain an API key from Oanda.  This will require an account.

#### Running the Console App

1. Open the solution in Visual Studio.

2. Right-click Stratysis.Console > Set as Startup Project.

3. Right-click Stratysis.Console > Manage User Secrets.

4. In the secrets.json file that opens, add the following, using the API key provided by Oanda:

   ```
   {
     "AppSettings":
     {    
       "QuandlApiKey": "xxxxxxxxxxxx-xxxxxxx",
       "OandaApiKey": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
     }
   }
   ```

   _(This is also where you add the QuandlApiKey if you're using the QuandlWebClient.)_

5. Save the secrets.json file and run the project.

#### Running the WPF App

1. Open the solution in Visual Studio.

2. Right-click Stratysis.WPF > Set as Startup Project.

3. Right-click Stratysis.WPF > Manage User Secrets.

4. In the secrets.json file that opens, add the following, using the API key provided by Oanda:

   ```
   {
     "AppSettings":
     {    
       "QuandlApiKey": "xxxxxxxxxxxx-xxxxxxx",
       "OandaApiKey": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
     }
   }
   ```

   _(This is also where you add the QuandlApiKey if you're using the QuandlWebClient.)_

5. Save the secrets.json file and run the project.

#### Adding a Strategy

Specific trading strategies are located in the Stratysis.Strategies project.  The SimpleBreakoutStrategy is a good example to follow (and the only example for now) for creating a new strategy.

1. Create `MyCustomStrategyParameters` implementing `IStrategyParameters` in Stratysis.Strategies.

   The strategy parameters class is used to manage the parameters specific to the custom strategy.

2. Create `MyCustomStrategy` deriving from `Strategy` in Stratysis.Strategies.

   The strategy class contains the strategy rules.  There are four main methods to implement:

   - `Initialize(BacktestParameters parameters, IStrategyParameters strategyParameters)`
   - `ProcessNewData(Slice slice)`
   - `EvaluateEntryConditions(string security, Slice slice)`
   - `EvaluateExistConditions(Position openPosition, string security, Slice slice)`

3. Add the following to the dictionary in StrategiesService:
   `{ typeof(MyCustomStrategy), typeof(MyCustomStrategyParameters) }`

4. Add `MyCustomStrategyParametersViewModel` to Stratysis.Wpf\ViewModels\StrategyParameters.

5. Add the following to the dictionary in StrategyParametersViewModelFactory:

   `{ typeof(MyCustomStrategy), typeof(MyCustomStrategyParametersViewModel) }`

6. Add `MyCustomStrategyParametersView` to Stratysis.Wpf\Views\StrategyParameters.



For now, the backtest results are only visible by setting a breakpoint on the `_strategyRunner.RunAsync(..)` call and inspecting the results, but I'll be adding visualization of the results to the app soon.



## Stay in Touch

If you find Stratysis helpful or have any comments or questions, I'd love to hear from you.  Feel free to send me a message on Twitter at [@Jon_Blankenship](https://twitter.com/Jon_Blankenship).



## Disclaimer

Stratysis is not intended to be an example of how to build an application or develop a trading system.  While there may be things that can be gleaned from the codebase pertaining to either topic, teaching is not the intent here.

Stratysis is **not** a production-level application, and it should not be used to make investment decisions.  Nothing in this repository nor any output of the application constitutes investment advice, and I am not responsible for any profits or losses incurred as a result of using Stratysis.