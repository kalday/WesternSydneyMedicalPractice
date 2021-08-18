using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
// Add the following references
using WesternSydneyMedicalPractice.Classes;
using System.Data;
using System.Data.SqlClient;

namespace WesternSydneyMedicalPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Field Variables
        // Get all the Practitioners from the Database
        Practitioners allPractitioners = new Practitioners();

        #endregion Field Variables
        public MainWindow()
        {
            InitializeComponent();

            // Fist time the window loads o into the Practitioner's Tab.
            tabctrMain.SelectedItem = tabItemPractitioners;

            // Load the Practitioner's Tab ListView
            LoadPractitionersListView();

        }

        private void tabctrMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #region Practitioners Tab

        #region Practitioner Event Handlers
        private void LoadPractitionersListView()
        {
            // Bind the Practitioners collection to the ListView control
            lvPractitioners.ItemsSource = allPractitioners;
        }

        private void btnAddNewPractitioner_Click(object sender, RoutedEventArgs e)
        {
            // toggle button between 'Add New' and 'Save'
            if(btnAddNewPractitioner.Content.ToString() == "Add New")
            {
                // change the UI to add a new practitioner 
                
                // toggle button 
                btnAddNewPractitioner.Content = "Save";

                // deselect the ListView item 
                lvPractitioners.SelectedIndex = -1;

                // disable ListView control 
                lvPractitioners.IsEnabled = false;

                // clear the controls
                clearPractitionerTabControls();

                // show cancel button and disable Update, Delete and Navigation buttons 
                btnCancelPractitioner.IsEnabled = true;
                btnUpdatePractitioner.IsEnabled = false;
                btnDeletePractitioner.IsEnabled = false;

                // ----- Navigation Buttons
                btnFirstRecord.IsEnabled = false;
                btnPreviousRecord.IsEnabled = false;
                btnNextRecord.IsEnabled = false;
                btnLastRecord.IsEnabled = false;

            }
            else
            {
                // save practitioner 

                // check if the controls Validate (that there is data in the controls)
                if (ValidatePractControls())    // default to test for true
                {
                    // save 
                    Practitioner newPractitioner = new Practitioner();

                    AssignPropertiesToPractitioner(newPractitioner);

                    if (newPractitioner.Insert() == 1)
                    {
                        MessageBox.Show("New Practitioner's details successfully saved.", "SUCCESS: Details Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                        // refresh data in ListView (i.e. get allPractitioners from db)
                        RefreshPractitionerList();

                        // move to the last item in the ListView
                        lvPractitioners.SelectedIndex = lvPractitioners.Items.Count - 1;
                        lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong. The Practitioner's Details were not saved!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    btnAddNewPractitioner.Content = "Add New";
                    btnCancelPractitioner.IsEnabled = false;
                    btnUpdatePractitioner.IsEnabled = true;
                    btnDeletePractitioner.IsEnabled = true;
                    lvPractitioners.IsEnabled = true;

                    // navigation buttons 
                    btnFirstRecord.IsEnabled = true;
                    btnPreviousRecord.IsEnabled = true;
                    btnNextRecord.IsEnabled = true;
                    btnLastRecord.IsEnabled = true;
                }
            }
        }

        private void RefreshPractitionerList()
        {
            // get latest view of practitoiners from database
            allPractitioners = null;
            allPractitioners = new Practitioners();
            LoadPractitionersListView();
        }

        private void AssignPropertiesToPractitioner(Practitioner newPractitioner)
        {
            newPractitioner.FirstName = txtPracFirstName.Text;
            newPractitioner.LastName = txtPracLastName.Text;
            
            newPractitioner.Street = txtPracStreet.Text;
            newPractitioner.Suburb = txtPracSuburb.Text;
            newPractitioner.State = cboPractState.Text;
            newPractitioner.PostCode = txtPracPostCode.Text;
            
            newPractitioner.HomePhone = txtHomePhone.Text;
            newPractitioner.Mobile = txtPracMobile.Text;

            newPractitioner.RegistrationNumber = txtPracRegistration.Text;

            newPractitioner.Monday = (bool)chkBxMonday.IsChecked;
            newPractitioner.Tuesday= (bool)chkBxTuesday.IsChecked;
            newPractitioner.Wednesday = (bool)chkBxWednesday.IsChecked;
            newPractitioner.Thursday = (bool)chkBxThursday.IsChecked;
            newPractitioner.Friday = (bool)chkBxFriday.IsChecked;
            newPractitioner.Saturday = (bool)chkBxSaturday.IsChecked;
            newPractitioner.Sunday = (bool)chkBxSunday.IsChecked;
        }

        private bool ValidatePractControls()
        {
            if(cboPractType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Practitioner Type", "Practitioner Type", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                cboPractType.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtPracFirstName.Text))
            {
                MessageBox.Show("Please enter a First Name", "First Name", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracFirstName.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtPracLastName.Text))
            {
                MessageBox.Show("Please enter a Last Name", "Last Name", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracLastName.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtPracStreet.Text))
            {
                MessageBox.Show("Please enter a Street Address", "Street Number & Name", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracStreet.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtPracSuburb.Text))
            {
                MessageBox.Show("Please enter a Suburb", "Suburb", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracSuburb.Focus();

                return false;
            }
            else if (cboPractState.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a State", "State", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                cboPractState.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtPracPostCode.Text))
            {
                MessageBox.Show("Please enter a Postcode", "Postcode", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracPostCode.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtHomePhone.Text))
            {
                MessageBox.Show("Please enter a Home Phone number", "Home Phone", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtHomePhone.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtPracMobile.Text))
            {
                MessageBox.Show("Please enter a Mobile number", "Mobile", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracMobile.Focus();

                return false;
            }
            else if (string.IsNullOrEmpty(txtPracRegistration.Text))
            {
                MessageBox.Show("Please enter a Registration Number", "Registration Number", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                txtPracRegistration.Focus();

                return false;
            }
            else
            {
                return true;
            }
        }

        private void clearPractitionerTabControls()
        {
            txtPractitioner_ID.Clear();
            cboPractType.SelectedIndex = -1;
            txtPracFirstName.Clear();
            txtPracLastName.Clear();
            txtPracStreet.Clear();
            txtPracSuburb.Clear();
            cboPractState.SelectedIndex = -1;
            txtPracPostCode.Clear();
            txtHomePhone.Clear();
            txtPracMobile.Clear();
            txtPracRegistration.Clear();
        }

        private void btnUpdatePractitioner_Click(object sender, RoutedEventArgs e)
        {
            // check a practitioner is selected in ListView
            if (lvPractitioners.SelectedItem != null)
            {
                string message = "Practitioner details will be updated." + Environment.NewLine + "Do you wish to continue?";
                string caption = "UPDATE";

                if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // instantiate practitioner object selected in ListView
                    Practitioner selectedPractitioner = (Practitioner)lvPractitioners.SelectedItem;

                    int selectedIndex = lvPractitioners.SelectedIndex;

                    // assign updated values from user interface to Practitioner object 
                    AssignPropertiesToPractitioner(selectedPractitioner);

                    try
                    {
                        if (selectedPractitioner.Update() == 1)
                        {
                            MessageBox.Show("SUCCESS: Practitioner details successfully updated", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);

                            RefreshPractitionerList();

                            // go back to practitioner
                            lvPractitioners.SelectedIndex = selectedIndex;
                            lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "ERROR: Something went wrong" + Environment.NewLine + "The Practitioner's details failed to update." + Environment.NewLine + ex.Message;

                        MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }

        private void btnDeletePractitioner_Click(object sender, RoutedEventArgs e)
        {
            // ask user if they want to delete the practitioner 
            string message = "DELETE: Confirm to delete this practitioner" + Environment.NewLine + "The Practitioner's Details and ALL appointments will be permanently deleted. This cannot be reversed.";

            // if YES (to a message box question)
            if (MessageBox.Show(message, "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // instantiate practitioner and make object the one selected in ListView 
                Practitioner selectedPractitioner = (Practitioner)lvPractitioners.SelectedItem;

                try
                {
                    // check if .Delete method of Practitioner class returns 1 (success) 
                    if (selectedPractitioner.Delete() == 1)
                    {
                        MessageBox.Show("Practitioner successfully deleted.", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                catch (Exception ex)
                {
                    message = "ERROR:" + Environment.NewLine + "The practitioner's details failed to delete." + Environment.NewLine + ex.Message;
                    MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
                
            
            
            
        }

        private void btnCancelPractitioner_Click(object sender, RoutedEventArgs e)
        {
            lvPractitioners.IsEnabled = true;

            btnAddNewPractitioner.Content = "Add New";
            btnCancelPractitioner.IsEnabled = false;
            btnUpdatePractitioner.IsEnabled = true;
            btnDeletePractitioner.IsEnabled = true;

            // navigation buttons 
            btnFirstRecord.IsEnabled = true;
            btnPreviousRecord.IsEnabled = true;
            btnNextRecord.IsEnabled = true;
            btnLastRecord.IsEnabled = true;

            // clear controls
            clearPractitionerTabControls();
            LoadPractitionersListView();

            lvPractitioners.SelectedIndex = 0;
            lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);

        }

        private void btnFirstRecord_Click(object sender, RoutedEventArgs e)
        {
            // Check if the ListView has items in it first
            if (lvPractitioners.Items.Count > 0)
            {
                lvPractitioners.SelectedIndex = 0;
                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }
        }
        private void btnLastRecord_Click(object sender, RoutedEventArgs e)
        {
            if (lvPractitioners.Items.Count > 0)
            {
                lvPractitioners.SelectedIndex = lvPractitioners.Items.Count - 1;
                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }
        }

        private void btnNextRecord_Click(object sender, RoutedEventArgs e)
        {
            //Check if there is another record/row/item to move to...
            if (lvPractitioners.SelectedIndex < lvPractitioners.Items.Count - 1)
            {
                lvPractitioners.SelectedIndex = lvPractitioners.SelectedIndex + 1;
                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }
        }

        private void btnPreviousRecord_Click(object sender, RoutedEventArgs e)
        {
            // Check that there is another record above
            if (lvPractitioners.SelectedIndex >= 1)
            {
                lvPractitioners.SelectedIndex = lvPractitioners.SelectedIndex - 1;
                lvPractitioners.ScrollIntoView(lvPractitioners.SelectedItem);
            }

        }

        private void lvPractitioners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboPractState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion Practitioner Event Handlers

        #endregion Practitioners Tab

        #region Patients Tab
        private void lvPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void btnPatFirstRecord_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnPatPreviousRecord_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnPatNextRecord_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnPatLastRecord_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cboGender_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        #endregion Patients Tab

    }
}