using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace MSG.EMailChecker
{
    public enum Operation
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals,
        And,
        Or,
        Not
    }

    public class Filter
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public Operation Operation { get; set; }
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Key) && Value != null)
            {
                var operation = "eq";

                switch (Operation)
                {
                    case Operation.Equals:
                        operation = "eq";
                        break;
                    case Operation.NotEquals:
                        operation = "ne";
                        break;
                    case Operation.GreaterThan:
                        operation = "gt";
                        break;
                    case Operation.GreaterThanOrEquals:
                        operation = "ge";
                        break;
                    case Operation.LessThan:
                        operation = "lt";
                        break;
                    case Operation.LessThanOrEquals:
                        operation = "le";
                        break;
                    case Operation.And:
                        operation = "and";
                        break;
                    case Operation.Or:
                        operation = "or";
                        break;
                    case Operation.Not:
                        operation = "not";
                        break;
                    default:
                        break;
                }
                var filterString = $"{Key} { operation} {Value.ToString()}";
                if (Value.GetType() == typeof(string) || Value.GetType() == typeof(char))
                    filterString = $"{Key} { operation} '{Value.ToString()}'";

                if (Value.GetType() == typeof(DateTimeOffset))
                {
                    filterString = $"{Key} { operation} {((DateTimeOffset)Value).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", new CultureInfo("en-Us"))}";
                }

                return filterString;
            }
            return base.ToString();
        }

    }

    public class EMailChecker
    {
        private static GraphServiceClient _graphClient;
        private DateTimeOffset _startDateTime;
        public List<Filter> Filters { get; set; }


        public EMailChecker(DateTimeOffset startDateTime)
        {
            _startDateTime = startDateTime;
            _graphClient = AuthenticationHelper.GetAuthenticatedClient();
            Filters = new List<Filter>();
        }

        public void CheckEMails(CancellationToken cancellationToken)
        {
            try
            {
                if (_graphClient != null)
                {
                    bool checkEMail = true;

                    var user = _graphClient.Me.Request().GetAsync().Result;
                    string displayName = user.DisplayName;

                    Console.WriteLine($"\nHello, { displayName }. Let's start to check your e-mails.");

                    string filter = string.Empty;
                    List<QueryOption> options = new List<QueryOption>();

                    Console.WriteLine($"Please enter an specifig FROM e-mail address:(empty for all)");
                    var fromAddress = Console.ReadLine();
                    Console.WriteLine("Ok then. Let's start.\n");

                    if (!string.IsNullOrEmpty(fromAddress))
                    {
                        Filters.Add(new Filter()
                        {
                            Key = "from/emailAddress/address",
                            Operation = Operation.Equals,
                            Value = fromAddress

                        });
                    }

                    while (checkEMail)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"New e-mail check from: {_startDateTime.ToString()}");
                        Console.ResetColor();

                        filter = string.Empty;
                        foreach (var item in Filters)
                        {
                            if (item.Key == "ReceivedDateTime")
                                item.Value = _startDateTime;
                            filter += $" {item.ToString()} and";

                        }
                        if (!string.IsNullOrEmpty(filter))
                        {
                            filter = filter.Substring(0, filter.Length - 3).Trim();
                            options.Clear();
                            options.Add(new QueryOption("$filter", filter));
                        }

                        var newEMailRequest = _graphClient.Me.MailFolders.Inbox.Messages.Request(options);
                        var newEMails = newEMailRequest.GetAsync().Result;

                        Display(newEMails);
                        if (newEMails.Count == 0)
                        {
                            _startDateTime = DateTimeOffset.UtcNow.AddSeconds(-5);
                        }
                        else
                        {
                            _startDateTime = newEMails.OrderByDescending(s => s.ReceivedDateTime).FirstOrDefault().ReceivedDateTime.Value.AddSeconds(1);
                        }

                        Thread.Sleep(5000);
                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Can not create Graph client");
                    Console.ResetColor();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Display(ICollection<Message> mails)
        {

            foreach (var item in mails.OrderBy(o => o.ReceivedDateTime.Value))
            {
                //TODO: Just do what you want with an ordinary e-mail message
                //Example:
                //*Parse the content and process it for some business
                //*Create an auto service-ticket
                //*Write an auto-reply for some cases
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\tSubject:");
                Console.ResetColor();
                Console.Write($"{ item.Subject.PadRight(50)}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"- Received: ");
                Console.ResetColor();
                Console.Write($"{item.ReceivedDateTime.ToString()}\n");

            }

        }


    }
}
