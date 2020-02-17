using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunsets.Transactions.Import
{
    public class CsvEntry
    {
        public CsvEntry(string line)
        {
            Columns = line.Split(',');
        }

        public IList<string> Columns { get; }
    }

    public class CsvFile
    {
        private CsvFile()
        {
        }

        public IList<CsvEntry> Rows { get; private set; }

        public IList<string> Header { get; private set; }

        public static CsvFile Load(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var file = new CsvFile();

                string headerLine = reader.ReadLine();
                file.Header = new List<string>(headerLine.Split(','));
                file.Rows = new List<CsvEntry>();
                while (!reader.EndOfStream)
                {
                    file.Rows.Add(new CsvEntry(reader.ReadLine()));
                }
                return file;
            }

        }
    }

    public class CsvImporter
    {
        public void Import(CsvFile file, Account account)
        {
            int dateIndex = file.Header.IndexOf("Post Date");
            int amountIndex = file.Header.IndexOf("Amount");

            foreach (var entry in file.Rows)
            {
                DateTime date = DateTime.Parse(entry.Columns[dateIndex]);
                decimal amount = decimal.Parse(entry.Columns[amountIndex]);

                ITransaction transaction;
                if (amount < 0)
                {
                    transaction = new Expense(-amount);
                }
                else
                {
                    transaction = new Income(amount);
                }
                
                account.AddTransaction(transaction, date);
            }
        }
    }
}
