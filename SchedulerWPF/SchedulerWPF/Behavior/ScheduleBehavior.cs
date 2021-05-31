using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace SchedulerWPF
{
    public class ScheduleBehavior : Behavior<Grid>
    {
        Grid grid;
        SfScheduler schedule;
        Button visibleAppointments;
        ScheduleAppointmentCollection appointmentCollection;
        protected override void OnAttached()
        {
            grid = this.AssociatedObject as Grid;
            visibleAppointments = grid.Children[0] as Button;
            schedule = grid.Children[1] as SfScheduler;

            schedule.ViewChanged += OnSchedulerViewChanged;
            visibleAppointments.Click += OnVisibleAppointmentsClicked;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            schedule.ViewChanged -= OnSchedulerViewChanged;
            visibleAppointments.Click -= OnVisibleAppointmentsClicked;
            grid = null;
            schedule = null;
            visibleAppointments = null;
            appointmentCollection = null;
        }

        private void OnVisibleAppointmentsClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (appointmentCollection.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                for (int i = 0; i < appointmentCollection.Count; i++)
                {
                    message.AppendLine(appointmentCollection[i].Subject.ToString());
                }
                MessageBox.Show("This month contains " + appointmentCollection.Count + " appointments\n\n" + message.ToString());
            }
            else
            {
                MessageBox.Show("No appointments", "Appointment Details", MessageBoxButton.OK);
            }
        }

        private void OnSchedulerViewChanged(object sender, ViewChangedEventArgs e)
        {
            GetVisibleAppointment(e.NewValue.ActualStartDate);
        }

        private ScheduleAppointmentCollection GetVisibleAppointment(DateTime startDate)
        {
            appointmentCollection = new ScheduleAppointmentCollection();
            var app = (this.schedule.DataContext as SchedulerViewModel).Appointments;
            foreach (var appointment in app)
            {
                if (appointment.StartTime.Month == startDate.Month)
                {
                    appointmentCollection.Add(appointment);
                }
            }
            return appointmentCollection;
        }
    }
}
