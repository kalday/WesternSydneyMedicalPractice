using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional libraries
using System.Data;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice.Classes
{
    /// <summary>
    /// Creates a collection (List) of Appointments
    /// </summary>
    public class Appointments : List<Appointment>
    {
        public Appointments(Patient patient)
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("usp_GetAppointDetailByPatientId", patient.Patient_ID) };
            DataTable appointmentsTable = myDAL.ExecuteStoredProc("usp_GetAppointDetailByPatientId", parameters);

            foreach (DataRow appointmentRow in appointmentsTable.Rows)
            {
                //Create a new instance of an Appointment for each row
                Appointment appointment = new Appointment(appointmentRow);

                //Add the appointment to this class's internal List.
                this.Add(appointment);
            }
        }

        public Appointments(Practitioner practitioner)
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Practitioner_ID", practitioner.Practitioner_ID) };

            DataTable appointmentsTable = myDAL.ExecuteStoredProc("usp_GetAppointDetailByPractitionerId", parameters);

            foreach (DataRow appointmentRow in appointmentsTable.Rows)
            {
                //Create a new instance of an Appointment for each row
                Appointment appointment = new Appointment(appointmentRow);

                //Add the appointment to this class's internal List.
                this.Add(appointment);
            }
        }
    }
}
