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
    public sealed class Practitioner : Person
    {
        #region Private Field Variables
        private DataTable _dtPractitioner;  // holds data pulled from database through DataAccessLayer class 
        #endregion Private Field Variables

        #region Public Properties
        public int Practitioner_ID { get; set; }
        public string RegistrationNumber { get; set; }
        public string PractnrTypeName_Ref { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        #endregion Public Properties

        #region Constructors 
        /// <summary>
        /// Default Constructor: Instantiates a NEW 'empty' Practitioner object.
        /// </summary>
        public Practitioner() : base () { /* DEFAULT */ }
        /// <summary>
        /// Overloaded Constructor: Instantiates an existing (in the database) Practitioner object.
        /// </summary>
        /// <param name="practitioner_ID">int: the ID of the Practitioner to be instantiated.</param>
        public Practitioner(int practitioner_ID) : base ()
        {
            this.Practitioner_ID = practitioner_ID;

            // call method
            GetPractitioner();
        }

        public Practitioner(DataRow practitionerRow) : base ()
        {
            LoadPractitionerProperties(practitionerRow);
        }
        #endregion Constructors  
        
        #region Private Methods
        private void LoadPractitionerProperties(DataRow practitionerRow)
        {
            // assign Practitioner details to class properties
            this.Practitioner_ID = (int)practitionerRow["Practitioner_ID"];
            this.FirstName = practitionerRow["FirstName"].ToString();
            this.LastName = practitionerRow["LastName"].ToString();
            this.Street = practitionerRow["Street"].ToString();
            this.Suburb = practitionerRow["Suburb"].ToString();
            this.State = practitionerRow["State"].ToString();
            this.PostCode = practitionerRow["PostCode"].ToString();
            this.HomePhone = practitionerRow["HomePhone"].ToString();
            this.Mobile = practitionerRow["Mobile"].ToString();
            this.RegistrationNumber = practitionerRow["RegistrationNumber"].ToString();
            this.PractnrTypeName_Ref = practitionerRow["PractnrTypeName_Ref"].ToString();

            this.Monday = (bool)practitionerRow["Monday"];
            this.Tuesday = (bool)practitionerRow["Tuesday"];
            this.Wednesday = (bool)practitionerRow["Wednesday"];
            this.Thursday = (bool)practitionerRow["Thursday"];
            this.Friday = (bool)practitionerRow["Friday"];
            this.Saturday = (bool)practitionerRow["Saturday"];
            this.Sunday = (bool)practitionerRow["Sunday"];

            // build appointments class - get practitioner's appointments and assign them to appointments property 
            Appointments appointments = new Appointments(this);
            this.Appointments = appointments;
        }
        private void GetPractitioner()
        {
            SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");

            try
            {
                // get Practitioner's details from database
                // set up parameters 
                SqlParameter[] parameters = { new SqlParameter("@Practitioner_ID", this.Practitioner_ID) };

                this._dtPractitioner = myDAL.ExecuteStoredProc("usp_GetPractitioner", parameters);

                // check if DataTable has content
                if ((this._dtPractitioner != null) && this._dtPractitioner.Rows.Count > 0)
                {
                    // map (assign) data returned from database to properties of Practitioner instance
                    LoadPractitionerProperties(this._dtPractitioner.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve Practitioner Details! ", ex);
            }
        }
        
        #endregion Private Methods

        #region Public Methods

        #region METHOD: Update()
        /// <summary>
        /// Update the details for the Practitioner in the database.
        /// </summary>
        /// <returns>int: Returns 1 if the update succeeded and 0 if failed.</returns>
        public override int Update()
        {
            try
            {
                // instantiate
                SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");

                SqlParameter[] parameters =
                    {
                        new SqlParameter("@Practitioner_ID", this.Practitioner_ID),
                        new SqlParameter("@FirstName", this.FirstName),
                        new SqlParameter("@LastName", this.LastName),
                        new SqlParameter("@Street", this.Street),
                        new SqlParameter("@Suburb", this.Suburb),
                        new SqlParameter("@State", this.State),
                        new SqlParameter("@PostCode", this.PostCode),
                        new SqlParameter("@HomePhone", this.HomePhone),
                        new SqlParameter("@Mobile", this.Mobile),
                        new SqlParameter("@RegistrationNumber", this.RegistrationNumber),
                        new SqlParameter("@PractnrTypeName_Ref", this.PractnrTypeName_Ref),
                        new SqlParameter("@Monday", this.Monday),
                        new SqlParameter("@Tuesday", this.Tuesday),
                        new SqlParameter("@Wednesday", this.Wednesday),
                        new SqlParameter("@Thursday", this.Thursday),
                        new SqlParameter("@Friday", this.Friday),
                        new SqlParameter("@Saturday", this.Saturday),
                        new SqlParameter("@Sunday", this.Sunday)
                    };

                // calling stored procedure
                int rowsAffected = myDAL.ExecuteNonQuerySP("usp_UpdatePractioner", parameters);

                return rowsAffected;

            }
            catch (Exception ex)
            {
                throw new Exception("The Practitioner's details could not be updated! ", ex);
            }

        }
        #endregion METHOD: Update()

        #region METHOD: Insert()
        /// <summary>
        /// Inserts the details for a new Practitioner in the database.
        /// </summary>
        /// <returns>int: Returns 1 if the update succeeded and 0 if failed.</returns>
        public override int Insert()
        {
            try
            {
                // instantiate
                SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");

                SqlParameter[] parameters =
                    {
                        new SqlParameter("@FirstName", this.FirstName),
                        new SqlParameter("@LastName", this.LastName),
                        new SqlParameter("@Street", this.Street),
                        new SqlParameter("@Suburb", this.Suburb),
                        new SqlParameter("@State", this.State),
                        new SqlParameter("@PostCode", this.PostCode),
                        new SqlParameter("@HomePhone", this.HomePhone),
                        new SqlParameter("@Mobile", this.Mobile),
                        new SqlParameter("@RegistrationNumber", this.RegistrationNumber),
                        new SqlParameter("@PractnrTypeName_Ref", this.PractnrTypeName_Ref),
                        new SqlParameter("@Monday", this.Monday),
                        new SqlParameter("@Tuesday", this.Tuesday),
                        new SqlParameter("@Wednesday", this.Wednesday),
                        new SqlParameter("@Thursday", this.Thursday),
                        new SqlParameter("@Friday", this.Friday),
                        new SqlParameter("@Saturday", this.Saturday),
                        new SqlParameter("@Sunday", this.Sunday)
                    };

                // calling stored procedure
                int rowsAffected = myDAL.ExecuteNonQuerySP("usp_InsertPractitioner", parameters);

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("The new Practitioner's details could not be inserted! ", ex);
            }
        }
        #endregion METHOD: Insert()

        #region METHOD: Delete()
        /// <summary>
        /// Deletes a Practitioner in the database.
        /// </summary>
        /// <returns>int: Returns 1 if the update succeeded and 0 if failed.</returns>
        public override int Delete()
        {
            try
            {
                // instantiate
                SqlDataAccessLayer myDAL = new SqlDataAccessLayer("cnnStrWSMP");

                SqlParameter[] parameters =
                    {
                        new SqlParameter("@Practitioner_ID", this.Practitioner_ID)
                    };

                // calling stored procedure
                int rowsAffected = myDAL.ExecuteNonQuerySP("usp_DeletePractitioner", parameters);

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("The new Practitioner could not be deleted! ", ex);
            }
        }
        #endregion METHOD: Delete()

        #region METHOD: Load()
        /// <summary>
        /// Loads the details of the Practitioner from the database.
        /// </summary>
        /// <param name="practitioner_ID">int: the ID of the Practitioner to retrieve</param>
        /// <returns>int: Returns 1 if the update succeeded and 0 if failed.</returns>
        public override void Load(int practitioner_ID)
        {
            this.Practitioner_ID = practitioner_ID;

            GetPractitioner();
        }
        #endregion METHOD: Load()

        #endregion Public Methods
    }
}
