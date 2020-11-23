using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdventOfCode
{
    public class MainWindowViewModel : BaseViewModel
    {
        public IEnumerable<DayAttribute> Days { get; set; }
        public DayAttribute SelectedDay { get; set; }
        public string InputText { get; set; }
        public string OutputText { get; set; }
        public string TestName { get; set; }
        private DayAttribute _lastRunDay;
        private string _lastRunInput;
        private string _lastRunOutput;
        private int _lastRunPart;

        public MainWindowViewModel()
        {
            RunPartOneCommand = new RelayCommand(async x => await RunDay(1), x => CanRun());
            RunPartTwoCommand = new RelayCommand(async x => await RunDay(2), x => CanRun());
            CreateTestCommand = new RelayCommand(x => CreateTest(), x => CanCreateTest());
            RunAllTestsCommand = new RelayCommand(async x => await RunAllTests(), x => CanRunAllTests());
            RunTodayTestsCommand = new RelayCommand(async x => await RunTodayTests(), x => CanRunTodayTests());

            Days = GetDayAttributes().ToList();
            SelectedDay = GetDefaultDay();
        }

        public ICommand RunPartOneCommand { get; set; }
        public ICommand RunPartTwoCommand { get; set; }
        public ICommand CreateTestCommand { get; set; }
        public ICommand RunAllTestsCommand { get; set; }
        public ICommand RunTodayTestsCommand { get; set; }

        private DayAttribute GetDefaultDay()
        {
            return Days.FirstOrDefault(d => d.Year == AdventConfig.DefaultYear && d.Day == AdventConfig.DefaultDay);
        }

        private bool CanRun()
        {
            return SelectedDay != null;
        }

        private bool CanRunAllTests()
        {
            if (!Directory.Exists(AdventConfig.TestFileFolder))
            {
                return false;
            }

            var files = Directory.GetFiles(AdventConfig.TestFileFolder, "*.test");

            return files.Any();
        }

        private bool CanRunTodayTests()
        {
            if (!Directory.Exists(AdventConfig.TestFileFolder))
            {
                return false;
            }

            if (SelectedDay == null)
            {
                return false;
            }

            var files = Directory.GetFiles(AdventConfig.TestFileFolder, $"{SelectedDay.Year}-{SelectedDay.Day}-*.test");

            return files.Any();
        }

        private async Task RunAllTests()
        {
            StartNewTestLog();
            LogMessage("=== Running All Tests ===");

            await RunTests("*.test");
        }

        private async Task RunTodayTests()
        {
            StartNewTestLog();
            LogMessage($"=== Running Tests For {SelectedDay.Year} Day {SelectedDay.Day} ===");

            await RunTests($"{SelectedDay.Year}-{SelectedDay.Day}-*.test");
        }

        private async Task RunTests(string fileMatchPattern)
        {
            LogMessage($"Searching for tests in {AdventConfig.TestFileFolder}");

            var tests = Directory.GetFiles(AdventConfig.TestFileFolder, fileMatchPattern);

            LogMessage($"{tests.Length} tests found");
            var passCount = 0;
            var failCount = 0;

            foreach (var test in tests)
            {
                var result = await RunTest(test);

                if (result)
                {
                    passCount++;
                }
                else
                {
                    failCount++;
                }
            }

            LogMessage($"{passCount} PASSED - {failCount} FAILED");
            LogMessage("=== Test Run Complete ===");
        }

        private async Task<bool> RunTest(string testFile)
        {
            var contents = File.ReadAllText(testFile);
            var fileName = Path.GetFileNameWithoutExtension(testFile);
            var year = int.Parse(fileName.Split('-')[0]);
            var day = int.Parse(fileName.Split('-')[1]);
            var part = int.Parse(fileName.Split('-')[2]);
            var testName = fileName.Substring(year.ToString().Length + day.ToString().Length + 4);

            var split = contents.LastIndexOf(Environment.NewLine);

            var input = contents.Substring(0, split);
            var output = contents.Substring(split + Environment.NewLine.Length);

            var runner = GetDayInstance(new DayAttribute(year, day));
            runner.Log = NullLogger;

            LogMessage($"Running Test: {year} Day {day} Part {part}: {testName}...");

            var testTask = new Task<(string result, long elapsed)>(() => RunDay(input, runner, part));
            testTask.Start();

            // TODO: disable buttons and show spinner

            await testTask;

            if (testTask.Result.result == output)
            {
                LogMessage($"PASSED - Result: {testTask.Result.result} (Elapsed: {testTask.Result.elapsed})");
                return true;
            }
            else
            {
                LogMessage($"FAILED - Expected Result: {output} - Actual Result: {testTask.Result.result} (Elapsed: {testTask.Result.elapsed})");
                return false;
            }
        }

        private bool CanCreateTest()
        {
            return !string.IsNullOrWhiteSpace(TestName) && _lastRunDay != null && !string.IsNullOrWhiteSpace(_lastRunInput) && !string.IsNullOrWhiteSpace(_lastRunOutput);
        }

        private void CreateTest()
        {
            if (!IsTestNameUnique(TestName, _lastRunDay, _lastRunPart))
            {
                LogMessage($"Create Test Failed, a test with the name {TestName} already exists for {SelectedDay.Year}.{SelectedDay.Day}");
                return;
            }

            CreateTestFile(TestName, _lastRunDay, _lastRunPart, _lastRunInput, _lastRunOutput);
        }

        private bool IsTestNameUnique(string testName, DayAttribute testDay, int testPart)
        {
            var fileName = GetTestFileName(testName, testDay, testPart);

            return !File.Exists(fileName);
        }

        private void CreateTestFile(string testName, DayAttribute testDay, int testPart, string input, string output)
        {
            var fileName = GetTestFileName(testName, testDay, testPart);

            Directory.CreateDirectory(AdventConfig.TestFileFolder);
            File.WriteAllText(fileName, CreateTestFileContents(input, output));

            LogMessage($"Test Created: {fileName}");
        }

        private string GetTestFileName(string testName, DayAttribute testDay, int testPart)
        {
            var fileName = $"{testDay.Year}-{testDay.Day}-{testPart}-{testName}.test";
            return Path.Combine(AdventConfig.TestFileFolder, fileName);
        }

        private string CreateTestFileContents(string input, string output)
        {
            return $"{input}{Environment.NewLine}{output}";
        }

        private async Task RunDay(int part)
        {
            StartNewLog(SelectedDay.Year, SelectedDay.Day);

            var input = GetInput(SelectedDay);

            LogMessage($"====== Input ======");
            LogMessage(input);
            LogMessage($"======= Running Day {SelectedDay.Year}.{SelectedDay.Day} Part {part} =======");

            var day = GetDayInstance(SelectedDay);

            var dayTask = new Task<(string result, long elapsed)>(() => RunDay(input, day, part));
            dayTask.Start();

            // TODO: disable buttons and show spinner

            await dayTask;

            LogMessage($"========= DONE (Elapsed: {dayTask.Result.elapsed}ms) =========");
            LogMessage($"ANSWER: {dayTask.Result.result}");

            Clipboard.SetText(dayTask.Result.result);

            _lastRunDay = SelectedDay;
            _lastRunPart = part;
            _lastRunInput = input;
            _lastRunOutput = dayTask.Result.result;
        }

        private (string result, long elapsed) RunDay(string input, BaseDay runner, int part)
        {
            string result;
            var start = Stopwatch.StartNew();

            if (part == 1)
            {
                result = runner.PartOne(input);
            }
            else
            {
                result = runner.PartTwo(input);
            }

            var end = start.ElapsedMilliseconds;

            return (result, end);
        }

        private string GetInput(DayAttribute day)
        {
            if (InputText?.Trim().Length > 0)
            {
                return InputText;
            }

            Directory.CreateDirectory(AdventConfig.InputFileFolder);

            var inputFileName = $"{day.ToString()}.txt";
            var inputFile = Path.Combine(AdventConfig.InputFileFolder, inputFileName);

            if (!File.Exists(inputFile))
            {
                DownloadInput(day, inputFile);
            }

            InputText = File.ReadAllText(inputFile);
            NotifyChange(nameof(InputText));

            return InputText;
        }

        private void DownloadInput(DayAttribute day, string inputFile)
        {
            var url = $"https://adventofcode.com/{day.Year}/day/{day.Day}/input";

            if (!File.Exists(AdventConfig.SessionCookieFile))
            {
                throw new ArgumentException($"Cannot find Session Cookie file, please check AdventConfig.cs [{AdventConfig.SessionCookieFile}]");
            }

            var sessionCookie = File.ReadAllText(AdventConfig.SessionCookieFile);

            using var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Cookie, sessionCookie);
            client.DownloadFile(url, inputFile);
        }

        private BaseDay GetDayInstance(DayAttribute day)
        {
            var dayType = GetDayType(day);

            var instance = (BaseDay)Activator.CreateInstance(dayType);
            instance.Log = LogMessage;

            return instance;
        }

        private IEnumerable<DayAttribute> GetDayAttributes()
        {
            return GetDayTypes().Select(t => t.GetCustomAttribute<DayAttribute>())
                                .OrderBy(a => a.Year)
                                .ThenBy(a => a.Day);
        }

        private IEnumerable<Type> GetDayTypes()
        {
            return Assembly.GetExecutingAssembly()
                           .ExportedTypes
                           .Where(t => t.IsSubclassOf(typeof(BaseDay)))
                           .Where(t => t.CustomAttributes.Any(a => a.AttributeType == typeof(DayAttribute)));
        }

        private Type GetDayType(DayAttribute day)
        {
            return GetDayTypes().Single(t => t.GetCustomAttribute<DayAttribute>().Year == day.Year &&
                                             t.GetCustomAttribute<DayAttribute>().Day == day.Day);
        }

        private void LogMessage(string message)
        {
            var timestamp = $"[{DateTime.Now.ToString("hh:mm:ss")}] ";
            var logMsg = message.Replace("\n", $"\n{timestamp}");
            logMsg = $"{timestamp}{logMsg}";

            if (!string.IsNullOrWhiteSpace(OutputText))
            {
                logMsg = Environment.NewLine + logMsg;
            }

            OutputText += logMsg;

            if (AdventConfig.LogFilesEnabled)
            {
                _logWriter.Write(logMsg);
            }

            NotifyChange(nameof(OutputText));
        }

        private void NullLogger(string message)
        {
        }

        private StreamWriter _logWriter;

        private void StartNewLog(int year, int day)
        {
            if (AdventConfig.LogFilesEnabled)
            {
                var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}-{year}.{day}.log";
                var path = Path.Combine(AdventConfig.LogFileFolder, fileName);

                if (_logWriter != null)
                {
                    _logWriter.Close();
                }

                Directory.CreateDirectory(AdventConfig.LogFileFolder);
                _logWriter = File.CreateText(path);
                _logWriter.AutoFlush = true;
            }

            OutputText = string.Empty;
            NotifyChange(nameof(OutputText));
        }

        private void StartNewTestLog()
        {
            if (AdventConfig.LogFilesEnabled)
            {
                var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssffff")}-Tests.log";
                var path = Path.Combine(AdventConfig.LogFileFolder, fileName);

                if (_logWriter != null)
                {
                    _logWriter.Close();
                }

                Directory.CreateDirectory(AdventConfig.LogFileFolder);
                _logWriter = File.CreateText(path);
                _logWriter.AutoFlush = true;
            }

            OutputText = string.Empty;
            NotifyChange(nameof(OutputText));
        }
    }
}
