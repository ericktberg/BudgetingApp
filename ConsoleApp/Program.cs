using Sunsets.Application;
using Sunsets.Application.MVVM;
using Sunsets.Transactions;
using Sunsets.Transactions.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace Sunsets.ConsoleApp
{
    public class CreateStatementViewModel : ViewModelBase
    {
        private decimal _balance;

        private DateTime _date;

        public CreateStatementViewModel(Account account)
        {
            Account = account;

            CreateStatementCommand = new RelayCommand(obj => CreateStatement());
        }

        #region Dependencies

        private Account Account { get; }

        #endregion Dependencies

        #region Commands

        public ICommand CreateStatementCommand { get; }

        #endregion Commands

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private void CreateStatement()
        {
            Statement statement = new Statement(Balance);

            Account.AddStatement(statement, Date);
        }
    }

    public class ImportViewModel : ViewModelBase
    {
        private string _importPath;

        public ImportViewModel(Account account)
        {
            Account = account;

            ImportCommand = new RelayCommand(obj => Import());
        }

        #region Dependencies

        private Account Account { get; }

        #endregion Dependencies

        #region Commands

        public ICommand ImportCommand { get; }

        #endregion Commands

        public string ImportPath
        {
            get { return _importPath; }
            set
            {
                _importPath = value;
                OnPropertyChanged(nameof(ImportPath));
            }
        }

        private void Import()
        {
            CsvFile file = CsvFile.Load(File.OpenRead(ImportPath));
            CsvImporter importer = new CsvImporter();

            importer.Import(file, Account);
        }
    }

    public class Program
    {
        public static string DefaultFileLocation => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string DefaultFileName => "Finance.json";

        public static string DefaultFilePath => Path.Combine(DefaultFileLocation, "Sunsets", DefaultFileName);

        private static CommandLineParser Parser { get; } = new CommandLineParser();

        public static CommandName ParseCommand(string[] args)
        {
            switch (args[0])
            {
                case "import":
                    return CommandName.Import;

                case "statement":
                    return CommandName.Statement;

                case "transaction":
                    return CommandName.Transaction;

                case "balance":
                    return CommandName.Balance;

                case "account":
                    return CommandName.Account;

                default:
                    throw new NotImplementedException();
            }
        }

        private static void CreateAccountFunction(string[] args)
        {
            FileManager files = new FileManager();
            AccountManager manager = files.GetAccountManagerFromPath(DefaultFilePath);

            string accountName = Parser.ParseSetting("Name", args);
            string typeString = Parser.ParseSetting("Type", args);

            AccountType type = (AccountType)Enum.Parse(typeof(AccountType), typeString);

            CreateAccountViewModel viewModel = new CreateAccountViewModel(manager)
            {
                AccountName = accountName,
                AccountType = type
            };

            viewModel.CreateAccountCommand.Execute(null);

            files.SaveAccountManagerToPath(DefaultFilePath, manager);
        }

        private static void ImportFunction(string[] args)
        {
            FileManager files = new FileManager();
            AccountManager manager = files.GetAccountManagerFromPath(DefaultFilePath);

            string accountName = Parser.ParseSetting("Name", args);
            string importPath = Parser.ParseSetting("File", args);

            Account account = manager.GetAccount(accountName);

            ImportViewModel viewmodel = new ImportViewModel(account)
            {
                ImportPath = importPath,
            };
            viewmodel.ImportCommand.Execute(null);

            files.SaveAccountManagerToPath(DefaultFilePath, manager);
        }

        private static void Main(string[] args)
        {
            CommandName command = ParseCommand(args);
            // [command] [subcommand] [options] [parameters]
            switch (command)
            {
                case CommandName.Import:
                    ImportFunction(args);
                    break;

                case CommandName.Statement:
                    StatementFunction(args);
                    break;

                case CommandName.Transaction:
                    break;

                case CommandName.Balance:
                    break;

                case CommandName.Account:
                    if (Parser.ParseFlag("create", args))
                    {
                        CreateAccountFunction(args);
                    }
                    else if (Parser.ParseFlag("balance", args))
                    {
                        if (Parser.ParseFlag("-all", args))
                        {
                        }
                        else
                        {
                            DateTime date;
                            if (Parser.ParseFlag("-day", args))
                            {
                                date = ParseDate(args);
                            }
                            else
                            {
                                date = DateTime.Now;
                            }
                        }
                    }
                    break;
            }
        }

        private static DateTime ParseDate(string[] args)
        {
            return DateTime.Parse(Parser.ParseSetting("Date", args));
        }

        private static void StatementFunction(string[] args)
        {
            FileManager files = new FileManager();
            AccountManager manager = files.GetAccountManagerFromPath(DefaultFilePath);

            string accountName = Parser.ParseSetting("Name", args);
            decimal balance = decimal.Parse(Parser.ParseSetting("Balance", args));
            DateTime date = DateTime.Parse(Parser.ParseSetting("Date", args));

            Account account = manager.GetAccount(accountName);

            CreateStatementViewModel viewmodel = new CreateStatementViewModel(account)
            {
                Balance = balance,
                Date = date,
            };
            viewmodel.CreateStatementCommand.Execute(null);

            files.SaveAccountManagerToPath(DefaultFilePath, manager);
        }
    }

    public class ViewBalanceViewModel : ViewModelBase
    {
        private IEnumerable<Tuple<DateTime, decimal>> _endingBalances;

        public ViewBalanceViewModel(Account account)
        {
            Account = account;

        }

        #region Dependencies

        private Account Account { get; }

        #endregion Dependencies

        #region Commands

        #endregion Commands

        public IEnumerable<FinancialDay> EndingBalances => Account.Calendar.Days;

        public 
    }
}