using DashboardService;
using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace DashboardService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;)
        public Service1()
        {
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; //number in miliseconds
            timer.Enabled = true;

        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            timer.Enabled = false;
            timer.Dispose();
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            int time = Convert.ToInt32(ConfigurationManager.AppSettings["Time"]);
            TimeSpan startTime = new TimeSpan(time, 33, 0); // 4:00 PM
            if ((now.DayOfWeek != DayOfWeek.Saturday && now.DayOfWeek != DayOfWeek.Sunday) &&
                now.TimeOfDay.Hours == startTime.Hours && now.TimeOfDay.Minutes == startTime.Minutes)
            {
                WriteToFile("Service hit to method at " + DateTime.Now);

                UploadCSData _pastdata = new UploadCSData();
                var Result = _pastdata.GetValues();
                UploadCreditData _creditdata = new UploadCreditData();
                var Result2 = _creditdata.GetValues();
                //UploadDebtData _debtdata = new UploadDebtData();
                //var result3 = _debtdata.GetValues();
            }
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
