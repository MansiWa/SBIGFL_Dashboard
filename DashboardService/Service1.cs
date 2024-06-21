using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace DashboardService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Schedule the initial execution
            ScheduleTimer();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        private void ScheduleTimer()
        {
            // Calculate the time until the next run time (5:25 PM or configured time)
            int time = Convert.ToInt32(ConfigurationManager.AppSettings["Time"]);
            DateTime now = DateTime.Now;
            DateTime nextRunTime = new DateTime(now.Year, now.Month, now.Day, time, 25, 0); // Assuming 5:25 PM as the target time

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1); // Move to the next day if it's already past the scheduled time
            }

            double interval = (nextRunTime - now).TotalMilliseconds;

            // Set the timer to trigger at the calculated next run time
            timer.Elapsed += OnTimer;
            timer.Interval = interval;
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs args)
        {
            timer.Stop(); // Stop the timer to avoid re-entry
            try
            {
                // Execute the desired task
                //UploadCSData _pastdata = new UploadCSData();
                //var Result = _pastdata.GetValues();
                //UploadCreditData _creditdata = new UploadCreditData();
                //var Result2 = _creditdata.GetValues();
                UploadDebtData _debtdata = new UploadDebtData();
                var result3 = _debtdata.GetValues();

                // Task executed successfully, reschedule for the next day
                ScheduleTimer();
            }
            catch (Exception ex)
            {
                // Log any exceptions if necessary
            }
        }
    }
}
