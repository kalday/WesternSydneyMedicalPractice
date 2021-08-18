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
    public class Practitioners : List<Practitioner>
    {
        #region Constructor
        /// <summary>
        /// Retrieves all Practitioners from the database 
        /// </summary>
        public Practitioners()
        {
            // get all practitioners 
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");

            // call stored procedure 
            DataTable practitionerTable = myDAL.ExecuteStoredProc("usp_GetAllPractitioners");

            // loop through dt returned and instantiate each Practitioner adding each to this class' internal list 
            foreach (DataRow practitionerRow in practitionerTable.Rows)
            {
                // instantiate practitioner 
                Practitioner newPractitioner = new Practitioner(practitionerRow);
                this.Add(newPractitioner);
            }
        }
        
        public Practitioners(DayOfWeek dayAvailable)
        {
            // get practitioners available on a particular day 
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");

            // define the parameter for the day of the week required 
            SqlParameter[] parameters = { new SqlParameter("@WeekDay", dayAvailable.ToString()) };

            // call DAL to execute stored procedure
            DataTable practitionerTable = myDAL.ExecuteStoredProc("usp_GetPractByDayAvail", parameters);

            // loop through the results, adding each Practitioner object to class' internal list
            foreach (DataRow practitionerRow in practitionerTable.Rows)
            {
                Practitioner newPractitioner = new Practitioner(practitionerRow);
                this.Add(newPractitioner);
            }

        }
        #endregion Constructor

        #region Public Methods
        public void Refresh()
        {
            // clear practitioners in list
            this.Clear();

            // get all practitioners 
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");

            // call stored procedure 
            DataTable practitionerTable = myDAL.ExecuteStoredProc("usp_GetAllPractitioners");

            // loop through dt returned and instantiate each Practitioner adding each to this class' internal list 
            foreach (DataRow practitionerRow in practitionerTable.Rows)
            {
                // instantiate practitioner 
                Practitioner newPractitioner = new Practitioner(practitionerRow);
                this.Add(newPractitioner);
            }
        }
        #endregion Public Methods 
    }
}
