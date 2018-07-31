using MSG.EMailChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMailCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

           
                Console.WriteLine($"" +
                    @" 
 ___         __  __          _   _      ___   _                 _  
| __|  ___  |  \/  |  __ _  (_) | |    / __| | |_    ___   __  | |__  ___   _ _ 
| _|  |___| | |\/| | / _` | | | | |   | (__  | ' \  / -_) / _| | / / / -_) | '_|
|___|       |_|  |_| \__,_| |_| |_|    \___| |_||_| \___| \__| |_\_\ \___| |_|  
                     
                    ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Please login...");
                Console.ResetColor();

                EMailChecker p = new EMailChecker(DateTimeOffset.Now.AddHours(-5));
                p.Filters.Add(new Filter()
                {
                    Key = "ReceivedDateTime",
                    Operation = Operation.GreaterThan,
                    Value = DateTimeOffset.Now
                });
                CancellationTokenSource cancel = new CancellationTokenSource();

                var task1 = Task.Factory.StartNew(() => p.CheckEMails(cancel.Token), cancel.Token);

                Task.WaitAny(task1);
                
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You have got an error! :( {ex.Message}");
                return;
            }
        }
    }
}
