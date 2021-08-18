using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice.Classes
{
    public sealed class Appointment
    {
        /*
         * SEALED
         * - prevents other classes from inheriting the class
         * - cannot be modified in derived classes
         */

        #region Public Properties
        public int Practitioner_ID { get; set; }
        public string PractitionerType { get; set; }
        public string PractitionerFirstName { get; set; }
        public string PractitionerLastName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public int Patient_ID { get; set; }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Appoinmtent Default Constructor - Instantiates an empty appointment object.
        /// </summary>
        public Appointment()
        {
            //Default Constructor does nothing
        }
        /// <summary>
        /// Creates a NEW Appointment with the supplied date, time, patient, practitioner.
        /// </summary>
        /// <param name="appointmentDate">DateTime: The date of the desired appointment.</param>
        /// <param name="appointmentTime">TimeSpan: The time of the desired appointment.</param>
        /// <param name="patient_id">int: The id of the patient for whom the appointment is being made.</param>
        /// <param name="practitioner_id">int: The id of the practitioner that the appointment is with.</param>

        public Appointment(DateTime appointmentDate, TimeSpan appointmentTime, int patient_id, int practitioner_id)
        {
            //Map the details of the appointment to the properties of this class (Appointment)
            this.AppointmentDate = appointmentDate;
            this.AppointmentTime = appointmentTime;
            this.Patient_ID = patient_id;
            this.Practitioner_ID = practitioner_id;
        }

        /// <summary>
        /// Appointment Constructor: creates an Appointment from the values passed in a DataRow. Used by the Appointment(s) class (a List class).
        /// </summary>
        /// <param name="appointmentRow"></param>

        public Appointment(DataRow appointmentRow)
        {
            this.Practitioner_ID = (int)appointmentRow["Practitioner_ID"];
            this.PractitionerType = appointmentRow["PractnrTypeName_Ref"].ToString();
            this.PractitionerFirstName = appointmentRow["FirstName"].ToString();
            this.PractitionerLastName = appointmentRow["LastName"].ToString();
            this.AppointmentDate = (DateTime)appointmentRow["AppointmentDate"];
            //this.AppointmentDate = DateTime.Parse(appointmentRow["AppointmentDate"].ToString());
            this.AppointmentTime = (TimeSpan)appointmentRow["AppointmentTime_Ref"];
            this.Patient_ID = (int)appointmentRow["Patient_ID"];
        }
        #endregion Constructors

        #region Public Methods
        #region GetDetails
        /// <summary>
        /// Retrieves the details of an appointment for a given Patient.
        /// </summary>
        /// <returns>DataTable: Appointment Details for a given patient.</returns>
        public DataTable GetDetails()
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Patient_Id", this.Patient_ID) };
            DataTable appointmentDetails = myDAL.ExecuteStoredProc("usp_GetAppointDetailByPatientId", parameters);
            return appointmentDetails;
        }

        /// <summary>
        /// Retrieves the details of an appointment for a given Patient passing the Patient_ID.
        /// </summary>
        /// <returns>DataTable: Appointment Details for a given patient.</returns>
        public DataTable GetDetails(int patient_Id)
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Patient_Id", patient_Id) };
            DataTable appointmentDetails = myDAL.ExecuteStoredProc("usp_GetAppointDetailByPatientId", parameters);
            return appointmentDetails;
        }

        #endregion GetDetails

        #region GetPractitionerAppointments
        /// <summary>
        /// Retrieves all of the appointments for a given practitioner
        /// </summary>
        /// <param name="practitioner_Id">int: The ID of the practitioner for whom we want a list of appointments</param>
        /// <returns></returns>
        public DataTable GetPractitionerAppointments(int practitioner_Id)
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");
            SqlParameter[] parameters = { new SqlParameter("@Practitioner_ID", practitioner_Id) };
            DataTable appointmentDetails = myDAL.ExecuteStoredProc("usp_GetAppointmentDetailsByPractitionerId", parameters);
            return appointmentDetails;
        }
        #endregion GetPractitionerAppointments

        #region MakeAppointment
        public int MakeAppointment()
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Practitioner_ID", this.Practitioner_ID),
                new SqlParameter("@AppointmentDate", this.AppointmentDate.ToShortDateString()),
                new SqlParameter("@AppointmentTime", this.AppointmentTime),
                new SqlParameter("@Patient_ID", this.Patient_ID)
            };

            //We have to explicitly define (convert) the date value for AppointmentDate and the Time
            //value for AppointmentTime to SqlServer date and time types.
            parameters[1].SqlDbType = SqlDbType.Date;
            parameters[2].SqlDbType = SqlDbType.Time;

            int rowsAffected = myDAL.ExecuteNonQuerySP("usp_CreateAppointment", parameters);

            return rowsAffected;
        }

        #endregion MakeAppointment

        #region CancelAppointment
        /// <summary>
        /// Cancel (DELETE) an existing appointment in the database.
        /// </summary>
        /// <returns>int: 1 if appointment is DELETED, 0 if NOT</returns>
        public int CancelAppointment()
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Practitioner_ID", this.Practitioner_ID),
                new SqlParameter("@AppointmentDate", this.AppointmentDate.ToShortDateString()),
                new SqlParameter("@AppointmentTime", this.AppointmentTime),
                new SqlParameter("@Patient_ID", this.Patient_ID)
            };

            //We have to explicitly define (convert) the date value for AppointmentDate and the Time
            //value for AppointmentTime to SqlServer date and time types.
            parameters[1].SqlDbType = SqlDbType.Date;
            parameters[2].SqlDbType = SqlDbType.Time;

            int rowsAffected = myDAL.ExecuteNonQuerySP("usp_CancelAppointment", parameters);

            return rowsAffected;
        }

        #endregion CancelAppointment


        #endregion Public Methods
    }
}
