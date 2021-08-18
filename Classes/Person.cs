using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WesternSydneyMedicalPractice.Classes
{
    /// <summary>
    /// Base class Person: Not implementation. Must be inherited.
    /// </summary>
    public abstract class Person 
    {
        /*
         * 'ABSTRACT' 
         *  - the class MUST be inherited from 
         *  - cannot be instantiated
         */  
        
        #region Field/Member Variables
        
        /*  FIELD VARIABLES
         *  - prefixed with '_' an underscore
         *  - used to implement ENCAPSULATION 
         */

        private int _personID;

        #endregion Field/Member Variables

        #region Public Properties
        public virtual int PersonID
        {
            get => _personID;
            set => _personID = value;
        }

        /*
         * AUTO-PROPERTY
         * - field variable hidden 
         * - not accessible from within the class 
         */
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Suburb { get; set; }
        public virtual string State { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string HomePhone { get; set; }
        #endregion Public Properties

        #region Public Methods
        /*
         * VIRTUAL Method
         * - can be redefined in derived classes
         * - implementation in the base class AND derived class
         * - derived class OVERRIDES base class implementation
         */
        public virtual int Insert()
        {
            throw new NotImplementedException();
        }
        public virtual void Load(int ID)
        {
            throw new NotImplementedException();
        }
        public virtual int Update()
        {
            throw new NotImplementedException();
        }
        public virtual int Delete()
        {
            throw new NotImplementedException();
        }
        #endregion Public Methods

    }
}
